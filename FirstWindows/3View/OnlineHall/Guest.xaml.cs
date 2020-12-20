using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// <summary>
    /// Interaktionslogik für Guest.xaml
    /// </summary>
    public partial class Guest : Page
    {
        private OnlineHallModel _ModelView;
        private OnlineHallModel ModelView { 
            set 
            {
                value.PropertyChanged += (sender, arg) =>
                {
                    if (arg.PropertyName == nameof(value.IsInAGameWithAdversary))
                    {
                        void IsInAGameWithAdversary1()
                        {
                            ListGames.GetBindingExpression(ListView.IsEnabledProperty).UpdateTarget();
                        }
                        WpfBasicMechanisms.DispatcherWrapper(IsInAGameWithAdversary1, Dispatcher);
                    }
                };
                value.JoinedToGame += (isJoined) =>
                {
                    void JoinedToGame1()
                    {
                        if (isJoined)
                        {
                            Join.Text = TranslationList.GoOut.ToString();
                        }
                        else
                        {
                            Join.Text = TranslationList.Join.ToString();
                            JoinButton.IsChecked = false;
                        }
                    }
                    WpfBasicMechanisms.DispatcherWrapper(JoinedToGame1, Dispatcher);
                    
                };
                value.OnlineGameChanged += () =>
                {
                    void OnlineGameChanged1()
                    {
                        ListGames.GetBindingExpression(ListView.ItemsSourceProperty).UpdateTarget();
                    }
                    WpfBasicMechanisms.DispatcherWrapper(OnlineGameChanged1, Dispatcher);
                };
                _ModelView = value;
            } 
            get 
            {
                return _ModelView;
            } 
        }

        private ListGuest TranslationList;

        public Guest(OnlineHallModel onlineHallViewModel)
        {
            InitializeComponent();
            ModelView = onlineHallViewModel;
            this.DataContext = ModelView;
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();

            Guest guest = this;
            TranslationList = Translater.ÜbersetzungMeth(new ListGuest(ref guest));
        }
    }
}
