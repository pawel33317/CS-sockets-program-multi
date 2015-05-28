using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace socketserver1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tworzymy gniazdo do połączenia
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //ustalamy port i ip na ktorym ma działać nasz serwer 0 oznacza "127.0.0.1"
            sck.Bind(new IPEndPoint(0,1234));
            sck.Listen(100);//maksymalna ilosc polaczen oczekujacych

            //Tworzymy gniezdo nasłuchu które czaka na jakieś dane wysłane od jakiegoś klienta
            Socket accepted = sck.Accept();

            //tworzy tablicę o takim rozmiarze jak rozmiar buforu danych czyli >= jak otrzymane dane
            byte[] Buffer = new byte[accepted.SendBufferSize];

            //zapisuje do Buffer pobrane dane i dodatkowo zrwaca ilość pobranych bajtów
            int bytesRead = accepted.Receive(Buffer);

            //tworzymy tablicę o takim rozmiarze jak otrzymane dane (nie jak całkowyty rozmiar buffora)
            byte[] formatted = new byte[bytesRead];

            //przepisujemy ze starej tablicy do nowej tylko przesłane dane
            for(int i = 0; i<bytesRead;i++)
                formatted[i] = Buffer[i];

            //formatujemy dane do stringa
            string strData = Encoding.ASCII.GetString(formatted);
            Console.Write(strData + "\r\n");

            //oczekiwanie na zamknięcie konsoli
            Console.Read();

            //zamknięcie gniazda
            sck.Close();
        }
    }
}
