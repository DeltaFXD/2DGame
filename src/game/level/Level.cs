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
        int[,] map = new int[10, 10];

        /// <summary>
        /// Level inicializalas
        /// </summary>
        public void init()
        {
            //Temp
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    map.SetValue(1, x, y);
                }
            }
        }

        public void Render(CanvasDrawingSession cds)
        {
            //Super Basic Map Render
            CanvasBitmap bitmap = null;
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
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
