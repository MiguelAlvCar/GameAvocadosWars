using System;
using System.Linq;
using DTO_Models;
using BasicElements.ViewModel;
using BasicElements;
using Ninject;
using BasicElements.AbstractServerInternetCommunication;
using System.Globalization;

namespace OnlineGameChatAndStore
{
    public class ChatOnlineGame: ViewModelBase
    {
        public int ID { set; get; }

        public PlayerDTO Host { set; get; }
        private PlayerDTO _guest;
        public PlayerDTO Guest
        {
            set
            {
                _guest = value;
                if (OnlineGamesStoreRef != null)
                {
                    if (value == null && !OnlineGamesStoreRef.VisibleOnlineGames.Contains(this))
                        OnlineGamesStoreRef._visibleOnlineGames.Add(this);
                    IInternetCommunicationMainModelView mainMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
                    if (value != null && (value != mainMV.LoggedPlayer.Player && Host != mainMV.LoggedPlayer.Player))
                        OnlineGamesStoreRef._visibleOnlineGames.Remove(this);
                }
            }
            get => _guest;
        }

        // in order to set the visibility of the onlineGame in the store
        public OnlineChatGamesStore OnlineGamesStoreRef;

        private string _Description;
        public string Description
        {
            set
            {
                SetProperty(ref _Description, value);
            }
            get => _Description;
        }
        public Chat Chat { set; get; } = new Chat();
        public DateTime CreationTime { set; get; }

        public OnlineGameDTO ConvertToOnlineGameDTO()
        {
            OnlineGameDTO onlineGDTO = new OnlineGameDTO(this.Host);
            onlineGDTO.Guest = this.Guest;
            onlineGDTO.Description = this.Description;
            onlineGDTO.CreationTime = this.CreationTime;
            return onlineGDTO;
        }
    }
}
