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
using System.Windows.Controls.Primitives;
using ViewGame.View.Game;
using BasicElements;
using ViewModelHotSeat;
using WpfBasicElements;


namespace ViewGame.View.Resources
{
    public partial class ViewTerrain : UserControl
    {

        ViewModelTerrain _MVterrain;
        public ViewModelTerrain MVterrain
        {
            get => _MVterrain;
            set
            {
                _MVterrain = value;

                if (value == null)
                    return;

                _MVterrain.PropertyChanged += (sender, arg) =>
                {
                    ViewModelTerrain terr = sender as ViewModelTerrain;
                    if (arg.PropertyName == nameof(terr.TerrainType))
                    {
                        void TerrainType1()
                        {
                            typeTerrainView = ViewTerrainType.terrainTypes[value.TerrainType];
                        }
                        WpfBasicMechanisms.DispatcherWrapper(TerrainType1, Dispatcher);
                        
                    }
                    else if (arg.PropertyName == nameof(terr.unit))
                    {
                        void Unit1()
                        {
                            if (terr.unit != null)
                            {
                                ViewUnit unit = terr.unit.OnGetViewUnitFromViewModelUnit() as ViewUnit;
                                unitview = unit;
                                if (HotSeat.ViewModel.Gamestate == GamestateMV.Battle)
                                    BindingOperations.GetMultiBindingExpression(HotSeat.MovementText, TextBlock.TextProperty).UpdateTarget();
                            }
                        }
                        WpfBasicMechanisms.DispatcherWrapper(Unit1, Dispatcher);
                        
                    }
                    else if (arg.PropertyName == nameof(terr.MovementFocused))
                    {
                        focusedmovement = value.MovementFocused;
                    }
                    else if (arg.PropertyName == nameof(terr.FireFocused))
                    {
                        focusedfire = value.FireFocused;
                    }
                    else if (arg.PropertyName == nameof(terr.FirePosible))
                    {
                        fireposible = value.FirePosible;
                    }
                    else if (arg.PropertyName == nameof(terr.MovementPosible))
                    {
                        movementposible = value.MovementPosible;
                    }
                    else if (arg.PropertyName == nameof(terr.MultipleAttack))
                    {
                        IsAttackedByMultiple = value.MultipleAttack;
                    }
                    else if (arg.PropertyName == nameof(terr.Hide))
                    {
                        Hide = value.Hide;
                    }
                };
                _MVterrain.DeploymentAreaSet += (colorDeployment) => 
                {
                    void DeploymentAreaSet1()
                    {
                        deploymentAreaView = colorDeployment;
                    }
                    WpfBasicMechanisms.DispatcherWrapper(DeploymentAreaSet1, Dispatcher);
                    
                };
                _MVterrain.riverChanged += (nuEnd1, nuNew) =>
                {
                    void RiverChanged1()
                    {
                        Polygon imagePoly = new Polygon();
                        imagePoly.Points.Add(new Point(50, -20));
                        imagePoly.Points.Add(new Point(100, 10));
                        imagePoly.Points.Add(new Point(100, 70));
                        imagePoly.Points.Add(new Point(50, 100));
                        imagePoly.Points.Add(new Point(0, 70));
                        imagePoly.Points.Add(new Point(0, 10));
                        imagePoly.Height = 120;
                        imagePoly.Width = 100;
                        ImageBrush ImageRiver = new ImageBrush();
                        Uri uri = new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/RiverBridge/River.png");
                        ImageRiver.ImageSource = new BitmapImage(uri);
                        imagePoly.Fill = ImageRiver;
                        RotateTransform rotateTransform = new RotateTransform();
                        rotateTransform.CenterX = 50;
                        rotateTransform.CenterY = 40;
                        rotateTransform.Angle = nuNew;
                        imagePoly.RenderTransform = rotateTransform;
                        PanelRiverBackground.Children.Add(imagePoly);
                        if (nuEnd1 != nuNew)
                        {
                            // For removing the riverbegin when a second end is added
                            if (PanelRiverForeground.Children.Count == 0 && PanelRiverBackground.Children.Count == 3)
                            {
                                PanelRiverBackground.Children.RemoveAt(1);
                            }
                            if (Math.Abs(nuEnd1 - nuNew) == 60 || Math.Abs(nuEnd1 - nuNew) == 300)
                            {
                                if (nuEnd1 == 300 && nuNew == 0)
                                    nuEnd1 = -60;
                                if (nuNew == 300 && nuEnd1 == 0)
                                    nuNew = -60;
                                Polygon imageBack = new Polygon();
                                imageBack.Points.Add(new Point(50, -20));
                                imageBack.Points.Add(new Point(100, 10));
                                imageBack.Points.Add(new Point(100, 70));
                                imageBack.Points.Add(new Point(50, 100));
                                imageBack.Points.Add(new Point(0, 70));
                                imageBack.Points.Add(new Point(0, 10));
                                imageBack.Height = 120;
                                imageBack.Width = 100;
                                ImageBrush ImageRiver1 = new ImageBrush();
                                Uri uri1 = new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/RiverBridge/RCurve1.png");
                                ImageRiver1.ImageSource = new BitmapImage(uri1);
                                imageBack.Fill = ImageRiver1;
                                RotateTransform rotateTransform1 = new RotateTransform();
                                rotateTransform1.CenterX = 50;
                                rotateTransform1.CenterY = 40;
                                rotateTransform1.Angle = Math.Max(nuEnd1, nuNew);
                                imageBack.RenderTransform = rotateTransform1;
                                PanelRiverBackground.Children.Add(imageBack);

                                Polygon imageFore = new Polygon();
                                imageFore.Points.Add(new Point(50, -20));
                                imageFore.Points.Add(new Point(100, 10));
                                imageFore.Points.Add(new Point(100, 70));
                                imageFore.Points.Add(new Point(50, 100));
                                imageFore.Points.Add(new Point(0, 70));
                                imageFore.Points.Add(new Point(0, 10));
                                imageFore.Height = 120;
                                imageFore.Width = 100;
                                ImageBrush ImageRiver2 = new ImageBrush();
                                Uri uri2 = new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/RiverBridge/RCurve1WithoutS.png");
                                ImageRiver2.ImageSource = new BitmapImage(uri2);
                                imageFore.Fill = ImageRiver2;
                                RotateTransform rotateTransform2 = new RotateTransform();
                                rotateTransform2.CenterX = 50;
                                rotateTransform2.CenterY = 40;
                                rotateTransform2.Angle = Math.Max(nuEnd1, nuNew);
                                imageFore.RenderTransform = rotateTransform2;
                                PanelRiverForeground.Children.Add(imageFore);
                            }
                            else if (Math.Abs(nuEnd1 - nuNew) == 120 || Math.Abs(nuEnd1 - nuNew) == 240)
                            {
                                if ((nuEnd1 == 300 && nuNew == 60) || (nuEnd1 == 240 && nuNew == 0))
                                    nuEnd1 -= 360;
                                if ((nuNew == 300 && nuEnd1 == 60) || (nuNew == 240 && nuEnd1 == 0))
                                    nuNew -= 360;
                                Polygon imageBack = new Polygon();
                                imageBack.Points.Add(new Point(50, -20));
                                imageBack.Points.Add(new Point(100, 10));
                                imageBack.Points.Add(new Point(100, 70));
                                imageBack.Points.Add(new Point(50, 100));
                                imageBack.Points.Add(new Point(0, 70));
                                imageBack.Points.Add(new Point(0, 10));
                                imageBack.Height = 120;
                                imageBack.Width = 100;
                                ImageBrush ImageRiver1 = new ImageBrush();
                                Uri uri1 = new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/RiverBridge/RCurve2.png");
                                ImageRiver1.ImageSource = new BitmapImage(uri1);
                                imageBack.Fill = ImageRiver1;
                                RotateTransform rotateTransform1 = new RotateTransform();
                                rotateTransform1.CenterX = 50;
                                rotateTransform1.CenterY = 40;
                                rotateTransform1.Angle = Math.Max(nuEnd1, nuNew);
                                imageBack.RenderTransform = rotateTransform1;
                                //imageGrid.SetValue(Canvas.TopProperty, -20.0);
                                PanelRiverBackground.Children.Add(imageBack);

                                Polygon imageFore = new Polygon();
                                imageFore.Points.Add(new Point(50, -20));
                                imageFore.Points.Add(new Point(100, 10));
                                imageFore.Points.Add(new Point(100, 70));
                                imageFore.Points.Add(new Point(50, 100));
                                imageFore.Points.Add(new Point(0, 70));
                                imageFore.Points.Add(new Point(0, 10));
                                imageFore.Height = 120;
                                imageFore.Width = 100;
                                ImageBrush ImageRiver2 = new ImageBrush();
                                Uri uri2 = new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/RiverBridge/RCurve2WithoutS.png");
                                ImageRiver2.ImageSource = new BitmapImage(uri2);
                                imageFore.Fill = ImageRiver2;
                                RotateTransform rotateTransform2 = new RotateTransform();
                                rotateTransform2.CenterX = 50;
                                rotateTransform2.CenterY = 40;
                                rotateTransform2.Angle = Math.Max(nuEnd1, nuNew);
                                imageFore.RenderTransform = rotateTransform2;
                                //imageGrid.SetValue(Canvas.TopProperty, -20.0);
                                PanelRiverForeground.Children.Add(imageFore);
                            }
                            else if (Math.Abs(nuEnd1 - nuNew) == 180)
                            {
                                Polygon imageBack = new Polygon();
                                imageBack.Points.Add(new Point(50, -20));
                                imageBack.Points.Add(new Point(100, 10));
                                imageBack.Points.Add(new Point(100, 70));
                                imageBack.Points.Add(new Point(50, 100));
                                imageBack.Points.Add(new Point(0, 70));
                                imageBack.Points.Add(new Point(0, 10));
                                imageBack.Height = 120;
                                imageBack.Width = 100;
                                ImageBrush ImageRiver1 = new ImageBrush();
                                Uri uri1 = new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/RiverBridge/RCurve3.png");
                                ImageRiver1.ImageSource = new BitmapImage(uri1);
                                imageBack.Fill = ImageRiver1;
                                RotateTransform rotateTransform1 = new RotateTransform();
                                rotateTransform1.CenterX = 50;
                                rotateTransform1.CenterY = 40;
                                rotateTransform1.Angle = nuEnd1;
                                imageBack.RenderTransform = rotateTransform1;
                                //imageGrid.SetValue(Canvas.TopProperty, -20.0);
                                PanelRiverBackground.Children.Add(imageBack);

                                Polygon imageFore = new Polygon();
                                imageFore.Points.Add(new Point(50, -20));
                                imageFore.Points.Add(new Point(100, 10));
                                imageFore.Points.Add(new Point(100, 70));
                                imageFore.Points.Add(new Point(50, 100));
                                imageFore.Points.Add(new Point(0, 70));
                                imageFore.Points.Add(new Point(0, 10));
                                imageFore.Height = 120;
                                imageFore.Width = 100;
                                ImageBrush ImageRiver2 = new ImageBrush();
                                Uri uri2 = new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/RiverBridge/RCurve3WithoutS.png");
                                ImageRiver2.ImageSource = new BitmapImage(uri2);
                                imageFore.Fill = ImageRiver2;
                                RotateTransform rotateTransform2 = new RotateTransform();
                                rotateTransform2.CenterX = 50;
                                rotateTransform2.CenterY = 40;
                                rotateTransform2.Angle = nuEnd1;
                                imageFore.RenderTransform = rotateTransform2;
                                //imageGrid.SetValue(Canvas.TopProperty, -20.0);
                                PanelRiverForeground.Children.Add(imageFore);
                            }
                        }
                        else
                        {
                            if (nuEnd1 >= 180)
                                nuEnd1 -= 360;

                            Polygon imagePoly1 = new Polygon();
                            imagePoly1.Points.Add(new Point(50, -20));
                            imagePoly1.Points.Add(new Point(100, 10));
                            imagePoly1.Points.Add(new Point(100, 70));
                            imagePoly1.Points.Add(new Point(50, 100));
                            imagePoly1.Points.Add(new Point(0, 70));
                            imagePoly1.Points.Add(new Point(0, 10));
                            imagePoly1.Height = 120;
                            imagePoly1.Width = 100;
                            ImageBrush ImageRiver1 = new ImageBrush();
                            Uri uri1;
                            if (nuEnd1 > -70 && nuEnd1 < 70)
                                uri1 = new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/RiverBridge/RiverBegin.png");
                            else
                                uri1 = new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/RiverBridge/RiverBeginInv.png");
                            ImageRiver1.ImageSource = new BitmapImage(uri1);
                            imagePoly1.Fill = ImageRiver1;
                            RotateTransform rotateTransform1 = new RotateTransform();
                            rotateTransform1.CenterX = 50;
                            rotateTransform1.CenterY = 40;
                            if (nuEnd1 > -70 && nuEnd1 < 70)
                                rotateTransform1.Angle = nuEnd1;
                            else
                                rotateTransform1.Angle = nuEnd1 - 180;
                            imagePoly1.RenderTransform = rotateTransform1;
                            PanelRiverBackground.Children.Add(imagePoly1);
                        }
                    }
                    WpfBasicMechanisms.DispatcherWrapper(RiverChanged1, Dispatcher);
                };
                _MVterrain.riverCleared += () =>
                {
                    void RiverCleared1()
                    {
                        PanelRiverBackground.Children.Clear();
                        PanelRiverForeground.Children.Clear();
                    }
                    WpfBasicMechanisms.DispatcherWrapper(RiverCleared1, Dispatcher);
                    
                };
                _MVterrain.addBridge += (nu1, nu2) =>
                {
                    void AddBridge1()
                    {
                        Ellipse imagePoly = new Ellipse();
                        imagePoly.Height = 100;
                        imagePoly.Width = 100;
                        imagePoly.SetValue(Canvas.TopProperty, -10.0);
                        ImageBrush ImageRiver = new ImageBrush();
                        Uri uri = new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/RiverBridge/Bridge.png");
                        ImageRiver.ImageSource = new BitmapImage(uri);
                        imagePoly.Fill = ImageRiver;
                        RotateTransform rotateTransform = new RotateTransform();
                        rotateTransform.CenterX = 50;
                        rotateTransform.CenterY = 50;
                        rotateTransform.Angle = ((nu1 + nu2) / 2) - 90;
                        imagePoly.RenderTransform = rotateTransform;
                        PanelBridge.Children.Add(imagePoly);
                    }
                    WpfBasicMechanisms.DispatcherWrapper(AddBridge1, Dispatcher);
                    
                };
                _MVterrain.clearBridge += () =>
                {
                    void ClearBridge1()
                    {
                        if (PanelBridge != null)
                            PanelBridge.Children.Clear();
                    }
                    WpfBasicMechanisms.DispatcherWrapper(ClearBridge1, Dispatcher);
                    
                };
                _MVterrain.DisposeViewTerrain += () => {
                    MVterrain = null;
                    HotSeat.ListTerrains.Remove(this);
                    HotSeat.UpdateAfterCreatingMap -= SetPanelRiverToNull;
                    HotSeat = null;
                    unitview = null;
                };

                typeTerrainView = ViewTerrainType.terrainTypes[value.TerrainType];
            }
        }

        private HotSeatViewModel ViewModel { get => MVterrain.ViewModel; }

        private GamePage HotSeat;

        public ViewTerrain(int y, int x, ViewModelTerrain terr, GamePage hotSeat)
        {
            Y = y;
            X = x;
            HotSeat = hotSeat;
            MVterrain = terr;

            CommandBindings.Add(new CommandBinding(TerrainLeftDown, TerrainLeftDown_Executed, TerrainLeftDown_CanExecute));
            CommandBindings.Add(new CommandBinding(TerrainRightDown, TerrainRightDown_Executed, TerrainRightDown_CanExecute));
            InputBindings.Add(new MouseBinding(TerrainLeftDown, new MouseGesture(MouseAction.LeftClick)));
            InputBindings.Add(new MouseBinding(TerrainRightDown, new MouseGesture(MouseAction.RightClick)));
            MouseEnter += TerrainMouseEnter;
            MouseRightButtonUp += RightUp;
            HotSeat.ListTerrains.Add(this);
            PanelRiverBackground = new Canvas();
            PanelRiverForeground = new Canvas();
            PanelBridge = new Canvas();
            if (HotSeat.ViewModel.Gamestate == GamestateMV.CreatingMap)
            {
                HotSeat.UpdateAfterCreatingMap += SetPanelRiverToNull;
                PanelRiverCreator = AddRiverCreators();
            }

            InitializeComponent();
        }

        #region EventsHandler and Helpers

        Canvas AddRiverCreators()
        {
            Canvas can = new Canvas();

            Polygon poly1 = new Polygon();
            poly1.Points.Add(new Point(50, -10));
            poly1.Points.Add(new Point(50, -20));
            poly1.Points.Add(new Point(100, 10));
            poly1.Points.Add(new Point(90, 15));
            poly1.MouseEnter += CreateRiver;
            poly1.Name = "Ri120";
            poly1.Fill = Brushes.Transparent;
            can.Children.Add(poly1);

            Polygon poly2 = new Polygon();
            poly2.Points.Add(new Point(100, 70));
            poly2.Points.Add(new Point(90, 65));
            poly2.Points.Add(new Point(90, 15));
            poly2.Points.Add(new Point(100, 10));
            poly2.MouseEnter += CreateRiver;
            poly2.Name = "Ri180";
            poly2.Fill = Brushes.Transparent;
            can.Children.Add(poly2);

            Polygon poly3 = new Polygon();
            poly3.Points.Add(new Point(90, 65));
            poly3.Points.Add(new Point(100, 70));
            poly3.Points.Add(new Point(50, 100));
            poly3.Points.Add(new Point(50, 90));
            poly3.MouseEnter += CreateRiver;
            poly3.Name = "Ri240";
            poly3.Fill = Brushes.Transparent;
            can.Children.Add(poly3);

            Polygon poly4 = new Polygon();
            poly4.Points.Add(new Point(0, 70));
            poly4.Points.Add(new Point(10, 65));
            poly4.Points.Add(new Point(50, 90));
            poly4.Points.Add(new Point(50, 100));
            poly4.MouseEnter += CreateRiver;
            poly4.Name = "Ri300";
            poly4.Fill = Brushes.Transparent;
            can.Children.Add(poly4);

            Polygon poly5 = new Polygon();
            poly5.Points.Add(new Point(10, 65));
            poly5.Points.Add(new Point(0, 70));
            poly5.Points.Add(new Point(0, 10));
            poly5.Points.Add(new Point(10, 15));
            poly5.MouseEnter += CreateRiver;
            poly5.Name = "Ri0";
            poly5.Fill = Brushes.Transparent;
            can.Children.Add(poly5);

            Polygon poly6 = new Polygon();
            poly6.Points.Add(new Point(50, -10));
            poly6.Points.Add(new Point(50, -20));
            poly6.Points.Add(new Point(0, 10));
            poly6.Points.Add(new Point(10, 15));
            poly6.MouseEnter += CreateRiver;
            poly6.Name = "Ri60";
            poly6.Fill = Brushes.Transparent;
            can.Children.Add(poly6);

            return can;
        }

        #endregion

        public RoutedCommand TerrainLeftDown =
            new RoutedCommand("TerrainLeftDown", typeof(ViewTerrain), new InputGestureCollection());
        public void TerrainLeftDown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !ViewModel.IsChangingToNextTurn;
        }
        public void TerrainLeftDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MVterrain.LeftDown();
        }

        public RoutedCommand TerrainRightDown =
            new RoutedCommand("TerrainRightDown", typeof(ViewTerrain), new InputGestureCollection());
        public void TerrainRightDown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !ViewModel.IsChangingToNextTurn; ;
        }
        public void TerrainRightDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MVterrain.RightDown();
        }

        public void CreateRiver(object sender, MouseEventArgs e)
        {
            if (ViewModel.TogButtonRiverChecked && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Polygon poly = sender as Polygon;
                MVterrain.CreateRiver(Convert.ToInt32(poly.Name.Substring(2)));
            }
        }

        public void TerrainMouseEnter(object sender, MouseEventArgs e)
        {
            if (!ViewModel.IsChangingToNextTurn)
            {
                if (Mouse.RightButton == MouseButtonState.Pressed)
                {
                    ViewModel.movFocusedTerrain = null;
                    MVterrain.RightDown();
                }
                else if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    MVterrain.LeftDown();
                }
            }
        }

        public void RightUp(object sender, MouseEventArgs e)
        {
            MVterrain.RightUp();
        }

        #region Properties

        public int X { set; get; }
        public int Y { set; get; }

        private ViewUnit _unitview;
        public ViewUnit unitview
        {
            get
            {
                return _unitview;
            }
            set
            {
                if (_unitview != null)
                {
                    _unitview.Terrain = null;
                }
                _unitview = value;
                if (value != null)
                {                    
                    if (value.Parent is Canvas)
                        ((Canvas)value.Parent).Children.Remove(value);
                    else if (value.Terrain != null)
                        value.Terrain.unitview = null;
                    value.Terrain = this;
                }
                this.Unit = value;
            }
        }

        public static readonly DependencyProperty typeTerrainViewProperty = DependencyProperty.Register(nameof(typeTerrainView), typeof(Uri), typeof(ViewTerrain));
        internal Uri typeTerrainView
        {
            get { return (Uri)GetValue(typeTerrainViewProperty); }
            set
            {
                SetValue(typeTerrainViewProperty, value);
            }
        }

        public static readonly DependencyProperty deploymentAreaViewProperty = DependencyProperty.Register(nameof(deploymentAreaView), typeof(ArmyColor), typeof(ViewTerrain));
        internal ArmyColor deploymentAreaView
        {
            get { return (ArmyColor)GetValue(deploymentAreaViewProperty); }
            set
            {
                SetValue(deploymentAreaViewProperty, value);
            }
        }

        public static readonly DependencyProperty focusedmovementProperty = DependencyProperty.Register(nameof(focusedmovement), typeof(bool), typeof(ViewTerrain));
        public bool focusedmovement
        {
            get { return (bool)GetValue(focusedmovementProperty); }
            set
            {
                SetValue(focusedmovementProperty, value);
            }
        }

        public static readonly DependencyProperty focusedfireProperty = DependencyProperty.Register(nameof(focusedfire), typeof(bool), typeof(ViewTerrain));
        internal bool focusedfire
        {
            get { return (bool)GetValue(focusedfireProperty); }
            set
            {
                SetValue(focusedfireProperty, value);
            }
        }

        public static readonly DependencyProperty fireposibleProperty = DependencyProperty.Register(nameof(fireposible), typeof(bool), typeof(ViewTerrain));
        internal bool fireposible
        {
            get { return (bool)GetValue(fireposibleProperty); }
            set
            {
                SetValue(fireposibleProperty, value);
            }
        }

        public static readonly DependencyProperty movementposibleProperty = DependencyProperty.Register(nameof(movementposible), typeof(bool), typeof(ViewTerrain));
        public bool movementposible
        {
            get { return (bool)GetValue(movementposibleProperty); }
            set
            {
                SetValue(movementposibleProperty, value);
            }
        }

        public static readonly DependencyProperty IsAttackedByMultipleProperty = DependencyProperty.Register(nameof(IsAttackedByMultiple), typeof(bool), typeof(ViewTerrain));
        internal bool IsAttackedByMultiple
        {
            get { return (bool)GetValue(IsAttackedByMultipleProperty); }
            set
            {
                SetValue(IsAttackedByMultipleProperty, value);
            }
        }

        public static readonly DependencyProperty HideProperty = DependencyProperty.Register(nameof(Hide), typeof(bool), typeof(ViewTerrain));
        internal bool Hide
        {
            get { return (bool)GetValue(HideProperty); }
            set
            {
                void Hide1()
                {
                    SetValue(HideProperty, value);
                }
                WpfBasicMechanisms.DispatcherWrapper(Hide1, Dispatcher);
                
            }
        }

        public static readonly DependencyProperty PanelRiverCreatorProperty =
            DependencyProperty.Register("PanelRiverCreator", typeof(object), typeof(ViewTerrain));
        public object PanelRiverCreator
        {
            get { return (object)GetValue(PanelRiverCreatorProperty); }
            set { SetValue(PanelRiverCreatorProperty, value); }
        }
        private void SetPanelRiverToNull()
        { 
            PanelRiverCreator = null; 
        }

    public static readonly DependencyProperty PanelRiverBackgroundProperty =
            DependencyProperty.Register("PanelRiverBackground", typeof(Canvas), typeof(ViewTerrain));
        public Canvas PanelRiverBackground
        {
            get { return (Canvas)GetValue(PanelRiverBackgroundProperty); }
            set { SetValue(PanelRiverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PanelRiverForegroundProperty =
            DependencyProperty.Register("PanelRiverForeground", typeof(Canvas), typeof(ViewTerrain));
        public Canvas PanelRiverForeground
        {
            get { return (Canvas)GetValue(PanelRiverForegroundProperty); }
            set { SetValue(PanelRiverForegroundProperty, value); }
        }

        public static readonly DependencyProperty PanelBridgeProperty =
            DependencyProperty.Register("PanelBridge", typeof(Canvas), typeof(ViewTerrain));
        public Canvas PanelBridge
        {
            get { return (Canvas)GetValue(PanelBridgeProperty); }
            set { SetValue(PanelBridgeProperty, value); }
        }

        public static readonly DependencyProperty UnitProperty =
           DependencyProperty.Register("Unit", typeof(ViewUnit), typeof(ViewTerrain));
        public ViewUnit Unit
        {
            get { return (ViewUnit)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        

        #endregion
    }
}

