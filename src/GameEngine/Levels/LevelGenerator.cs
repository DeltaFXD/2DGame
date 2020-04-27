﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Levels
{
    class LevelGenerator : Level
    {

        Random random;
        public LevelGenerator(int seed, int size)
        {
            random = new Random(seed);
            Generate(size);
        }

        int NextGaussian(int mean, int sigma)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double r = mean + sigma * Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return (int)Math.Round(r);
        }

        void Generate(int size)
        {
            List<Room> rooms = new List<Room>();

            //Start position
            int s = random.Next(4);

            int sx = s % 2 * (size - 16);
            int sy = (s >> 1) * (size - 16);

            Room start = new Room(sx, sy, 16, 16);

            rooms.Add(start);

            //Generate overlapping random rooms
            for (int i = 0; i < (size * size / 20); i++)
            {
                int x = random.Next(size);
                int y = random.Next(size);
                int w = NextGaussian(20, 10);
                int h = NextGaussian(20, 10);
                if (w < 10 || h < 10) continue;
                if ((x + w) < size && (y + h) < size)
                {
                    Room room = new Room(x, y, w, h);
                    if (!start.IsInside(room)) rooms.Add(room);
                }
            }
            Debug.WriteLine("Generated overlapping rooms");

            //Filter out overlapping rooms based on size
            int c = rooms.Count;
            Debug.WriteLine("Generated: " + c + " rooms");
            FilterRooms(rooms);
            c -= rooms.Count;
            Debug.WriteLine("Filtered out: " + c + " rooms");

            //Create map
            CreateMap(rooms, size);
            Debug.WriteLine("Map created");

            //Generate sectors
            Sector sector = new Sector(0, 0, size, size);
            sector.Finalise();
            map.AddSector(sector);
            CreateSectors(rooms);
            Debug.WriteLine("Sectors created");
        }

        void FilterRooms(List<Room> rooms)
        {
            //Start from 1 because 0(start room) prevented adding ones that would overlap
            int i = 1;
            int removed = 0;
            while (true)
            {
                if (i < rooms.Count)
                {
                    List<Room> temp = new List<Room>();
                    Room r = rooms[i];
                    foreach (Room room in rooms)
                    {
                        if (r.Id != room.Id)
                        {
                            if (r.IsInside(room))
                            {
                                if ((r.Width * r.Height) > (room.Height * room.Width))
                                {
                                    temp.Add(room);
                                    removed++;
                                }
                                else
                                {
                                    temp.Add(r);
                                    i--;
                                    removed++;
                                    break;
                                }
                            }
                        }
                    }
                    temp.ForEach(room => rooms.Remove(room));
                    i++;
                }
                else
                {
                    Debug.WriteLine("Filtering stopped after: " + i + " iteration, removed: " + removed + " rooms");
                    break;
                }
            }
            if (removed != 0)
            {
                FilterRooms(rooms);
            }
        }

        void CreateMap(List<Room> rooms, int size)
        {
            int[,] floor = new int[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    floor[x, y] = 0;
                }
            }

            //Fill floor with tiles
            rooms.ForEach(room =>
            {
                for (int y = room.Y;y < (room.Y+room.Height); y++)
                {
                    for (int x = room.X; x< (room.X+room.Width);x++)
                    {
                        int r = random.Next(100);
                        if (r < 80) floor[x, y] = 2;
                        else floor[x, y] = 6;
                    }
                }
            });

            map = new Map(size, size, floor);
        }

        void CreateSectors(List<Room> rooms)
        {
            rooms.ForEach(room =>
            {
                int[] hor = new int[room.Width];
                int[] hor_2 = new int[(room.Width + 2)];
                int[] hor_NT = new int[(room.Width + 2)];
                int[] hor_ST = new int[(room.Width + 2)];
                int[] ver = new int[room.Height];
                int[] ver_WT = new int[room.Height];
                int[] ver_ET = new int[room.Height];
                int[] ver_2 = new int[(room.Height + 2)];

                hor_NT[0] = 18;
                hor_ST[0] = 16;
                hor_2[0] = 7;
                for (int i = 1; i < (room.Width + 1); i++)
                {
                    hor_NT[i] = 11;
                    hor_ST[i] = 15;
                    hor_2[i] = 3;
                }
                hor_NT[room.Width + 1] = 12;
                hor_ST[room.Width + 1] = 14;
                hor_2[room.Width + 1] = 9;
                for (int i = 0; i < room.Width;i++)
                {
                    hor[i] = 3;
                    
                }
                for (int i = 0; i < room.Height;i++)
                {
                    ver[i] = 4;
                    ver_WT[i] = 17;
                    ver_ET[i] = 13;
                }
                ver_2[0] = 10;
                for (int i = 1; i < (room.Height + 1); i++)
                {
                    ver_2[i] = 4;
                }
                ver_2[room.Height + 1] = 8;

                Sector sector = new Sector(room.X, room.Y, room.Width, room.Height);

                sector.AddWall(new Wall(0.0f, 0.0f, 0, room.Width, hor, WallOrientation.Horizontal));
                sector.AddWall(new Wall(0.0f, 0.0f, 1, room.Width, hor, WallOrientation.Horizontal));
                sector.AddWall(new Wall(-1.0f, -1.0f, 2, room.Width + 2, hor_NT, WallOrientation.HorizontalTop));

                sector.AddWall(new Wall(0.0f, 0.0f, 0, room.Height, ver, WallOrientation.Vertical));
                sector.AddWall(new Wall(0.0f, 0.0f, 1, room.Height, ver, WallOrientation.Vertical));
                sector.AddWall(new Wall(-1.0f, 0.0f, 2, room.Height, ver_WT, WallOrientation.VerticalTop));

                sector.AddWall(new Wall(-1.0f, room.Height + 0.5f, 0, room.Width + 2, hor_2, WallOrientation.Horizontal));
                sector.AddWall(new Wall(-1.0f, room.Height + 0.5f, 1, room.Width + 2, hor_2, WallOrientation.Horizontal));
                sector.AddWall(new Wall(-1.0f, room.Height, 2, room.Width + 2, hor_ST, WallOrientation.HorizontalTop));

                sector.AddWall(new Wall(room.Width + 0.5f, -1.0f, 0, room.Height + 2, ver_2, WallOrientation.Vertical));
                sector.AddWall(new Wall(room.Width + 0.5f, -1.0f, 1, room.Height + 2, ver_2, WallOrientation.Vertical));
                sector.AddWall(new Wall(room.Width, 0.0f, 2, room.Height, ver_ET, WallOrientation.VerticalTop));

                sector.Finalise();

                map.AddSector(sector);
            });
        }
    }
}
