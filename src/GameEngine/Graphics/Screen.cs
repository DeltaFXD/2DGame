using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

using GameEngine.Utilities;

namespace GameEngine.Graphics
{
    public enum RenderMode
    {
        Normal,
        Normal2X,
        Isometric,
        VerticalIso,
        HorizontalIso
    }
    class Screen
    {
        /* 
         * 
         */
        static Matrix3x2 horizontalIso = new Matrix3x2(2 * Coordinate.root2, Coordinate.root2, 0.0f, -2 * Coordinate.root2, 0.0f, 0.0f);
        /* 
         *
         */
        static Matrix3x2 verzicalIso = new Matrix3x2(0.0f, -2 * Coordinate.root2, -2 * Coordinate.root2, Coordinate.root2, 0.0f, 0.0f);
        /*  Isometric Transformation matrix
         *  2*sqrt(2)   -2*sqrt(2)  0
         *  sqrt(2)     sqrt(2)     0
         */
        static Matrix3x2 iso = new Matrix3x2(2 * Coordinate.root2, Coordinate.root2, -2 * Coordinate.root2, Coordinate.root2, 0.0f, 0.0f);
        /*  
         *  2   0   0
         *  0   2   0
         */
        static Matrix3x2 normal2x = new Matrix3x2(2.0f, 0.0f, 0.0f, 2.0f, 0.0f, 0.0f);
        /*  
         *  1   0   0
         *  0   1   0
         */
        static Matrix3x2 normal = new Matrix3x2(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f);

        int width, height;
        float xOffset, yOffset;
        int drawWidth = 480;
        int drawHeightAbs = 288;
        static Rect sprite_base = new Rect(0, 0, 32, 32);
        CanvasDrawingSession cds = null;

        public RenderMode Mode { get; private set; }

        public Screen(int width, int height)
        {
            this.width = width;
            this.height = height;
            xOffset = 0;
            yOffset = 0;
        }

        public void SetRenderMode(RenderMode mode)
        {
            if (cds == null) return;
            switch (mode)
            {
                case RenderMode.Normal:
                    cds.Transform = normal;
                    break;
                case RenderMode.Normal2X:
                    cds.Transform = normal2x;
                    break;
                case RenderMode.Isometric:
                    cds.Transform = iso;
                    break;
                case RenderMode.VerticalIso:
                    cds.Transform = verzicalIso;
                    break;
                case RenderMode.HorizontalIso:
                    cds.Transform = horizontalIso;
                    break;
            }
            Mode = mode;
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

        public void RenderProjectile(float xPos, float yPos, Sprite sprite, Matrix4x4 angle,int xOff, int yOff ,float opacity = 1.0f)
        {
            if (cds == null) return;
            //Boundary check
            if (0 > (xPos - xOffset + sprite.GetWidth()) || drawWidth < (xPos - xOffset - sprite.GetWidth()) || -drawHeightAbs > (yPos - yOffset + sprite.GetHeight()) || drawHeightAbs < (yPos - yOffset)) return;

            Rect rect = new Rect(0, 0, sprite.GetWidth(), sprite.GetHeight());
            //Draw
            cds.DrawImage(sprite.GetBitmap(), -xOff, -yOff, rect, opacity, CanvasImageInterpolation.NearestNeighbor, angle);
        }

        public void RenderMinimap(float xPos, float yPos, Rect window, CanvasBitmap minimap)
        {
            if (xPos < 0 || xPos > width || yPos < 0 || yPos > height) return;

            cds.DrawImage(minimap, xPos, yPos, window, 1.0f, CanvasImageInterpolation.NearestNeighbor);
        }

        public void RenderParticle(Vector2 pos, Sprite sprite, float opacity = 1.0f)
        {
            if (cds == null) return;
            //Boundary check
            if (0 > (pos.X - xOffset + sprite.GetWidth()) || drawWidth < (pos.X - xOffset - sprite.GetWidth()) || -drawHeightAbs > (pos.Y - yOffset + sprite.GetHeight()) || drawHeightAbs < (pos.Y - yOffset)) return;
            Rect rect = new Rect(0, 0, sprite.GetWidth(), sprite.GetHeight());
            //Draw
            cds.DrawImage(sprite.GetBitmap(), pos.X - xOffset, pos.Y - yOffset, rect, opacity, CanvasImageInterpolation.NearestNeighbor);
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
