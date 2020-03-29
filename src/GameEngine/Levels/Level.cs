using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

using GameEngine.Interfaces;
using GameEngine.Entities;
using GameEngine.Graphics;
using GameEngine.Utilities;

namespace GameEngine.Levels
{
    class Level : IUpdateable
    {
        //Temp
        protected int mapWidth, mapHeight;
        int tileSize;
        protected int[,] map;
        Stopwatch watch = new Stopwatch();
        List<Entity> entities = new List<Entity>();
        Matrix3x2 iso;

        /// <summary>
        /// Level inicializalas
        /// </summary>
        public void Init()
        {
            tileSize = 32;
            float root2 = (float) Math.Sqrt(2.0f);
            /*  Transformation matrix
             *  2*sqrt(2)   -2*sqrt(2)  0
             *  sqrt(2)     sqrt(2)     0
             */
            iso = new Matrix3x2(2 * root2, root2, -2 * root2, root2, 0.0f, 0.0f);
            watch.Start();
        }

        public void AddEntity(Entity entity)
        {
            entity.Initalize(this);
            entities.Add(entity);
        }

        public void Render(Vector2 playerCoords, Screen screen)
        {
            //Offset to center screen
            Vector2 screenOffset = Coordinate.IsoToNormal(new Vector2((screen.GetWidth() / 2), (screen.GetHeight() / 2)));

            //Calculate offset
            int xScroll = (int)Math.Round(playerCoords.X - screenOffset.X);
            int yScroll = (int)Math.Round(playerCoords.Y - screenOffset.Y);

            //Set offset
            screen.SetOffset(xScroll, yScroll);

            //Setting render mode to isometric
            screen.SetRenderMode(iso);

            //Render tiles
            CanvasBitmap sprite;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                   //Render
                    sprite = Sprite.GetSprite(map[x, y]);
                    if (sprite != null)
                    {
                        screen.RenderRectangle(x * tileSize, y * tileSize, tileSize, sprite);
                    }
                }
            }
            //Render entities
            entities.ForEach(entity => entity.Render(screen));
            //TODO: Render walls
            
        }

        public void Update()
        {
            //Update entities
            entities.ForEach(entity => entity.Update());
        }

        public bool TileCollision(int xChange, int yChange, int width, int height)
        {
            bool solid = false;
            for (int i = 0; i < 4; i++)
            {
                // i % 2 and i >> 1 creates all 4 corners of a tile (0,0) (0,1) (1,0) (1,1)
                int xFuture = (xChange + (i % 2) * width) / tileSize;
                int yFuture = (yChange + (i >> 1) * height) / tileSize;
                Tile tile = GetTile(xFuture, yFuture);
                if (tile == null) continue;
                else if (tile.IsSolid()) solid = true;
            }
            return solid;
        }
        Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight) return null;
            return Tile.GetTile(map[x, y]);
        }
    }
}
