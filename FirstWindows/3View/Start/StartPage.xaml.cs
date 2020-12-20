using System;
using System.Windows;
using System.Windows.Controls;
using FirstWindows.View.Translation;
using FirstWindows.View.InitialDialog;
using System.Windows.Navigation;
using WpfBasicElements;
using Translation;
using BasicElements;
using WpfBasicElements.AbstractClasses;
using Ninject;
using WpfBasicElements.AbstractInterceptors;
using ViewModelOnlineGame;

namespace FirstWindows.View.Start
{
    public partial class StartPage : Page
    {
        public StartList TranslationList;

        ISaveViewGame HotSeat;

        public StartPage(ISaveViewGame hotSeat)
        {
            HotSeat = hotSeat;

            InitializeComponent();

            if (hotSeat.IViewModel is OnlineGameViewModel)
            {
                MapLoadButton.IsEnabled = false;
                GameLoadButton.IsEnabled = false;
            }

            if (!HotSeat.IViewModel.IsInBattle)
                GameSaveButton.IsEnabled = false;
            StartPage start1 = this;
            TranslationList = Translater.ÜbersetzungMeth(new StartList(start1));
        }

        private void Spiel_verlassenMeth(object sender, RoutedEventArgs e)
        {
            if (HotSeat.IViewModel is IConnectionCloseable)
                ((IConnectionCloseable)HotSeat.IViewModel).CloseConnection();
            Window.GetWindow(this).Close();
        }

        private void Zurück_zum_SpielMeth(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(null);
        }

        private void KarteSpeichern(object sender, RoutedEventArgs e)
        {
            BasicMechanisms.Kernel.Get<IFirstPageInterceptorFromSaveLoad>().
                NavigateToSaveLoadPage(StartFrame.NavigationService, (string)MapSaveButton.Content, true, true, HotSeat);
        }

        private void SpielSpeichern(object sender, RoutedEventArgs e)
        {
            BasicMechanisms.Kernel.Get<IFirstPageInterceptorFromSaveLoad>().
                NavigateToSaveLoadPage(StartFrame.NavigationService, (string)GameSaveButton.Content, false, true, HotSeat);
        }

        private void KarteLaden(object sender, RoutedEventArgs e)
        {
            BasicMechanisms.Kernel.Get<IFirstPageInterceptorFromSaveLoad>().
                NavigateToSaveLoadPage(StartFrame.NavigationService, (string)MapLoadButton.Content, true, false, HotSeat);
        }

        private void SpielLaden(object sender, RoutedEventArgs e)
        {
            BasicMechanisms.Kernel.Get<IFirstPageInterceptorFromSaveLoad>().
                NavigateToSaveLoadPage(StartFrame.NavigationService, (string)GameLoadButton.Content, false, false, HotSeat);
        }
        
        private void MainMenu(object sender, RoutedEventArgs e)
        {
            HotSeat.IViewModel.RemoveAllUnits();
            HotSeat.IViewModel.RemoveModelMap();
            if (HotSeat.IViewModel is IConnectionCloseable)
                ((IConnectionCloseable)HotSeat.IViewModel).CloseConnection();
            //object window = Window.GetWindow(this);
            //if (window == null)
            object window = Window.GetWindow((DependencyObject)HotSeat);
            ((IPrincipalWindow)window).MainFrame.Navigate(new FirstPage());
        }

        private void Optionen_Click(object sender, RoutedEventArgs e)
        {            
            Action returnNavigate = delegate ()
            {
                StartFrame.Navigate(null);
            };

            Action translate = delegate {
                HotSeat.Translate();
                StartPage startW = this;
                Translater.ÜbersetzungMeth(new StartList(startW));
            };
            OptionsPage optionen = new OptionsPage(BasicMechanisms.Kernel.Get<IMusic>().MusicVolume, BasicMechanisms.Kernel.Get<IEffects>().VolumenEffekte, returnNavigate, translate);
            StartFrame.Navigate(optionen);
        }
    }
}
