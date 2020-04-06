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
        List<Vector2> _path;
        bool _hasPath = false;
        int _pathIndex = 0;

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

            if (Mouse.GetButton() == Mouse.Button.Left && !_hasPath)
            {
                Vector2 vec2 = Mouse.GetIsoCoordinate();
                _path = AStar.FindPath((int)position.X / 32, (int)position.Y / 32, (int)vec2.X / 32, (int)vec2.Y / 32);
                if (_path != null) _hasPath = true;
            }

            if (_hasPath)
            {
                Vector2 currentDest = _path[_pathIndex];

                if (currentDest.X * 32 != position.X)
                {
                    if (currentDest.X * 32 > position.X)
                        xChange++;
                    else
                        xChange--;
                }
                if (currentDest.Y * 32 != position.Y)
                {
                    if (currentDest.Y * 32 > position.Y)
                        yChange++;
                    else
                        yChange--;
                }
                if (xChange == 0 && yChange == 0)
                {
                    _pathIndex++;
                    if (_pathIndex == _path.Count())
                    {
                        Debug.WriteLine("Destination reached");
                        _path = null;
                        _pathIndex = 0;
                        _hasPath = false;
                    }
                }
            }

            /*
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
            */
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
