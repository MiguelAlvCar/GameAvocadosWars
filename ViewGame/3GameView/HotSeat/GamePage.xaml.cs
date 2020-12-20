using ArmyAndUnitTypes;
using BasicElements;
using ViewGame.View.OtherWindowsMain;
using ViewGame.View.Resources;
using ViewGame.View;
using Styles.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Translation;
using Ninject;
using WpfBasicElements.AbstractClasses;
using Styles.CommonPages;
using BasicElements.AbstractClasses;
using OnlineGameChatAndStore;
using ViewModelHotSeat;
using ViewModelOnlineGame;
using WpfBasicElements;

namespace ViewGame.View.Game
{
    public partial class GamePage: Page, ISaveViewGame
    {      

        public AutoResetEvent autoResetEventEle = new AutoResetEvent(false);

        private OnlineGameViewModel _OnlineViewModel;
        private OnlineGameViewModel OnlineViewModel
        {
            get => _OnlineViewModel;
            set 
            {
                Zum_Heer_2.Text = TranslationList.ToBattle.ToString();

                foreach (ChatEntry entry in value.OnlineManager.ChatGame.Chat.GetChatList())
                {
                    EntryInFlowDocument.Write(entry, ChatList);
                }
                ScrollViewer scroll = WpfBasicMechanisms.FindScrollViewer(FlowDocu);
                if (scroll != null)
                    scroll.ScrollToBottom();

                CommandBinding SubmitMessageCommandBinding = new CommandBinding(value.SubmitMessage, value.SubmitMessageExecuted, value.SubmitMessageCanExecute);
                this.CommandBindings.Add(SubmitMessageCommandBinding);
                value.MessageSubmitted += () =>
                {
                    NewMessageBox.Text = "";
                };
                Chat.EntryAdded += AddChatEntry;
                Unloaded += (a, b) =>
                {
                    Chat.EntryAdded -= AddChatEntry;
                };

                TextboxPlayer1.Text = value.OnlineManager.ChatGame.Host.Name;
                TextboxPlayer2.Text = value.OnlineManager.ChatGame.Guest.Name;

                value.Player1 = value.OnlineManager.ChatGame.Host.Name;
                value.Player2 = value.OnlineManager.ChatGame.Guest.Name;

                value.PropertyChanged += (sender, arg) =>
                {
                    OnlineGameViewModel onlineViewModel = sender as OnlineGameViewModel;
                    if (arg.PropertyName == nameof(onlineViewModel.IsMapConfirmed))
                    {
                        if (onlineViewModel.IsMapConfirmed)
                        {
                            if (BrushButtonToReset == null)
                                BrushButtonToReset = ToCreateArmiesButton.Background;
                            ToCreateArmiesButton.Background = Brushes.Orange;
                            ToCreateArmies.Text = TranslationList.IsConfirmed.ToString();
                        }
                        else
                        {
                            ToCreateArmiesButton.Background = BrushButtonToReset;
                            ToCreateArmies.Text = TranslationList.ToCreateArmies.ToString();
                        }
                    }
                    else if (arg.PropertyName == nameof(onlineViewModel.HasAdversaryConfirmedMap))
                    {
                        void asAdversaryConfirmedMap1()
                        {
                            if (onlineViewModel.HasAdversaryConfirmedMap)
                            {
                                if (BrushButtonToReset == null)
                                     BrushButtonToReset = ToCreateArmiesButton.Background;
                                ToCreateArmiesButton.Background = Brushes.Green;
                                ToCreateArmies.Text = TranslationList.AdversaryHasConfirmed.ToString();
                            }
                            else
                            {
                                ToCreateArmiesButton.Background = BrushButtonToReset;
                                ToCreateArmies.Text = TranslationList.ToCreateArmies.ToString();
                            }
                        }

                        WpfBasicMechanisms.DispatcherWrapper(asAdversaryConfirmedMap1, Dispatcher);
                    }
                    else if (arg.PropertyName == nameof(onlineViewModel.IsArmyConfirmed))
                    {
                        Button[] buttons = new Button[] { ToArmy2Button, ToBattleButton };
                        foreach (Button button in buttons)
                        {
                            if (onlineViewModel.IsArmyConfirmed)
                            {
                                if (BrushButtonToReset == null)
                                    BrushButtonToReset = button.Background;
                                button.Background = Brushes.Orange;
                                ((TextBlock)button.Content).Text = TranslationList.IsConfirmed.ToString();
                            }
                            else
                            {
                                button.Background = BrushButtonToReset;
                                ((TextBlock)button.Content).Text = TranslationList.ToBattle.ToString();
                            }
                        }
                    }
                    else if (arg.PropertyName == nameof(onlineViewModel.HasAdversaryConfirmedArmy))
                    {
                        void HasAdversaryConfirmedArmy1()
                        {
                            Button[] buttons = new Button[] { ToArmy2Button, ToBattleButton };
                            foreach (Button button in buttons)
                            {
                                if (onlineViewModel.HasAdversaryConfirmedArmy)
                                {
                                    if (BrushButtonToReset == null)
                                        BrushButtonToReset = button.Background;
                                    button.Background = Brushes.Green;
                                    ((TextBlock)button.Content).Text = TranslationList.AdversaryHasConfirmed.ToString();
                                }
                                else
                                {
                                    button.Background = BrushButtonToReset;
                                    ((TextBlock)button.Content).Text = TranslationList.ToBattle.ToString();
                                }
                            }
                        }

                        WpfBasicMechanisms.DispatcherWrapper(HasAdversaryConfirmedArmy1, Dispatcher);

                        
                        
                    }
                };
                value.NotificationAdversaryOffline += () =>
                {
                    void NotificationAdversaryOffline1()
                    {
                        HotSeatFrame.Navigate(new WPopup(TranslationList.AdversaryOffline.ToString()));
                    }

                    WpfBasicMechanisms.DispatcherWrapper(NotificationAdversaryOffline1, Dispatcher);
                };

                _OnlineViewModel = value;
            }
            
        }
        private HotSeatViewModel _ViewModel;
        public HotSeatViewModel ViewModel
        {
            get {
                if (_ViewModel == null)
                    return _OnlineViewModel;
                else
                    return _ViewModel;
            }
            set
            {
                if (value != null)
                {
                    value.PropertyChanged += (sender, arg) =>
                    {
                        HotSeatViewModel mainWindowMV = sender as HotSeatViewModel;
                        if (arg.PropertyName == nameof(mainWindowMV.Gamestate))
                        {
                            void Gamestate1()
                            {
                                if (mainWindowMV.Gamestate == GamestateMV.CreatingMap)
                                {
                                    Karte.IsEnabled = true;
                                    Heer1.IsEnabled = false;
                                    Heer2.IsEnabled = false;
                                    Karte.IsSelected = true;
                                    PanelButtonUnitInfo.Visibility = Visibility.Collapsed;
                                    NextTurn.Visibility = Visibility.Collapsed;
                                    TabControlCreateMap.Visibility = Visibility.Visible;
                                    TurnCounter.Visibility = Visibility.Collapsed;
                                    MadeByMiguelAndBackGround.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    if (UpdateAfterCreatingMap != null)
                                        UpdateAfterCreatingMap();
                                    if (mainWindowMV.Gamestate == GamestateMV.DeployingRedUnits)
                                    {
                                        foreach (ViewTerrain terr in ListTerrains)
                                        {
                                            terr.Unit = null;
                                        }
                                        HoeheTopRasterBerechnen();
                                        Karte.IsEnabled = false;
                                        Heer1.IsEnabled = true;
                                        Heer2.IsEnabled = false;
                                        Heer1.IsSelected = true;
                                        PanelButtonUnitInfo.Visibility = Visibility.Visible;
                                        NextTurn.Visibility = Visibility.Collapsed;
                                        TabControlCreateMap.Visibility = Visibility.Visible;
                                        TurnCounter.Visibility = Visibility.Collapsed;
                                        Army1Selecter.GetBindingExpression(UnitSelecterPanel.ArmyTypeProperty).UpdateTarget();
                                        Army1Selecter.GetBindingExpression(UnitSelecterPanel.InitialPointsProperty).UpdateTarget();
                                    }
                                    else if (mainWindowMV.Gamestate == GamestateMV.DeployingBlueUnits)
                                    {
                                        HoeheTopRasterBerechnen();
                                        Karte.IsEnabled = false;
                                        Heer1.IsEnabled = false;
                                        Heer2.IsEnabled = true;
                                        Heer2.IsSelected = true;
                                        PanelButtonUnitInfo.Visibility = Visibility.Visible;
                                        NextTurn.Visibility = Visibility.Collapsed;
                                        TabControlCreateMap.Visibility = Visibility.Visible;
                                        TurnCounter.Visibility = Visibility.Collapsed;
                                        if (! (ViewModel is OnlineGameViewModel))
                                            HotSeatFrame.Navigate(new BlueTurnChange(this));
                                        Army2Selecter.GetBindingExpression(UnitSelecterPanel.ArmyTypeProperty).UpdateTarget();
                                        Army2Selecter.GetBindingExpression(UnitSelecterPanel.InitialPointsProperty).UpdateTarget();
                                    }
                                    else
                                    {
                                        HoeheTopRasterBerechnen();
                                        PanelButtonUnitInfo.Visibility = Visibility.Visible;
                                        NextTurn.Visibility = Visibility.Visible;
                                        TabControlCreateMap.Visibility = Visibility.Collapsed;
                                        MadeByMiguelAndBackGround.Visibility = Visibility.Collapsed;
                                        TurnCounter.Visibility = Visibility.Visible;
                                        TurnCounterText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                                    }
                                }
                            }
                            WpfBasicMechanisms.DispatcherWrapper(Gamestate1, Dispatcher);
                        }
                        else if (arg.PropertyName == nameof(mainWindowMV.selectedUnit))
                        {
                            void SelectedUnit1()
                            {
                                if (mainWindowMV.selectedUnit != null)
                                {
                                    if (mainWindowMV.selectedUnit.ModifierUnitViewModel == Modifier.Flight)
                                    {
                                        IEffects iEffects = BasicMechanisms.Kernel.Get<IEffects>();
                                        iEffects.Play(StandardEffects.Scream.EffectUri, iEffects.AttackEffectsMediaPlayer);
                                    }

                                ((ViewUnit)mainWindowMV.selectedUnit.OnGetViewUnitFromViewModelUnit()).Selected = true;

                                    UnitInfoButton.IsEnabled = true;

                                    if (mainWindowMV.selectedUnit.IsRangeUnit)
                                    {
                                        if (mainWindowMV.selectedUnit.Unit.ArmyAffiliation == ViewModel.ColorTurn
                                            && mainWindowMV.IsLocalPlayerInHisOwnTurn)
                                        {
                                            Fire.Visibility = Visibility.Visible;

                                            Fire.IsChecked = false;
                                        }
                                        PanelRangeUnit.Visibility = Visibility.Visible;
                                    }
                                    else
                                    {
                                        Fire.Visibility = Visibility.Collapsed;
                                        PanelRangeUnit.Visibility = Visibility.Collapsed;
                                    }

                                    if (mainWindowMV.selectedUnit.HasSpecialStregth)
                                        SpecialStrengthText.Visibility = Visibility.Visible;
                                    else
                                        SpecialStrengthText.Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    CollapsePanelUnitInfo();
                                    UnitInfoButton.IsEnabled = false;
                                }
                            }

                            WpfBasicMechanisms.DispatcherWrapper(SelectedUnit1, Dispatcher);
                            
                        }
                        else if (arg.PropertyName == nameof(mainWindowMV.turn))
                        {
                            void ColorTurn1()
                            {
                                TurnCounterText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                            }

                            WpfBasicMechanisms.DispatcherWrapper(ColorTurn1, Dispatcher);
                        }
                        else if (arg.PropertyName == nameof(mainWindowMV.ColorTurn))
                        {
                            void ColorTurn1()
                            {
                                NextTurn.GetBindingExpression(Button.IsEnabledProperty).UpdateTarget();
                                Thread thread = new Thread(() =>
                                {
                                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                                    {
                                        if (ViewModel.ColorTurn == ArmyColor.Blue)
                                        {
                                            HotSeatFrame.Navigate(new BlueTurnChange(this));
                                        }
                                        else
                                        {
                                            HotSeatFrame.Navigate(new RedTurnChange(this));
                                        }
                                    });

                                });
                                thread.Start();
                            }

                            WpfBasicMechanisms.DispatcherWrapper(ColorTurn1, Dispatcher);
                            
                        }
                        else if (arg.PropertyName == nameof(mainWindowMV.Player1))
                        {
                            foreach (object item in ComboboxPlayer1.Items)
                            {
                                if (Convert.ToString(item) == mainWindowMV.Player1)
                                    ComboboxPlayer1.SelectedItem = item;
                            }
                            txtbox1.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                        }
                        else if (arg.PropertyName == nameof(mainWindowMV.Player2))
                        {
                            foreach (object item in ComboboxPlayer2.Items)
                            {
                                if (Convert.ToString(item) == mainWindowMV.Player2)
                                    ComboboxPlayer2.SelectedItem = item;
                            }
                        }
                        else if (arg.PropertyName == nameof(mainWindowMV.Player1Points) ||
                            arg.PropertyName == nameof(mainWindowMV._Player1Points))
                        {
                            void Player1Points1()
                            {
                                txtbox1.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                            }
                            
                            WpfBasicMechanisms.DispatcherWrapper(Player1Points1, Dispatcher);
                        }
                        else if (arg.PropertyName == nameof(mainWindowMV.Player2Points) ||
                            arg.PropertyName == nameof(mainWindowMV._Player2Points))
                        {
                            void Player2Points1()
                            {
                                txtbox2.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                            }
                            
                            WpfBasicMechanisms.DispatcherWrapper(Player2Points1, Dispatcher);
                        }
                        else if (arg.PropertyName == nameof(mainWindowMV.Army1) ||
                            arg.PropertyName == nameof(mainWindowMV._Army1))
                        {
                            void Army11()
                            {
                                foreach (object item in Army1Select.Items)
                                {
                                    if (((ArmyType)item).ArmyID == mainWindowMV.Army1.ArmyID)
                                        Army1Select.SelectedItem = item;
                                }
                            }
                            
                            WpfBasicMechanisms.DispatcherWrapper(Army11, Dispatcher);
                        }
                        else if (arg.PropertyName == nameof(mainWindowMV.Army2) ||
                            arg.PropertyName == nameof(mainWindowMV._Army2))
                        {
                            void Army21()
                            {
                                foreach (object item in Army2Select.Items)
                                {
                                    if (((ArmyType)item).ArmyID == mainWindowMV.Army2.ArmyID)
                                        Army2Select.SelectedItem = item;
                                }
                            }
                            
                            WpfBasicMechanisms.DispatcherWrapper(Army21, Dispatcher);
                        }
                    };
                    value.OpenPanelUnitInfoSelectingUnit += () => 
                        PanelUnitInfo.Visibility = Visibility.Visible;
                    value.LenghtWidthChanged += (len, wid, fromSaved) =>
                    {
                        void LenghtWidthChanged1()
                        {
                            // for the changes from internet
                            sliderlength.Value = len;
                            sliderwidth.Value = wid;

                            Raster.Children.Clear();
                            if (!fromSaved)
                                ViewModel.RemoveModelMap();
                            ListTerrains.Clear();

                            Raster.SetValue(Canvas.WidthProperty, Convert.ToDouble((20 * 5 * wid) + 10 * 5));
                            Raster.SetValue(Canvas.HeightProperty, Convert.ToDouble((18 * 5 * len) + 6 * 5));
                            Raster.Width = ViewModel.mapWidth * 100 + 50.0;

                            System.Timers.Timer aTimer;
                            aTimer = new System.Timers.Timer(150);
                            aTimer.Elapsed += (sender34, e34) =>
                                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                                {
                                    TotalBreite = Raster.Margin.Right + Raster.Margin.Left +
                                    Raster.Width * Raster.LayoutTransform.Value.M11;
                                    HoeheRasterBerechnen();
                                });
                            aTimer.AutoReset = false;
                            aTimer.Start();
                        }

                        WpfBasicMechanisms.DispatcherWrapper(LenghtWidthChanged1, Dispatcher);                        
                    };
                    value.RemoveSelectedUnit += (selected) =>
                    {
                        // In order to not get a exception from OnGetView=NoInstance
                        void Selected1()
                        {
                            if (selected != null)
                                ((ViewUnit)selected.OnGetViewUnitFromViewModelUnit()).Selected = false;
                        }

                        WpfBasicMechanisms.DispatcherWrapper(Selected1, Dispatcher);
                        
                    };
                    value.NotAllRedUnitDeployed += () =>
                    {
                        WPopup pop = new WPopup(TranslationList.PleaseDeploy.ToString());
                        HotSeatFrame.Navigate(pop);
                        return;
                    };
                    value.NoUnitsDeployed += () =>
                    {
                        WPopup pop = new WPopup(TranslationList.PleaseNoUnits.ToString());
                        HotSeatFrame.Navigate(pop);
                        return;
                    };
                    value.TerrainCreated += (y, x, VMTerr) =>
                    {
                        void TerrainCreated1()
                        {
                            GamePage hotseat = this;
                            ViewTerrain terrView = new ViewTerrain(y, x, VMTerr, hotseat);

                            short gerade;
                            if ((y % 2 < -0.1 || y % 2 < 0.1))
                            {
                                gerade = 10 * 5;
                            }
                            else
                            {
                                gerade = 0;
                            }
                            terrView.SetValue(Canvas.LeftProperty, (double)0 + (x - 1) * 100 + gerade);
                            terrView.SetValue(Canvas.TopProperty, (double)20 + (y - 1) * 90);

                            Raster.Children.Add(terrView);
                        }

                        WpfBasicMechanisms.DispatcherWrapper(TerrainCreated1, Dispatcher);
                    };
                    value.UnitCreated += (uni) => {

                        void UnitCreated1()
                        {
                            HotSeatViewModel viewModel = _ViewModel ?? _OnlineViewModel;
                            new ViewUnit(viewModel, uni);
                        }
                        WpfBasicMechanisms.DispatcherWrapper(UnitCreated1, Dispatcher);

                    };
                    value.ViewModelAfterNextTurn += () => {
                        if (BasicMechanisms.Kernel.CanResolve<INextTurnViewInterceptor>())
                            BasicMechanisms.Kernel.Get<INextTurnViewInterceptor>().OnChangedToNextTurnInHotSeat(this); 
                    };
                    value.attackerEffect += (uri) =>
                    {
                        void AttackerEffect1()
                        {
                            IEffects iEffects = BasicMechanisms.Kernel.Get<IEffects>();
                            iEffects.Play(uri, iEffects.AttackEffectsMediaPlayer);
                        }
                        WpfBasicMechanisms.DispatcherWrapper(AttackerEffect1, Dispatcher);
                    };
                    value.defenderEffect += (uri) =>
                    {
                        void DefenderEffect1()
                        {
                            IEffects iEffects = BasicMechanisms.Kernel.Get<IEffects>();
                            iEffects.Play(uri, iEffects.AttackEffectsMediaPlayer);
                        }
                        WpfBasicMechanisms.DispatcherWrapper(DefenderEffect1, Dispatcher);
                        
                    };
                    value.Win += (namePlayer) =>
                    {
                        void UnitCreated1()
                        {
                            VictoryPage victoryWindow = new VictoryPage(TranslationList.GloriousVictory1 + namePlayer + TranslationList.GloriousVictory2);
                            HotSeatFrame.Navigate(victoryWindow);
                        }
                        WpfBasicMechanisms.DispatcherWrapper(UnitCreated1, Dispatcher);
                        
                    };

                    if (value is OnlineGameViewModel)
                    {
                        OnlineViewModel = (OnlineGameViewModel)value;
                        return;
                    }

                    _ViewModel = value;
                }                
            }
        }
        public IHotSeatViewModel IViewModel
        {
            set => ViewModel = (HotSeatViewModel)value;
            get => ViewModel;
        }

        public event Action UpdateAfterCreatingMap;

        public HotSeatList TranslationList;
        public GamePage(HotSeatViewModel modelView = null, OnlineManager onlineManager = null)
        {
            try
            {
                if (modelView == null)
                {
                    if (onlineManager != null)
                        modelView = new OnlineGameViewModel(onlineManager);
                    else
                        modelView = new HotSeatViewModel();
                }
                InitializeComponent();

                TopRaster.SizeChanged += (a, b) => HoeheTopRasterBerechnen();
                Raster.MouseLeave += MapMouseLeave;

                this.Loaded += (sender, e) =>
                {
                    Window.GetWindow(this).Background = BasicMechanisms.Kernel.Get<IViewGameInterceptor>().GetWallpaper();
                };

                TranslationList = Translater.ÜbersetzungMeth(new HotSeatList(this));

                ViewModel = modelView;
                this.DataContext = ViewModel;
                HideFlowDocumentButton.Content = TranslationList.HideFlowDocument.ToString();
            }
            catch (Exception e)
            {
                string CurrentDirectory = System.Environment.CurrentDirectory;
                string Directory = CurrentDirectory.Substring(0, 2);
                FileStream filest = new FileStream(Directory + "\\Miguel.txt", FileMode.Create);
                BinaryWriter BinWriter = new BinaryWriter(filest);
                BinWriter.Seek(0, SeekOrigin.Begin);
                BinWriter.Write("MainWindow \n Exception: " + e + "\nMessage: " + e.Message + "\nSource: " + e.Source + "\nTarget: " + e.TargetSite + "\nData: " + e.Data);
                BinWriter.Close();
                throw e;
            }
        }

        public void Translate()
        {
            TranslationList = Translater.ÜbersetzungMeth(new HotSeatList(this));
            BinderUpdater.UpdateTranslationHotSeat(this);
        }


        #region Other clicks and events (map slip)

        #region Events map slip

        private System.Timers.Timer aTimer;

        private void KarteAus(object sender, MouseEventArgs e)
        {
            aTimer.AutoReset = false;
        }

        public TranslateTransform VerschiebenTranform = new TranslateTransform();

        public double Xtransform = 0;
        public double Ytransform = 0;

        internal double TotalBreite = 400;
        internal double TotalHohe = 400;
        internal double TotalHoheTopRaster = 0;
        internal double TotalHoheRaster = 0;

        double Bildschirmbreite = System.Windows.SystemParameters.PrimaryScreenWidth;
        double Bildschirmhoehe = System.Windows.SystemParameters.PrimaryScreenHeight;

        public void HoeheTopRasterBerechnen()
        {
            double BottomHeight;
            if (NextTurn.ActualHeight + NextTurn.Margin.Top + NextTurn.Margin.Bottom > PanelStartAndUnitInfo.ActualHeight + PanelStartAndUnitInfo.Margin.Top + PanelStartAndUnitInfo.Margin.Bottom)
                BottomHeight = NextTurn.ActualHeight + NextTurn.Margin.Top + NextTurn.Margin.Bottom;
            else
                BottomHeight = PanelStartAndUnitInfo.ActualHeight + PanelStartAndUnitInfo.Margin.Top + PanelStartAndUnitInfo.Margin.Bottom;

            TotalHoheTopRaster = TopRaster.ActualHeight + BottomHeight + Raster.Margin.Top;
            TotalHoeheBerechnen();
        }
        public void HoeheRasterBerechnen()
        {
            TotalHoheRaster = Raster.ActualHeight * Raster.LayoutTransform.Value.M11;
            TotalHoeheBerechnen();
        }
        public void TotalHoeheBerechnen()
        {
            TotalHohe = TotalHoheTopRaster + TotalHoheRaster;
        }

        void VerschiebenEvent(object sender, ElapsedEventArgs e)
        {
            Raster.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
            {
                double TotalverschiebungX = VerschiebenTranform.X + Xtransform;
                if (VerschiebenTranform.X + Xtransform <= 100)
                {
                    if (TotalBreite + TotalverschiebungX + 100 > Bildschirmbreite || (VerschiebenTranform.X < 0 && Xtransform > 0))
                    {
                        if (Xtransform >= 0)
                            VerschiebenTranform.X = TotalverschiebungX;
                        else
                            VerschiebenTranform.X = TotalverschiebungX;
                    }
                }

                double TotalverschiebungY = VerschiebenTranform.Y + Ytransform;
                if (TotalverschiebungY <= 20)
                {
                    if (Ytransform >= 0)
                        VerschiebenTranform.Y = TotalverschiebungY;
                    else
                    {
                        if (TotalHohe + TotalverschiebungY + 20 > Bildschirmhoehe)
                            VerschiebenTranform.Y = TotalverschiebungY;
                    }

                }
                Raster.RenderTransform = VerschiebenTranform;
            });
        }

        private void KarteRechts(object sender, MouseEventArgs e)
        {
            Xtransform = -20;
            Ytransform = 0;
            aTimer = new System.Timers.Timer(15);
            aTimer.Elapsed += VerschiebenEvent;
            aTimer.AutoReset = true;
            aTimer.Start();
        }

        private void KarteLinks(object sender, MouseEventArgs e)
        {
            Xtransform = 20;
            Ytransform = 0;
            aTimer = new System.Timers.Timer(15);
            aTimer.Elapsed += VerschiebenEvent;
            aTimer.AutoReset = true;
            aTimer.Start();
        }

        private void KarteUnten(object sender, MouseEventArgs e)
        {
            Xtransform = 0;
            Ytransform = -15;
            aTimer = new System.Timers.Timer(15);
            aTimer.Elapsed += VerschiebenEvent;
            aTimer.AutoReset = true;
            aTimer.Start();
        }

        private void KarteOben(object sender, MouseEventArgs e)
        {
            Xtransform = 0;
            Ytransform = 15;
            aTimer = new System.Timers.Timer(15);
            aTimer.Elapsed += VerschiebenEvent;
            aTimer.AutoReset = true;
            aTimer.Start();
        }

        private void KarteRechtsUnten(object sender, MouseEventArgs e)
        {
            Xtransform = -20;
            Ytransform = -15;
            aTimer = new System.Timers.Timer(15);
            aTimer.Elapsed += VerschiebenEvent;
            aTimer.AutoReset = true;
            aTimer.Start();
        }

        private void KarteRechtsOben(object sender, MouseEventArgs e)
        {
            Xtransform = -20;
            Ytransform = 15;
            aTimer = new System.Timers.Timer(15);
            aTimer.Elapsed += VerschiebenEvent;
            aTimer.AutoReset = true;
            aTimer.Start();
        }

        private void KarteLinksUnten(object sender, MouseEventArgs e)
        {
            Xtransform = 20;
            Ytransform = -15;
            aTimer = new System.Timers.Timer(15);
            aTimer.Elapsed += VerschiebenEvent;
            aTimer.AutoReset = true;
            aTimer.Start();
        }

        private void KarteLinksOben(object sender, MouseEventArgs e)
        {
            Xtransform = 20;
            Ytransform = 15;
            aTimer = new System.Timers.Timer(15);
            aTimer.Elapsed += VerschiebenEvent;
            aTimer.AutoReset = true;
            aTimer.Start();
        }

        #endregion        

        double RasteScale = 1;

        double ScaleRate = 1.1;

        private void Raster_MouseWheel(object sender, MouseWheelEventArgs e)
        {            
            if (e.Delta > 0)
            {
                pt.ScaleX *= ScaleRate;
                RasteScale = pt.ScaleX;
                pt.ScaleY *= ScaleRate;
                //st.ScaleX *= ScaleRate;
                //st.ScaleY *= ScaleRate;
                //sst.ScaleX *= ScaleRate;
                //sst.ScaleY *= ScaleRate;
            }
            else
            {
                pt.ScaleX /= ScaleRate;
                RasteScale = pt.ScaleX;
                pt.ScaleY /= ScaleRate;
                //st.ScaleX /= ScaleRate;
                //st.ScaleY /= ScaleRate;
                //sst.ScaleX /= ScaleRate;
                //sst.ScaleY /= ScaleRate;
            }
            TotalBreite = Raster.Margin.Right + Raster.Margin.Left +
            Raster.ActualWidth * Raster.LayoutTransform.Value.M11;
            HoeheRasterBerechnen();
        }

        public int Rundezahl { get; set; } = 1;
        
        public int richtung;

        void MapMouseLeave(object sender, MouseEventArgs e)
        {
            ViewModel.movFocusedTerrain = null;
            //ViewModel.movFocusedTerrain = null;
        }

        private void HideFlowDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = HideablePanelFlowDocument.Visibility == Visibility.Visible; ;
            if (isChecked)
            {
                HideFlowDocumentButton.Content = TranslationList.ShowFlowDocument.ToString();
                HideablePanelFlowDocument.Visibility = Visibility.Collapsed;
            }
            else
            {
                HideFlowDocumentButton.Content = TranslationList.HideFlowDocument.ToString();
                HideablePanelFlowDocument.Visibility = Visibility.Visible;
            }
        }

        private void AddChatEntry(ChatEntry entry)
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

        private Brush BrushButtonToReset;

        #endregion

        #region Map

        #region Map-1th zone: Draw Map and Terrain Collection

        private void StartClick(object sender, RoutedEventArgs e)
        {
            HotSeatFrame.Navigate(BasicMechanisms.Kernel.Get<IViewGameInterceptor>().GetStartPage(this));
        }

        public List<ViewTerrain> ListTerrains = new List<ViewTerrain>();        

        private void DrawMapGrid(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateMap(Convert.ToInt16(sliderlength.Value), Convert.ToInt16(sliderwidth.Value));
            Raster_Zeichnen.HasError = false;
        }


        #endregion

        #region Map-2nd zone

        private void Terrain(object sender, RoutedEventArgs e)
        {
            foreach (ToggleButton tou in ToggleButtongrid.Children)
            {
                tou.IsChecked = false;
            }

            ToggleButton a = sender as ToggleButton;
            a.IsChecked = true;
        }

        #endregion

        #region Map-3rd zone

        private void Neuer_SpielerMeth(object sender, RoutedEventArgs e)
        {
            NewPlayerPage newPlayer = new NewPlayerPage(this);
            HotSeatFrame.Navigate(newPlayer);
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            txtbox1.TextChanged += BindingToPlayer2;
            txtbox2.TextChanged += BindingToPlayer1;
            txtbox2.Text = txtbox1.Text;
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            txtbox1.TextChanged -= BindingToPlayer2;
            txtbox2.TextChanged -= BindingToPlayer1;
        }

        public void BindingToPlayer1(object sender, RoutedEventArgs e)
        {
            txtbox1.Text = ((ValidatedTextBox)sender).Text;
        }

        public void BindingToPlayer2(object sender, RoutedEventArgs e)
        {
            txtbox2.Text = ((ValidatedTextBox)sender).Text;
        }

        private void ToArmyCreation(object sender, RoutedEventArgs e)
        {       
            txtbox1.GetBindingExpression(ValidatedTextBox.TextProperty).UpdateSource();
            txtbox2.GetBindingExpression(ValidatedTextBox.TextProperty).UpdateSource();
            ComboboxPlayer1.GetBindingExpression(ValidatedComboBox.SelectedItemProperty).UpdateSource();
            ComboboxPlayer2.GetBindingExpression(ValidatedComboBox.SelectedItemProperty).UpdateSource();
            Army1Select.GetBindingExpression(ValidatedComboBox.SelectedItemProperty).UpdateSource();
            Army2Select.GetBindingExpression(ValidatedComboBox.SelectedItemProperty).UpdateSource();

            if (Validation.GetHasError(Karte) || Validation.GetHasError(txtbox2) || Validation.GetHasError(txtbox1) ||
                Validation.GetHasError(ComboboxPlayer1) || Validation.GetHasError(ComboboxPlayer2) ||
                Validation.GetHasError(Army1Select) || Validation.GetHasError(Army2Select))
            {
                if (Raster.Children.Count == 0)
                {
                    Raster_Zeichnen.HasError = true;
                    Raster_Zeichnen.ErrorMessage = TranslationList.BitteRaster.ToString();
                }
                return;
            }

            if (Raster.Children.Count == 0)
            {
                Raster_Zeichnen.HasError = true;
                Raster_Zeichnen.ErrorMessage = TranslationList.BitteRaster.ToString();
                return;
            }

            if (!(ViewModel is OnlineGameViewModel))
                ViewModel.ChangeGamestate(GamestateMV.DeployingRedUnits);
            else
            {
                if (OnlineViewModel != null)
                    OnlineViewModel.IsMapConfirmed = !OnlineViewModel.IsMapConfirmed;
            }
        }

        #endregion

        #endregion


        #region DeployUnits

        private void UncheckedEinheitsinfo(object sender, RoutedEventArgs e)
        {
            PanelUnitInfo.Visibility = Visibility.Visible;
        }

        private void CheckedEinheitsinfo(object sender, RoutedEventArgs e)
        {
            CollapsePanelUnitInfo();
        }

        internal void CollapsePanelUnitInfo()
        {
            PanelUnitInfo.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}
