using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using System.Diagnostics;
using System.Numerics;

using GameEngine.Graphics;
using GameEngine.Utilities;
using GameEngine.Inputs;
using GameEngine.Entities.Projectiles;
using System;
using GameEngine.Levels;

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
        int fireRate;
        int pathfinding_cooldown = 0;
        int penality = 1;

        public int Ammo { get; private set; }

        public Dummy(float x, float y) : base()
        {
            position.X = x;
            position.Y = y;
            Ammo = 100;
            fireRate = 0;
        }

        public override void Update()
        {
            CheckHP();
            IsDead();
            int xChange = 0;
            int yChange = 0;
            if (fireRate > 0) fireRate--;
            //AI HERE
            if (pathfinding_cooldown > 0) pathfinding_cooldown--;
            level.GetPlayers().ForEach(player =>
            {
                float dist = (player.GetX() - position.X) * (player.GetX() - position.X) + (player.GetY() - position.Y) * (player.GetY() - position.Y);
                if (dist < (Map.tileSize * Map.tileSize * 8))
                {
                    UpdateShooting(player.GetXY() - position);
                } else if (!_hasPath && dist < (Map.tileSize * Map.tileSize * 16 * 16) && level.InLineOfSight((int)position.X, (int)position.Y, (int)player.GetX(), (int)player.GetY())) {
                    _path = new List<Vector2>
                    {
                        new Vector2(player.GetX() / Map.tileSize, player.GetY() / Map.tileSize)
                    };
                    _hasPath = true;
                    penality = 1;
                }
                else if (!_hasPath && pathfinding_cooldown <= 0 && dist < (Map.tileSize * Map.tileSize * 16 * 16))
                {
                    pathfinding_cooldown = 60 * penality;
                    penality++;
                    _path = AStar.FindPath((int)position.X / 32, (int)position.Y / 32, (int)player.GetX() / 32, (int)player.GetY() / 32);
                    if (_path != null)
                    {
                        _hasPath = true;
                        penality = 1;
                    }
                }
            });

            if (_hasPath)
            {
                Vector2 currentDest = _path[_pathIndex];
                currentDest *= Map.tileSize;
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
                        //Debug.WriteLine("Destination reached");
                        pathfinding_cooldown = 0;
                        _path = null;
                        _pathIndex = 0;
                        _hasPath = false;
                    }/* else
                        Debug.WriteLine("Next node" + _path[_pathIndex]);*/
                }
            }

            prev_moving = moving;
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
            UpdateSprite();
        }

        private void UpdateShooting(Vector2 vec2)
        {
            if (fireRate <= 0 && Ammo != 0)
            {
                double angle = Math.Atan2(vec2.Y, vec2.X);
                Shoot(position + new Vector2(28, 28), angle);
                fireRate = BasicProjectile.GetRateOfFire();
                Ammo--;
            }
        }

        void UpdateSprite()
        {
            if (previous_direction == direction && prev_moving == moving)
            {
                return;
            }
            else
            {
                switch (direction)
                {
                    case Direction.East:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            break;
                        }
                    case Direction.NorthEast:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            break;
                        }
                    case Direction.North:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            break;
                        }
                    case Direction.NorthWest:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            break;
                        }
                    case Direction.West:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            break;
                        }
                    case Direction.SouthWest:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            break;
                        }
                    case Direction.South:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            break;
                        }
                    case Direction.SouthEast:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("testmob");
                            }
                            break;
                        }
                }
            }
        }
        public override void Render(Screen screen)
        {
            if (sprite == null) sprite = AnimatedSprite.GetAnimatedSprite("testmob");
            screen.RenderEntity(Coordinate.NormalToIso(position) / 2, renderBox, sprite.GetSprite());
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
