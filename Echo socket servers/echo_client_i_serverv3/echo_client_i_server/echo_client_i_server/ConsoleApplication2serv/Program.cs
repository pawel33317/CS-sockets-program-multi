using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApplication2serv
{
    class Program
    {
        static void Main(string[] args)
        {
            int rec;
 
            //Tworzymy gniazdo do połączenia
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
 
            //ustalamy port i ip na ktorym ma działać nasz serwer 0 oznacza "127.0.0.1"
            sck.Bind(new IPEndPoint(0,2000));
            sck.Listen(0);//maksymalna ilosc polaczen oczekujacych
 
            //Tworzymy gniezdo nasłuchu które czaka na jakieś dane wysłane od jakiegoś klienta
            Socket acc = sck.Accept();
 
            //tworzymy blok danych do wysłania
            byte[] buffer = Encoding.Default.GetBytes("echo-server");
            byte[] buffer2;
            //pętla aby serwer mógł odbierać >1 wiadomość
            while (true)
            {
                //wysyłamy dane
                acc.Send(buffer, 0, buffer.Length, 0);
 
                //tworzymy miejsce na dane do odebrania
                buffer2 = new byte[255];
 
                //odbieramy dane zapisujemy do buffer i do rec zapisujemy ich długość
                rec = acc.Receive(buffer2, 0, buffer2.Length, 0);
 
                //skracamy buffer do długości danych
                Array.Resize(ref buffer2, rec);
 
                //wypisujemy otrzymane dane
                Console.WriteLine("Otrzymano: {0}", Encoding.Default.GetString(buffer2));
            }
 
            //zamykamy gniazdo
            sck.Close();
            //oczekiwanie na zamknięcie konsoli
            Console.Read();
 
        }
    }
}
