using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;
using FirstWindows.View.Translation;
using System.Windows;
using Styles.Validation;
using DTO_Models;
using System.Windows.Navigation;
using BasicElements.AbstractServerInternetCommunication;
using BasicElements;
using Ninject;


namespace FirstWindows.View.OnlineHall
{
    public class PasswordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (UserLogin.UserLoginInstance.IsInitialized)
            {
                LoginModel details = new LoginModel();
                details.Name = UserLogin.UserLoginInstance.NameInput.Text;
                details.Password = UserLogin.UserLoginInstance.PasswordInput.Password;

                IEnumerable<OnlineGameDTO> onlineGamesList = new List<OnlineGameDTO>();
                if (BasicMechanisms.Kernel.Get<ICommunicationWithServer>().InternetLogin(details, out onlineGamesList, out bool connectionSucceeded))
                { 
                    UserLogin.UserLoginInstance.ReturnNavigate(onlineGamesList);
                    return ValidationResult.ValidResult;
                }
                else
                {
                    if (connectionSucceeded)
                    {
                        UserLogin.UserLoginInstance.NameInput.GetBindingExpression(ValidatedTextBox.TextProperty).UpdateSource();
                        return new ValidationResult(false, UserLogin.UserLoginInstance.TranslationList.ValidationMessage);
                    }
                    else
                    {
                        UserLogin.UserLoginInstance.PopUpAndClose();
                        return new ValidationResult(false, UserLogin.UserLoginInstance.TranslationList.ValidationMessage);
                    }
                    
                }
            }
            return ValidationResult.ValidResult;
        }
    }

    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (UserLogin.UserLoginInstance.IsInitialized)
                return new ValidationResult(false, UserLogin.UserLoginInstance.TranslationList.ValidationMessage);
            return ValidationResult.ValidResult;
        }
    }
}
