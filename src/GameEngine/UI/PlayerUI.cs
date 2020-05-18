using GameEngine.Entities.Mobs;
using GameEngine.Graphics;
using GameEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace GameEngine.UI
{
    class PlayerUI : IRenderable , IUpdateable
    {
        static int HP_bar_width = 200;
        Player Player { get; set; }
        Rect HP_bar = new Rect(0, 0, HP_bar_width, 20);
        Rect HP_Frame = new Rect(0, 0, 208, 28);

        public PlayerUI(Player player)
        {
            Player = player;
        }

        public void Update()
        {
            if (Player.GetHP() < 0)
            {
                HP_bar.Width = 0;
            }
            else
            {
               HP_bar.Width = Player.GetHP() / (double)Player.GetMaxHP() * HP_bar_width;
            }
        }

        public void Render(Screen screen)
        {
            screen.SetRenderMode(RenderMode.Normal);
            float x = screen.GetWidth() / 20.0f;
            float y = screen.GetHeight() * 0.9f;
            screen.RenderSprite(x - 4, y - 4, HP_Frame, AnimatedSprite.GetAnimatedSprite("hpFrame").GetSprite(), false);
            screen.RenderSprite(x, y, HP_bar, AnimatedSprite.GetAnimatedSprite("hpBar").GetSprite(), false);
        }
    }
}
