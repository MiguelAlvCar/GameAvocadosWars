using System;
using DTO_Models;
using BasicElements.ViewModel;
using OnlineGameChatAndStore;
using ViewModelOnlineGame;
using ViewModelOnlineGame.P2PCommunication;
using BasicElements;
using Ninject;
using BasicElements.AbstractClasses;
using Ninject.Parameters;

namespace FirstWindows.Controller
{
    public partial class OnlineHallModel : ViewModelBaseEvery
    {
        private void ConnectIPAndPlayer(PlayerDTO player)
        {
            if (player.ID == OnlineGamesStorage.CurrentChatGame.Host.ID)
            {
                Transmiter = BasicMechanisms.Kernel.Get<AbstractOnlineHallTransmiter>(
                                new[] {
                            new ConstructorArgument ("addresse", player.GlobalIP),
                            new ConstructorArgument ("port", 6112),
                            new ConstructorArgument("maintainConnectionPoolingMiliseconds", 3000) });
            }
        }

        private void NewChatMessageFromAdversary(string message)
        {
            if (OnlineGamesStorage.CurrentChatGame != null)
            {
                PlayerDTO adversary = !IsGuest ? OnlineGamesStorage.CurrentChatGame.Guest : OnlineGamesStorage.CurrentChatGame.Host;
                AddMessageToChat(message, adversary, !IsGuest);
            }
        }

        private void ChangeDescription(string description)
        {
            if (OnlineGamesStorage.CurrentChatGame != null)
                OnlineGamesStorage.CurrentChatGame.Description = description;
        }

        private void GoToMap(bool hasAdversaryConfirmed, bool isConfirmed, string ID)
        {
            if (OnlineGamesStorage.CurrentChatGame != null &&
                OnlineGamesStorage.CurrentChatGame.Guest != null &&
                Adversary.ID == ID)
            {
                if (hasAdversaryConfirmed && isConfirmed)
                    GoToGame();
                else
                    HasAdversaryConfirmed = hasAdversaryConfirmed;
            }
        }

        private void AdversaryGoesFromGame()
        {
            if (OnlineGamesStorage.VisibleOnlineGames.CurrentItem != null &&
                            ((ChatOnlineGame)OnlineGamesStorage.VisibleOnlineGames.CurrentItem).Guest != null)
            {
                Transmiter.Close();
                Transmiter = null;
                // can the listener be put to null?, if exception just remove line
                Listener = null;
                if (((ChatOnlineGame)OnlineGamesStorage.VisibleOnlineGames.CurrentItem).Guest.ID == MainMV.LoggedPlayer.Player.ID)
                {
                    JoinedToGame(false);
                    HasAdversaryConfirmed = false;
                    IsAdversaryConfirmed = false;
                    OnlineGamesStorage.Remove(OnlineGamesStorage.CurrentChatGame);
                    OnlineGamesStorage.VisibleOnlineGames.MoveCurrentTo(null);
                }
                else
                {
                    IsAdversaryConfirmed = false;
                    HasAdversaryConfirmed = false;
                    ((ChatOnlineGame)OnlineGamesStorage.VisibleOnlineGames.CurrentItem).Guest = null;
                    StartOpenedListener();
                }
                IsInAGameWithAdversary = false;
                AdversaryisGone();
            }
        }

        public event Action<OnlineManager> NavigateToViewGamePage;
        private void GoToGame()
        {
            Listener.OnlineHallDecoder.MaintainConnectionPoolingMiliseconds = 10500;
            Transmiter.OnlineHallEncoder.MaintainConnectionPoolingMiliseconds = 4000;
            aTimerForMaintainConnectionWithServer.Stop();

            Listener.OnlineHallDecoder.NewChatMessage -= NewChatMessageFromAdversary;
            Listener.OnlineHallDecoder.ToMap -= GoToMap;
            Listener.OnlineHallDecoder.GoFromGame -= AdversaryGoesFromGame;
            Listener.OnlineHallDecoder.ChangeDescription -= ChangeDescription;
            Listener.OnlineHallDecoder.MaintainP2PConnection -= AdversaryGoesFromGame;
            OnlineManager abstractOnlineManager = new OnlineManager((AbstractOnlineGameTransmiter)Transmiter, (AbstractOnlineGameListener)Listener, OnlineGamesStorage.CurrentChatGame, IsGuest);
            NavigateToViewGamePage(abstractOnlineManager);
        }
    }
}
