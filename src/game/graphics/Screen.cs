using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace GameEngine
{
    class Screen
    {
        int width, height;
        int xOffset, yOffset;
        static Rect sprite_base = new Rect(0, 0, 32, 32);
        CanvasDrawingSession cds = null;

        public Screen(int width, int height)
        {
            this.width = width;
            this.height = height;
            xOffset = 0;
            yOffset = 0;
        }

        public void setCDS(CanvasDrawingSession cds)
        {
            this.cds = cds;
        }

        public void renderRectangle(float xPos, float yPos, int spriteSize, CanvasBitmap sprite)
        {
            if (cds == null) return;
            //Boundary check
            if ((-spriteSize * 5) > (xPos - xOffset) || width < (xPos - xOffset) || (-spriteSize * 20) > (yPos - yOffset) || height < (yPos - yOffset)) return;
            //Draw
            cds.DrawImage(sprite, xPos - xOffset, yPos - yOffset, sprite_base, 1, CanvasImageInterpolation.NearestNeighbor);
        }

        public void renderRectangle(Vector2 pos,int spriteSize, CanvasBitmap sprite)
        {
            renderRectangle(pos.X, pos.Y, spriteSize, sprite);
        }

        public void setOffset(int xOffset, int yOffset)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }
    }
}
