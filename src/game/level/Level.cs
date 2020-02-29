using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    class Level : Renderable
    {
        //Temp
        int mapWidth, mapHeight;
        int[,] map;

        /// <summary>
        /// Level inicializalas
        /// </summary>
        public void init()
        {
            mapWidth = 10;
            mapHeight = 10;
            map = new int[mapWidth, mapHeight];
            //Temp
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    map.SetValue(1, x, y);
                }
            }
        }

        public void Render(CanvasDrawingSession cds)
        {
            //Super Basic Map Render
            CanvasBitmap bitmap = null;
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (Sprite.Sprites.TryGetValue(map[x, y], out bitmap))
                    {
                        cds.DrawImage(bitmap, x * 32, y * 32);
                    }
                }
            }
        }
    }
}
