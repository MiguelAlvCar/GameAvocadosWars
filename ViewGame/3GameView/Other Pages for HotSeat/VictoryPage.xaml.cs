using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewGame.View.Game;
using ViewGame.View;
using System.Windows.Navigation;
using WpfBasicElements;
using Translation;
using Sound;
using Ninject;
using BasicElements;
using WpfBasicElements.AbstractClasses;

namespace ViewGame.View.OtherWindowsMain
{
    /// <summary>
    /// Interaktionslogik für SiegFenster.xaml
    /// </summary>
    public partial class VictoryPage : Page
    {
        public VictoryList TranslationList;

        public VictoryPage()
        {
            InitializeComponent();
            BasicMechanisms.Kernel.Get<IMusic>().MusicMediaPlayer.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart) delegate { BasicMechanisms.Kernel.Get<IMusic>().MusicMediaPlayer.Stop(); });

            VictoryPage victory = this;
            TranslationList = Translater.ÜbersetzungMeth(new VictoryList(ref victory)/*, BinderUpdater.UpdateTranslationHotSeat*/);
        }

        public VictoryPage(string nachricht) : this()
        {
            Message = nachricht;
        }

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                Textbox1.Text = value;
            }
        }
        private void close(object sender, RoutedEventArgs e)
        {
            ((IPrincipalWindow)Window.GetWindow(this)).MainFrame.Navigate(BasicMechanisms.Kernel.Get<IViewGameInterceptor>().GetFirstButtonsPage());
            BasicMechanisms.Kernel.Get<IMusic>().MusicMediaPlayer.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate { BasicMechanisms.Kernel.Get<IMusic>().MusicMediaPlayer.Play(); });
            NavigationService.GetNavigationService(this).Navigate(null);
        }
    }
}
