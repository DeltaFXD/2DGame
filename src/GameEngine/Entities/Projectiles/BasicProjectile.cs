using GameEngine.Entities.Mobs;
using GameEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Entities.Projectiles
{
    class BasicProjectile : Projectile
    {

        public BasicProjectile(Vector2 origin, double angle, Mob owner) : base(origin, angle, owner)
        { 
            //TODO
        }
        public override void Render(Screen screen)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        protected override void Move()
        {
            throw new NotImplementedException();
        }
    }
}
