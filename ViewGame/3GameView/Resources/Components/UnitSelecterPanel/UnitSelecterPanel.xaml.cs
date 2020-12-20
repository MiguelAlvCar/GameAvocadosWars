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
using BasicElements;
using ArmyAndUnitTypes;
using Translation;
using ViewModelHotSeat;

namespace ViewGame.View.Resources
{
    /// <summary>
    /// Interaktionslogik für UnitSelecterPanel.xaml
    /// </summary>
    public partial class UnitSelecterPanel : UserControl
    {
        public UnitSelecterPanel()
        {
            InitializeComponent();
            Loaded += (sender, e) =>
            {
                Points = InitialPoints;
                foreach (UnitRow unitRow1 in UnitRow_Points_Dic.Keys)
                {
                    unitRow1.RestPoints = Points;
                }
            };

            Translater.ÜbersetzungMeth(new UnitSelecterList(this));
        }

        static UnitSelecterPanel()
        {
            InitialPointsProperty = DependencyProperty.Register(nameof(InitialPoints), typeof(double), typeof(UnitSelecterPanel));
            PointsProperty = DependencyProperty.Register(nameof(Points), typeof(double), typeof(UnitSelecterPanel));
            FrameworkPropertyMetadata meta = new FrameworkPropertyMetadata(OnArmyTypeChanged);
            ArmyTypeProperty = DependencyProperty.Register(nameof(ArmyType), typeof(ArmyType), typeof(UnitSelecterPanel), meta);
            ArmyColorProperty = DependencyProperty.Register(nameof(ArmyColor), typeof(ArmyColor), typeof(UnitSelecterPanel));
        }

        public static readonly DependencyProperty InitialPointsProperty;
        public double InitialPoints
        {
            get { return (double)GetValue(InitialPointsProperty); }
            set { SetValue(InitialPointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty;
        public double Points
        {
            get { return (double)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty ArmyTypeProperty;
        public ArmyType ArmyType
        {
            get { return (ArmyType)GetValue(ArmyTypeProperty); }
            set { SetValue(ArmyTypeProperty, value); }
        }
        public static void OnArmyTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UnitSelecterPanel unitSelecterPanel =  sender as UnitSelecterPanel;
            unitSelecterPanel.UnitRowPanel.Children.Clear();
            foreach (AbstractUnitType unitType in unitSelecterPanel.ArmyType.Units)
            {
                UnitRow unitRow = new UnitRow(unitType);
                unitRow.CounterChanged += unitSelecterPanel.UnitRowCounterChanged;
                unitSelecterPanel.UnitRow_Points_Dic.Add(unitRow, 0);
                unitSelecterPanel.UnitRowPanel.Children.Add(unitRow);
            }
        }

        public static readonly DependencyProperty ArmyColorProperty;
        public ArmyColor ArmyColor
        {
            get { return (ArmyColor)GetValue(ArmyColorProperty); }
            set { SetValue(ArmyColorProperty, value); }
        }

        void UnitRowCounterChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UnitRow unitRow = e.Source as UnitRow;
            UnitRow_Points_Dic[unitRow] = e.NewValue;
            Points = InitialPoints - UnitRow_Points_Dic.Values.Sum();
            foreach(UnitRow unitRow1 in UnitRow_Points_Dic.Keys)
            {
                unitRow1.RestPoints = Points;
            }
        }

        private Dictionary<UnitRow, double> UnitRow_Points_Dic = new Dictionary<UnitRow, double>();

        public void Translate()
        {
            Translater.ÜbersetzungMeth(new UnitSelecterList(this));
        }

        private void CreateUnits_Click(object sender, RoutedEventArgs e)
        {
            List<ViewUnit> ListVUnit = new List<ViewUnit>();

            foreach (UnitRow unitRow in UnitRowPanel.Children)
            {
                List<ViewModelUnit> ListVMUnits = unitRow.CreateUnits(ArmyColor, (HotSeatViewModel)DataContext);
                foreach (ViewModelUnit unit in ListVMUnits)
                {
                    ListVUnit.Add(unit.OnGetViewUnitFromViewModelUnit() as ViewUnit);
                }
            }

            Camp.Children.Clear();
            int counter = 0;
            foreach (ViewUnit unit in ListVUnit)
            {
                unit.SetValue(Canvas.LeftProperty, (double)80 * (counter % 4));
                unit.SetValue(Canvas.TopProperty, (double)100 * (Math.Floor((double)counter / 4)));
                Camp.Children.Add(unit);
                counter++;
            }
            if (counter > 4)
                Camp.Height = 100 * (Math.Ceiling((double)counter / 4));
        }
    }
}
