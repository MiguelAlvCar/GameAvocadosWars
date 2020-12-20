using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBasicElements.AbstractInterceptors;
using System.Windows.Navigation;
using FirstWindows.View.InitialDialog;
using System.Windows.Controls;

namespace FirstWindows.Interceptors
{
    public class SaveLoadInterceptor: ISaveLoadInterceptor
    {
        public void ReturnFirstWindow(Page page)
        {
            page.NavigationService.Navigate(new ButtonsPage());
        }
    }
}
