using Windows.System;

namespace GameEngine.Inputs
{
    class KeyBoard
    {
        public bool up, down, left, right, reload;
        public void Update(VirtualKey vk, bool value)
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
