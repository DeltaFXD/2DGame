using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace GameEngine.Networking.Packets
{
    class Ping : Packet
    {
        long _time;
        public Ping(long time) : base(Code.Ping)
        {
            _time = time;
        }

        public Ping() : base(Code.Ping)
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
