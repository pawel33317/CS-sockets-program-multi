using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
namespace socketclient2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tworzymy gniazdo do połączenia
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Ustalamy gdzie ma się połączyć
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2000);
            
            //Łączymy się
            try
            {
                sck.Connect(endPoint);
            }
            catch
            {
                Console.Write("nie można połączyć \n\r");
                Main(args);
            }

            //zmienna zadeklarowana wczesniej aby zakonczyc wpisz k
            string msg = "";

            //pętla aby przesłać wiele wiadomości
            while (msg != "k")
            {
                //napiszniewiadomości -> przekonwertowanie do byte -> wysłanie
                Console.Write("Podaj wiadomość: ");
                msg = Console.ReadLine();
                byte[] msgBuffer = Encoding.Default.GetBytes(msg);
                sck.Send(msgBuffer, 0, msgBuffer.Length, 0);

                //utworzenie miejsca na wiadomość do odebrania
                byte[] buffer = new byte[255];

                //pobrane wiadomości zapisanie do buffer i zapisanie gługości do length
                int rec = sck.Receive(buffer, 0, buffer.Length, 0);

                //zmniejszenie buffer do długości pobranej wiadomości
                Array.Resize(ref buffer, rec);

                //wypisanie
                Console.WriteLine("Otrzymano: {0}", Encoding.Default.GetString(buffer)); 
            }

            //oczekiwanie na zamknięcie konsoli
            Console.Read();

            //zamknięcie gniazda
            sck.Close();

        }
    }
}
