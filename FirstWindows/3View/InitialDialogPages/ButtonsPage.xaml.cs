using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FirstWindows.View.Translation;
using FirstWindows.View.Start;
using FirstWindows.View.OnlineHall;
using FirstWindows.Controller;
using WpfBasicElements;
using Translation;
using BasicElements;
using Ninject;
using WpfBasicElements.AbstractClasses;
using WpfBasicElements.AbstractInterceptors;
using BasicElements.AbstractServerInternetCommunication;
using System.Collections.Generic;
using DTO_Models;

namespace FirstWindows.View.InitialDialog
{
    /// <summary>
    /// Interaction logic for ButtonsPage.xaml
    /// </summary>
    public partial class ButtonsPage : Page
    {
        private ListFirst TranslationList;

        public ButtonsPage()
        {
            InitializeComponent();
            ButtonsPage buttonsPage = this;
            TranslationList = Translater.ÜbersetzungMeth(new ListFirst(buttonsPage));
        }

        private void HotSeatClick(object sender, RoutedEventArgs e)
        {
            ((IPrincipalWindow)Window.GetWindow(this)).MainFrame.Navigate(BasicMechanisms.Kernel.Get<ISaveViewGame>());
        }

        private void OnlineClick(object sender, RoutedEventArgs e)
        {
            IInternetCommunicationMainModelView mainMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            if (mainMV.LoggedPlayer != null)
            {
                OnlineHallModel onlineHallViewModel = new OnlineHallModel(Dispatcher);
                onlineHallViewModel.GetPoolGamesWrapper();
                NavigationService.GetNavigationService(this).Navigate(new OnlineHallPage(onlineHallViewModel));
            }
            else
            {
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                Action<IEnumerable<OnlineGameDTO>> returnNavigate = delegate (IEnumerable<OnlineGameDTO> onlineGamesList)
                {
                    if (mainMV.LoggedPlayer != null)
                    {
                        void UpdateGames(object sender1, NavigationEventArgs e1)
                        {
                            if (onlineGamesList != null)
                                ((OnlineHallPage)((Frame)sender1).Content).ModelView.UpdateGamesStore(onlineGamesList);
                            navigationService.LoadCompleted -= UpdateGames;
                        }
                        navigationService.LoadCompleted += UpdateGames;

                        navigationService.Navigate(new OnlineHallPage(new OnlineHallModel(Dispatcher)));
                    }
                    else
                    {
                        navigationService.Navigate(new ButtonsPage());
                    }
                };
                UserLogin userLogin = new UserLogin(returnNavigate);
                navigationService.Navigate(userLogin);
            }
        } 

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            ISaveViewGame hotSeatPageFake = null;
            BasicMechanisms.Kernel.Get<IFirstPageInterceptorFromSaveLoad>().
                NavigateToSaveLoadPage(NavigationService.GetNavigationService(this), TranslationList.LoadGame.ToString(), false, false, hotSeatPageFake);
        }

        private void OptionsClick(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            Action returnNavigate = delegate ()
            {
                navigationService.Navigate(new ButtonsPage()); ;
            };
            OptionsPage optionen = new OptionsPage(BasicMechanisms.Kernel.Get<IMusic>().MusicVolume, BasicMechanisms.Kernel.Get<IEffects>().VolumenEffekte, returnNavigate, delegate { });
            navigationService.Navigate(optionen);
        }

        private void QuitClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
