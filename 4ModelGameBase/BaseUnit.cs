using System;
using System.ComponentModel;
using BasicElements;
using System.Collections.Generic;
using System.Linq;
using ModelGameBase.Actions;
using System.Diagnostics;

namespace Model
{
    [Serializable()]
    public abstract class UnitModel : ModelBase
    {
        public static string Designation { set; get; }
        public static string GetDesignation<Unit>() where Unit : UnitModel
        {
            string typeunitWithPrefix = typeof(Unit).ToString();
            int IndexDoc = typeunitWithPrefix.LastIndexOf('.') + 1;
            string typeunitWithoutPrefix = typeunitWithPrefix.Substring(IndexDoc, typeunitWithPrefix.Length - IndexDoc);
            return typeunitWithoutPrefix;
        }

        public UnitModel()
        {
            LifeRest = LifeBasis;
            MoralRest = Moralbasis;
            Fleeing = false;
        }

        #region Binds with terrain and game

        public static Func<object> GetGameMap;
        public static object OnGetGameMap()
        {
            return GetGameMap();
        }

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

        public static bool ProofIfAddArmyListNull()
        {
            if (_AddArmyList != null)
            {
                return false;
            }
            else
                return true;            
        }
        
        private static Action<UnitModel, ArmyColor> _AddArmyList;
        public static event Action<UnitModel, ArmyColor> AddArmyList
        {
            add { _AddArmyList += value; }
            remove { _AddArmyList -= value; }
        }
        private ArmyColor _heerzugeh;
        public ArmyColor heerzugeh
        {
            get
            {
                return _heerzugeh;
            }
            set
            {
                _AddArmyList(this, value);
                SetProperty(ref _heerzugeh, value);
            }
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

        public virtual bool defend(UnitModel attacker, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out Type effectType)
        {
            extraMoralDamage = 0;
            double doubleLifeDamage;
            if (InTerrain.defendBonus != null && InTerrain.defendBonus > 1)
                doubleLifeDamage = calculateLifeDamage(attacker) * InTerrain.defendBonus ?? 0;
            else
                doubleLifeDamage = calculateLifeDamage(attacker);


            
            moralDamage = Convert.ToInt16(calculateMoralDamage(doubleLifeDamage));
            lifeDamage = Convert.ToInt16(doubleLifeDamage);
            Debug.WriteLine("moralDamage Short: " + moralDamage + "; doubleLifeDamage: " + doubleLifeDamage);
            Debug.WriteLine("lifeDamage Short: " + lifeDamage + "; doubleLifeDamage: " + doubleLifeDamage);

            effectType = null;

            // defenderAvoidAttacker
            return false;
        }

        public virtual bool attack(UnitModel defender, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out Type effectType)
        {
            extraMoralDamage = 0;

            double doubleLifeDamage;
            double defenderBonus = 1;
            if (defender.InTerrain.defendBonus != null && defender.InTerrain.defendBonus < 1)
            {
                defenderBonus = 1 / defender.InTerrain.defendBonus ?? 1;
            }
            if (InTerrain.attackBonus != null)
                doubleLifeDamage = calculateLifeDamage(defender) * defenderBonus * InTerrain.attackBonus ?? 0;
            else
                doubleLifeDamage = calculateLifeDamage(defender) * defenderBonus;

            moralDamage = Convert.ToInt16(  calculateMoralDamage(doubleLifeDamage)  );
            lifeDamage = Convert.ToInt16( doubleLifeDamage );
            Debug.WriteLine("moralDamage Short: " + moralDamage + "; doubleLifeDamage: " + doubleLifeDamage);
            Debug.WriteLine("lifeDamage Short: " + lifeDamage + "; doubleLifeDamage: " + doubleLifeDamage);

            effectType = typeof(UnitModel);
            //attackerAvoidDefender
            return false;
        }

        public double calculateLifeDamage(UnitModel defender)
        {
            Debug.WriteLine("In calculateLifeDamage, (1 + (MoralRest / 100)): " + (1 + (MoralRest / 100)) + "; (2 + (defender.MoralRest / 100)): " + (2 + (defender.MoralRest / 100)));
            return (13 * Strength * (1 + (MoralRest / 100))) / (defender.Strength * (2 + (defender.MoralRest / 100)));
        }

        public double calculateMoralDamage(double lifeDamage)
        {
            Debug.WriteLine("In calculateMoralDamage, double lifeDamage: " + lifeDamage + "; lifeDamage * 2.4: " + lifeDamage * 2.4);
            return lifeDamage * 2.4;
        }

        #endregion

        #region Turn, Hide/Reveal

        public virtual void NextTurnToOwn(ActionsLoader gameBase)
        {
            if (InTerrain.river && !InTerrain.bridge)
                this.MovementRest = 1;
            else
                this.MovementRest = this.Movementbasis;
            gameBase.VisibleTerrains.ShowVisibles(this);
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
        public Action<UnitModel> _DisposeFromGame;
        public event Action<UnitModel> DisposeFromGame
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
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(heerzugeh)));
        }
    }

    [Serializable()]
    public abstract class RangeUnit : UnitModel
    {
        public byte Reichweite { get; set; }

        public double Fernkampf { get; set; }

        public virtual void RangeAttack(UnitModel defender, out short lifeDamage, out short moralDamage, out Type effectType)
        {
            double doubleLifeDamage;
            double defenderBonus = 1;
            if (defender.InTerrain.defendBonus != null && defender.InTerrain.defendBonus < 1)
            {
                defenderBonus = 1 / defender.InTerrain.defendBonus ?? 1;
            }
            doubleLifeDamage = calculateRangedLifeDamage(defender) * defenderBonus;


            moralDamage = Convert.ToInt16(calculateMoralDamage(doubleLifeDamage));
            lifeDamage = Convert.ToInt16(doubleLifeDamage);

            effectType = typeof(RangeUnit);

            MovementRest = 0;
        }

        abstract public double calculateRangedLifeDamage(UnitModel defender);
    }

    [Serializable()]
    public abstract class Cavalry : UnitModel, INextTurnAfterNextTurnUnit
    {
        public Cavalry() { }

        public override bool HasSpecialStregth { get; set; } = true;

        abstract public double AngriffStaerke { get; set; }

        private bool _angriffbereit = false;
        public bool Angriffbereit
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

        public override bool attack(UnitModel defender, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out Type effectType)
        {
            bool avoidDefender = base.attack(defender, out lifeDamage, out moralDamage, out extraMoralDamage, out effectType);

            if (Angriffbereit)
            {
                lifeDamage *= Convert.ToInt16(AngriffStaerke);
                moralDamage *= Convert.ToInt16(AngriffStaerke);
                Angriffbereit = false;
                effectType = typeof(Cavalry);
            }

            //attackerAvoidDefender
            return avoidDefender;
        }

        public override bool defend(UnitModel attacker, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out Type effectType)
        {
            Angriffbereit = false;
            return base.defend(attacker, out lifeDamage, out moralDamage, out extraMoralDamage, out effectType);
        }
        
        public void NextTurnAfterNextTurn()
        {
            if (!Fleeing)
            {
                IEnumerable<Terrain> list = InTerrain.Adjacents.
                Where(x => x.unitInTerrain != null && x.unitInTerrain.heerzugeh != this.heerzugeh && x.unitInTerrain.MoralRest > 0);

                if (list.Count() > 0) Angriffbereit = false;
                else if (!this.Fleeing) Angriffbereit = true;
            }
        }
    }

    [Serializable()]
    public abstract class MoralModifier : UnitModel
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

    public interface IUncontrolableUnit
    {
        void NextTurnElefant(System.Collections.Stack RandomDirectionsParticular, ElefantAttackStorage listStorage, ActionsLoader game);
    }
}
