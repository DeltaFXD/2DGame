using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

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

        /// <summary>
        /// Level inicializalas
        /// </summary>
        public void init()
        {
            tileSize = 32;
            watch.Start();
        }

        public void addEntity(Entity entity)
        {
            entity.initalize(this);
            entities.Add(entity);
        }

        public void Render(int xScroll, int yScroll, Screen screen)
        {
            screen.setOffset(xScroll, yScroll);
            //Render tiles
            CanvasBitmap sprite;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                   //Draw
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
    }
}
