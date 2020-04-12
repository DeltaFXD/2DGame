using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

using GameEngine.Interfaces;
using GameEngine.Entities;
using GameEngine.Graphics;
using GameEngine.Utilities;
using System.Threading;
using GameEngine.Entities.Mobs;

namespace GameEngine.Levels
{
    class Level : IUpdateable
    {
        Stopwatch watch = new Stopwatch();
        List<Entity> entities = new List<Entity>();
        //protected Action mapLoading;

        protected Map map;

        /// <summary>
        /// Level inicializalas
        /// </summary>
        public void Init()
        {
            watch.Start();
        }

        public void InitAStar()
        {
            int[,] graph_map = new int[map.GetWidth(), map.GetHeight()];
            for (int y = 0; y < map.GetHeight(); y++)
            {
                for (int x = 0; x < map.GetWidth(); x++)
                {
                    Tile tile = map.GetTile(x, y);
                    if (tile == null)
                    {
                        graph_map[x, y] = -1;
                        continue;
                    }
                    if (tile.IsSolid())
                        graph_map[x, y] = -1;
                    else
                        graph_map[x, y] = 1;
                }
            }
            AStar.Initialize(graph_map, map.GetWidth(), map.GetHeight());
        }

        public bool TileCollisionForParticles(double x, double y, int size)
        {
            throw new NotImplementedException();
        }

        //TODO: implement
        public List<Mob> GetMobs()
        {
            throw new NotImplementedException();
        }

        public void AddEntity(Entity entity)
        {
            entity.Initalize(this);
            entities.Add(entity);
            if (map != null)
            {
                map.AddEntity(entity);
            }
        }

        public void Render(Vector2 playerCoords, Screen screen)
        {
            if (map == null) return;
            //Render map
            map.Render(playerCoords, screen);
        }

        public void Update()
        {
            //Remove entities from map first
            if (map != null)
            {
                entities.ForEach(entity =>
                {
                    if (entity.IsRemoved()) map.RemoveEntity(entity);
                });
            }
            //Remove entities
            entities.RemoveAll(entity => entity.IsRemoved());

            //Update entities
            entities.ForEach(entity => entity.Update());

            //Update map after entities
            if (map != null) map.UpdateSectors(entities);
        }

        public bool TileCollision(int xChange, int yChange, int width, int height)
        {
            bool solid = false;
            if (map == null) return solid;
            for (int i = 0; i < 4; i++)
            {
                // i % 2 and i >> 1 creates all 4 corners of a tile (0,0) (0,1) (1,0) (1,1)
                int xFuture = (xChange + (i % 2) * width);
                int yFuture = (yChange + (i >> 1) * height);
                if (0 > xFuture && xFuture > -Map.tileSize)
                {
                    xFuture = -1;
                }
                else
                {
                    xFuture /= Map.tileSize;
                }
                if (0 > yFuture && yFuture > -Map.tileSize)
                {
                    yFuture = -1;
                }
                else
                {
                    yFuture /= Map.tileSize;
                }
                Tile tile = map.GetTile(xFuture, yFuture);
                if (tile == null) solid = true;
                else if (tile.IsSolid()) solid = true;
            }
            return solid;
        }
    }
}
