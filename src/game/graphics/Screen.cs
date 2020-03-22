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
        int drawWidth = 480;
        int drawHeightAbs = 288;
        static Rect sprite_base = new Rect(0, 0, 32, 32);
        CanvasDrawingSession cds = null;

        public Screen(int width, int height)
        {
            this.width = width;
            this.height = height;
            xOffset = 0;
            yOffset = 0;
        }

        public void SetRenderMode(Matrix3x2 mode)
        {
            if (cds == null) return;
            cds.Transform = mode;
        }

        public void setCDS(CanvasDrawingSession cds)
        {
            this.cds = cds;
        }

        public void renderRectangle(float xPos, float yPos, int spriteSize, CanvasBitmap sprite)
        {
            if (cds == null) return;
            //Boundary check
            if (0 > (xPos - xOffset + spriteSize) || drawWidth < (xPos - xOffset - spriteSize) || -drawHeightAbs > (yPos - yOffset) || drawHeightAbs < (yPos - yOffset)) return;
            //Draw
            cds.DrawImage(sprite, xPos - xOffset, yPos - yOffset, sprite_base, 1, CanvasImageInterpolation.NearestNeighbor);
        }

        public void renderRectangle(Vector2 pos,int spriteSize, CanvasBitmap sprite)
        {
            renderRectangle(pos.X, pos.Y, spriteSize, sprite);
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public void setOffset(int xOffset, int yOffset)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }
    }
}
