using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Input;
using DTO_Models;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using BasicElements.ViewModel;
using BasicElements;
using Ninject;
using BasicElements.AbstractServerInternetCommunication;
using WpfBasicElements;
using OnlineGameChatAndStore;
using BasicElements.AbstractClasses;
using Ninject.Parameters;
using System.Windows.Threading;
using System.Timers;
using System.Net.Sockets;

namespace FirstWindows.Controller
{
    public partial class OnlineHallModel : ViewModelBaseEvery
    {
        IInternetCommunicationMainModelView MainMV;

        public OnlineHallModel(Dispatcher dispacher)
        {
            MainMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            OnlineGamesStorage = new OnlineChatGamesStore(dispacher);
            InternetCommunication = BasicMechanisms.Kernel.Get<ICommunicationWithServer>();

            SortDescription sort = new SortDescription("CreationTime", ListSortDirection.Descending);
            OnlineGamesStorage.VisibleOnlineGames.SortDescriptions.Add(sort);
            IsNewHostGameCreated = false;
            IsInAGameWithAdversary = false;
            ConfirmAdversary = new RelayCommand(ConfirmAdversaryExecuted, ConfirmAdversaryCanExecute);
            HostGame = new RelayCommand(HostGameExecuted, HostGameCanExecute);
            GoBack = new RelayCommand(GoBackExecuted, GoBackCanExecute);
            UpdatePool = new RelayCommand(UpdatePoolExecuted, UpdatePoolCanExecute);
            JoinToGame = new RelayCommand(JoinToGameExecuted, JoinToGameCanExecute);
            IsAdversaryConfirmed = false;
        }

        public OnlineChatGamesStore OnlineGamesStorage { get; set; }
        public ICommunicationWithServer InternetCommunication;
        
        public event Action HostGameCreatedViewEvent;
        public event Action HostGameRemoved;
        private bool _isNewHostGameCreated;
        public bool IsNewHostGameCreated
        {
            get => _isNewHostGameCreated;
            set
            {
                if (value && HostGameCreatedViewEvent != null)
                {
                    HostGameCreatedViewEvent();
                }                    
                else if (HostGameRemoved != null)
                    HostGameRemoved();
                _isNewHostGameCreated = value;
            }
        }
        
        private bool _isGuest;
        public bool IsGuest
        {
            get => _isGuest;
            set
            {
                if (value)
                {
                    IsNewHostGameCreated = false;
                    IsInAGameWithAdversary = false;
                }  
                else
                {
                    if (!CancelGuest())
                    {
                        return;
                    }
                }  
                SetPropertyEveryTimePropChanged(ref _isGuest, value);
            }
        }

        private PlayerDTO Adversary
        {
            get
            {
                if (IsGuest)
                    return OnlineGamesStorage.CurrentChatGame.Host;
                else
                    return OnlineGamesStorage.CurrentChatGame.Guest;
            }
        }

        private bool _IsInAGameWithAdversary;
        public bool IsInAGameWithAdversary
        {
            get => _IsInAGameWithAdversary;
            set
            {
                SetPropertyEveryTimePropChanged(ref _IsInAGameWithAdversary, value);
            }
        }

        private bool _HasAdversaryConfirmed;
        private bool HasAdversaryConfirmed
        {
            set
            {
                if (value != _HasAdversaryConfirmed)
                {
                    AdversaryConfirms(value);
                    _HasAdversaryConfirmed = value;
                }
            }
            get
            {
                return _HasAdversaryConfirmed;
            }
        }

        public event Action AdversaryisGone;

        public AbstractOnlineHallTransmiter Transmiter { get; set; }
        private AbstractOnlineHallListener _Listener;
        public AbstractOnlineHallListener Listener { 
            set 
            {
                if (value != null)
                {
                    value.OnlineHallDecoder.ConnectIPAndPlayer += ConnectIPAndPlayer;
                    value.OnlineHallDecoder.NewChatMessage += NewChatMessageFromAdversary;
                    value.OnlineHallDecoder.ToMap += GoToMap;
                    value.OnlineHallDecoder.GoFromGame += AdversaryGoesFromGame;
                    value.OnlineHallDecoder.ChangeDescription += ChangeDescription;
                    value.OnlineHallDecoder.MaintainP2PConnection += AdversaryGoesFromGame;
                }
                
                _Listener = value;
            } 
            get 
            {
                return _Listener;
            } 
        }

        private IOpenedListener OpenedListener { set; get; }

        public void GetPoolGamesWrapper()
        {
            IEnumerable<OnlineGameDTO> listOnlineGames = InternetCommunication.GetPoolGames(MainMV.LoggedPlayer.Player);
            UpdateGamesStore(listOnlineGames);
        }

        public event Action OnlineGameChanged;
        internal void UpdateGamesStore(IEnumerable<OnlineGameDTO> ListOnlineGames)
        {
            IEnumerable<OnlineGameDTO> GameToSet = ListOnlineGames.Where(x => OnlineGamesStorage._visibleOnlineGames.All(y => y.ID != x.ID));
            IEnumerable<OnlineGameDTO> GameToUpdate = ListOnlineGames.Except(GameToSet);
            IEnumerable<ChatOnlineGame> GameToDelete = OnlineGamesStorage._visibleOnlineGames.Where(x => ListOnlineGames.All(y => y.ID != x.ID));

            while (GameToDelete.Count() > 0)
            {
                OnlineGamesStorage.Remove(GameToDelete.First());
            }
            if (GameToDelete.Count() > 0)
            {
                OnlineGamesStorage.VisibleOnlineGames.Refresh();
            }

            foreach (OnlineGameDTO onlineGameDTO in GameToUpdate)
            {
                ChatOnlineGame onlineGame = OnlineGamesStorage._visibleOnlineGames.First(x => x.ID == onlineGameDTO.ID);
                onlineGame.Description = onlineGameDTO.Description;
            }
            if (GameToUpdate.Count() > 0)
            {
                OnlineGamesStorage.VisibleOnlineGames.Refresh();
            }

            foreach (OnlineGameDTO onlineGameDTO in GameToSet)
            {
                ChatOnlineGame onlineGame = new ChatOnlineGame();
                onlineGame.Guest = onlineGameDTO.Guest;
                onlineGame.Host = onlineGameDTO.Host;
                onlineGame.ID = onlineGameDTO.ID;
                onlineGame.Description = onlineGameDTO.Description;
                onlineGame.CreationTime = onlineGameDTO.CreationTime;
                onlineGame.PropertyChanged += (sender1, arg) =>
                {
                    if (arg.PropertyName == nameof(onlineGame.Description))
                    {
                        OnlineGameChanged();
                    }
                };
                //onlineGame.AdversaryHasConfirmed = onlineGameDTO.AdversaryHasConfirmed;

                OnlineGamesStorage.Add(onlineGame);
            }
        }

        #region Commands

        public void AddMessageToChat(string message, PlayerDTO player, bool isHostPlayer)
        {
            
            if (OnlineGamesStorage.CurrentChatGame != null)
            {
                ChatEntry newChatEntry = new ChatEntry(message, player, isHostPlayer);
                OnlineGamesStorage.CurrentChatGame.Chat.AddChatEntry(newChatEntry);
            }
        }
        public event Action MessageSubmitted;
        public RoutedCommand SubmitMessage { get; } = new RoutedCommand();
        public void SubmitMessageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !String.IsNullOrEmpty((string)e.Parameter) && OnlineGamesStorage.CurrentChatGame != null;
        }
        public void SubmitMessageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Transmiter.OnlineHallEncoder.NewChatMessage((string)e.Parameter);
            AddMessageToChat((string)e.Parameter, MainMV.LoggedPlayer.Player, IsGuest);
            MessageSubmitted();
        }

        private void StartOpenedListener()
        {
            OpenedListener = BasicMechanisms.Kernel.Get<IOpenedListener>();
            OpenedListener.OnlineHallDecoder.ConnectIPAndPlayer += (player) =>
            {
                string IP = player.GlobalIP;
                if (IP != null && OnlineGamesStorage.VisibleOnlineGames.CurrentItem != null)
                {
                    OpenedListener.Close();
                    OpenedListener = null;

                    OnlineGamesStorage.CurrentChatGame.Guest = player;
                    bool succeeded = InternetCommunication.PutHostGame(OnlineGamesStorage.CurrentChatGame.ConvertToOnlineGameDTO());
                    if (!succeeded)
                        OnlineGamesStorage.CurrentChatGame.Guest = null;

                    Listener = BasicMechanisms.Kernel.Get<AbstractOnlineHallListener>(new[] {
                            new ConstructorArgument("maintainConnectionPoolingMiliseconds", 7500) });
                    Listener.Listen();
                    Transmiter = BasicMechanisms.Kernel.Get<AbstractOnlineHallTransmiter>(
                        new[] {
                            new ConstructorArgument ("addresse", IP),
                            new ConstructorArgument ("port", 6112),
                            new ConstructorArgument("maintainConnectionPoolingMiliseconds", 3000) });
                    ((ChatOnlineGame)OnlineGamesStorage.VisibleOnlineGames.CurrentItem).Guest = player;

                    PlayerDTO player1 = GetPlayerWithIP(IP);
                    
                    Transmiter.OnlineHallEncoder.ConnectIPAndPlayer(player1);

                    IsInAGameWithAdversary = true;
                    AdversaryComesIn(true);
                }
                else
                {
                    string CurrentDirectory = System.Environment.CurrentDirectory;
                    string Directory = CurrentDirectory.Substring(0, 2);
                    FileStream filest = new FileStream(Directory + "\\Miguel.txt", FileMode.Create);
                    BinaryWriter BinWriter = new BinaryWriter(filest);
                    BinWriter.Seek(0, SeekOrigin.Begin);
                    BinWriter.Write("The opened listener could processed the ConnectIPAndPlayer-signal \n");
                    BinWriter.Write("IP: " + IP + "; CurrentItem is null: " + OnlineGamesStorage.VisibleOnlineGames.CurrentItem != null);
                    BinWriter.Close();
                }
            };

            OpenedListener.Listen();
        }

        private PlayerDTO GetPlayerWithIP(string foreignGlobalIP)
        {
            PlayerDTO player1 = (PlayerDTO)MainMV.LoggedPlayer.Player.Clone();
            int secondPart = Convert.ToInt32(foreignGlobalIP.Split('.')[1]);
            bool ds = secondPart >= 16 && secondPart <= 31;
            if (foreignGlobalIP.Substring(0, 7) == "192.168")
            {
                string IPadress = player1.LocalIPs.FirstOrDefault(x => x.IP.Substring(0, 11) == foreignGlobalIP.Substring(0, 11)).IP;

                player1.GlobalIP = IPadress;
            }
            else if ((foreignGlobalIP.Substring(0, 3) == "172" &&
                Convert.ToInt32(foreignGlobalIP.Split('.')[1]) >= 16 && Convert.ToInt32(foreignGlobalIP.Split('.')[1]) <= 31))
            {
                string IPadress = player1.LocalIPs.FirstOrDefault(x => x.IP.Substring(0, 7) == foreignGlobalIP.Substring(0, 7)).IP;

                player1.GlobalIP = IPadress;
            }
            else if (foreignGlobalIP.Substring(0, 3) == "10.")
            {
                string IPadress = player1.LocalIPs.FirstOrDefault(x => x.IP.ToString().Substring(0, 3) == "10").IP;

                player1.GlobalIP = IPadress;
            }

            player1.LocalIPs = null;

            return player1;
        }

        public event Action<bool> AdversaryComesIn;
        public event Action<bool> AdversaryConfirms;
        public RoutedCommand ConfirmDescription { get; } = new RoutedCommand();
        public void ConfirmDescriptionCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !String.IsNullOrEmpty((string)e.Parameter);
        }
        public void ConfirmDescriptionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!IsNewHostGameCreated)
            {
                OnlineGameDTO onlineGameDTO1 = new OnlineGameDTO(MainMV.LoggedPlayer.Player);
                onlineGameDTO1.Description = (string)e.Parameter;
                PostNewHosGame(onlineGameDTO1);
            }
            else
            {
                OnlineGameDTO onlineGameDTO1 = OnlineGamesStorage.CurrentChatGame.ConvertToOnlineGameDTO();
                onlineGameDTO1.Description = (string)e.Parameter;
                bool succeeded = InternetCommunication.PutHostGame(onlineGameDTO1);
                if (Transmiter != null)
                    Transmiter.OnlineHallEncoder.ChangeDescription(onlineGameDTO1.Description);
            }

            OnlineGamesStorage.CurrentChatGame.Description = (string)e.Parameter;


        }
        private System.Timers.Timer aTimerForMaintainConnectionWithServer { set; get; } = new System.Timers.Timer();
        private void PostNewHosGame(OnlineGameDTO onlineGameDTO)
        {
            StartOpenedListener();

            int gameID = InternetCommunication.PostHostGame(onlineGameDTO);
            if (gameID > 0)
            {
                IsNewHostGameCreated = true;
                ChatOnlineGame newGame = new ChatOnlineGame() { Host = MainMV.LoggedPlayer.Player, ID = gameID };
                OnlineGamesStorage.Add(newGame);
                OnlineGamesStorage.VisibleOnlineGames.MoveCurrentTo(newGame);

                //Server interval 13000
                aTimerForMaintainConnectionWithServer.Interval = 4000;
                aTimerForMaintainConnectionWithServer.Elapsed += MaintainConnectionServer;
                aTimerForMaintainConnectionWithServer.AutoReset = true;
                aTimerForMaintainConnectionWithServer.Start();
            }
            else
            {
                throw new InvalidDataException();
            }
        }
        private void MaintainConnectionServer(object sender, ElapsedEventArgs e)
        {
            InternetCommunication.MaintainConnection(((ChatOnlineGame)OnlineGamesStorage.VisibleOnlineGames.CurrentItem).ID);
        }


        public bool CancelGuest()
        {
            if (OnlineGamesStorage.CurrentChatGame != null && OnlineGamesStorage.CurrentChatGame.Guest != null &&
                        OnlineGamesStorage.CurrentChatGame.Guest.ID == MainMV.LoggedPlayer.Player.ID)
            {
                if (Transmiter != null)
                    Transmiter.OnlineHallEncoder.GoFromGame();
                Transmiter = null;
                Listener.Close();
                Listener = null;
                IsInAGameWithAdversary = false;
                OnlineGameDTO onlineGameDTO = OnlineGamesStorage.CurrentChatGame.ConvertToOnlineGameDTO();
                onlineGameDTO.Guest = new PlayerDTO("", 0, "");
                bool succeeded = InternetCommunication.PutHostGame(onlineGameDTO);
                if (succeeded)
                    OnlineGamesStorage.CurrentChatGame.Guest = null;
                return succeeded;
            }
            else
                return true;
        }

        private bool _isAdversaryConfirmed;
        public bool IsAdversaryConfirmed
        {
            get => _isAdversaryConfirmed;
            set
            {
                SetProperty(ref _isAdversaryConfirmed, value);
            }
        }
        public ICommand ConfirmAdversary { get; set; }
        private bool ConfirmAdversaryCanExecute(object obj)
        {
            return OnlineGamesStorage.CurrentChatGame != null && OnlineGamesStorage.CurrentChatGame.Guest != null;
        }
        private void ConfirmAdversaryExecuted(object obj)
        {
            IsAdversaryConfirmed = !IsAdversaryConfirmed;
            Transmiter.OnlineHallEncoder.ToMap(IsAdversaryConfirmed, HasAdversaryConfirmed, MainMV.LoggedPlayer.Player.ID);
            if (HasAdversaryConfirmed && IsAdversaryConfirmed)
                GoToGame();
        }

        public ICommand HostGame { get; set; }
        private bool HostGameCanExecute(object obj)
        {
            return IsGuest && (OnlineGamesStorage.CurrentChatGame == null || OnlineGamesStorage.CurrentChatGame.Guest == null);
        }
        private void HostGameExecuted(object obj)
        {
            IsGuest = false;
            OnlineGamesStorage.VisibleOnlineGames.MoveCurrentTo(0);
            IsAdversaryConfirmed = false;
        }

        public event Action NavigateToMainPage;
        public ICommand GoBack { get; set; }
        private bool GoBackCanExecute(object obj)
        {
            return true;
        }
        private void GoBackExecuted(object obj)
        {
            if (IsGuest)
            {
                CancelGuest();
                NavigateToMainPage();                
            }
            else
            {
                IsGuest = true;
                IsAdversaryConfirmed = false;
                aTimerForMaintainConnectionWithServer.Stop();
                aTimerForMaintainConnectionWithServer.Elapsed -= MaintainConnectionServer;
                if (OnlineGamesStorage.CurrentChatGame != null)
                {
                    if (OpenedListener != null)
                    {
                        OpenedListener.Close();
                        OpenedListener = null;
                    }
                    if (Transmiter != null)
                        Transmiter.OnlineHallEncoder.GoFromGame();
                    Transmiter = null;
                    if (Listener != null)
                    {
                        Listener.Close();
                        Listener = null;
                    }
                    InternetCommunication.DeleteGame(OnlineGamesStorage.CurrentChatGame.ID);
                    OnlineGamesStorage.Remove(OnlineGamesStorage.CurrentChatGame);
                    OnlineGamesStorage.VisibleOnlineGames.MoveCurrentTo(null);
                }
            }
        }

        public ICommand UpdatePool { get; set; }
        private bool UpdatePoolCanExecute(object obj)
        {
            return true;
        }
        private void UpdatePoolExecuted(object obj)
        {
            GetPoolGamesWrapper();
        }

        public event Action<bool> JoinedToGame;
        public ICommand JoinToGame { get; set; }
        private bool JoinToGameCanExecute(object obj)
        {
            return OnlineGamesStorage.CurrentChatGame != null;
        }
        private void JoinToGameExecuted(object obj)
        {
            try
            {
                //BasicMechanisms.Log("Begin try");
                if ((bool)obj)
                {
                    if (OnlineGamesStorage.CurrentChatGame.Host.GlobalIP != null)
                    {
                        //IPAddress[] IPs = OnlineGamesStorage.CurrentChatGame.Host.LocalIPs;

                        Listener = BasicMechanisms.Kernel.Get<AbstractOnlineHallListener>(new[] {
                            new ConstructorArgument("maintainConnectionPoolingMiliseconds", 7500) });
                        Listener.Listen();

                        PlayerDTO player = GetPlayerWithIP(OnlineGamesStorage.CurrentChatGame.Host.GlobalIP);

                        Transmiter = BasicMechanisms.Kernel.Get<AbstractOnlineHallTransmiter>(
                                new[] {
                            new ConstructorArgument ("addresse", OnlineGamesStorage.CurrentChatGame.Host.GlobalIP),
                            new ConstructorArgument ("port", 6112),
                            new ConstructorArgument("maintainConnectionPoolingMiliseconds", 3000) });

                        Transmiter.OnlineHallEncoder.ConnectIPAndPlayer(player);
                    }

                    IsInAGameWithAdversary = true;

                    OnlineGamesStorage.CurrentChatGame.Guest = MainMV.LoggedPlayer.Player;
                    JoinedToGame(true);
                }
                else
                {
                    HasAdversaryConfirmed = false;
                    CancelGuest();
                    JoinedToGame(false);
                }
            }
            catch (Exception e)
            {
                //BasicMechanisms.Log("Exception while joining to a game \nException: " + e + "\nMessage: " + e.Message + "\nSource: " + e.Source + "\nTarget: " + e.TargetSite + "\nData: " + e.Data, e);
                return;
            }

        }

        
        #endregion

    }
    
}
