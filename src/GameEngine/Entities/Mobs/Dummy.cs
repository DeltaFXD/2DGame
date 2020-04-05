using GameEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameEngine.Utilities;
using GameEngine.Inputs;
using System.Diagnostics;
using System.Numerics;

namespace GameEngine.Entities.Mobs
{
    class Dummy : Mob
    {
        public Dummy(float x, float y)
        {
            position.X = x;
            position.Y = y;
        }

        public override void Update()
        {
            CheckHP();
            int xChange = 0;
            int yChange = 0;

            //AI HERE

            //TEST CODE
            if (KeyBoard.upArrow)
            {
                yChange--;
                xChange--;
            }
            if (KeyBoard.downArrow)
            {
                yChange++;
                xChange++;
            }
            if (KeyBoard.leftArrow)
            {
                xChange--;
                yChange++;
            }
            if (KeyBoard.rightArrow)
            {
                xChange++;
                yChange--;
            }
            //TEST CODE

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
            screen.RenderEntity(Coordinate.NormalToIso(position) / 2, 32, AnimatedSprite.GetAnimatedSprite("testmob").GetSprite());
        }
    }
}
