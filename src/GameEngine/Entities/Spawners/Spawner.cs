using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Entities.Spawners
{
    abstract class Spawner : Entity
    {
        public enum Type
        {
            PARTICLE,
            MOB
        }

        float _z;

        protected int _amount;
        Type _type;

        public Spawner(float x, float y,float z, int amount, Type type)
        {
            position.X = x;
            position.Y = y;
            _z = z;
            _amount = amount;
            _type = type;
        }

        public Type GetSpawnerType()
        {
            return _type;
        }
    }
}
