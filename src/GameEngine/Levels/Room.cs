using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Levels
{
    class Room
    {
        static int next = 0;
        public int Id { get; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
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
            int xTL = X - 2;
            int yTL = Y - 2;
            int xBR = X + Width + 2;
            int yBR = Y + Height + 2;

            int oXBR = other.X + other.Width;
            int oYBR = other.Y + other.Height;

            if (xTL > oXBR || yTL > oYBR) return false;

            if (xBR < other.X || yBR < other.Y) return false;

            return true;
        }
    }
}
