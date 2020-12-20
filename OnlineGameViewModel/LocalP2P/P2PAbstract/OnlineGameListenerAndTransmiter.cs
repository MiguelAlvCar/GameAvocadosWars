
using System.Net;
using System;
using BasicElements.AbstractClasses;

namespace ViewModelOnlineGame.P2PCommunication
{
    public abstract class AbstractOnlineGameListener : AbstractOnlineHallListener
    {
        public abstract IOnlineGameDecoder OnlineGameDecoder { get; set; }
    }

    public abstract class AbstractOnlineGameTransmiter : AbstractOnlineHallTransmiter
    {
        public abstract IOnlineGameEncoder OnlineGameEncoder { set; get; }

        
    }
}
