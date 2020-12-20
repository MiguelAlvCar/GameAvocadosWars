using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using BasicElements;
using Ninject;
using BasicElements.AbstractServerInternetCommunication;
using System.Windows.Threading;
using WpfBasicElements;

namespace OnlineGameChatAndStore
{
    public class OnlineChatGamesStore
    {
        public ObservableCollection<ChatOnlineGame> _visibleOnlineGames;
        public ListCollectionView VisibleOnlineGames { get; }
        private Dispatcher WpfDispatcher;
        public OnlineChatGamesStore(Dispatcher dispacher)
        {
            WpfDispatcher = dispacher;
            _visibleOnlineGames = new ObservableCollection<ChatOnlineGame>();
            VisibleOnlineGames = new ListCollectionView(_visibleOnlineGames);
            VisibleOnlineGames.MoveCurrentTo(null);
        }

        private List<ChatOnlineGame> AllOnlineGames = new List<ChatOnlineGame>();
        public void Add(ChatOnlineGame onlineGame)
        {
            AllOnlineGames.Add(onlineGame);
            onlineGame.OnlineGamesStoreRef = this;
            if (onlineGame.Guest == null || onlineGame.Guest == BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>().LoggedPlayer.Player)
                _visibleOnlineGames.Add(onlineGame);
        }
        public void Remove(ChatOnlineGame onlineGame)
        {
            void Remove1()
            {
                AllOnlineGames.Remove(onlineGame);
                if (onlineGame != null)
                {
                    onlineGame.OnlineGamesStoreRef = null;
                    if (VisibleOnlineGames.Contains(onlineGame))
                        _visibleOnlineGames.Remove(onlineGame);
                }
            }
            WpfBasicMechanisms.DispatcherWrapper(Remove1, WpfDispatcher);
        }
        public ChatOnlineGame CurrentChatGame
        {
            get
            {
                return (ChatOnlineGame)VisibleOnlineGames.CurrentItem;
            }
        }
        
    }
}
