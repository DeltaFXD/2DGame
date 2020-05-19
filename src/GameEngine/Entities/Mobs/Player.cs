using System;

using GameEngine.Inputs;
using GameEngine.Graphics;
using GameEngine.Utilities;
using GameEngine.Entities.Projectiles;
using System.Numerics;
using Windows.Foundation;
using GameEngine.Levels;
using GameEngine.Entities.PickupAbles;

namespace GameEngine.Entities.Mobs
{
    class Player : Mob
    {
        static MobType type = MobType.PLAYER;
        static HitBox _hitBox = new HitBox(32, 64, 0, 0);
        static Rect renderBox = new Rect(0, 0, 32, 64);

        KeyBoard input;
        protected int fireRate;
        public int Ammo { get; private set; }

        protected Player(float x, float y) : base()
        {
            position.X = x;
            position.Y = y;
            fireRate = 0;
            Ammo = 50;
        }
        public Player(float x , float y, KeyBoard input) : base()
        {
            position.X = x;
            position.Y = y;
            this.input = input;
            fireRate = 0;
            Ammo = 50;
        }

        public override void Update()
        {
            //Debug.WriteLine(position);
            CheckHP();
            int xChange = 0;
            int yChange = 0;
            if (fireRate > 0) fireRate--;
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

            prev_moving = moving;
            if (xChange == 0 && yChange == 0)
            {
                moving = false;
            }

            if (xChange != 0 || yChange != 0)
            {
                moving = true;
                Move(xChange, yChange);
            }
            UpdateShooting();
            UpdateSprite();
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
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            break;
                        }
                    case Direction.NorthEast:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            break;
                        }
                    case Direction.North:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            break;
                        }
                    case Direction.NorthWest:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            break;
                        }
                    case Direction.West:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            break;
                        }
                    case Direction.SouthWest:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            break;
                        }
                    case Direction.South:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            break;
                        }
                    case Direction.SouthEast:
                        {
                            if (moving)
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            else
                            {
                                sprite = AnimatedSprite.GetAnimatedSprite("player");
                            }
                            break;
                        }
                }
            }
        }

        public void Consume(PickupAble pickup_able)
        {
            switch (pickup_able.Type)
            {
                case PickupType.Arrows:
                    {
                        Ammo += pickup_able.Value;
                        break;
                    }
                case PickupType.HP_Potion:
                    {
                        currentHP += pickup_able.Value;
                        CheckHP();
                        break;
                    }
                case PickupType.Mana_Potion:
                    {
                        break;
                    }
            }
        }

        private void UpdateShooting()
        {
            if (Mouse.GetButton() == Mouse.Button.Left && fireRate <= 0 && Ammo != 0)
            {
                Vector2 vec2 = Mouse.GetIsoCoordinate() - position;
                double angle = Math.Atan2(vec2.Y, vec2.X);
                Shoot(position + new Vector2(28, 28), angle);
                fireRate = BasicProjectile.GetRateOfFire();
                Ammo--;
            }
        }

        public override void Render(Screen screen)
        {
            if (sprite == null) sprite = AnimatedSprite.GetAnimatedSprite("player");
            screen.RenderEntity(Coordinate.NormalToIso(position) / 2, renderBox, sprite.GetSprite());
        }

        public bool IsWithin(Vector2 coord)
        {
            float xTL = position.X - 40 * Map.tileSize;
            float yTL = position.Y - 40 * Map.tileSize;
            float xBR = position.X + 40 * Map.tileSize;
            float yBR = position.Y + 40 * Map.tileSize;

            float x = coord.X;
            float y = coord.Y;

            if (x > xTL && x < xBR && y > yTL && y < yBR)
            {
                return true;
            }
            else
            {
                return false;
            }
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
