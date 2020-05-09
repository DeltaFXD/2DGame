using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Diagnostics;

using GameEngine.Networking.Packets;
using System.IO;
using Windows.UI.Xaml.Documents;

namespace GameEngine.Networking
{
    class Server
    {
        string Port { get; set; }
        bool running = false;
        List<Packet> send_buffer = new List<Packet>();
        List<Packet> receive_buffer = new List<Packet>();
        DatagramSocket socket;

        public Server(string port)
        {
            Port = port;
        }

        public async void StartServer()
        {
            if (!running)
            {
                try
                {
                    socket = new DatagramSocket();

                    //The ConnectionReceived event is raised when connections are received.
                    socket.MessageReceived += MessageReceived;

                    Debug.WriteLine("Server is about to bind...");

                    //Start listening for incoming UDP connections on the specified port. You can specify any port that's not currently in use.
                    await socket.BindServiceNameAsync(Port);

                    Debug.WriteLine("Server is bound to port number " + Port);
                    running = true;
                }
                catch (Exception e)
                {
                    SocketErrorStatus status = SocketError.GetStatus(e.GetBaseException().HResult);
                    Debug.WriteLine(status.ToString() + " : " + e.Message);
                }
            }
            else
            {
                throw new ArgumentException("Server already running!");
            }
        }

        public async void Update()
        {
            if (running && send_buffer.Count != 0)
            {
                using (Stream output = (await socket.GetOutputStreamAsync(socket.Information.RemoteAddress, Port)).AsStreamForWrite())
                {
                    using (var writer = new StreamWriter(output))
                    {
                        while (send_buffer.Count != 0)
                        {
                            await writer.WriteLineAsync(send_buffer[0].Code + send_buffer[0].GetData());
                            send_buffer.RemoveAt(0);
                        }
                        await writer.FlushAsync();
                    }
                }
            }
        }

        void MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            Debug.WriteLine("kaptam packetet");
            using (DataReader dataReader = args.GetDataReader())
            {
                while (dataReader.UnconsumedBufferLength != 0)
                {
                    Code code = (Code)dataReader.ReadInt32();
                    Packet p;
                    switch (code) {
                        case Code.Ping:
                            {
                                p = new Ping();
                                p.ConstructPacket(dataReader);
                                break;
                            }
                        case Code.Pong:
                            {
                                p = new Pong();
                                p.ConstructPacket(dataReader);
                                break;
                            }
                        default: p = null; break;
                    }

                    if (p != null)
                    {
                        receive_buffer.Add(p);
                    }
                    else
                    {
                        Debug.WriteLine("Unrecognized packet " + code);
                        break;
                    }
                }
            }

            sender.Dispose();
        }

        public void Send(Packet packet)
        {
            if (running)
            {
                send_buffer.Add(packet);
            }
        }

        public Packet GetNextReceived()
        {
            if (!running) return null;
            if (receive_buffer.Count == 0) return null;

            Packet p = receive_buffer[0];
            receive_buffer.Remove(p);

            return p;
        }
    }
}
