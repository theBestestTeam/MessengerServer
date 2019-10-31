/*
*	FILE			:	LocalServer.cs
*	PROJECT			:	PROG2121 - Windows and Mobile Programming
*	PROGRAMMER		:	Amy Dayasundara, Paul Smith
*	FIRST VERSION	:	2019 - 09 - 25
*	DESCRIPTION		:	This program is responsible for creating and establishing
*	                    the server. This is the point at which the clients are required
*	                    to communicate through. Any messages that need to be passed to 
*	                    another client is deconstructed here.
*
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace MessengerServer
{
    class LocalServer
    {
        //public static Hashtable userList = new Hashtable();
        private static byte[] _buffer = new byte[1024];
        private static List<Socket> _clientSockets = new List<Socket>();
        private static Socket _serverSocketListener = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

       // private static List<TcpClient> tcpClientTcp = new List<TcpClient>();
       // private static TcpListener serverListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 13000);
        static void Main(string[] args)
        {
            SetupServer();
            Console.ReadLine();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up server..");
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 100);

            //serverListener.Start();

            _serverSocketListener.Bind(new IPEndPoint(IPAddress.Any, 100));
            _serverSocketListener.Listen(5); //Backlog not important -- only worry when more client. Backlog - howmany pending connections can exist
            _serverSocketListener.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult asyncResult)
        {
            Socket socket = _serverSocketListener.EndAccept(asyncResult);
            _clientSockets.Add(socket);
            Console.WriteLine("Client Connected");

            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
            _serverSocketListener.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }
        private static void RecieveCallback(IAsyncResult asyncResult)
        {
            Socket socket = (Socket)asyncResult.AsyncState;

            int recieved = socket.EndReceive(asyncResult);
            byte[] dataBuff = new byte[recieved];
            Array.Copy(_buffer, dataBuff, recieved);

            //Text being recieved into server
            string text = Encoding.ASCII.GetString(dataBuff);
            Console.WriteLine("Text received: " + text);
           
            byte[] data = Encoding.ASCII.GetBytes(text);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
        }
        private static void SendCallback(IAsyncResult asyncResult)
        {
            Socket socket = (Socket)asyncResult.AsyncState;

            socket.EndSend(asyncResult);
        }
    }
}

//
// METHOD       : TxtMainBoxTextChanged
//
// DESCRIPTION  :
//      This method reads how many characters have been input into the text box
//
// PARAMETER    :
//      object sender           : Referenced to the textbox
//      TextChangedEventArgs e  : Event data changes
//
// RETURNS      :   None
//