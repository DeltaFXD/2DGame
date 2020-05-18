using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using System.Diagnostics;
using System.Numerics;

using GameEngine.Graphics;
using GameEngine.Utilities;
using GameEngine.Inputs;

namespace GameEngine.Entities.Mobs
{
    class Dummy : Mob
    {
        static MobType type = MobType.DUMMY;
        static HitBox _hitBox = new HitBox(20, 64, 4, 0);
        static Rect renderBox = new Rect(0, 0, 32, 64);

        List<Vector2> _path;
        bool _hasPath = false;
        int _pathIndex = 0;
        Vector2 _prev_position;

        public Dummy(float x, float y) : base()
        {
            position.X = x;
            position.Y = y;
        }

        public override void Update()
        {
            CheckHP();
            IsDead();
            int xChange = 0;
            int yChange = 0;

            //AI HERE

            /*if (Mouse.GetButton() == Mouse.Button.Left && !_hasPath)
            {
                Vector2 vec2 = Mouse.GetIsoCoordinate();
                _path = AStar.FindPath((int)position.X / 32, (int)position.Y / 32, (int)vec2.X / 32, (int)vec2.Y / 32);
                if (_path != null)
                {
                    _hasPath = true;
                    Debug.WriteLine("Next node" + _path[_pathIndex]);
                }
            }*/

            if (_hasPath)
            {
                Vector2 currentDest = _path[_pathIndex];
                currentDest *= 32;
                if (currentDest.X != position.X)
                {
                    if (currentDest.X > position.X)
                        xChange++;
                    else
                        xChange--;
                }
                if (currentDest.Y != position.Y)
                {
                    if (currentDest.Y > position.Y)
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
                    } else
                        Debug.WriteLine("Next node" + _path[_pathIndex]);
                }
            }

            if (xChange == 0 && yChange == 0)
            {
                moving = false;
            }
            _prev_position = position;
            if (xChange != 0 || yChange != 0)
            {
                moving = true;
                Move(xChange, yChange);
            }
        }
        public override void Render(Screen screen)
        {
            screen.RenderEntity(Coordinate.NormalToIso(position) / 2, renderBox, AnimatedSprite.GetAnimatedSprite("testmob").GetSprite());
        }

        public override bool IsHit(float x, float y)
        {
            return _hitBox.IsInside(x - position.X, y - position.Y);
        }

        public override MobType GetMobType()
        {
            return type;
        }
    }
}
