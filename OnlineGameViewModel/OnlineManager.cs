using System;
using System.Collections.Generic;
using System.Text;
using ModelGame.Abstract;
using OnlineGameChatAndStore;
using ViewModelOnlineGame.P2PCommunication;

namespace ViewModelOnlineGame
{
    public class OnlineManager
    {
        public OnlineManager(AbstractOnlineGameTransmiter transmiter, AbstractOnlineGameListener listener, ChatOnlineGame chatGame, bool isGuest)
        {
            Transmiter = transmiter;
            Listener = listener;
            ChatGame = chatGame;
            IsGuest = isGuest;
        }

        public AbstractOnlineGameTransmiter Transmiter { get; set; }
        public AbstractOnlineGameListener Listener { get; set; }
        public ChatOnlineGame ChatGame { get; set; }
        public bool IsGuest { get; set; }
    }
}
