using System;
using ModelGame;
using BasicElements.AbstractClasses;
using OnlineGameChatAndStore;

namespace OnlineModelGame
{
    public class OnlineGame: Game
    {
        public OnlineGame(OnlineManager onlineModel)
        {
            OnlineModel = onlineModel;
        }
        OnlineManager OnlineModel { set; get; }
    }
}
