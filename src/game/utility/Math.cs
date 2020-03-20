using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    class Math
    {
        public static Vector2 normalToIso(Vector2 vec2)
        {
            /*  Transformation matrix
             *  2*sqrt(2)   -2*sqrt(2)
             *  sqrt(2)     sqrt(2)
             */

            float x = vec2.X;
            float y = vec2.Y;

            vec2.X = (x * 2.828f - y * 2.828f);
            vec2.Y = (x * 1.414f + y * 1.414f);

            return vec2;
        }

        public static Vector2 isoToNormal(Vector2 vec2)
        {
            /*  Inverz Transformation matrix
             *  sqrt(2)/8   sqrt(2)/4
             *  -sqrt(2)/8     sqrt(2)/4
             */
            float x = vec2.X;
            float y = vec2.Y;

            vec2.X = (x * 0.177f + y * 0.353f);
            vec2.Y = (x * -0.177f + y * 0.353f);

            return vec2;
        }
    }
}
