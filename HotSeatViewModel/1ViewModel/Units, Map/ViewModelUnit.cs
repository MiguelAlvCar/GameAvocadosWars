using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using ModelGame;
using BasicElements;
using ArmyAndUnitTypes;
using Ninject;
using BasicElements.AbstractForArmyInterceptor;

namespace ViewModelHotSeat
{
    public class ViewModelUnit : ModelBase
    {
        HotSeatViewModel ViewModel;
        public ViewModelUnit (ModelUnit unit, HotSeatViewModel viewModel)
        {
            ViewModel = viewModel;
            Unit = unit;
        }

        public event Func<object> GetViewUnitFromViewModelUnit;

        public object OnGetViewUnitFromViewModelUnit()
        {
            if (GetViewUnitFromViewModelUnit != null)
                return GetViewUnitFromViewModelUnit();
            else return null;
        }

        private ModelUnit _unit;
        public ModelUnit Unit
        {

            get => _unit;
            set
            {
                _unit = value;

                if (value == null)
                    return;
                
                RestMovementUnitViewModel = Unit.MovementRest;
                BaseMovementUnitViewModel = Unit.Movementbasis;
                MoralUnitViewModel = Unit.MoralRest;
                ModifierSetter(Unit, this);
                LifeUnitViewModel = Unit.LifeRest;
                Stregth = Unit.Strength;
                HasSpecialStregth = Unit.HasSpecialStregth;
                if (value is RangeUnit)
                {
                    Range = ((RangeUnit)Unit).Reichweite;
                    RangedStregth = ((RangeUnit)Unit).FernStrength;
                    IsRangeUnit = true;
                }
                else
                    IsRangeUnit = false;


                AffiliationUnitView = value.ArmyAffiliation;

                _unit.GetViewModelUnitFromModelUnit += () =>
                {
                    return this;
                };

                _unit.DisposeUnitViewModel += () =>
                {
                    OnDisposeUnitViewModel(this);
                    Unit = null;
                    ViewModel = null;
                };

                _unit.PropertyChanged += (sender, arg) =>
                {
                    ModelUnit uni = sender as ModelUnit;
                    if (arg.PropertyName == nameof(uni.MovementRest))
                    {
                        RestMovementUnitViewModel = uni.MovementRest;
                    }                   
                    else if (arg.PropertyName == nameof(uni.LifeRest))
                    {                        
                        LifeUnitViewModel = uni.LifeRest;
                    }
                    else if (arg.PropertyName == nameof(uni.MoralRest))
                    {
                        MoralUnitViewModel = uni.MoralRest;
                        if (ViewModel != null && this == ViewModel.selectedUnit)
                            ViewModel.OnUpdateSelectedUnit();
                        // Because the moral is updated after the life
                    }
                    else if (arg.PropertyName == nameof(uni.ArmyAffiliation))
                    {
                        AffiliationUnitView = uni.ArmyAffiliation;
                    }
                    else
                    {
                        ModifierSetter(uni, this);
                    }
                };

                _unit.HideUnit += OnHideUnit;
                _unit.RevealUnit += OnRevealUnit;
            }
        }

        public AbstractUnitType UnitType { set; get; }

        public double Stregth { get; set; }

        public bool HasSpecialStregth { get; set; }

        public bool IsRangeUnit { get; set; }

        public double RangedStregth { get; set; }

        public byte Range { get; set; }

        public short BaseMovementUnitViewModel { get; set; }

        private short _RestMovementUnitViewModel;
        public short RestMovementUnitViewModel { get => _RestMovementUnitViewModel; set => SetProperty(ref _RestMovementUnitViewModel, value); }

        private short _moralUnitView;
        public short MoralUnitViewModel { get => _moralUnitView; set => SetProperty(ref _moralUnitView, value); }

        private short _lifeUnitView;
        public short LifeUnitViewModel { get => _lifeUnitView; set => SetProperty(ref _lifeUnitView, value); }

        private ArmyColor _affiliationUnitView;
        public ArmyColor AffiliationUnitView { get => _affiliationUnitView; set => SetProperty(ref _affiliationUnitView, value); }        

        private Modifier _modifierUnitView;
        public Modifier ModifierUnitViewModel { get => _modifierUnitView; set => SetProperty(ref _modifierUnitView, value); }

        public void ModifierSetter(ModelUnit uni, ViewModelUnit unitView)
        {
            Modifier modifier = BasicMechanisms.Kernel.Get<IModifierConverters>().GetModifierFromModel(uni);
            unitView.ModifierUnitViewModel = modifier;
        }

        public event Action HideUnit;
        public void OnHideUnit()
        {
            HideUnit();
        }

        public event Action RevealUnit;
        public void OnRevealUnit()
        {
            RevealUnit();
        }

        #region Dispose

        public void DisposeUnitViewModelFromViewModel ()
        {
            ViewModel.Game.DisposeUnitModel(Unit);
        }

        public event Action DisposeUnitViewModel;

        private static void OnDisposeUnitViewModel(ViewModelUnit uni)
        {
            uni.DisposeUnitViewModel();
        }

        #endregion
    }

    public enum ViewMovement
    {
        Green, Yellow, Red
    }
}
