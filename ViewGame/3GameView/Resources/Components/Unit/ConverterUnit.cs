using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BasicElements;
using Ninject;
using BasicElements.AbstractForArmyInterceptor;
using ViewModelHotSeat;

namespace ViewGame.View.Resources
{
    public class ConverterUnitType : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            Uri uri = ((ViewModelUnit)value).UnitType.ImageUri;
            ImageBrush ImageUnit = new ImageBrush();
            if (uri != null)
                ImageUnit.ImageSource = new BitmapImage(uri);

            return ImageUnit;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterModifier : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri = BasicMechanisms.Kernel.Get<IModifierToUriConverter>().GetUri((Modifier)value);

            ImageBrush ImageUnit = new ImageBrush();
            if (uri != null)
                ImageUnit.ImageSource = new BitmapImage(uri);

            return ImageUnit;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterLife : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * 36 / 100;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterMoral : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * 36 / 100;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterAffilation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RadialGradientBrush gradient = new RadialGradientBrush();
            GradientStop stop1 = new GradientStop();
            GradientStop stop2 = new GradientStop();
            stop1.Offset = 1.0;
            stop2.Offset = 0.75;
            stop1.Color = Colors.Black;
            if ((ArmyColor)value == ArmyColor.Blue)
            {
                stop2.Color = Color.FromArgb(255, 00, 85, 255);
            }
            else if ((ArmyColor)value == ArmyColor.Red)
            {
                stop2.Color = Colors.Red;
            }
            gradient.GradientStops.Add(stop1);
            gradient.GradientStops.Add(stop2);
            return gradient;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterMovement : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RadialGradientBrush gradient = new RadialGradientBrush();
            GradientStop stop1 = new GradientStop();
            GradientStop stop2 = new GradientStop();
            stop1.Offset = 0.0;
            stop2.Offset = 0.35;
            gradient.RadiusX = 0.75;
            gradient.RadiusY = 1;
            gradient.Center = new Point(0.55, 0.45);
            gradient.GradientOrigin = new Point(0.55, 0.45);
            
            if ((ViewMovement)value == ViewMovement.Green)
            {
                stop1.Color = Color.FromRgb(66, 217, 64);
                stop2.Color = Colors.Green;
            }
            else if ((ViewMovement)value == ViewMovement.Red)
            {
                stop1.Color = Color.FromRgb(255, 145, 60);
                stop2.Color = Colors.Red;
            }
            else if ((ViewMovement)value == ViewMovement.Yellow)
            {
                stop1.Color = Colors.White;
                stop2.Color = Color.FromRgb(255, 160, 0);
            }
            gradient.GradientStops.Add(stop1);
            gradient.GradientStops.Add(stop2);
            return gradient;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WhiteElipOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((ArmyColor)value) == ArmyColor.Red)
                return 0.11;
            else
                return 0.07;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WhiteElipVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ModifierBorderVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((Modifier)value) == Modifier.None)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(bool)value)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
