/*
*	FILE			:	HandleClient.cs
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
using System.Collections;
using System.Net.Sockets;
using System.Threading;

namespace MessengerServer
{
    public class HandleUser
    {
        TcpClient userSoc;
        string userNumber;
        Hashtable userList;

        public void startUser(TcpClient userSocket, string uNumber, Hashtable hashtable)
        {
            this.userSoc = userSocket;
            this.userNumber = uNumber;
            this.userList = hashtable;

            Thread userThread = new Thread(runChat);
            userThread.Start();
        }

        private void runChat()
        {
            int count = 0;
            byte[] messageFrom = new byte[10025];
            string messageFromClient = null;
            //Byte[] sendMessage = null;
            //string serResp = null;
            string reqCounter = null;

            while(true)
            {
                try
                {
                    count += 1;
                    NetworkStream stream = userSoc.GetStream();
                    stream.Read(messageFrom, 0, (int)userSoc.ReceiveBufferSize);
                    messageFromClient = System.Text.Encoding.ASCII.GetString(messageFrom);
                    messageFromClient = messageFromClient.Substring(0, messageFromClient.IndexOf("$"));
                    Console.WriteLine("From client - " + userNumber + " : " + messageFromClient);
                    reqCounter = Convert.ToString(count);

                    //LocalServer.broadcast(messageFromClient, userNumber, true);

                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                }
            }
        }
    }
}