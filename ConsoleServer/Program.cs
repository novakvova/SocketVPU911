using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleServer
{
    class Program
    {
        static int port = 2895; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            String strHostName = string.Empty;
            // Getting Ip address of local machine...
            // First get the host name of local machine.
            strHostName = Dns.GetHostName();
            Console.WriteLine("Local Machine's Host Name: " + strHostName);
            // Then using host name, get the IP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            var addr = ipEntry.AddressList.ToList();
            addr.Add(IPAddress.Parse("127.0.0.1"));
            Console.WriteLine("Оберіть IP address");
            for (int i = 0; i < addr.Count; i++)
            {
                Console.WriteLine("{0}. {1} ", i+1, addr[i].ToString());
            }
            Console.Write("->_");
            int selectIP = int.Parse(Console.ReadLine());

            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(addr[selectIP-1], port);

            // создаем сокет
            Socket socketA = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, 
                ProtocolType.Tcp);

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                socketA.Bind(ipPoint);

                // начинаем прослушивание
                socketA.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    //socketSveta
                    //socketPetro
                    Socket handler = socketA.Accept();
                    var clientEndPoint = handler.RemoteEndPoint;
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    //Читмаємо відповідь від клієнта
                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0); //Працюємо доки не дойшли до кінця повідомлення
                    Console.WriteLine("IP: {0}", clientEndPoint);
                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    // отправляем ответ
                    string message = "ваше сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        //Console.WriteLine("Hello World!");
        }
    }
}
