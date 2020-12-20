using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewGame.View.Game;
using System.Windows.Navigation;
using Translation;

namespace ViewGame.View.OtherWindowsMain
{
    /// <summary>
    /// Interaktionslogik für Rundewechsel.xaml
    /// </summary>
    public partial class BlueTurnChange : Page
    {
        private GamePage GamePage;
        public BlueTurnChange(GamePage gamePage)
        {
            GamePage = gamePage;

            InitializeComponent();

            BlueTurnChange ListBlueTurn = this;
            Translater.ÜbersetzungMeth(new ListBlueTurn(ref ListBlueTurn));
        }

        private void close(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(null);
            GamePage.autoResetEventEle.Set();
        }
    }
}
