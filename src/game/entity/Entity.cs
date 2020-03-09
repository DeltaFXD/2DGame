using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    abstract class Entity : IRenderable , IUpdateable
    {
        protected int x, y;
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

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public abstract void Render(Screen screen);
        public abstract void update();
    }
}
