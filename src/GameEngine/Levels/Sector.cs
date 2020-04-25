using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

using GameEngine.Graphics;
using GameEngine.Utilities;
using GameEngine.Entities;
using GameEngine.Entities.Mobs;
using System;

namespace GameEngine.Levels
{
    class Sector
    {
        static Map _map;

        readonly int _x, _y;
        readonly int _width, _height;
        bool final = false;

        List<Entity> entities = new List<Entity>();
        List<Wall> walls = new List<Wall>();

        public Sector(int x, int y,int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public void Render(Vector2 playerXY, Screen screen)
        {
            if (!final) return;

            //Store old offset
            Vector2 old_offset = screen.GetOffset();

            //Caculate new offset for entities
            Vector2 entity_offset = new Vector2(-screen.GetWidth() / 4, -screen.GetHeight() / 4);
            entity_offset += Coordinate.NormalToIso(playerXY) / 2;

            //Sort entities
            entities.Sort(Entity.CompareByDistance);
            
            int w = 0;
            int e = 0;
            while ((w < walls.Count || e < entities.Count))
            {
                if (w != walls.Count)
                {
                    if (e != entities.Count)
                    {
                        if (walls[w].IsBehind(entities[e].GetX(), entities[e].GetY()))
                        {
                            if (!(entities[e] is Mob))
                            {
                                //Set render mode to iso
                                screen.SetRenderMode(RenderMode.Isometric);

                                //Render
                                entities[e].Render(screen);

                                e++;
                            }
                            else
                            {
                                //Set new offset for entities
                                screen.SetOffset(entity_offset);

                                //Set render mode to normal2X
                                screen.SetRenderMode(RenderMode.Normal2X);

                                //Render
                                entities[e].Render(screen);

                                e++;

                                //Set old offset
                                screen.SetOffset(old_offset);
                            }
                        }
                        else
                        {
                            walls[w].Render(playerXY, screen);
                            w++;
                        }
                    }
                    else
                    {
                        walls[w].Render(playerXY, screen);
                        w++;
                    }
                }
                else
                {
                    if (!(entities[e] is Mob))
                    {
                        //Set render mode to iso
                        screen.SetRenderMode(RenderMode.Isometric);

                        //Render
                        entities[e].Render(screen);

                        e++;
                    }
                    else
                    {
                        //Set new offset for entities
                        screen.SetOffset(entity_offset);

                        //Set render mode to normal2X
                        screen.SetRenderMode(RenderMode.Normal2X);

                        //Render
                        entities[e].Render(screen);

                        e++;

                        //Set old offset
                        screen.SetOffset(old_offset);
                    }
                }
            }

            //Set old offset
            screen.SetOffset(old_offset);
        }

        public void AddWall(Wall wall)
        {
            if (final) throw new ArgumentException("Tried to add wall to finalised Sector");
            walls.Add(wall);
            wall.AddSectorOffset(_x, _y);
        }

        public void Finalise()
        {
            if (!final)
            {
                walls.Sort();
                final = true;
            }
        }

        public void AddEntity(Entity entity)
        { 
            if (entities.Contains(entity)) return;

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
            if (x >= _x && y >= _y && x < (_x + _width) && y < (_y + _height))
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
