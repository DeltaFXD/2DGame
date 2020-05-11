namespace GameEngine.Entities
{
    class HitBox
    {

        int _width;
        int _height;
        int _x;
        int _y;

        // Offset from x and y
        public HitBox(int width, int height, int xoffset, int yoffset)
        {
            _width = width + xoffset;
            _height = height + yoffset;
            _x = xoffset;
            _y = yoffset;
        }

        public bool IsInside(float x, float y)
        {
            if (_x < x && x < _width && _y < y && y < _height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsInside(float x, float y, HitBox hitbox)
        {
            float xt = x + hitbox._x;
            float yt = y + hitbox._y;
            float wt = x + hitbox._width;
            float ht = y + hitbox._height;
            if (((_x < xt && xt < _width) || (_x < wt && wt < _width)) && ((_y < yt && yt < _height) || (_y < ht && ht < _height)))
            {
                return true;
            }
            else if (((xt < _x && _x < wt) || (xt < _width && _width < wt)) && ((yt < _y && _y < ht) || (yt < _height && _height < ht)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
