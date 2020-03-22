using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace GameEngine
{
    class Level : IUpdateable
    {
        protected int mapWidth, mapHeight;
        int tileSize;
        protected int[,] map;
        Stopwatch watch = new Stopwatch();
        List<Entity> entities = new List<Entity>();
        Matrix3x2 iso;
        Matrix3x2 normal;

        /// <summary>
        /// Level inicializalas
        /// </summary>
        public void init()
        {
            tileSize = 32;
            iso = new Matrix3x2(2.828f, 1.414f, -2.828f, 1.414f, 0.0f, 0.0f);
            normal = new Matrix3x2(2.0f, 0.0f, 0.0f, 2.0f, 0.0f, 0.0f);
            watch.Start();
        }

        public void addEntity(Entity entity)
        {
            entity.initalize(this);
            entities.Add(entity);
        }

        public void render(Vector2 playerCoors, Screen screen)
        {
            //-----RENDERING TILES-----
            //Offset to center screen
            Vector2 screenOffset = Coordinate.isoToNormal(new Vector2((screen.getWidth() / 2), (screen.getHeight() / 2)));

            //Calculate offset
            int xScroll = (int) Math.Round(playerCoors.X - screenOffset.X);
            int yScroll = (int) Math.Round(playerCoors.Y - screenOffset.Y);

            //Set offset
            screen.setOffset(xScroll, yScroll);
            
            //Setting render mode to isometric
            screen.setRenderMode(iso);

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
            //-----RENDERING TILES END-----

            //-----RENDERING ENTITIES-----
            Vector2 playerInIso = Coordinate.normalToIso(playerCoors);
            //Calculate new offset
            xScroll = (int) Math.Round(playerInIso.X -screen.getWidth() / 4);
            yScroll = (int) Math.Round(playerInIso.Y -screen.getHeight() / 4);

            //Set new offset
            screen.setOffset(xScroll, yScroll);
            
            //Setting render mode to normal
            screen.setRenderMode(normal);

            //Render entities
            entities.ForEach(entity => entity.render(screen));
            //-----RENDERING ENTITIES END-----

            //-----RENDERING WALLS-----
            //Calculate offset
            xScroll = (int)Math.Round(playerCoors.X - screenOffset.X);
            yScroll = (int)Math.Round(playerCoors.Y - screenOffset.Y);

            //Set offset
            screen.setOffset(xScroll, yScroll);

            //Setting render mode to isometric
            screen.setRenderMode(iso);

            //Rendering walls

            //TODO ADD

            //-----RENDERING WALLS END-----
        }

        public void update()
        {
            //Update entities
            entities.ForEach(entity => entity.update());
        }
    }
}
