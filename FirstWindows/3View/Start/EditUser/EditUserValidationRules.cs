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
using System.Windows.Media;
using System.Text.RegularExpressions;
using FirstWindows.Controller;
using DTO_Models;
using System.Windows.Navigation;
using BasicElements;
using Ninject;
using BasicElements.AbstractServerInternetCommunication;

namespace FirstWindows.View.Start
{
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string pattern = @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";
            if (!new Regex(pattern, RegexOptions.IgnoreCase).Match((string)value).Success)
            {
                return new ValidationResult(false, EditUserWindow.editUser.TranslationList.InvalidEmail);
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }

    public class FirstNewPasswordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (EditUserWindow.editUser.IsInitialized)
            {
                if (!string.IsNullOrEmpty(EditUserWindow.editUser.NewPassInput2.Password))
                {
                    if (EditUserWindow.editUser.IsOldPasswordToValidate)
                        EditUserWindow.editUser.OldPassInput.GetBindingExpression(ValidatedComboBox.TagProperty).UpdateSource();
                    EditUserWindow.editUser.IsOldPasswordToValidate = false;
                    if (EditUserWindow.editUser.IsSecondNewPasswordToValidate)
                    {
                        EditUserWindow.editUser.IsSecondNewPasswordToValidate = false;
                        EditUserWindow.editUser.NewPassInput2.GetBindingExpression(ValidatedComboBox.TagProperty).UpdateSource();                        
                    }                        
                    if (EditUserWindow.editUser.NewPassInput1.Password != EditUserWindow.editUser.NewPassInput2.Password)
                    {                            
                        return new ValidationResult(false, EditUserWindow.editUser.TranslationList.WrongNewPass);
                    }
                }
            }
            return ValidationResult.ValidResult;
        }
    }

    public class SecondNewPasswordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (EditUserWindow.editUser.IsInitialized)
            {
                if (EditUserWindow.editUser.IsOldPasswordToValidate)
                    EditUserWindow.editUser.OldPassInput.GetBindingExpression(ValidatedComboBox.TagProperty).UpdateSource();
                EditUserWindow.editUser.IsOldPasswordToValidate = false;
                if (EditUserWindow.editUser.IsFirstNewPasswordToValidate)
                {
                    EditUserWindow.editUser.IsFirstNewPasswordToValidate = false;
                    EditUserWindow.editUser.NewPassInput1.GetBindingExpression(ValidatedComboBox.TagProperty).UpdateSource();
                }                    
                if (EditUserWindow.editUser.NewPassInput1.Password != EditUserWindow.editUser.NewPassInput2.Password)
                {
                    return new ValidationResult(false, EditUserWindow.editUser.TranslationList.WrongNewPass);
                }
            }
            return ValidationResult.ValidResult;
        }
    }

    public class OldPasswordValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (EditUserWindow.editUser.IsInitialized)
            {
                if ( (!string.IsNullOrEmpty(EditUserWindow.editUser.NewPassInput1.Password)
                    || !string.IsNullOrEmpty(EditUserWindow.editUser.NewPassInput2.Password)  ) 
                    && string.IsNullOrEmpty(EditUserWindow.editUser.OldPassInput.Password)  )
                {
                    return new ValidationResult(false, EditUserWindow.editUser.TranslationList.OldPassNeed);
                }
                if (EditUserWindow.editUser.IsInternetValidation)
                {
                    if (BasicMechanisms.Kernel.Get<ICommunicationWithServer>().EditUser(new EditUserDTO()
                    {
                        ID = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>().LoggedPlayer.Player.ID,
                        NewEmail = EditUserWindow.editUser.EmailInput.Text,
                        NewName = EditUserWindow.editUser.UserNameInput.Text,
                        NewPassword = EditUserWindow.editUser.NewPassInput1.Password,
                        OldPassword = EditUserWindow.editUser.OldPassInput.Password
                    }))
                    {
                        NavigationService.GetNavigationService(EditUserWindow.editUser).GoBack();
                        return ValidationResult.ValidResult;
                    }
                    else
                    {
                        EditUserWindow.editUser.IsInternetValidation = false;
                        return new ValidationResult(false, EditUserWindow.editUser.TranslationList.WrongOldPass);
                    }
                }
                
            }            
            return ValidationResult.ValidResult;
        }
    }

    public partial class EditUserWindow
    {
        public bool IsOldPasswordToValidate = false;
        public bool IsSecondNewPasswordToValidate = false;
        public bool IsFirstNewPasswordToValidate = false;
        public bool IsInternetValidation = false;

        public void UpdateSourceTextBox(object sender, RoutedEventArgs e)
        {
            ((ValidatedTextBox)sender).GetBindingExpression(ValidatedTextBox.TextProperty).UpdateSource();
        }
        public void UpdateSourcePasswordBox(object sender, RoutedEventArgs e)
        {
            IsOldPasswordToValidate = true;
            IsSecondNewPasswordToValidate = true;
            IsFirstNewPasswordToValidate = true;
            ((PasswordBox)sender).GetBindingExpression(ValidatedComboBox.TagProperty).UpdateSource();
        }
    }
}