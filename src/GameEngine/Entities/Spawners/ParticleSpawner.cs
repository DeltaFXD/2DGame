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
        public ParticleSpawner(float x, float y, int amount, Type type) : base(x, y, amount, type)
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
