using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    class Player : Entity
    {

        public Player(float x , float y)
        {
            position.X = x;
            position.Y = y;
        }

        public override void update()
        {

        }

        public override void render(Screen screen)
        {
            screen.renderRectangle(position, 32, Sprite.getSprite(2));
        }
    }
}
