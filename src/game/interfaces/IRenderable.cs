using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;

namespace GameEngine
{
    interface IRenderable
    {
        /// <summary>
        /// Ezen methodussal tud rajzolni a canvas-re
        /// </summary>
        /// <param name="screen">Screen to render on</param>
        void render(Screen screen);
    }
}
