using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace multiserver
{
    class Sclient
    {
        public string ID { get; private set;}
        public IPEndPoint EndPoint{ get; private set;}
        Socket sck;

        public Sclient(Socket accepted)
        {
            //gnizado połączenia
            sck = accepted;

            //generowanie id
            ID = Guid.NewGuid().ToString();

            //zapisanie ip i portu
            EndPoint = (IPEndPoint)sck.RemoteEndPoint;

            //rozpoczęcie odbierania danych wywołuje callback
            sck.BeginReceive(new byte[] {0},0,0,0,callback,null);
        }

        void callback(IAsyncResult ar)
        {
            try
            {
                //pobiera dane
                sck.EndReceive(ar);

                //Pobiera dane
                byte[] buf = new byte[8192];

                int rec = sck.Receive(buf, buf.Length, 0);

                if (rec < buf.Length)
                {
                    Array.Resize<byte>(ref buf,rec);
                }

                //jeżeli pobrało dane wywołuje zdarzenie Received
                if (Received != null)
                {
                    Received(this,buf);
                }

                //rozpoczęcie odbierania danych wywołuje callback
                sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
            }
            //jeżeli wystąpił błąd lub rozłłączono wywołuje zdarzenie Disconnected
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Close();

                if (Disconnected != null)
                {
                    Disconnected(this);
                }
            }
        }

        public void Close()
        {
            sck.Close();
            sck.Dispose();
        }

        //delegata (otrzyma metodę "client_Received" od klasy głównej Main i przekazuje jej parametry)
        public delegate void SclintReceiveHandler(Sclient sender, byte[] data);

        //jw tylko inaczej ;p
        public delegate void SclientDisconnectedHandler(Sclient sender);

        //obsługuje delegatę SclintReceiveHandler
        public event SclintReceiveHandler Received;

        //obsługuje delegatę SclientDisconnectedHandler
        public event SclientDisconnectedHandler Disconnected;
    }
}
