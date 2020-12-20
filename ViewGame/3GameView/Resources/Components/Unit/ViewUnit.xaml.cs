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
using ArmyAndUnitTypes;
using ViewModelHotSeat;
using WpfBasicElements;

namespace ViewGame.View.Resources
{
    public partial class ViewUnit : UserControl
    {
        HotSeatViewModel ViewModel;

        static ViewUnit()
        {
            MovementUnitViewProperty = DependencyProperty.Register(nameof(MovementUnitView), typeof(ViewMovement), typeof(ViewUnit));
            LifeUnitViewProperty = DependencyProperty.Register(nameof(LifeUnitView), typeof(byte), typeof(ViewUnit));
            MoralUnitViewProperty = DependencyProperty.Register(nameof(MoralUnitView), typeof(byte), typeof(ViewUnit));
            UnitProperty = DependencyProperty.Register(nameof(Unit), typeof(ViewModelUnit), typeof(ViewUnit));
            AffilationUnitViewProperty = DependencyProperty.Register(nameof(AffilationUnitView), typeof(ArmyColor), typeof(ViewUnit));
            ModifierUnitViewProperty = DependencyProperty.Register(nameof(ModifierUnitView), typeof(Modifier), typeof(ViewUnit));
            FocusedProperty = DependencyProperty.Register(nameof(Focused), typeof(bool), typeof(ViewUnit));
            SelectedProperty = DependencyProperty.Register(nameof(Selected), typeof(bool), typeof(ViewUnit));
        }

        public ViewUnit(HotSeatViewModel viewModel, ViewModelUnit unit)
        {
            _Unit = unit;
            ViewModel = viewModel;

            InitializeComponent();

            MouseLeftButtonDown += SelectUnit;
            MouseEnter += (send, arg) => { if (arg.RightButton != MouseButtonState.Pressed) Focused = true; };
            MouseLeave += (send, arg) => { if (arg.RightButton != MouseButtonState.Pressed) Focused = false; };
            MouseRightButtonUp += (send, arg) => Focused = false;
        }

        private ViewModelUnit __Unit;
        public ViewModelUnit _Unit
        {
            get => __Unit;
            set
            {
                SetValue(UnitProperty, value);

                __Unit = value;

                if (value == null)
                    return;

                AffilationUnitView = value.AffiliationUnitView;

                UpdateMovement(value);
                  
                ModifierUnitView = value.ModifierUnitViewModel;
                LifeUnitView = Convert.ToByte(100 - value.LifeUnitViewModel);
                MoralUnitView = Convert.ToByte(100 - value.MoralUnitViewModel);

                __Unit.GetViewUnitFromViewModelUnit += () =>
                {
                    return this;
                };

                __Unit.DisposeUnitViewModel += () =>
                {
                    DisposeViewModel(this);
                    ViewModel = null;
                    Terrain = null;
                };

                __Unit.PropertyChanged += (sender, arg) =>
                {
                    ViewModelUnit uni = sender as ViewModelUnit;
                    if (arg.PropertyName == nameof(uni.RestMovementUnitViewModel))
                    {
                        UpdateMovement(uni);
                    }
                    else if (arg.PropertyName == nameof(uni.MoralUnitViewModel))
                    {
                        MoralUnitView = Convert.ToByte(100 - uni.MoralUnitViewModel);
                    }
                    else if (arg.PropertyName == nameof(uni.LifeUnitViewModel))
                    {
                        LifeUnitView = Convert.ToByte(100 - uni.LifeUnitViewModel);
                    }
                    else if (arg.PropertyName == nameof(uni.ModifierUnitViewModel))
                    {
                        void Modifier1()
                        {
                            ModifierUnitView = uni.ModifierUnitViewModel;
                        }

                        WpfBasicMechanisms.DispatcherWrapper(Modifier1, Dispatcher);

                    }
                    else if (arg.PropertyName == nameof(uni.AffiliationUnitView))
                    {
                        AffilationUnitView = uni.AffiliationUnitView;
                    }
                };

                __Unit.HideUnit += () => this.Visibility = Visibility.Collapsed;
                __Unit.RevealUnit += () =>
                    this.Visibility = Visibility.Visible;
            }
        }
        public static readonly DependencyProperty UnitProperty;
        public ViewModelUnit Unit
        {
            get { return (ViewModelUnit)GetValue(UnitProperty); }
        }

        public ViewTerrain Terrain { set; get; }

        private void UpdateMovement(ViewModelUnit unit)
        {
            if (unit.RestMovementUnitViewModel == unit.BaseMovementUnitViewModel)
                MovementUnitView = ViewMovement.Green;
            else if (unit.RestMovementUnitViewModel == 0)
                MovementUnitView = ViewMovement.Red;
            else if (unit.RestMovementUnitViewModel < unit.BaseMovementUnitViewModel && unit.RestMovementUnitViewModel > 0)
                MovementUnitView = ViewMovement.Yellow;
        }

        public static readonly DependencyProperty MovementUnitViewProperty;
        public ViewMovement MovementUnitView
        {
            get { return (ViewMovement)GetValue(MovementUnitViewProperty); }
            set
            {
                SetValue(MovementUnitViewProperty, value);
            }
        }

        public static readonly DependencyProperty MoralUnitViewProperty;
        public byte MoralUnitView
        {
            get { return (byte)GetValue(MoralUnitViewProperty); }
            set
            {
                SetValue(MoralUnitViewProperty, value);
            }
        }

        public static readonly DependencyProperty LifeUnitViewProperty;
        public byte LifeUnitView
        {
            get { return (byte)GetValue(LifeUnitViewProperty); }
            set
            {
                SetValue(LifeUnitViewProperty, value);
            }
        }

        public static readonly DependencyProperty AffilationUnitViewProperty;
        public ArmyColor AffilationUnitView
        {
            get { return (ArmyColor)GetValue(AffilationUnitViewProperty); }
            set
            {
                SetValue(AffilationUnitViewProperty, value);
            }
        }

        public static readonly DependencyProperty ModifierUnitViewProperty;
        public Modifier ModifierUnitView
        {
            get { return (Modifier)GetValue(ModifierUnitViewProperty); }
            set
            {
                SetValue(ModifierUnitViewProperty, value);
            }
        }

        public static readonly DependencyProperty FocusedProperty;
        public bool Focused
        {
            get { return (bool)GetValue(FocusedProperty); }
            set
            {
                SetValue(FocusedProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedProperty;
        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set
            {
                SetValue(SelectedProperty, value);
            }
        }

        private static void DisposeViewModel(ViewUnit uni)
        {

            void UnitCreated1()
            {
                if (uni.Terrain != null)
                {
                    uni.Terrain.Unit = null;
                }
                else
                {
                    Canvas parent = uni.Parent as Canvas;
                    if (parent != null)
                    {
                        parent.Children.Remove(uni);
                    }
                }
                uni._Unit = null;
                uni = null;
            }
            WpfBasicMechanisms.DispatcherWrapper(UnitCreated1, uni.Dispatcher);
        }

        public void SelectUnit(object sender, RoutedEventArgs e)
        {
            ViewUnit unitView = sender as ViewUnit;
            ViewModel.SelectUnit(unitView._Unit);
            e.Handled = true;
        }
    }
}
