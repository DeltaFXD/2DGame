using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    class Player : Entity
    {
        KeyBoard input;
        public Player(float x , float y, KeyBoard input)
        {
            position.X = x;
            position.Y = y;
            this.input = input;
        }

        public override void update()
        {
            int xChange = 0;
            int yChange = 0;
            if (input.up)
            {
                yChange--;
                xChange--;
            }
            if (input.down)
            {
                yChange++;
                xChange++;
            }
            if (input.left)
            {
                xChange--;
                yChange++;
            }
            if (input.right)
            {
                xChange++;
                yChange--;
            }

            position.X += xChange;
            position.Y += yChange;
        }

        public override void render(Screen screen)
        {
            screen.renderRectangle(Coordinate.normalToIso(position), 32, Sprite.getSprite(2));
        }
    }
}
