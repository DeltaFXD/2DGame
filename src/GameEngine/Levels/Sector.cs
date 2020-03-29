using System;
using System.Diagnostics;
using System.Numerics;

using GameEngine.Graphics;
using GameEngine.Utilities;

namespace GameEngine.Levels
{
    class Sector
    {
        /* 
         * FIXED
         */
        static Matrix3x2 horizontalIso = new Matrix3x2(2 * Coordinate.root2, Coordinate.root2, 0.0f, -2 * Coordinate.root2, 0.0f, 0.0f);
        /* 
         *
         */
        static Matrix3x2 verzicalIso = new Matrix3x2(0.0f, -2 * Coordinate.root2, -2 * Coordinate.root2, Coordinate.root2, 0.0f, 0.0f);

        bool north, east, west, south;
        int _x, _y;
        int _width, _height;
        int[] _northWall, _eastWall, _westWall, _southWall;

        public Sector(int x, int y, int width, int height, int[] northWall, int[] westWall, int[] eastWall, int[] southWall)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;

            //North - Észak
            if (northWall == null)
            {
                north = false;
            }
            else
            {
                north = true;
                _northWall = northWall;
            }
            //West - Nyugat
            if (westWall == null)
            {
                west = false;
            }
            else
            {
                west = true;
                _westWall = westWall;
            }
            //East - Kelet
            if (eastWall == null)
            {
                east = false;
            }
            else
            {
                east = true;
                _eastWall = eastWall;
            }
            //South - Dél
            if (southWall == null)
            {
                south = false;
            }
            else
            {
                south = true;
                _southWall = southWall;
            }
        }

        public void Render(Vector2 playerXY, Screen screen)
        {
            Vector2 offset = screen.GetOffset();
            if (north)
            {
                screen.SetRenderMode(horizontalIso);
                for (int i = 0; _width > i; i++)
                {
                    //Debug.WriteLine(playerXY.Y);
                    screen.RenderRectangle((i + _x - _y) * Map.tileSize + offset.Y, -_y * Map.tileSize + 2 * offset.Y, Map.tileSize, Sprite.GetSprite(_northWall[i]));
                }
            }
            if (west)
            {
                screen.SetRenderMode(verzicalIso);
                for (int i = 0; _height > i; i++)
                {
                    screen.RenderRectangleSpecialBounds(-_x * Map.tileSize + 2 * offset.X, (_y + i -_x) * Map.tileSize + offset.X, Map.tileSize, Sprite.GetSprite(_westWall[i]));
                }
            }
            /*
             TODO render entity
             */
            if (east)
            {
                screen.SetRenderMode(horizontalIso);
                for (int i = 0; _width > i; i++)
                {
                    screen.RenderRectangle((i + _x - _height - _y) * Map.tileSize + offset.Y, (-_y -_height) * Map.tileSize + 2 * offset.Y, Map.tileSize, Sprite.GetSprite(_eastWall[i]), 0.5f);
                }
            }
            if (south)
            {
                screen.SetRenderMode(verzicalIso);
                for (int i = 0; _height > i; i++)
                {
                    screen.RenderRectangleSpecialBounds((-_x - _width)* Map.tileSize + 2 * offset.X, (_y + i - _x - _width) * Map.tileSize + offset.X, Map.tileSize, Sprite.GetSprite(_westWall[i]), 0.5f);
                }
            }
        }
    }
}
