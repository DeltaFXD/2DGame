using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace GameEngine.Networking.Packets
{
    class Connected : Packet
    {
        public Connected() : base(Code.Connected)
        {

        }

        public override void ConstructPacket(DataReader dataReader)
        {

        }

        public override void WriteData(BinaryWriter writer)
        {
            writer.Write((int)Code);
        }
    }
}
