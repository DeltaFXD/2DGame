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
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public int CenterX { get; }
        public int CenterY { get; }
        public Room Parent { get; }
        public Room(int x, int y, int width, int height, Room parent = null)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Id = next;
            next++;
            Parent = parent;
            CenterX = x + width / 2;
            CenterY = y + height / 2;
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
