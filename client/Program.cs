using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NewClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("\nСоединение #" + i.ToString() + "\n");
                Connect("127.0.0.1", "HelloWorld! #" + i.ToString());
            }
            Console.WriteLine("\nНажмите Enter...");
            Console.Read();
        }

        static void Connect(string server, string message)
        {
            try
            {
                
                Int32 port = 8888;
                TcpClient client = new TcpClient(server, port);

               
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

              
                NetworkStream stream = client.GetStream();

              
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Отправлено: " + message);

               
                data = new Byte[256];
               
                String responseData = String.Empty;
              
                int bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Получено: " + responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}