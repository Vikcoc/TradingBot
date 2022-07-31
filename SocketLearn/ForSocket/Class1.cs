using System.Net.WebSockets;

namespace ForSocket
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public static class Class1
    {

        // Incoming data from the client.  
        public static string? Data;

        public static void StartListening()
        {
            // Data buffer for incoming data.  
            var bytes = new Byte[10];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    Data = null;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        Console.WriteLine(handler.Available);
                        Data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (handler.Available <= 0)
                        {
                            break;
                        }
                    }

                    // Show the data on the console.  
                    Console.WriteLine("Text received : {0}", Data);

                    // Echo the data back to the client.  
                    byte[] msg = Encoding.ASCII.GetBytes(Data);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public static async void Web()
        {
            var x = new ClientWebSocket();
            await x.ConnectAsync(new Uri("wss://stream.crypto.com/v2/market"), CancellationToken.None);

            var mem = new Memory<byte>();
            var y = await x.ReceiveAsync(mem, CancellationToken.None);
            Console.WriteLine(Encoding.ASCII.GetString(mem.ToArray()));
        }
    }
}