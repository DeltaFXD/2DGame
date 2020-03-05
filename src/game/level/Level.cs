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
    class Level : Renderable , Updateable
    {
        //Temp
        protected int mapWidth, mapHeight;
        //Map scrolling offset
        int xOffset, yOffset;
        //Maximum values for rendering boundaries
        int screenWidth, screenHeight;
        int tileSize;
        protected int[,] map;
        Rect sprite_base;
        Stopwatch watch = new Stopwatch();

        /// <summary>
        /// Level inicializalas
        /// </summary>
        public void init()
        {
            xOffset = 0;
            yOffset = 0;
            screenWidth = 1000;
            screenHeight = 500;
            tileSize = 32;
            sprite_base = new Rect(0, 0, 32, 32);

            watch.Start();
        }

        public void Render(CanvasDrawingSession cds)
        {
            //Super Basic Map Render
            CanvasBitmap sprite;
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    //Boundary Check
                    if ((-tileSize * 10) > (x * tileSize - xOffset) || screenWidth < (x * tileSize - xOffset) || (-tileSize * 20) > (y * tileSize - yOffset) || screenHeight < (y * tileSize - yOffset)) continue;
                    //Draw
                    sprite = Sprite.getSprite(map[x, y]);
                    if (sprite != null)
                    {
                        cds.DrawImage(sprite,x * tileSize - xOffset, y * tileSize - yOffset, sprite_base, 1,CanvasImageInterpolation.NearestNeighbor);
                    }
                }
            }
        }

        public void update()
        {
            //Test code
            //xOffset += Convert.ToInt32(2 * Math.Cos((watch.ElapsedMilliseconds / 2000 ) * (Math.PI / 180)) - 2 * Math.Sin((watch.ElapsedMilliseconds / 2000) * (Math.PI / 180)));
            //yOffset += Convert.ToInt32(2 * Math.Sin((watch.ElapsedMilliseconds / 2000 ) * (Math.PI / 180)) + 2 * Math.Cos((watch.ElapsedMilliseconds / 2000) * (Math.PI / 180)));
            xOffset = -120;
            yOffset = 120;
        }
    }
}
