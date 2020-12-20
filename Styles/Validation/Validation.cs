using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Styles.Validation
{
    public class ValidatedTextBox : TextBox
     {
        static ValidatedTextBox()
        {
            FrameworkPropertyMetadata meta = new FrameworkPropertyMetadata((double)25);
            ValidationFontSizeProperty = DependencyProperty.Register(nameof(ValidationFontSize), typeof(double), typeof(ValidatedTextBox), meta);
            IsToolTipSetProperty = DependencyProperty.Register(nameof(IsToolTipSet), typeof(bool), typeof(ValidatedTextBox));
        }

        public static readonly DependencyProperty ValidationFontSizeProperty;
        public double ValidationFontSize
        {
            get { return (double)GetValue(ValidationFontSizeProperty); }
            set
            {
                SetValue(ValidationFontSizeProperty, value);
            }
        }

        public static readonly DependencyProperty IsToolTipSetProperty;
        public bool IsToolTipSet
        {
            get { return (bool)GetValue(IsToolTipSetProperty); }
            set
            {
                SetValue(IsToolTipSetProperty, value);
            }
        }
    }
   
    public class ValidatedComboBox : ComboBox
    {
        static ValidatedComboBox()
        {
            FrameworkPropertyMetadata meta = new FrameworkPropertyMetadata((double)25);
            ValidationFontSizeProperty = DependencyProperty.Register(nameof(ValidationFontSize), typeof(double), typeof(ValidatedComboBox), meta);
            IsToolTipSetProperty = DependencyProperty.Register(nameof(IsToolTipSet), typeof(bool), typeof(ValidatedComboBox));
        }

        public static readonly DependencyProperty ValidationFontSizeProperty;
        public double ValidationFontSize
        {
            get { return (double)GetValue(ValidationFontSizeProperty); }
            set
            {
                SetValue(ValidationFontSizeProperty, value);
            }
        }

        public static readonly DependencyProperty IsToolTipSetProperty;
        public bool IsToolTipSet
        {
            get { return (bool)GetValue(IsToolTipSetProperty); }
            set
            {
                SetValue(IsToolTipSetProperty, value);
            }
        }
    }

    public class ValidatedButton : Button
    {
        static ValidatedButton()
        {
            FrameworkPropertyMetadata meta = new FrameworkPropertyMetadata((double)25);
            ValidationFontSizeProperty = DependencyProperty.Register(nameof(ValidationFontSize), typeof(double), typeof(ValidatedButton), meta);
            IsToolTipSetProperty = DependencyProperty.Register(nameof(IsToolTipSet), typeof(bool), typeof(ValidatedButton));
            HasErrorProperty = DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(ValidatedButton));
            ErrorMessageProperty = DependencyProperty.Register(nameof(ErrorMessage), typeof(string), typeof(ValidatedButton));
        }

        public static readonly DependencyProperty ValidationFontSizeProperty;
        public double ValidationFontSize
        {
            get { return (double)GetValue(ValidationFontSizeProperty); }
            set
            {
                SetValue(ValidationFontSizeProperty, value);
            }
        }

        public static readonly DependencyProperty IsToolTipSetProperty;
        public bool IsToolTipSet
        {
            get { return (bool)GetValue(IsToolTipSetProperty); }
            set
            {
                SetValue(IsToolTipSetProperty, value);
            }
        }

        public static readonly DependencyProperty HasErrorProperty;
        public bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            set
            {
                SetValue(HasErrorProperty, value);
            }
        }

        public static readonly DependencyProperty ErrorMessageProperty;
        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set
            {
                SetValue(ErrorMessageProperty, value);
            }
        }
    }
}
