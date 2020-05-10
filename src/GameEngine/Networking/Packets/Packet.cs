using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace GameEngine.Networking.Packets
{
    abstract class Packet
    {
        public Code Code { get; private set; }

        public Packet(Code code)
        {
            Code = code;
        }

        public abstract void WriteData(BinaryWriter dataWriter);

        public abstract void ConstructPacket(DataReader dataReader);
    }
}
