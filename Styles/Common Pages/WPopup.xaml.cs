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
using System.Windows.Navigation;
using Translation;

namespace Styles.CommonPages
{
    /// <summary>
    /// Interaktionslogik für Popup1.xaml
    /// </summary>
    public partial class WPopup : Page
    {
        public WPopup()
        {
            InitializeComponent();
        }

        public WPopup(string nachricht) : this()
        {
            Nachricht = nachricht;
        }

        private string _Nachricht;
        public string Nachricht
        {
            get
            {
                return _Nachricht;
            }
            set
            {
                _Nachricht = value;
                Textbox1.Text = value;
            }
        }

        private void Schliessen(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(null);
        }
    }
}
