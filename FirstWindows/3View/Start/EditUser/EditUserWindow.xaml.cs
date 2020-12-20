using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using FirstWindows.View.Translation;
using GameFrontEnd.View;
using FirstWindows.Controller;
using System.Text.RegularExpressions;
using Styles.Validation;
using System.Windows.Navigation;
using FirstWindows.View.Start;
using Translation;
using BasicElements.AbstractServerInternetCommunication;
using BasicElements;
using Ninject;
using WpfBasicElements;

namespace FirstWindows.View.Start
{
    /// <summary>
    /// Interaktionslogik für EditUser.xaml
    /// </summary>
    public partial class EditUserWindow : Page
    {
        public static EditUserWindow editUser;

        public ListEditUser TranslationList;
        private ICommunicationWithServer InternetCommunication;
        public EditUserWindow()
        {
            editUser = this;
            InitializeComponent();
            Confirm.Command = new RelayCommand(Confirm_ClickExecuted, Confirm_ClickCanExecute);
            UserNameInput.GetBindingExpression(ValidatedTextBox.TextProperty).UpdateTarget();
            EmailInput.GetBindingExpression(ValidatedTextBox.TextProperty).UpdateTarget();
            EditUserWindow editUser1 = this;
            TranslationList = Translater.ÜbersetzungMeth(new ListEditUser(ref editUser1));
            InternetCommunication = BasicMechanisms.Kernel.Get<ICommunicationWithServer>();
        }

        public ICommand Confirm_Click { get; }
        private bool Confirm_ClickCanExecute(object sender)
        {
            return !Validation.GetHasError(EmailInput) && !Validation.GetHasError(OldPassInput) &&
                !Validation.GetHasError(NewPassInput1) && !Validation.GetHasError(NewPassInput2);
        }
        private void Confirm_ClickExecuted(object sender)
        {
            IsInternetValidation = true;
            OldPassInput.GetBindingExpression(ValidatedComboBox.TagProperty).UpdateSource();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).GoBack();
        }
    }
}
