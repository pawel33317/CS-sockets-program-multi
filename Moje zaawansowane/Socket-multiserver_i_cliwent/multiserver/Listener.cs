using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace multiserver
{
    class Listener
    {
        Socket s;

        public bool Listening
        {
            get;
            private set;
        }

        public int Port 
        { 
            get; 
            private set;
        }

        //metoda tworząca gniazdo
        public Listener(int port)
        {
            Port = port;
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        //metoda odpalana jako wnoy wątek
        public void Start()
        {
            //jeżeli jest już nasłuchiwanie wyjdź
            if (Listening)
                return;

            //rozpoczęcie nasłuchiwania
            s.Bind(new IPEndPoint(0, Port));
            s.Listen(0);

            //BeginAccept metoda przyjmuje dwa parametry AsyncCallback pełnomocnika, 
            //który wskazuje metody wywołania zwrotnego accept i obiekt, 
            //który jest używany do przekazywania informacji o stanie do metody wywołania zwrotnego. 

            //rozpoczyna asynchroniczne przesyłanie danych - wywołuje callback
            s.BeginAccept(callback, null);
            Listening = true;
        }

        public void Stop()
        {
            if (!Listening)
                return;

            s.Close();
            s.Dispose();
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        void callback(IAsyncResult ar)
        {
            try
            {
                //kończy asynchroniczne przesyłanie danych
                Socket s = this.s.EndAccept(ar);

                //jeżeli nie wystąpiło zdarzenie
                if (SocketAccepted != null)
                {
                    //odpala zdarzenie do którego przesyła parametr s
                    //zdarzenie przesyła parametr s do delegaty
                    //delegata wykonuje metodę którą otrzymało od klasy głównej Program i przekazuje jej parametr s
                    SocketAccepted(s);
                }

                //rozpoczyna asynchroniczne przesyłanie danych - wywołuje callback
                this.s.BeginAccept(callback,null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //delegata (otrzyma metodę od klasy głównej Program i przekazuje jej zdarzenei s)
        public delegate void SocketAcceptedHandler(Socket e);

        //obsługuje delegatę SocketAcceptedHandler
        public event SocketAcceptedHandler SocketAccepted;
    }
}
