using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace GameEngine.Networking.Packets
{
    class Pong : Packet
    {
        long _time;
        public Pong(long time) : base(Code.Pong)
        {
            _time = time;
        }

        public Pong() : base(Code.Pong)
        {
            _time = 0;
        }

        public long GetTime()
        {
            return _time;
        }
        public override void ConstructPacket(DataReader dataReader)
        {
            _time = dataReader.ReadInt64();
        }

        public override string GetData()
        {
            return "" + Code + _time;
        }
    }
}
