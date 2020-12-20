using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections;

namespace ViewGame.View.Resources
{
    public class ConverterTerrainType : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri = (Uri)value;
            ImageBrush ImageTerrain = new ImageBrush();
            if (uri != null)
                ImageTerrain.ImageSource = new BitmapImage(uri);

            ImageTerrain.Stretch = Stretch.Fill;
            return ImageTerrain;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
