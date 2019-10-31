using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceModel;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.ComponentModel;

namespace MessengerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        TcpClient userSocket = new TcpClient();
        NetworkStream serverStream = null;
        IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13000);
        string messageData = null;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        TcpListener server = null;

        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public MainWindow()
        {
            InitializeComponent();
            //LoopConnect();
            _clientSocket.Connect(IPAddress.Loopback, 100);

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 100);
            server = new TcpListener(ipAddr, 100);
            //Console.ReadLine();
        }



        private static void LoopConnect()
        {
            int attempts = 0;
            while (!_clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    _clientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    
                    //Console.Clear();
                    //Console.WriteLine("Connection attempts: " + attempts.ToString());
                }
            }
            //Console.Clear();
            //Console.WriteLine("Connected");
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {

            if(!string.IsNullOrEmpty(Username.Text))
            {
                //userSocket.Connect("127.0.0.1", 13000);

                userSocket.Connect(iPEnd);
                serverStream = userSocket.GetStream();

                byte[] outName = Encoding.ASCII.GetBytes(Username.Text + "$");
                serverStream.Write(outName, 0, outName.Length);
                serverStream.Flush();

                
            }
            if (serverStream == null)
            {
                MessageBox.Show("Please connect to a server.");
                return;
            }
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            

            if (!string.IsNullOrEmpty(messageText.Text))
            {

                byte[] buffer = Encoding.ASCII.GetBytes(messageText.Text);
                _clientSocket.Send(buffer);

                byte[] recieveBuffer = new byte[1024];
                int recieve = _clientSocket.Receive(recieveBuffer);
                byte[] data = new byte[recieve];
                Array.Copy(recieveBuffer, data, recieve);



                //chatBox.Text = messageText.Text;

               // worker.DoWork += Worker_DoWork;
               // worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                //worker.RunWorkerAsync();
                //Console.WriteLine("Recieved: " + Encoding.ASCII.GetString(data));
                //messageData = "Connected to Message Server...";
                //message();
                ////userSocket.Connect("127.0.0.1", 13000);
                ////serverStream = userSocket.GetStream();

                //byte[] outName = Encoding.ASCII.GetBytes(messageText.Text + "$");
                //serverStream.Write(outName, 0, outName.Length);
                //serverStream.Flush();

                //Thread msgThread = new Thread(getMsg);
                //msgThread.Start();
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(_clientSocket.Connected)
            {
                Byte[] bytes = new Byte[256];
                String data = null;
                data = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            }
        }

        public void sendMessage(Socket userSocket, string message)
        {

        }
        private void getMsg()
        {
            while(true)
            {
                serverStream = userSocket.GetStream();
                int bufferSize = 0;
                byte[] thisStream = new byte[100025];
                string infoData = null;

                bufferSize = userSocket.ReceiveBufferSize;
                serverStream.Read(thisStream, 0, bufferSize);
                infoData = Encoding.ASCII.GetString(thisStream);

                messageData = "" + infoData;
                message();
            }
        }

        private void message()
        {
            
        }
    }
}
