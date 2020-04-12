﻿using GameEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameEngine.Utilities;

namespace GameEngine.Entities.Particles
{
    class Particle : Entity
    {
        static Random random = new Random();
        float _z;
        double xChange, yChange, zChange;
        int _lifeTime;
        int spriteID;

        public Particle(float x, float y, float z, int lifeTime, string sprite_name)
        {
            position.X = x;
            position.Y = y;
            _z = z;

            _lifeTime = random.Next(lifeTime);
            spriteID = Sprite.GetSpriteID(sprite_name);

            zChange = 0;

            xChange = NormalDistribution.NextGaussian();
            yChange = NormalDistribution.NextGaussian();
        }

        public override void Update()
        {
            _lifeTime--;
            if (_lifeTime < 0) Remove();

            zChange -= 0.1;
            if (_z < 0)
            {
                _z = 0;
                zChange *= -0.5;
                xChange *= 0.4;
                yChange *= 0.4;
            }

            Move();
        }

        void Move()
        {
            if (level.TileCollisionForParticles(position.X + xChange, position.Y , 10 /*tempt Sprite.GetSprite(spriteID).GetSize()*/))
            {
                xChange *= -1;
            }
            if (level.TileCollisionForParticles(position.X, position.Y + yChange, 10 /*tempt Sprite.GetSprite(spriteID).GetSize()*/))
            {
                yChange *= -1;
            }

            position.X += (float) xChange;
            position.Y += (float) yChange;
            _z += (float) zChange;
        }

        //TODO: implement screen.RenderSprite
        public override void Render(Screen screen)
        {
            //screen.RenderSprite(Coordinate.VirtualZAxisReduction(position, _z), Sprite.GetSprite(spriteID).GetSize());
        }
    }
}