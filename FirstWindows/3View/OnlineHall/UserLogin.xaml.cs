using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstWindows.View.Translation;
using Styles.CommonPages;
using Translation;
using FirstWindows.View.InitialDialog;
using DTO_Models;


namespace FirstWindows.View.OnlineHall
{
    /// <summary>
    /// Interaktionslogik für UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Page
    {
        public ListUserLogin TranslationList;
        public static UserLogin UserLoginInstance;

        public UserLogin(Action<IEnumerable<OnlineGameDTO>> returnNavigate)
        {
            UserLoginInstance = this;
            InitializeComponent();

            ReturnNavigate += returnNavigate;

            UserLogin userLogin = this;
            TranslationList = Translater.ÜbersetzungMeth(new ListUserLogin(ref userLogin));
        }

        public Action<IEnumerable<OnlineGameDTO>> ReturnNavigate;

        public void PopUpAndClose()
        {
            ButtonsPage buttons = new ButtonsPage();
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            void PopUp(object sender, NavigationEventArgs e)
            {
                ((ButtonsPage)((Frame)sender).Content).ButtonsFrame.Navigate(new WPopup(TranslationList.ConnectionToWebFailed.ToString()));
                navigationService.LoadCompleted -= PopUp;
            }            
            navigationService.LoadCompleted += PopUp;
            navigationService.Navigate(new ButtonsPage());
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ReturnNavigate(null);
            // NavigationService.GetNavigationService(this).Navigate(null);
        }

        public static RoutedCommand Confirm_Click { get; } = new RoutedCommand();
        public void Confirm_ClickCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !String.IsNullOrEmpty(PasswordInput.Password) && !String.IsNullOrEmpty(NameInput.Text);
        }
        public void Confirm_ClickExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PasswordInput.GetBindingExpression(PasswordBox.TagProperty).UpdateSource();
        }
    }

    public class UserLoginModel
    {
        public string Name { set; get; }
        public string PropertyForPasswordBinding { set; get; }
    }
}
