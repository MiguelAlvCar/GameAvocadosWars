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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstWindows.Controller;
using FirstWindows.View.Translation;
using Translation;
using WpfBasicElements;

namespace FirstWindows.View.OnlineHall
{

    public partial class Host : Page
    {
        private OnlineHallModel _modelView;
        public OnlineHallModel ModelView
        {
            get => 
                _modelView;
            set
            {
                value.PropertyChanged += (sender, arg) =>
                {
                    if (arg.PropertyName == nameof(value.IsInAGameWithAdversary))
                    {
                        void IsInAGameWithAdversary1()
                        {
                            StarsGrid.GetBindingExpression(Grid.WidthProperty).UpdateTarget();
                            GuestName.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                        }
                        WpfBasicMechanisms.DispatcherWrapper(IsInAGameWithAdversary1, Dispatcher);
                    }
                };
                value.HostGameCreatedViewEvent += () =>
                {
                    OnlineGameConfirmed.Visibility = Visibility.Visible;
                    OnlineGameNotConfirmed.Visibility = Visibility.Collapsed;
                };
                value.HostGameRemoved += () =>
                {
                    OnlineGameConfirmed.Visibility = Visibility.Collapsed;
                    OnlineGameNotConfirmed.Visibility = Visibility.Visible;
                };
                _modelView = value;
                value.AdversaryComesIn += (isComingIn) =>
                {
                    if (isComingIn)
                    {
                        CommandManager.InvalidateRequerySuggested();
                        StarsGrid.GetBindingExpression(Grid.WidthProperty).UpdateTarget();
                        GuestName.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                    }
                };
            }
        }

        private ListHost TranslationList;

        public Host(OnlineHallModel onlineHallViewModel)
        {
            InitializeComponent();
            ModelView = onlineHallViewModel;
            this.DataContext = ModelView;
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();

            Host host = this;
            TranslationList = Translater.ÜbersetzungMeth(new ListHost(ref host));
        }
    }
}
