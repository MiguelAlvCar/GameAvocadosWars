using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FirstWindows.View
{
    public static class GetCommonElements
    {
        public static ImageBrush GetWallpaper()
        {
            Random zufallHintergrund = new Random();
            int AleatoryWallpaper = zufallHintergrund.Next(0, 3);
            string VerzeichnisFuersHintergrund = "";
            switch (AleatoryWallpaper)
            {
                case 0:
                    VerzeichnisFuersHintergrund = "pack://application:,,,/FirstWindows;component/3View/Resources/Images/Wallpaper/Vikings.jpg";
                    break;
                case 1:
                    VerzeichnisFuersHintergrund = "pack://application:,,,/FirstWindows;component/3View/Resources/Images/Wallpaper/Kreuzritter.jpg";
                    break;
                case 2:
                    VerzeichnisFuersHintergrund = "pack://application:,,,/FirstWindows;component/3View/Resources/Images/Wallpaper/Hoplits.jpg";
                    break;
            }

            ImageBrush Wallpaper = new ImageBrush();
            Uri uri1 = new Uri(VerzeichnisFuersHintergrund);
            Wallpaper.ImageSource = new BitmapImage(uri1);
            return Wallpaper;
        }
    }
}
