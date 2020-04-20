using System;
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

        struct Room
        {
            static int next = 0;
            public int Id { get; }
            public int X { set; get; }
            public int Y { set; get; }
            public int Width { get; }
            public int Height { get; }

            public Room(int x, int y, int width, int height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Id = next;
                next++;
            }

            public bool IsInside(Room other)
            {
                int wt = other.X + other.Width;
                int ht = other.Y + other.Height;

                int tw = X + Width;
                int th = Y + Height;

                if (((X <= other.X && other.X <= tw) || (X <= wt && wt <= tw)) && ((Y <= other.Y && other.Y <= th) || (Y <= ht && ht <= th)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

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
        }

        void FilterRooms(List<Room> rooms)
        {
            //Start from 1 because 0(start room) prevented adding ones that would overlap
            int i = 1;
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
                                }
                                else
                                {
                                    temp.Add(r);
                                    i--;
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
                    Debug.WriteLine("Filtering stopped after: " + i + " iteration");
                    break;
                }
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
                        floor[x, y] = 10197916;
                    }
                }
            });

            map = new Map(size, size, floor);

            //Generate sectors
            map.AddSector(new Sector(0, 0, 0, size, size, null, null, null, null));
        }
    }
}
