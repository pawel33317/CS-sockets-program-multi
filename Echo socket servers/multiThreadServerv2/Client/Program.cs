using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket s;
            byte[] buffer;
            int result;
            String wiadomosc;
            int i = 0;
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            bool x = false;

            //pętla aż do rozpoczęcia połączenia
            while (x != true)
            {
                try
                {
                    s.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777));
                    x = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("nie mozna polaczyc");
                    Thread.Sleep(1000);
                }
            }

            try
            {
                while (i < 3)
                {
                    //wysyla
                    buffer = Encoding.Default.GetBytes("siema");
                    s.Send(buffer);

                    //odbiera
                    buffer = new byte[1024];
                    result = s.Receive(buffer, 0, buffer.Length, 0);


                    //wypisuje i czeka
                    wiadomosc = Encoding.ASCII.GetString(buffer, 0, result);
                    Console.WriteLine(wiadomosc);
                    Thread.Sleep(3000);
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("inny error");
            }

            Console.WriteLine("klient wysłał 3 wiadomości i zakończył działanie");

            s.Close();
            Console.ReadKey();

        }
    }
}
