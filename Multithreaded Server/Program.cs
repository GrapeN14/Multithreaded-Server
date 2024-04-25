using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MultiThreadServer
{
    class ExampleTcpListener
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                int MaxThreadsCount = Environment.ProcessorCount * 4;
                ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
                ThreadPool.SetMinThreads(2, 2);

                int port = 8888;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                int counter = 0;
                server = new TcpListener(localAddr, port);
                Console.OutputEncoding = Encoding.GetEncoding(866);
                Console.WriteLine("Конфигурация многопоточного сервера:");
                Console.WriteLine("IP-адрес: 127.0.0.1");
                Console.WriteLine("Порт: " + port.ToString());
                Console.WriteLine("Потоки: " + MaxThreadsCount.ToString());
                Console.WriteLine("\nСервер запущен\n");

                server.Start();

                while (true)
                {
                    Console.Write("Ожидание соединения... ");
                    ThreadPool.QueueUserWorkItem(ClientProcessing, server.AcceptTcpClient());
                    counter++;
                    Console.WriteLine("Соединение №" + counter.ToString() + "!");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\nНажмите Enter...");
            Console.Read();
        }

        static void ClientProcessing(object client_obj)
        {
            byte[] bytes = new byte[256];
            string data = null;
            TcpClient client = (TcpClient)client_obj;

            data = null;
            NetworkStream stream = client.GetStream();
            int i;

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, i);
                data = data.ToUpper();
                byte[] msg = Encoding.ASCII.GetBytes(data);
                stream.Write(msg, 0, msg.Length);
            }

            client.Close();
        }
    }
}
