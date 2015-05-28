using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace multiserver
{
    public partial class Main : Form
    {
        //Obiekt naszej klasy
        Listener listener;
        public Main()
        {
            InitializeComponent();

            //utworzenie instancji klasy z parametrem port
            listener = new Listener(8);

            //dodanie do dalegaty "SocketAcceptedHandler" w klasie Listener metody listener_SocketAccepted
            listener.SocketAccepted += new Listener.SocketAcceptedHandler(listener_SocketAccepted);

            //Dodaje zdarzenie do klasy Main 
            Load += new EventHandler(Main_Load);
        }

        //metoda wywoływana jeżeli nastąpiło zdarzenie
        void listener_SocketAccepted(System.Net.Sockets.Socket e)
        {

            //utworzenie obiektu naszej klasy parametr Socket
            Sclient sclient = new Sclient(e);

            //dodanie do delegaty "SclintReceiveHandler" w klasie Sclient metody client_Received
            sclient.Received += new Sclient.SclintReceiveHandler(client_Received);

            //dodanie do delegaty "SclientDisconnectedHandler" w klasie Sclient metody client_Disconnected
            sclient.Disconnected += new Sclient.SclientDisconnectedHandler(client_Disconnected);

            //z invoke gdyż metoda nie będzie odpalana z watku głównego i Windows.Forms by się wysypał
            Invoke((MethodInvoker)delegate
            {
                //dopisanie do listy nowego klienta :...
                ListViewItem i = new ListViewItem();

                //pobranie jego ip i portu
                i.Text = sclient.EndPoint.ToString();

                //pobranie id
                i.SubItems.Add(sclient.ID);

                //
                i.SubItems.Add("XX");
                i.SubItems.Add("XX");
                
                //konieczny badziew
                i.Tag = sclient;

                //dopisaniedo listy
                lstClients.Items.Add(i);
            });
        }

        void client_Disconnected(Sclient sender)
        {
            //z invoke gdyż metoda nie będzie odpalana z watku głównego i Windows.Forms by się wysypał
            Invoke((MethodInvoker)delegate
            {
                //wyszukanie klienta który się rozłączył na liście
                for (int i = 0; i < lstClients.Items.Count;i++ )
                {
                    //pobranie danych aktualnego na liście
                    Sclient sclient = lstClients.Items[i].Tag as Sclient;

                    //jeżeli aktualny klient to ten który się rozłączył to wywalamy z listy
                    if (sclient.ID == sender.ID)
                    {
                        lstClients.Items.RemoveAt(i);
                        //przerwanie pętli gdyż usunięto już
                        break;
                    }
                }
            });
        }

        void client_Received(Sclient sender, byte[] data)
        {
            //z invoke gdyż metoda nie będzie odpalana z watku głównego i Windows.Forms by się wysypał
            Invoke((MethodInvoker)delegate
            {
                //przeszukiwanie listy w celu znalezienia klienta który wysłał dane
                for (int i = 0; i < lstClients.Items.Count; i++)
                {
                    //pobranie danych aktualnego na liście
                    Sclient sclient = lstClients.Items[i].Tag as Sclient;

                    //jeżeli aktualny klient to ten który wysłał dane do wpisujemy przesłąne przez niego dane
                    if (sclient.ID == sender.ID)
                    {
                        lstClients.Items[i].SubItems[2].Text = Encoding.Default.GetString(data);
                        lstClients.Items[i].SubItems[3].Text = DateTime.Now.ToString();
                        break;
                    }
                }
            });
        }
        private void Main_Load(object sender, EventArgs e)
        {
            //odpalenie Listenera
            listener.Start();
        }
    }
}
