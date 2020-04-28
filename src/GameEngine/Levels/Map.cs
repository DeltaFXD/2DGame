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

        public Map(int width, int height, int[,] floor)
        {
            _width = width;
            _height = height;
            _floor = floor;

            Sector.SetMap(this);
        }

        public void AddSector(Sector sector)
        {
            sectors.Add(sector);
        }

        void LoadSectors(string sector_data)
        {
            //TODO
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

            //Setting render mode to isometric
            screen.SetRenderMode(RenderMode.Isometric);

            //Render tiles
            Sprite sprite;
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    //Render
                    sprite = Sprite.GetSprite(_floor[x, y]);
                    if (sprite != null)
                    {
                        screen.RenderRectangle(x * tileSize, y * tileSize, tileSize, sprite.GetBitmap());
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
