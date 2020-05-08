using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Networking.Packets
{
    abstract class Packet
    {
        Code _code;

        public Packet(Code code)
        {
            _code = code;
        }

    }
}
