using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using DTO_Models;

namespace BasicElements.AbstractServerInternetCommunication
{    
    public interface IInternetCommunicationMainModelView
    {
        UserPlayer LoggedPlayer { set; get; }
    }

    public class UserPlayer
    {
        public UserPlayer(PlayerDTO player)
        {
            Player = player;
        }

        public PlayerDTO Player { set; get; }
        public Cookie IdentityCookie { set; get; }
    }
}
