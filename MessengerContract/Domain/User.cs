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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerContract.Domain
{
    public class User
    {
        //User information
        public User()
        {
            //Make a unique identified for the user that is being logged in
            UserId = Guid.NewGuid().ToString().Split('-')[4];
        }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime TimeCreated { get; set; }
        //Dynamic data collection that provide notifications when an item get added,
        //removed, or when the whole list is refreshed
        public ObservableCollection<Message> UserMessages{ get; set; }
    }
}
