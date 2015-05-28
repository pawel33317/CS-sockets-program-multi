using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace multiThreadServerv2
{
    public partial class Server : Form
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public Server()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //w nowym wątku odpalamy serwer
            this.tcpListener = new TcpListener(IPAddress.Any, 7777);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            //odpalamy nowy wątek
            this.listenThread.Start();
        }


        private void ListenForClients()
        {
            //odpalamy nasłuchiwanie
            try
            {
                this.tcpListener.Start(); 
            }
            catch (Exception e)
            {
                this.SetTextOnListBox1("Serwer nie może zacząć nasłuchiwać");
                return;
            }
            this.SetTextOnListBox1("Serwer działa na porcie 7777");


            while (true)
            {
                //czeka na nowego kliena i blokuje pętlę 
                TcpClient client = this.tcpListener.AcceptTcpClient();
                this.SetTextOnListBox1("Nowy klient: " + client.Client.RemoteEndPoint);

                //uruchamia nowy wątek z parametrem
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }


        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            String clientIP = "" + tcpClient.Client.RemoteEndPoint.AddressFamily;

            //tworzymy stream pomiędzy klientam a wątkiem serwera
            NetworkStream clientStream = tcpClient.GetStream();

            int size = 1024;
            byte[] message = new byte[size];
            int bytesRead;
            ASCIIEncoding encoder = new ASCIIEncoding();

            while (true)
            {
                bytesRead = 0;
                try
                { 
                    //oczekuja na wiadomość od klienta i blokuje pętlę
                    bytesRead = clientStream.Read(message, 0, size);

                    if (bytesRead <= 0)
                    {
                        this.SetTextOnListBox1("Klient" + tcpClient.Client.RemoteEndPoint + " się rozłączył");
                        tcpClient.Close();
                        break;
                    }
                    else
                    {
                        this.SetTextOnListBox1("Wiadomość od: " + tcpClient.Client.RemoteEndPoint + ": " + encoder.GetString(message, 0, bytesRead));
                        //wysyła odebraną wiadomość od klienta do klienta
                        clientStream.Write(message, 0, bytesRead);
                    }
                }
                catch
                {
                    //jeżeli klient się rozłączył
                    if (bytesRead <= 0)
                    {
                        this.SetTextOnListBox1("Klient" + tcpClient.Client.RemoteEndPoint + " się rozłączył");
                        tcpClient.Close();
                        break;
                    }
                    //System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead)); 

                    //kończy wątek przez wyjście z pętli
                    tcpClient.Close();
                    break;

                }
            }
            //tcpClient.Close();
        }


        delegate void SetTextCallback(string text);

        private void SetTextOnListBox1(string text)
        { 
            if (this.listBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTextOnListBox1);
                //wykonuje delegatę w wątku w który utworzył kontrolkę
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.listBox1.Items.Insert(0, text);
            }
        } 
    }
}
