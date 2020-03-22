using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Numerics;

namespace GameEngine
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
        public void init()
        {
            tileSize = 32;
            iso = new Matrix3x2(2.828f, 1.414f, -2.828f, 1.414f, 0.0f, 0.0f);
            watch.Start();
        }

        public void addEntity(Entity entity)
        {
            entity.initalize(this);
            entities.Add(entity);
        }

        public void Render(Vector2 playerCoords, Screen screen)
        {
            //Offset to center screen
            Vector2 screenOffset = Coordinate.isoToNormal(new Vector2((screen.getWidth() / 2), (screen.getHeight() / 2)));

            //Calculate offset
            int xScroll = (int)Math.Round(playerCoords.X - screenOffset.X);
            int yScroll = (int)Math.Round(playerCoords.Y - screenOffset.Y);

            //Set offset
            screen.setOffset(xScroll, yScroll);

            //Setting render mode to isometric
            screen.SetRenderMode(iso);

            //Render tiles
            CanvasBitmap sprite;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                   //Render
                    sprite = Sprite.getSprite(map[x, y]);
                    if (sprite != null)
                    {
                        screen.renderRectangle(x * tileSize, y * tileSize, tileSize, sprite);
                    }
                }
            }
            //Render entities
            entities.ForEach(entity => entity.render(screen));
            //TODO: Render walls
            
        }

        public void update()
        {
            //Update entities
            entities.ForEach(entity => entity.update());
        }

        public bool TileCollision(int xChange, int yChange, int width, int height)
        {
            bool solid = false;
            for (int i = 0; i < 4; i++)
            {
                int xFuture = (xChange + (i % 2) * width) >> 5;
                int yFuture = (yChange + (i >> 1) * height) >> 5;
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
