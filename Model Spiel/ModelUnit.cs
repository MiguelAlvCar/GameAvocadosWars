using System;
using System.ComponentModel;
using BasicElements;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using BasicElements.AbstractForArmyInterceptor;

namespace ModelGame
{
    [Serializable()]
    public abstract class ModelUnit : ModelBase, IUnitModel, ICloneable
    {
        [NonSerialized()]
        public Game Game;

        public UnitID ID;

        public ModelUnit()
        {
            LifeRest = LifeBasis;
            MoralRest = Moralbasis;
            Fleeing = false;
        }

        #region Binds with terrain and game

        [NonSerialized()]
        private Func<object> _GetViewModelUnitFromModelUnit;
        public event Func<object> GetViewModelUnitFromModelUnit
        {
            add { _GetViewModelUnitFromModelUnit += value; }
            remove { _GetViewModelUnitFromModelUnit -= value; }
        }
        public object OnGetViewModelUnitFromModelUnit()
        {
            if (_GetViewModelUnitFromModelUnit != null)
                return _GetViewModelUnitFromModelUnit();
            else return null;
        }

        public Terrain InTerrain { set; get; }
        
        private ArmyColor _armyAffiliation;
        public ArmyColor ArmyAffiliation
        {
            get
            {
                return _armyAffiliation;
            }
            set
            {
                if (value == ArmyColor.Red)
                    Game.ListReds.Add(this);
                else if (value == ArmyColor.Blue)
                    Game.ListBlues.Add(this);
                SetProperty(ref _armyAffiliation, value);
            }
        }

        public virtual void ShowSpecialEffectWhileSelecting() 
        {
            Game.OnShowSpecialEffectWhileSelecting(this);
        }

        #endregion

        #region Basic properties

        public virtual bool HasSpecialStregth { get; set; } = false;

        public virtual short Movementbasis { get; set; }

        protected short _movementRest;
        virtual public short MovementRest
        {
            get { return _movementRest; }
            set
            {
                if (value < 0)
                {
                    SetProperty(ref _movementRest, (short)0);
                }
                else SetProperty(ref _movementRest, value);
            }
        }

        public const short Moralbasis = 100;

        protected short _moralRest;
        virtual public short MoralRest
        {
            get { return _moralRest; }
            set
            {
                if (value < 0) SetProperty<short>(ref _moralRest, 0);
                else if (value > 100) SetProperty<short>(ref _moralRest, 100);
                else if (value > LifeRest) SetProperty(ref _moralRest, LifeRest);
                else SetProperty(ref _moralRest, value);
            }
        }

        public const short LifeBasis = 100;

        private short _lifeRest;
        public short LifeRest
        {
            get { return _lifeRest; }
            set
            {
                if (value <= 0)
                {
                    SetProperty(ref _lifeRest, (short)0);
                    _DisposeFromGame(this);
                }
                else SetProperty(ref _lifeRest, value);
            }
        }

        public virtual double Strength { get; set; }

        protected bool _fleeing;
        virtual public bool Fleeing
        {
            get { return _fleeing; }
            set
            {
                SetProperty(ref _fleeing, value);
                if (value)
                {
                    FlightModifier = 5;
                }
            }
        }

        private int _FluchtModifikator;
        virtual public int FlightModifier
        {
            get { return _FluchtModifikator; }
            set
            {
                if (value == 0 && _FluchtModifikator != 0)
                {
                    this.Fleeing = false;
                    this.MoralRest = 7;
                }
                SetProperty(ref _FluchtModifikator, value);
            }
        }

        #endregion

        #region Combat

        public virtual bool defend(ModelUnit attacker, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out bool showSpecialDefense)
        {
            extraMoralDamage = 0;
            double doubleLifeDamage;
            if (InTerrain.defendBonus != null && InTerrain.defendBonus > 1)
                doubleLifeDamage = CalculateLifeDamage(attacker) * InTerrain.defendBonus ?? 0;
            else
                doubleLifeDamage = CalculateLifeDamage(attacker);


            
            moralDamage = Convert.ToInt16(CalculateMoralDamage(doubleLifeDamage));
            lifeDamage = Convert.ToInt16(doubleLifeDamage);
            Debug.WriteLine("moralDamage Short: " + moralDamage + "; doubleLifeDamage: " + doubleLifeDamage);
            Debug.WriteLine("lifeDamage Short: " + lifeDamage + "; doubleLifeDamage: " + doubleLifeDamage);

            showSpecialDefense = false;

            // defenderAvoidAttacker
            return false;
        }

        public virtual bool attack(ModelUnit defender, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out bool showSpecialAttack, out MovementItemDTO movementItemDTO)
        {
            extraMoralDamage = 0;

            double doubleLifeDamage;
            double defenderBonus = 1;
            if (defender.InTerrain.defendBonus != null && defender.InTerrain.defendBonus < 1)
            {
                defenderBonus = 1 / defender.InTerrain.defendBonus ?? 1;
            }
            if (InTerrain.attackBonus != null)
                doubleLifeDamage = CalculateLifeDamage(defender) * defenderBonus * InTerrain.attackBonus ?? 0;
            else
                doubleLifeDamage = CalculateLifeDamage(defender) * defenderBonus;

            moralDamage = Convert.ToInt16(  CalculateMoralDamage(doubleLifeDamage)  );
            lifeDamage = Convert.ToInt16( doubleLifeDamage );
            Debug.WriteLine("moralDamage Short: " + moralDamage + "; doubleLifeDamage: " + doubleLifeDamage);
            Debug.WriteLine("lifeDamage Short: " + lifeDamage + "; doubleLifeDamage: " + doubleLifeDamage);

            showSpecialAttack = false;

            movementItemDTO = new MovementItemDTO();
            movementItemDTO.ListUnitItems = new List<UnitItemDTO>();
            UnitItemDTO unitItem = new UnitItemDTO(defender);
            ((List<UnitItemDTO>)movementItemDTO.ListUnitItems).Add(unitItem);

            //attackerAvoidDefender
            return false;
        }

        public double CalculateLifeDamage(ModelUnit defender)
        {
            Debug.WriteLine("In calculateLifeDamage, (1 + (MoralRest / 100)): " + (1 + (MoralRest / 100)) + "; (2 + (defender.MoralRest / 100)): " + (2 + (defender.MoralRest / 100)));
            return (13 * Strength * (1 + (MoralRest / 100))) / (defender.Strength * (2 + (defender.MoralRest / 100)));
        }

        public double CalculateMoralDamage(double lifeDamage)
        {
            Debug.WriteLine("In calculateMoralDamage, double lifeDamage: " + lifeDamage + "; lifeDamage * 2.4: " + lifeDamage * 2.4);
            return lifeDamage * 2.4;
        }

        #endregion

        #region Turn, Hide/Reveal

        public virtual void NextTurnToOwn()
        {
            if (InTerrain.river && !InTerrain.bridge)
                this.MovementRest = 1;
            else
                this.MovementRest = this.Movementbasis;
        }

        public virtual void NextTurn()
        {
            if (this.Fleeing) this.FlightModifier--;
            if (this.MoralRest == 0 && !this.Fleeing) this.Fleeing = true;
            // Elefants' NextTurn-method increased also the moral
            if (!this.Fleeing)
            {
                MoralRest += Convert.ToInt16((LifeRest / 20) + 5);
            }
        }

        [NonSerialized()]
        private Action _HideUnit;
        public event Action HideUnit
        {
            add { _HideUnit += value; }
            remove { _HideUnit -= value; }
        }
        public void OnHideUnit()
        {
            _HideUnit();
        }
        [NonSerialized()]
        private Action _RevealUnit;
        public event Action RevealUnit
        {
            add { _RevealUnit += value; }
            remove { _RevealUnit -= value; }
        }
        public void OnRevealUnit()
        {
            _RevealUnit();
        }

        #endregion

        #region Dispose

        //only to be used by "Game"
        public void OnDisposeUnitViewModel()
        {
            _DisposeUnitViewModel();
        }
        [NonSerialized()]
        private Action _DisposeUnitViewModel;
        public event Action DisposeUnitViewModel
        {
            add { _DisposeUnitViewModel += value; }
            remove { _DisposeUnitViewModel -= value; }
        }

        [NonSerialized()]
        public Action<ModelUnit> _DisposeFromGame;
        public event Action<ModelUnit> DisposeFromGame
        {
            add { _DisposeFromGame += value; }
            remove { _DisposeFromGame -= value; }
        }

        #endregion

        public void AfterDeserialization()
        {
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(FlightModifier)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(Fleeing)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(LifeRest)));            
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(_moralRest)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(MovementRest)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(ArmyAffiliation)));
        }

        public ModelUnit CloneWithoutTerrain()
        {
            ModelUnit clonedUnit = (ModelUnit)Clone();
            clonedUnit.InTerrain = null;
            return clonedUnit;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    [Serializable()]
    public abstract class RangeUnit : ModelUnit
    {
        public byte Reichweite { get; set; }

        public double FernStrength { get; set; }

        public virtual void RangeAttack(ModelUnit defender, out short lifeDamage, out short moralDamage, out Type effectType)
        {
            double doubleLifeDamage;
            double defenderBonus = 1;
            if (defender.InTerrain.defendBonus != null && defender.InTerrain.defendBonus < 1)
            {
                defenderBonus = 1 / defender.InTerrain.defendBonus ?? 1;
            }
            doubleLifeDamage = calculateRangedLifeDamage(defender) * defenderBonus;


            moralDamage = Convert.ToInt16(CalculateMoralDamage(doubleLifeDamage));
            lifeDamage = Convert.ToInt16(doubleLifeDamage);

            effectType = typeof(RangeUnit);

            MovementRest = 0;
        }

        abstract public double calculateRangedLifeDamage(ModelUnit defender);
    }

    [Serializable()]
    public abstract class Cavalry : ModelUnit, INextTurnAfterNextTurnUnit
    {
        public Cavalry() { }

        public override bool HasSpecialStregth { get; set; } = true;

        abstract public double AngriffStaerke { get; set; }

        private bool _angriffbereit = false;
        public bool ReadyToCharge
        {
            get
            {
                return _angriffbereit;
            }
            set
            {
                SetProperty(ref _angriffbereit, value);
            }
        }

        public override bool attack(ModelUnit defender, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out bool showSpecialAttack, out MovementItemDTO movementItemDTO)
        {
            bool avoidDefender = base.attack(defender, out lifeDamage, out moralDamage, out extraMoralDamage, out showSpecialAttack, out movementItemDTO);

            if (ReadyToCharge)
            {
                lifeDamage *= Convert.ToInt16(AngriffStaerke);
                moralDamage *= Convert.ToInt16(AngriffStaerke);
                ReadyToCharge = false;
                showSpecialAttack = true;
            }

            //attackerAvoidDefender
            return avoidDefender;
        }

        public override bool defend(ModelUnit attacker, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out bool showSpecialDefense)
        {
            ReadyToCharge = false;
            return base.defend(attacker, out lifeDamage, out moralDamage, out extraMoralDamage, out showSpecialDefense);
        }
        
        public void NextTurnAfterNextTurn()
        {
            if (!Fleeing)
            {
                IEnumerable<Terrain> list = InTerrain.Adjacents.
                Where(x => x.unitInTerrain != null && x.unitInTerrain.ArmyAffiliation != this.ArmyAffiliation && x.unitInTerrain.MoralRest > 0);

                if (list.Count() > 0) ReadyToCharge = false;
                else if (!this.Fleeing) ReadyToCharge = true;
            }
        }

       
    }

    [Serializable()]
    public abstract class MoralModifier : ModelUnit
    {
        public MoralModifier() { }

        public abstract double MoralFactor { get; set; }
        public void DirectAccessMoral(short moralrest)
        {
            _moralRest = moralrest;
        }
    }

    public interface INextTurnAfterNextTurnUnit
    {
        void NextTurnAfterNextTurn();
    }

    public interface IMultipleAttackerUnit
    {
        bool InRage { set; get; }
    }
}
