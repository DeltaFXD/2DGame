using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Entities.Mobs
{
    abstract class Mob : Entity
    {
        protected int direction = 2;
        protected int maxHP = 100;
        protected int currentHP = 100;

        protected bool moving = false;

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
