using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace SocketTests
{
    public static class Class1
    {
        private static bool _live;

        public static void StartClient()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes("I am writing a much larger test to see what happens hopefully it is larger than 1024 bytes (It wasn't)");

                    // Send the data through the socket.  w5
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void StartForCrypto()
        {
            _live = true;
            var bytes = new byte[1024];

            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                var ipHostInfo = Dns.GetHostEntry("https://api.crypto.com/v2");
                var ipAddress = ipHostInfo.AddressList[0];
                var remoteEP = new IPEndPoint(ipAddress, 80);

                // Create a TCP/IP  socket.  
                var sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint);

                    var data = "";

                    while (_live)
                    {
                        int bytesRec = sender.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (sender.Available <= 0)
                        {
                            Console.WriteLine(data);
                            data = "";
                        }
                    }

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void StopForCrypto()
        {
            _live = false;
        }

        public static async void Web()
        {
            var x = new ClientWebSocket();
            await x.ConnectAsync(new Uri("wss://stream.crypto.com/v2/market"), CancellationToken.None);

            var mem = new Memory<byte>();
            var y = await x.ReceiveAsync(mem, CancellationToken.None);
            Console.WriteLine(Encoding.ASCII.GetString(mem.ToArray()));
            //https://exchange-docs.crypto.com/spot/index.html?csharp#websocket-subscriptions
            //https://docs.microsoft.com/en-us/dotnet/api/system.net.websockets.clientwebsocket?view=net-6.0
            //todo remember you gotta use websockets to subscribe
        }
    }
}
