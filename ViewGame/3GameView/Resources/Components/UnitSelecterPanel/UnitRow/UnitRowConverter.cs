using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Data;
using Translation;
using ArmyAndUnitTypes;

namespace ViewGame.View.Resources
{
    class PointsConverter : IMultiValueConverter
    {
        private UnitRowConvertersList TranslationList;

        public PointsConverter()
        {
            TranslationList = Translater.ÜbersetzungMeth(new UnitRowConvertersList(this));
        }

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] != null && value[1] != null)
            {
                AbstractUnitType unitType = (AbstractUnitType)value[0];
                return unitType.Points * (int)value[1] + TranslationList.Point.ToString();
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class UnitNameConverter : IValueConverter
    {
        private UnitRowConvertersList TranslationList;
        public UnitNameConverter()
        {
            TranslationList = Translater.ÜbersetzungMeth(new UnitRowConvertersList(this));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                AbstractUnitType unitType = (AbstractUnitType)value;
                return unitType.Name + "(" + unitType.Points + TranslationList.Point.ToString() + ")" ;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitRowConvertersList : TranslationList
    {
        public UnitRowConvertersList(object converter)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = Point, Spanish = " p.", German =" Pkt.", English =" p."},

            };
        }
        public StringBuilder Point { set; get; } = new StringBuilder();
    }
}
