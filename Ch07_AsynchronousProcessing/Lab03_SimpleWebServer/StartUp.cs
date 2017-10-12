namespace Lab03_SimpleWebServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class StartUp
    {
        public static void Main()
        {
            int port = 1337;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ipAddress, port);
            tcpListener.Start();

            Console.WriteLine("Server started.");
            Console.WriteLine($"Listening to TCP Clients at 127.0.0.1:{port}");
            Console.WriteLine(new string('-', 4));

            Task
                .Run(async () => await Connect(tcpListener))
                .GetAwaiter()
                .GetResult();
        }

        private static async Task Connect(TcpListener listener)
        {
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                byte[] bufferRead = new byte[1024];
                await client.GetStream().ReadAsync(bufferRead, 0, bufferRead.Length);

                string clientMessage = Encoding.UTF8.GetString(bufferRead);
                Console.WriteLine(clientMessage.Trim('\0'));

                string responseMessage = "HTTP/1.1 200 OK\n\r" +
                                         "Content-Type: text/plain\n\r" +
                                         "\n\r" +
                                         "Hello from server!\n\r";
                byte[] bufferWrite = Encoding.UTF8.GetBytes(responseMessage);
                await client.GetStream().WriteAsync(bufferWrite, 0, bufferWrite.Length);
                //Console.WriteLine(responseMessage);

                client.Dispose();
            }
        }
    }
}
