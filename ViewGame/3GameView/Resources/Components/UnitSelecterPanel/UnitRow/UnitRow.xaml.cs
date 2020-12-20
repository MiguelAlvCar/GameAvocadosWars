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
using System.Collections;
using ViewModelHotSeat;

namespace ViewGame.View.Resources
{
    /// <summary>
    /// Interaktionslogik für PanelRow.xaml
    /// </summary>
    public partial class UnitRow : UserControl
    {
        public UnitRow(AbstractUnitType unitType)
        {
            InitializeComponent();
            UnitType = unitType;
            Counter = 0;
        }

        static UnitRow()
        {
            FrameworkPropertyMetadata meta = new FrameworkPropertyMetadata(OnCounterChanged);
            CounterProperty = DependencyProperty.Register(nameof(Counter), typeof(int), typeof(UnitRow), meta);
            UnitTypeProperty = DependencyProperty.Register(nameof(UnitType), typeof(AbstractUnitType), typeof(UnitRow));

            CounterChangedEvent = EventManager.RegisterRoutedEvent("CounterChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(UnitRow));
        }

        public static readonly RoutedEvent CounterChangedEvent;

        public event RoutedPropertyChangedEventHandler<double> CounterChanged
        {
            add { AddHandler(CounterChangedEvent, value); }
            remove { RemoveHandler(CounterChangedEvent, value); }
        }

        public static readonly DependencyProperty CounterProperty;
        internal int Counter
        {
            get { return (int)GetValue(CounterProperty); }
            set { SetValue(CounterProperty, value); }
        }
        public static void OnCounterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UnitRow unitRow = sender as UnitRow;
            RoutedPropertyChangedEventArgs<double> args = new RoutedPropertyChangedEventArgs<double>(0, unitRow.Counter * unitRow.UnitType.Points);
            args.RoutedEvent = UnitRow.CounterChangedEvent;
            unitRow.RaiseEvent(args);
        }

        public static readonly DependencyProperty UnitTypeProperty;
        internal AbstractUnitType UnitType
        {
            get { return (AbstractUnitType)GetValue(UnitTypeProperty); }
            set { SetValue(UnitTypeProperty, value); }
        }

        // To be set from UnitSelecterPanel
        internal double RestPoints { set; get; }

        private void MoreUnits_Click(object sender, RoutedEventArgs e)
        {
            if (RestPoints >= UnitType.Points)
                ++Counter;
        }

        private void LessUnits_Click(object sender, RoutedEventArgs e)
        {
            if (Counter > 0)
                --Counter;
        }

        public List<ViewModelUnit> CreateUnits(ArmyColor armyColor, HotSeatViewModel hotSeatViewModel)
        {
            ArrayList untypifiedUnitList = new ArrayList();
            UnitType.CreateModelUnitInGame(hotSeatViewModel.Game, Counter, untypifiedUnitList, armyColor);

            List<ViewModelUnit> typifiedUnitList = new List<ViewModelUnit>();
            foreach (object unit in untypifiedUnitList)
            {
                ViewModelUnit UVunit = unit as ViewModelUnit;
                typifiedUnitList.Add(UVunit);
            }
            return typifiedUnitList;
        }
    }
}
