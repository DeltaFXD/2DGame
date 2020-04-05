using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace GameEngine.Graphics
{
    class Screen
    {
        int width, height;
        float xOffset, yOffset;
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

        public void SetCDS(CanvasDrawingSession cds)
        {
            this.cds = cds;
        }

        public void RenderRectangle(float xPos, float yPos, int spriteSize, CanvasBitmap sprite, float opacity = 1.0f)
        {
            if (cds == null) return;
            //Boundary check
            if (0 > (xPos - xOffset + spriteSize) || drawWidth < (xPos - xOffset - spriteSize) || -drawHeightAbs > (yPos - yOffset + spriteSize) || drawHeightAbs < (yPos - yOffset)) return;
            //Draw
            cds.DrawImage(sprite, xPos - xOffset, yPos - yOffset, sprite_base, opacity, CanvasImageInterpolation.NearestNeighbor);
        }


        //TODO: Recheck bounds for this
        public void RenderEntity(Vector2 pos, int spriteSize, CanvasBitmap sprite, float opacity = 1.0f)
        {
            if (cds == null) return;
            //Boundary check
            if (0 > (pos.X - xOffset + spriteSize) || width / 2 < (pos.X - xOffset - spriteSize) || -drawHeightAbs > (pos.Y - yOffset + spriteSize) || height / 2 < (pos.Y - yOffset)) return;
            //Draw
            cds.DrawImage(sprite, pos.X - xOffset - spriteSize / 2, pos.Y - yOffset, sprite_base, opacity, CanvasImageInterpolation.NearestNeighbor);
        }
        public void RenderRectangleSpecialBounds(float xPos, float yPos, int spriteSize, CanvasBitmap sprite, float opacity = 1.0f)
        {
            if (cds == null) return;
            //Boundary check
            if (-(drawWidth + spriteSize) > (xPos - xOffset + spriteSize)) return;
            if (0 < (xPos - xOffset - spriteSize)) return;
            if (-(drawWidth + spriteSize) > (yPos - yOffset)) return;
            if (0 < (yPos - yOffset)) return;
            //Draw
            cds.DrawImage(sprite, xPos - xOffset, yPos - yOffset, sprite_base, opacity, CanvasImageInterpolation.NearestNeighbor);
        }

        public void RenderRectangle_NOCHECK(float xPos, float yPos, int spriteSize, CanvasBitmap sprite)
        {
            if (cds == null) return;
            //Boundary check
            if (0 > (xPos - xOffset + spriteSize)) Debug.WriteLine("X NEGATIVE");
            if (drawWidth < (xPos - xOffset - spriteSize)) Debug.WriteLine("X POZITIVE");
            if (-drawHeightAbs > (yPos - yOffset)) Debug.WriteLine("Y NEGATIVE");
            if (drawHeightAbs < (yPos - yOffset)) Debug.WriteLine("Y POZITIVE");
            //Draw
            cds.DrawImage(sprite, xPos - xOffset, yPos - yOffset, sprite_base, 1, CanvasImageInterpolation.NearestNeighbor);
        }

        public void RenderRectangle(Vector2 pos,int spriteSize, CanvasBitmap sprite)
        {
            RenderRectangle(pos.X, pos.Y, spriteSize, sprite);
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public void SetOffset(int xOffset, int yOffset)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
        }

        public void SetOffset(Vector2 offset)
        {
            xOffset = offset.X;
            yOffset = offset.Y;
        }

        public Vector2 GetOffset()
        {
            return new Vector2(xOffset, yOffset);
        }
    }
}
