using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ViewModelOnlineGame;
using System.Text;
using Translation;

namespace ViewGame.View.Game
{

    abstract class ConverterNoConvertBack : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ConverterBreite : ConverterNoConvertBack
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string breite = TranslationConverter.GetTranslation().Width.ToString() + value as string;
            return breite;
        }
    }
    

    class ConverterLaenge : ConverterNoConvertBack
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string laenge = TranslationConverter.GetTranslation().Length.ToString() + value as string;
            return laenge;
        }
    }

    class ConverterMovementText : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            string RestLife = System.Convert.ToString(value[0]);
            string BaseLife = System.Convert.ToString(value[1]);
            return TranslationConverter.GetTranslation().Bewegung + RestLife + " / " + BaseLife;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ConverterStregthText : ConverterNoConvertBack
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TranslationConverter.GetTranslation().Kraft.ToString() + value;
        }
    }

    class ConverterRangeText : ConverterNoConvertBack
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TranslationConverter.GetTranslation().Range.ToString() + value;
        }
    }

    class ConverterRangedStregthText : ConverterNoConvertBack
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TranslationConverter.GetTranslation().Fernkampf.ToString() + value;
        }
    }

    class ConverterLifeText : ConverterNoConvertBack
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TranslationConverter.GetTranslation().Leben.ToString() + value + " / 100";
        }
    }

    class ConverterMoralText : ConverterNoConvertBack
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TranslationConverter.GetTranslation().Moral.ToString() + value + " / 100";
        }
    }
    

    class ConverterTurn : ConverterNoConvertBack
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TranslationConverter.GetTranslation().Turn.ToString() + value;
        }
    }

    class OnlineVisibleConverter : ConverterNoConvertBack
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OnlineGameViewModel)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
    }

    class HotSeatVisibleConverter : ConverterNoConvertBack
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OnlineGameViewModel)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }
    }
}