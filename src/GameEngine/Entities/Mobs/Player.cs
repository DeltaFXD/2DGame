using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameEngine.Inputs;
using GameEngine.Graphics;

namespace GameEngine.Entities.Mobs
{
    class Player : Mob
    {
        KeyBoard input;
        public Player(float x , float y, KeyBoard input)
        {
            position.X = x;
            position.Y = y;
            this.input = input;
        }

        public override void Update()
        {
            CheckHP();
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

            if (xChange == 0 && yChange == 0)
            {
                moving = false;
            }

            if (xChange != 0 || yChange != 0)
            {
                moving = true;
                Move(xChange, yChange);
            }
        }

        public override void Render(Screen screen)
        {
            screen.RenderRectangle(position, 32, AnimatedSprite.GetAnimatedSprite("player").GetSprite());
        }
    }
}
