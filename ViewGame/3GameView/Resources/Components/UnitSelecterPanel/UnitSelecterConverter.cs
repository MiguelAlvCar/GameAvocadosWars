using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using Translation;

namespace ViewGame.View.Resources
{
    class RestPointsConverter : IValueConverter
    {
        private UnitRowConvertersList TranslationList;

        public RestPointsConverter()
        {
            TranslationList = Translater.ÜbersetzungMeth(new UnitRowConvertersList(this));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                double restPoints = (double)value;
                return restPoints + TranslationList.Point.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
