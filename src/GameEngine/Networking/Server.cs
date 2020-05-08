using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Diagnostics;

using GameEngine.Networking.Packets;

namespace GameEngine.Networking
{
    class Server
    {
        string Port { get; set; }
        bool running = false;
        List<Packet> send_buffer = new List<Packet>();
        List<Packet> receive_buffer = new List<Packet>();

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
                    var socket = new DatagramSocket();

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

        void MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            using (DataReader dataReader = args.GetDataReader())
            {
                //request = dataReader.ReadString(dataReader.UnconsumedBufferLength).Trim();
            }
        }

        public void Send(Packet packet)
        {
            
        }

        public Packet GetNextReceived()
        {
            throw new NotImplementedException();
        }
    }
}
