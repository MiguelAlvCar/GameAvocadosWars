using System;
using System.Collections.Generic;
using System.Text;
using BasicElements.AbstractClasses;
using OnlineGameChatAndStore;

namespace OnlineModelGame
{
    public class OnlineManager: AbstractOnlineManager
    {
        public OnlineManager (ITransmiter transmiter, IListener listener, ChatOnlineGame chatGame): base (transmiter, listener, chatGame)
        {
            
        }
    }
}
