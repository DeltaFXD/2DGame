using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;

namespace GameEngine
{
    interface Renderable
    {
        /// <summary>
        /// Ezen methodussal tud rajzolni a canvas-re
        /// </summary>
        /// <param name="cds">Canvas felulet amire lehet rajzolni</param>
        void Render(CanvasDrawingSession cds);
    }
}
