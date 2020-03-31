using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using GameEngine.Graphics;
using GameEngine.Utilities;
using GameEngine.Inputs;
using Microsoft.Graphics.Canvas;
using System.Diagnostics;

namespace GameEngine.Levels
{
    class Map
    {
        /*  Transformation matrix
         *  2*sqrt(2)   -2*sqrt(2)  0
         *  sqrt(2)     sqrt(2)     0
         */
        static Matrix3x2 iso = new Matrix3x2(2 * Coordinate.root2, Coordinate.root2, -2 * Coordinate.root2, Coordinate.root2, 0.0f, 0.0f);
        public static readonly int tileSize = 32;
        readonly int _width;
        readonly int _height;
        readonly int[,] _floor;
        Vector2 test = new Vector2(0.0f, 0.0f);

        List<Sector> sectors = new List<Sector>();

        public Map(int width, int height, int[,] floor, string sector_data)
        {
            _width = width;
            _height = height;
            _floor = floor;

            LoadSectors(sector_data);
        }

        void LoadSectors(string sector_data)
        {
            sectors.Add(new Sector(0, 0, 0, 10, 7, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4 }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 0, 0, 4, 4, 4 }));
            sectors.Add(new Sector(0, 0, 1, 10, 7, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4 }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 0, 0, 4, 4, 4 }));
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height) return null;
            return Tile.GetTile(_floor[x, y]);
        }

        public void Render(Vector2 playerXY, Screen screen)
        {
            //Offset to center screen
            Vector2 screenOffset = Coordinate.IsoToNormal(new Vector2((screen.GetWidth() / 2), (screen.GetHeight() / 2)));

            //Calculate offset
            int xScroll = (int)Math.Round(playerXY.X - screenOffset.X);
            int yScroll = (int)Math.Round(playerXY.Y - screenOffset.Y);

            //Set offset
            screen.SetOffset(xScroll, yScroll);
            //screen.SetOffset(0, 0);

            //Setting render mode to isometric
            screen.SetRenderMode(iso);

            //Render tiles
            CanvasBitmap sprite;
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    //Render
                    sprite = Sprite.GetSprite(_floor[x, y]);
                    if (sprite != null)
                    {
                        screen.RenderRectangle(x * tileSize, y * tileSize, tileSize, sprite);
                    }
                }
            }
            if (KeyBoard.upArrow) test.Y += 1.0f;
            if (KeyBoard.downArrow) test.Y -= 1.0f;
            if (KeyBoard.rightArrow) test.X += 1.0f;
            if (KeyBoard.leftArrow) test.X -= 1.0f;
            //Debug.WriteLine("playerX: " + playerXY.X + " playerY: " + playerXY.Y + " offsetX: " + xScroll + " offsetY:" + yScroll + " myX: " + test.X + " myY: " + test.Y);
            //Render sectors
            sectors.ForEach(sector => sector.Render(test, screen));

            //TEMP
            //Setting render mode to isometric
            screen.SetRenderMode(iso);
        }
    }
}
