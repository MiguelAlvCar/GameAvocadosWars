using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBasicElements.AbstractClasses;
using System.Windows.Controls;
using System.Windows.Media;
using FirstWindows.View.Start;
using FirstWindows.View;
using FirstWindows.View.InitialDialog;

namespace FirstWindows.Interceptors
{
    public class ViewGameInterceptor: IViewGameInterceptor
    {
        public Page GetStartPage(ISaveViewGame hotSeat)
        {
            return new StartPage(hotSeat);
        }
        public Page GetFirstButtonsPage()
        {
            return new ButtonsPage();
        }
        public ImageBrush GetWallpaper()
        {
            return GetCommonElements.GetWallpaper();
        }
    }
}
