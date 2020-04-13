using GameEngine.Entities.Mobs;
using GameEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using GameEngine.Entities.Spawners;
using GameEngine.Utilities;

namespace GameEngine.Entities.Projectiles
{
    class BasicProjectile : Projectile
    {
        static readonly int rateOfFire = 10;
        static readonly int particleLife = 30;
        static readonly int particleAmount = 30;
        static readonly int xOffsetSize = 8;
        static readonly int yOffsetSize = 8;
        static readonly int projectileSize = 16;
        static Sprite sprite = Sprite.GetSprite(Sprite.GetSpriteID("basic_projectile"));

        static readonly HitBox hitBox = new HitBox(16, 16, 4, 0);

        public BasicProjectile(Vector2 origin,float z, double angle, Mob owner) : base(origin, z, angle, owner)
        {
            _range = 320;
            _speed = 2;
            _damage = 10;
            changeXY.X = (float)(_speed * Math.Cos(angle));
            changeXY.Y = (float)(_speed * Math.Sin(angle));
        }

        public static int GetRateOfFire()
        {
            return rateOfFire;
        }

        public override void Update()
        {
            Move();
        }

        protected override void Move()
        {
            if (level.TilePenetration(position + changeXY, projectileSize, xOffsetSize, yOffsetSize))
            {
                position += changeXY;
            } 
            else
            {
                if (changeXY.X < 0 && changeXY.Y < 0)
                {
                    level.AddEntity(new ParticleSpawner(position.X, position.Y, _z, particleLife, particleAmount, "particle_normal"));
                } 
                else if (changeXY.X > 0 && changeXY.Y > 0)
                {
                    level.AddEntity(new ParticleSpawner(position.X, position.Y, _z, particleLife, particleAmount, "particle_normal"));
                }
                else if (changeXY.X < 0 && changeXY.Y > 0)
                {
                    level.AddEntity(new ParticleSpawner(position.X, position.Y, _z, particleLife, particleAmount, "particle_normal"));
                }
                else if (changeXY.X > 0 && changeXY.Y < 0)
                {
                    level.AddEntity(new ParticleSpawner(position.X, position.Y, _z, particleLife, particleAmount, "particle_normal"));
                }
                Remove();
            }
            if (Distance() > _range) Remove();
            if (EntityCollision(position.X, position.Y, hitBox))
            {
                Remove();
                level.AddEntity(new ParticleSpawner(position.X, position.Y, _z, particleLife, particleAmount, "particle_red"));
            }
        }
        public override void Render(Screen screen)
        {
            Vector2 vec2 = Coordinate.VirtualZAxisReduction(position, _z);
            screen.RenderProjectile(vec2.X, vec2.Y, sprite, _angle);
        }
    }
}
