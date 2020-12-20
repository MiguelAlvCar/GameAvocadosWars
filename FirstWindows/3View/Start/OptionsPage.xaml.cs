using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using FirstWindows.View.Translation;
using FirstWindows.View.OnlineHall;
using System.Windows.Navigation;
using Translation;
using BasicElements;
using WpfBasicElements.AbstractClasses;
using Ninject;
using BasicElements.AbstractServerInternetCommunication;
using DTO_Models;

namespace FirstWindows.View.Start
{
    /// <summary>
    /// Interaktionslogik für Optionen.xaml
    /// </summary>
    public partial class OptionsPage : Page
    {
        public ListOptions TranslationList;

        Action ReturnNavigate;

        Action Translate;

        public OptionsPage(double musicVolumen, double effektsVolumen, Action returnNavigate, Action translate)
        {
            InitializeComponent();

            ReturnNavigate = returnNavigate;

            Player.DataContext = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();

            Translate = translate;

            OptionsPage options = this;
            TranslationList = Translater.ÜbersetzungMeth(new ListOptions(ref options)/*, BinderUpdater.UpdateTranslationHotSeat*/);
            
            SliderMusik.Value = musicVolumen;
            SliderEffekte.Value = effektsVolumen;
            int Index;
            switch (Translater.Language)
            {
                case BasicElements.Language.Spanish:
                    Index = 0;
                    break;
                case BasicElements.Language.German:
                    Index = 1;
                    break;
                case BasicElements.Language.English:
                    Index = 2;
                    break;
                default:
                    Index = 1;
                    break;                
            }
            ComboboxSprache.SelectedIndex = Index;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            switch (ComboboxSprache.SelectedIndex)
            {
                // Spanisch
                case 0:
                    Translater.Language = BasicElements.Language.Spanish;
                    break;
                // Deutsch
                case 1:
                    Translater.Language = BasicElements.Language.German;
                    break;
                //Englisch
                case 2:
                    Translater.Language = BasicElements.Language.English;
                    break;
                default:
                    Translater.Language = BasicElements.Language.English;
                    break;
            }
            FileStream filest = new FileStream(System.Environment.CurrentDirectory + "\\Daten.dat", FileMode.Open);
            BinaryWriter BinWriter = new BinaryWriter(filest);
            BinWriter.Seek(0, SeekOrigin.Begin);
            BinWriter.Write((byte)Translater.Language);
            BinWriter.Write(BasicMechanisms.Kernel.Get<IMusic>().MusicVolume);
            BinWriter.Write(BasicMechanisms.Kernel.Get<IEffects>().VolumenEffekte);
            BinWriter.Close();

            Translate();

            ReturnNavigate();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ReturnNavigate();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>().LoggedPlayer = null;
            LoggedInPanel.GetBindingExpression(DockPanel.VisibilityProperty).UpdateTarget();
            LoggedOutPanel.GetBindingExpression(DockPanel.VisibilityProperty).UpdateTarget();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            OptionsPage optionen = this;

            Action<IEnumerable<OnlineGameDTO>> returnNavigate = delegate (IEnumerable<OnlineGameDTO> onlineGamesList)
            {
                navigationService.Navigate(optionen);

                LoggedInPanel.GetBindingExpression(DockPanel.VisibilityProperty).UpdateTarget();
                LoggedOutPanel.GetBindingExpression(DockPanel.VisibilityProperty).UpdateTarget();
                UserNameInput.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                StarsGrid.GetBindingExpression(Grid.WidthProperty).UpdateTarget();
                AbilityInput.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                BattlesInput.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                WonBattlesInput.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                EmailInput.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            };
            UserLogin userLogin = new UserLogin(returnNavigate);
            navigationService.Navigate(userLogin);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new EditUserWindow());
        }

        private void SliderMusik_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BasicMechanisms.Kernel.Get<IMusic>().MusicVolume = SliderMusik.Value;
            BasicMechanisms.Kernel.Get<IMusic>().MusicMediaPlayer.Volume = SliderMusik.Value;
        }

        private void SliderEffekte_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BasicMechanisms.Kernel.Get<IEffects>().VolumenEffekte = SliderEffekte.Value;
        }
    }
}
