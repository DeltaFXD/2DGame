using System.Numerics;

using GameEngine.Entities.Projectiles;

namespace GameEngine.Entities.Mobs
{
    abstract class Mob : Entity
    {
        protected int direction = 2;
        protected int maxHP = 100;
        protected int currentHP = 100;

        protected bool moving = false;

		MobType killer = MobType.NONE;

		protected Mob() : base()
		{

		}

        protected void Move(int xChange, int yChange)
        {
			if (xChange != 0 && yChange != 0)
			{
				Move(xChange, 0);
				Move(0, yChange);
				return;
			}

			if (xChange > 0) direction = 1;
			if (xChange < 0) direction = 3;
			if (yChange > 0) direction = 2;
			if (yChange < 0) direction = 0;

			if (!Collision(xChange, yChange))
			{
				position.X += xChange;
				position.Y += yChange;
			}
		}

		public int GetHP()
		{
			return currentHP;
		}

		public bool Damaged(int damage)
		{
			currentHP -= damage;
			if (currentHP <= 0) return true;
			return false;
		}

		public void SetKiller(MobType type)
		{
			if (killer == MobType.NONE) killer = type;
		}

		protected void Shoot(Vector2 projectileXY, double angle)
		{
			level.AddEntity(new BasicProjectile(projectileXY, 16, angle, this));
		}

		public abstract bool IsHit(float x, float y, HitBox hitbox);

		public abstract MobType GetMobType();

		protected void CheckHP()
		{
			if (currentHP > maxHP) currentHP = maxHP;
		}

		public void IsDead()
		{
			if (currentHP <= 0) Remove();
		}

		bool Collision(int xChange, int yChange)
		{
			return level.TileCollision((int) position.X + xChange, (int) position.Y + yChange, 31, 31);
		}
    }
}
