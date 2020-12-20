using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using Styles.Validation;
using ViewGame.View;

namespace ViewGame.View.Game
{
    public class PointsValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            return ValidationResult.ValidResult;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            int points;
            try
            {
                points = Convert.ToInt32((string)value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, TranslationValidation.GetTranslation().PleaseNumber);
            }
            if (points > 3)
            {
                return ValidationResult.ValidResult;
            }
            else
                return new ValidationResult(false, TranslationValidation.GetTranslation().PleasePoints);

        }
    }

    public class NoPlayerSelectedValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            return ValidationResult.ValidResult;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            if (value != null)
            {
                return ValidationResult.ValidResult;
            }
            else
                return new ValidationResult(false, TranslationValidation.GetTranslation().PleasePlayer);
        }
    }

    public class NoArmySelectedValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            return ValidationResult.ValidResult;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            if (value != null)
            {
                return ValidationResult.ValidResult;
            }
            else
                return new ValidationResult(false, TranslationValidation.GetTranslation().PleaseArmy);
        }
    }

    public partial class GamePage : Page
    {
        public void UpdateSourceTextBox(object sender, RoutedEventArgs e)
        {
            ((ValidatedTextBox)sender).GetBindingExpression(ValidatedTextBox.TextProperty).UpdateSource();
        }
        public void UpdateSourceComboBox(object sender, RoutedEventArgs e)
        {
            ((ValidatedComboBox)sender).GetBindingExpression(ValidatedComboBox.SelectedItemProperty).UpdateSource();
        }
    }
}
