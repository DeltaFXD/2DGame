using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Networking;
using System.Diagnostics;

using GameEngine.Networking.Packets;

namespace GameEngine.Networking
{
    class Client
    {
        string IP { get; set; }
        string Port { get; set; }
        bool running = false;
        List<Packet> send_buffer = new List<Packet>();
        List<Packet> receive_buffer = new List<Packet>();

        public Client(string ip, string port)
        {
            IP = ip;
            Port = port;
        }

        public async void StartClient()
        {
            if (!running)
            {
                try
                {
                    //Create the DatagramSocket and establish a connection to the server.
                    var socket = new DatagramSocket();

                    //The ConnectionReceived event is raised when connections are received.
                    socket.MessageReceived += MessageReceived;

                    // The server hostname that we will be establishing a connection to.
                    var hostName = new HostName(IP);

                    Debug.WriteLine("Client is about to bind...");

                    await socket.BindServiceNameAsync(Port);

                    Debug.WriteLine("Client is bound to port number " + Port);
                    running = true;
                }
                catch (Exception e)
                {
                    SocketErrorStatus status = SocketError.GetStatus(e.GetBaseException().HResult);
                    Debug.WriteLine(status.ToString() + " : " + e.Message);
                }

                // Send a request to the echo server.
                /*string request = "Hello, World!";
                using (var serverDatagramSocket = new Windows.Networking.Sockets.DatagramSocket())
                {
                    using (Stream outputStream = (await serverDatagramSocket.GetOutputStreamAsync(hostName, DatagramSocketPage.ServerPortNumber)).AsStreamForWrite())
                    {
                        using (var streamWriter = new StreamWriter(outputStream))
                        {
                            await streamWriter.WriteLineAsync(request);
                            await streamWriter.FlushAsync();
                        }
                    }
                }*/
            }
            else
            {
                throw new ArgumentException("Client already running!");
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
