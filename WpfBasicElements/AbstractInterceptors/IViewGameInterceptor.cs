using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfBasicElements.AbstractClasses
{
    public interface IViewGameInterceptor
    {
        Page GetFirstButtonsPage();
        Page GetStartPage(ISaveViewGame hotSeat);
        ImageBrush GetWallpaper();
    }
}
