using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tworzymy gniazdo do połączenia
            

            //Ustalamy gdzie ma się połączyć
            

            //Łączymy się

            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2000);
            //zmienna zadeklarowana wczesniej aby zakonczyc wpisz k

                //napiszniewiadomości -> przekonwertowanie do byte -> wysłanie
            while (true)
            {
                Thread.Sleep(100);
                try
                {
                    sck.Connect(endPoint);
                    
                }
                catch
                {
                    Console.Write("nie można połączyć \n\r");
                    Main(args);
                }
                 

                string msg = "echo-client";
                byte[] msgBuffer = Encoding.Default.GetBytes(msg);
                sck.Send(msgBuffer, 0, msgBuffer.Length, 0);

                //utworzenie miejsca na wiadomość do odebrania
                byte[] buffer = new byte[255];

                //pobrane wiadomości zapisanie do buffer i zapisanie gługości do length
                int rec = sck.Receive(buffer, 0, buffer.Length, 0);

                //zmniejszenie buffer do długości pobranej wiadomości
                Array.Resize(ref buffer, rec);
                Console.WriteLine("Otrzymano: {0}", Encoding.Default.GetString(buffer));
                //Thread.Sleep(1000);
                //wypisanie
                sck.Disconnect(false);
                sck.Close();

            }
            
            
            sck.Dispose();  
            //oczekiwanie na zamknięcie konsoli
            Console.Read();

            //zamknięcie gniazda
            

        }
    }
}
