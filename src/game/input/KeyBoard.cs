using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;

namespace GameEngine
{
    class KeyBoard
    {
        public bool up, down, left, right, reload;
        public void update(VirtualKey vk, bool value)
        {
            switch (vk)
            {
                case VirtualKey.W: up = value; break;
                case VirtualKey.S: down = value; break;
                case VirtualKey.A: left = value; break;
                case VirtualKey.D: right = value; break;
                case VirtualKey.F5: reload = value; break;
            }
        }
    }
}
