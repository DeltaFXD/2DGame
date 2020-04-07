using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using GameEngine.Entities.Mobs;

namespace GameEngine.Entities.Projectiles
{
    abstract class Projectile : Entity
    {
		protected readonly Vector2 _origin;
		protected double _angle;
		protected float _changeXY;
		protected float _distance;
		protected double _speed, _range, _damage;
		protected Mob _owner;

		public Projectile(Vector2 origin, double angle, Mob owner)
		{
			position = origin;
			_owner = owner;
			_origin = origin;
			_angle = angle;
		}

		protected bool EntityCollision(float x, float y, HitBox hitbox)
		{
			bool collision = false;
			Mob mob = null;
			List<Mob> mobs = level.GetMobs();
			for (int i = 0; i < mobs.Count(); i++)
			{
				mob = mobs[i];
				if (mob.IsHit(x, y, hitbox) && mob.GetMobType() != _owner.GetMobType())
				{
					if (mob.Damaged((int)_damage)) mob.SetKiller(_owner.GetMobType());
					collision = true;
				}
			}
			return collision;
		}

		protected abstract void Move();
	}
}
