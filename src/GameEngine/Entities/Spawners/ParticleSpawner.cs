using GameEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Entities.Spawners
{
    class ParticleSpawner : Spawner
    {
        public ParticleSpawner(float x, float y,float z,int  lifeTime,int amount, string sprite_name) : base(x, y, z, amount, Type.PARTICLE)
        {
        }

        public override void Render(Screen screen)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
