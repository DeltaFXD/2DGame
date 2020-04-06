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

using GameEngine.Entities;

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

        List<Sector> sectors = new List<Sector>();

        public Map(int width, int height, int[,] floor, string sector_data)
        {
            _width = width;
            _height = height;
            _floor = floor;

            LoadSectors(sector_data);
            Sector.SetMap(this);
        }

        void LoadSectors(string sector_data)
        {
            sectors.Add(new Sector(0, 0, 0, 11, 8, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 0 }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 0, 0, 4, 4, 4, 4 }));
            sectors.Add(new Sector(0, 0, 1, 11, 8, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 0 }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 0, 0, 4, 4, 4, 4 }));

            sectors.Add(new Sector(11, 0, 0, 13, 8, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0 }, null, new int[] { 3, 3, 3, 3, 0, 0, 0, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4 }));
            sectors.Add(new Sector(11, 0, 1, 13, 8, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0 }, null, new int[] { 3, 3, 3, 3, 0, 0, 0, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4 }));

            sectors.Add(new Sector(24, 0, 0, 27, 8, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4 }));
            sectors.Add(new Sector(24, 0, 1, 27, 8, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4 }));

            sectors.Add(new Sector(0, 8, 0, 11, 5, null, new int[] { 4, 4, 4, 4, 0 }, new int[] { 3, 3, 3, 3, 0, 0, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4 }));
            sectors.Add(new Sector(0, 8, 1, 11, 5, null, new int[] { 4, 4, 4, 4, 0 }, new int[] { 3, 3, 3, 3, 0, 0, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4 }));

            sectors.Add(new Sector(0, 13, 0, 11, 6, null, new int[] { 4, 4, 4, 4, 4, 0 }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 0, 0, 4 }));
            sectors.Add(new Sector(0, 13, 1, 11, 6, null, new int[] { 4, 4, 4, 4, 4, 0 }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 0, 0, 4 }));

            sectors.Add(new Sector(0, 19, 0, 11, 19, null, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0 }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4 }));
            sectors.Add(new Sector(0, 19, 1, 11, 19, null, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0 }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 4 }));

            sectors.Add(new Sector(11, 8, 0, 29, 19, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 4, 4, 4, 4, 4, 4 }));
            sectors.Add(new Sector(11, 8, 1, 29, 19, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 4, 4, 4, 4, 4, 4 }));

            sectors.Add(new Sector(11, 27, 0, 29, 11, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 }));
            sectors.Add(new Sector(11, 27, 1, 29, 11, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 }));

            sectors.Add(new Sector(40, 8, 0, 11, 30, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 }));
            sectors.Add(new Sector(40, 8, 1, 11, 30, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 }));

            sectors.Add(new Sector(0, 38, 0, 17, 13, null, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 0, 0, 0, 4, 4, 4, 4 }));
            sectors.Add(new Sector(0, 38, 1, 17, 13, null, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 0, 0, 0, 4, 4, 4, 4 }));

            sectors.Add(new Sector(17, 38, 0, 22, 13, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 0, 0, 4, 4, 4, 4, 4, 0, 0, 0, 4 }));
            sectors.Add(new Sector(17, 38, 1, 22, 13, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 0, 0, 4, 4, 4, 4, 4, 0, 0, 0, 4 }));

            sectors.Add(new Sector(39, 38, 0, 12, 6, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4 }));
            sectors.Add(new Sector(39, 38, 1, 12, 6, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4 }));

            sectors.Add(new Sector(39, 44, 0, 12, 7, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4 }));
            sectors.Add(new Sector(39, 44, 1, 12, 7, null, null, new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, new int[] { 4, 4, 4, 4, 4, 4, 4 }));
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

            //Render sectors
            sectors.ForEach(sector => sector.Render(playerXY, screen));
        }

        public void AddEntity(Entity entity)
        {
            int entityX = (int)entity.GetX() / tileSize;
            int entityY = (int)entity.GetY() / tileSize;

            foreach (Sector sector in sectors)
            {
                if (sector.IsInside(entityX, entityY))
                {
                    sector.AddEntity(entity);
                    break;
                }
            }
        }

        public void RemoveEntity(Entity entity)
        {
            sectors.ForEach(sector => sector.RemoveEntity(entity));
        }

        public void UpdateSectors(List<Entity> entities)
        {
            entities.ForEach(entity =>
            {
                bool inside = false;
                foreach (Sector sector in sectors)
                {
                    if (sector.Contains(entity))
                    {
                        inside = true;
                        int entityX = (int)entity.GetX() / tileSize;
                        int entityY = (int)entity.GetY() / tileSize;
                        if (!sector.IsInside(entityX, entityY)) {
                            sector.RemoveEntity(entity);
                            AddEntity(entity);
                        } 
                    }
                }
                if (!inside)
                {
                    AddEntity(entity);
                }
            });
        }
    }
}
