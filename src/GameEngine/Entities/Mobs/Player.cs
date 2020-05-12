using System;

using GameEngine.Inputs;
using GameEngine.Graphics;
using GameEngine.Utilities;
using GameEngine.Entities.Projectiles;
using System.Numerics;

namespace GameEngine.Entities.Mobs
{
    class Player : Mob
    {
        static MobType type = MobType.PLAYER;
        static HitBox _hitBox = new HitBox(20, 32, 12, 0);

        KeyBoard input;
        protected int fireRate;

        protected Player(float x, float y) : base()
        {
            position.X = x;
            position.Y = y;
            fireRate = 0;
        }
        public Player(float x , float y, KeyBoard input) : base()
        {
            position.X = x;
            position.Y = y;
            this.input = input;
            fireRate = 0;
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
        }

        private void UpdateShooting()
        {
            if (Mouse.GetButton() == Mouse.Button.Left && fireRate <= 0)
            {
                Vector2 vec2 = Mouse.GetIsoCoordinate() - position;
                double angle = Math.Atan2(vec2.Y, vec2.X);
                Shoot(position , angle);
                fireRate = BasicProjectile.GetRateOfFire();
            }
        }

        public override void Render(Screen screen)
        {
            screen.RenderEntity(Coordinate.NormalToIso(position) / 2, 32, AnimatedSprite.GetAnimatedSprite("player").GetSprite());
        }

        public override bool IsHit(float x, float y, HitBox hitbox)
        {
            return _hitBox.IsInside(x - position.X, y - position.Y, hitbox);
        }

        public override MobType GetMobType()
        {
            return type;
        }
    }
}
