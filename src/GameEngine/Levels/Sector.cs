using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

using GameEngine.Graphics;
using GameEngine.Utilities;
using GameEngine.Entities;

namespace GameEngine.Levels
{
    class Sector
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
        static Matrix3x2 normal = new Matrix3x2(2.0f, 0.0f, 0.0f, 2.0f, 0.0f, 0.0f);

        static Map _map;

        readonly bool north, east, west, south;
        readonly int _x, _y, _z;
        readonly int _width, _height;
        int[] _northWall, _eastWall, _westWall, _southWall;

        List<Entity> entities = new List<Entity>();

        public Sector(int x, int y,int z, int width, int height, int[] northWall, int[] westWall, int[] eastWall, int[] southWall)
        {
            _x = x;
            _y = y;
            _z = z;
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
                    if (_northWall[i] == 0) continue;
                    screen.RenderRectangle((i + _x - _y) * Map.tileSize + offset.Y, (-_y + _z) * Map.tileSize + 2 * offset.Y, Map.tileSize, Sprite.GetSprite(_northWall[i]));
                }
            }
            if (west)
            {
                screen.SetRenderMode(verzicalIso);
                for (int i = 0; _height > i; i++)
                {
                    if (_westWall[i] == 0) continue;
                    screen.RenderRectangleSpecialBounds((-_x + _z) * Map.tileSize + 2 * offset.X, (_y + i - _x) * Map.tileSize + offset.X, Map.tileSize, Sprite.GetSprite(_westWall[i]));
                }
            }
            if (_z == 0)
            {
                //Store old offset
                Vector2 old_offset = screen.GetOffset();

                //Caculate new offset for entities
                Vector2 entity_offset = new Vector2(-screen.GetWidth() / 4, -screen.GetHeight() / 4);
                entity_offset += Coordinate.NormalToIso(playerXY) / 2;

                //Set new offset for entities
                screen.SetOffset(entity_offset);

                //Set render mode to isometric
                screen.SetRenderMode(normal);

                //Render entities inside sector
                entities.ForEach(entity => entity.Render(screen));

                //Set old offset
                screen.SetOffset((int)old_offset.X, (int)old_offset.Y);
            }
            if (east)
            {
                float opacity;
                screen.SetRenderMode(horizontalIso);
                for (int i = 0; _width > i; i++)
                {
                    if (_eastWall[i] == 0) continue;
                    opacity = 1.0f;
                    int isoX = _x + i;
                    int isoY = _y + _height - 1;
                    if (playerXY.X / Map.tileSize > (isoX - 3) && playerXY.X / Map.tileSize < (isoX + 3) && playerXY.Y / Map.tileSize > (isoY - 3) && playerXY.Y / Map.tileSize < (isoY + 3)) opacity = 0.5f;
                    //if (_map.GetTile(isoX, isoY)== null) opacity = 1.0f;
                    screen.RenderRectangle((i + _x - _height - _y) * Map.tileSize + offset.Y, (-_y -_height + _z) * Map.tileSize + 2 * offset.Y, Map.tileSize, Sprite.GetSprite(_eastWall[i]), opacity);
                }
            }
            if (south)
            {
                float opacity;
                screen.SetRenderMode(verzicalIso);
                for (int i = 0; _height > i; i++)
                {
                    if (_southWall[i] == 0) continue;
                    opacity = 1.0f;
                    int isoX = _x + _width - 1;
                    int isoY = _y + i;
                    if (playerXY.X / Map.tileSize > (isoX - 3) && playerXY.X / Map.tileSize < (isoX + 3) && playerXY.Y / Map.tileSize > (isoY - 3) && playerXY.Y / Map.tileSize < (isoY + 3)) opacity = 0.5f;
                    //if (_map.GetTile(isoX, isoY) == null) opacity = 1.0f;
                    screen.RenderRectangleSpecialBounds((-_x - _width + _z)* Map.tileSize + 2 * offset.X, (_y + i - _x - _width) * Map.tileSize + offset.X, Map.tileSize, Sprite.GetSprite(_southWall[i]), opacity);
                }
            }
        }

        public void AddEntity(Entity entity)
        {
            if (entities.Contains(entity) || _z != 0) return;

            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            if (entities.Contains(entity))
            {
                entities.Remove(entity);
            }
        }

        public bool IsInside(int x, int y)
        {
            if (x >= _x && y >= _y && x <= (_x + _width) && y <= (_y + _height) && _z == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(Entity entity)
        {
            return entities.Contains(entity);
        }

        public static void SetMap(Map map)
        {
            _map = map;
        }
    }
}
