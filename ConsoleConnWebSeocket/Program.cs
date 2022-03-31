using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleConnWebSeocket
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("8==={======> X <======}===8");
            Console.WriteLine("[][][][] PROGRAM - 02 [][][][]");
            Console.WriteLine("press enter to cont .....");
            Console.ReadLine();
            using(ClientWebSocket client = new ClientWebSocket())
            {
                Uri serverUri = new Uri("ws://localhost:5200/send");
                //Uri serverUri = new Uri("ws://127.0.0.1:7890/EchoAll");
                var cts = new CancellationTokenSource();
                try
                {
                    await client.ConnectAsync(serverUri, cts.Token);
                    while(client.State == WebSocketState.Open)
                    {
                        Console.WriteLine("enter message to send");
                        string message = Console.ReadLine();
                        if (!string.IsNullOrEmpty(message))
                        {
                            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                            await client.SendAsync(byteToSend, WebSocketMessageType.Text, true, cts.Token);
                            var responseBuffer = new byte[1024];
                            var offset = 0;
                            var packet = 1024;
                            while (true)
                            {
                                ArraySegment<byte> byteRecieved = new ArraySegment<byte>(responseBuffer, offset, packet);
                                WebSocketReceiveResult response = await client.ReceiveAsync(byteRecieved, cts.Token);

                                var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                                Console.WriteLine(responseMessage);
                                if (response.EndOfMessage)
                                    break;
                            }
                        }
                    }
                }
                catch (WebSocketException e)
                { Console.WriteLine(e.Message); }
            }

            Console.ReadLine();
        }
    }
}
