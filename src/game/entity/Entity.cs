using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    abstract class Entity : IRenderable , IUpdateable
    {
        protected Vector2 position;
        protected Level level;
        bool removed = false;

        public bool isRemoved()
        {
            return removed;
        }

        public void initalize(Level level)
        {
            this.level = level;
        }

        protected void remove()
        {
            removed = true;
        }

        public float getX()
        {
            return position.X;
        }

        public float getY()
        {
            return position.Y;
        }

        public Vector2 getXY()
        {
            return position;
        }

        public abstract void render(Screen screen);
        public abstract void update();
    }
}
