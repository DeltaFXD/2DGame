using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using GameEngine.Interfaces;
using GameEngine.Levels;
using GameEngine.Graphics;

namespace GameEngine.Entities
{
    abstract class Entity : IRenderable , IUpdateable, IComparable<Entity>
    {
        protected Vector2 position;
        protected Level level;
        bool removed = false;

        public bool IsRemoved()
        {
            return removed;
        }

        public void Initalize(Level level)
        {
            this.level = level;
        }

        protected void Remove()
        {
            removed = true;
        }

        public float GetX()
        {
            return position.X;
        }

        public float GetY()
        {
            return position.Y;
        }

        public Vector2 GetXY()
        {
            return position;
        }

        public abstract void Render(Screen screen);
        public abstract void Update();

        public int CompareTo(Entity other)
        {
            if (other == null) return -1;
            double mine = position.X * position.X + position.Y * position.Y;
            double others = other.position.X * other.position.X + other.position.Y * other.position.Y;
            if (mine > others)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
