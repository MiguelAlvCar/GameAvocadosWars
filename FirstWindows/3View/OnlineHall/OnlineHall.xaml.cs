
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FirstWindows.Controller;
using System.Windows.Navigation;
using FirstWindows.View.Translation;
using FirstWindows.View.InitialDialog;
using Translation;
using OnlineGameChatAndStore;
using System.Threading;
using BasicElements;
using WpfBasicElements.AbstractClasses;
using Ninject;
using WpfBasicElements;
using Ninject.Parameters;
using System;

namespace FirstWindows.View.OnlineHall
{
    public partial class OnlineHallPage : Page
    {
        static private OnlineHallPage _Instance;
        static public OnlineHallPage InstanceForDebugger { 
            get 
            {
                return _Instance;
            } 
            set 
            {
                if (BasicMechanisms.Test)
                    _Instance = value;
            } 
        }

        private OnlineHallModel _modelView;
        public OnlineHallModel ModelView
        {
            get => _modelView;
            set
            {
                value.AdversaryisGone += () =>
                {
                    void AdversaryisGone1()
                    {
                        CommandManager.InvalidateRequerySuggested();
                    }
                    WpfBasicMechanisms.DispatcherWrapper(AdversaryisGone1, Dispatcher);
                    
                };
                    
                value.NavigateToMainPage += () =>
                    NavigationService.GetNavigationService(this).Navigate(new ButtonsPage());
                Chat.EntryAdded += AddChatEntry;
                Unloaded += (a, b) =>
                {
                    Chat.EntryAdded -= AddChatEntry;
                };
                value.MessageSubmitted += () =>
                {
                    NewMessageBox.Text = "";
                };
                value.NavigateToViewGamePage += (onlineManager) =>
                {
                    void NavigateToViewGamePage1()
                    {
                        ((IPrincipalWindow)Window.GetWindow(this)).MainFrame.Navigate(BasicMechanisms.Kernel.Get<ISaveViewGame>(
                        new[] {
                        new ConstructorArgument ("onlineManager", onlineManager)}));
                    }
                    WpfBasicMechanisms.DispatcherWrapper(NavigateToViewGamePage1, Dispatcher);
                };
                value.OnlineGamesStorage.VisibleOnlineGames.CurrentChanged += (sender, args) =>
                {
                    ChatList.ListItems.Clear();
                    if (value.OnlineGamesStorage.CurrentChatGame != null)
                    {
                        foreach (ChatEntry entry in value.OnlineGamesStorage.CurrentChatGame.Chat.GetChatList())
                        {
                            EntryInFlowDocument.Write(entry, ChatList);
                        }
                        ScrollViewer scroll = WpfBasicMechanisms.FindScrollViewer(FlowDocu);
                        if (scroll != null)
                            scroll.ScrollToBottom();

                    }
                };
                

                value.PropertyChanged += (sender, arg) =>
                {
                    OnlineHallModel onlineHallModelView = sender as OnlineHallModel;
                    if (arg.PropertyName == nameof(onlineHallModelView.IsGuest))
                    {
                        if (onlineHallModelView.IsGuest)
                        {
                            OnlineHallFrame.Navigate(new Guest(ModelView));
                        }
                        else
                        {
                            OnlineHallFrame.Navigate(new Host(ModelView));
                        }
                    }
                    else if (arg.PropertyName == nameof(onlineHallModelView.IsAdversaryConfirmed))
                    {
                        if (onlineHallModelView.IsAdversaryConfirmed)
                        {
                            if (BrushButtonToReset == null)
                                BrushButtonToReset = AdverConfirmButton.Background;
                            AdverConfirmButton.Background = Brushes.Orange;
                            ConfirmAdv.Text = TranslationList.DontConfirmAdversaryButton.ToString();
                        }
                        else
                        {
                            AdverConfirmButton.Background = BrushButtonToReset;
                            ConfirmAdv.Text = TranslationList.ConfirmAdversaryButton.ToString();
                        }
                    }
                };

                value.AdversaryConfirms += (hasAdversaryConfirmed) => 
                {
                    void AdversaryConfirms()
                    {
                        if (hasAdversaryConfirmed)
                        {
                            if (BrushButtonToReset == null)
                                BrushButtonToReset = AdverConfirmButton.Background;
                            AdverConfirmButton.Background = Brushes.Green;
                            ConfirmAdv.Text = TranslationList.AdversaryHasConfirmed.ToString();
                        }
                        else
                        {
                            AdverConfirmButton.Background = BrushButtonToReset;
                            ConfirmAdv.Text = TranslationList.ConfirmAdversaryButton.ToString();
                        }
                    }

                    WpfBasicMechanisms.DispatcherWrapper(AdversaryConfirms, Dispatcher);
                };

                _modelView = value;
            }
        }

        private ListOnlineHall TranslationList;

        public OnlineHallPage(OnlineHallModel onlineHallViewModel)
        {
            InstanceForDebugger = this;
            InitializeComponent();
            ModelView = onlineHallViewModel;
            this.DataContext = ModelView;
            CommandBinding SubmitMessageCommandBinding = new CommandBinding(ModelView.SubmitMessage, ModelView.SubmitMessageExecuted, ModelView.SubmitMessageCanExecute);
            this.CommandBindings.Add(SubmitMessageCommandBinding);
            CommandBinding ConfirmDescriptionCommandBinding = new CommandBinding(ModelView.ConfirmDescription, ModelView.ConfirmDescriptionExecuted, ModelView.ConfirmDescriptionCanExecute);
            this.CommandBindings.Add(ConfirmDescriptionCommandBinding);

            ModelView.IsGuest = true;

            OnlineHallPage onlineHall = this;
            TranslationList = Translater.ÜbersetzungMeth(new ListOnlineHall (ref onlineHall));
        }

        private Brush BrushButtonToReset;

        private void HostGame_Click(object sender, RoutedEventArgs e)
        {
            NewMessageBox.Text = "";
        }

        private void AddChatEntry (ChatEntry entry) 
        {
            void AddChatEntry1()
            {
                EntryInFlowDocument.Write(entry, ChatList);
                ScrollViewer scroll = WpfBasicMechanisms.FindScrollViewer(FlowDocu);
                if (scroll != null)
                    scroll.ScrollToBottom();
            }

            WpfBasicMechanisms.DispatcherWrapper(AddChatEntry1, Dispatcher);
        }
    }
}
