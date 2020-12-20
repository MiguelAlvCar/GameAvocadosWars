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
    /// Interaktionslogik für PopupYesNo.xaml
    /// </summary>
    public partial class PopupYesNo : Page
    {
        Action<object[]> confirmmethod;
        object[] para1;
        public PopupYesNo(string YesButton, string NoButton, string Message, Action<object[]> action, object[] para)
        {
            confirmmethod = action;
            InitializeComponent();
            Yes.Content = YesButton;
            No.Content = NoButton;
            Textbox1.Text = Message;
            para1 = para;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            confirmmethod(para1);
            NavigationService.GetNavigationService(this).Navigate(null);
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(null);
        }
    }
}
