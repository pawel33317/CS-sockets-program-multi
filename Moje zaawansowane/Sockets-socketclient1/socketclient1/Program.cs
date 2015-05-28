using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace socketclient1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tworzymy gniazdo do połączenia
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Ustalamy gdzie ma się połączyć
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),1234);

            //Łączymy się
            try
            {
                sck.Connect(localEndPoint);
            }
            catch
            {
                Console.Write("nie można połączyć");
                Main(args);
            }

            //Podajemy tekst do wysłania
            Console.Write("Podaj tekst: ");
            string text = Console.ReadLine();

            //Formatujemy tekst do wysłania
            byte[] data = Encoding.ASCII.GetBytes(text);

            //wysyłamy
            sck.Send(data);
            Console.WriteLine("Dane wysłane! \r\n");

            //oczekiwanie na zamknięcie konsoli
            Console.Read();

            //zamknięcie gniazda
            sck.Close();
        }
    }
}
