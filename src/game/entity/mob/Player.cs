using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    class Player : Entity
    {

        public Player(int x , int y)
        {
            this.x = x;
            this.y = y;
        }

        public override void update()
        {

        }

        public override void Render(Screen screen)
        {
            throw new NotImplementedException();
        }
    }
}
