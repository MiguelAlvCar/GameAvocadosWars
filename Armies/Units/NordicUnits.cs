using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelGame;
using ModelGame.Actions;

namespace Armies
{
    [Serializable()]
    public class Berseker : MoralModifier, IMultipleAttackerUnit
    {
        public Berseker()
        {
            HasSpecialStregth = true;
            Movementbasis = 2;
            MovementRest = Movementbasis;
            Strength = 5;
        }

        override public bool Fleeing
        {
            get { return _fleeing; }
            set
            {
                InRage = value;
            }
        }

        private bool _inRage = false;

        public bool InRage
        {
            get { return _inRage; }
            set
            {
                if (value && !_inRage)
                {
                    FlightModifier = 5;
                }
                SetProperty(ref _inRage, value);
            }
        }

        override public double MoralFactor { get; set; } = 1.15;

        override public short MoralRest
        {
            get { return _moralRest; }
            set
            {
                short moralrest = Convert.ToInt16(Math.Round(_moralRest - (MoralFactor * (_moralRest - value))));
                if (moralrest > 100) moralrest = 100;
                if (moralrest <= 0)
                {
                    SetProperty<short>(ref _moralRest, 100);
                    if (!this.InRage)
                        this.InRage = true;
                }
                else if (!this.InRage)
                {                    
                    if (moralrest < 0) moralrest = 0;
                    else if (moralrest > LifeRest) moralrest = LifeRest;
                    SetProperty<short>(ref _moralRest, moralrest);
                }
            }
        }

        private int _flightModifier;

        override public int FlightModifier
        {
            get { return _flightModifier; }
            set
            {
                _flightModifier = value;

                if (value == 0)
                {
                    this.InRage = false;
                    this.MoralRest += 7;
                }
            }
        }

        public override void ShowSpecialEffectWhileSelecting() 
        {
            Game.OnShowSpecialEffectWhileSelecting(this);
        }

        public override bool attack(ModelUnit defender, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out bool showSpecialAttack, out MovementItemDTO movementItemDTO)
        {
            movementItemDTO = new MovementItemDTO();
            if (InRage)
            {
                foreach (Terrain ter in InTerrain.Adjacents)
                {
                    if (ter.unitInTerrain != null && ter.unitInTerrain.ArmyAffiliation != ArmyAffiliation && ter.unitInTerrain != defender)
                    {
                        base.attack(ter.unitInTerrain, out lifeDamage, out moralDamage, out extraMoralDamage, out showSpecialAttack, out MovementItemDTO movementItemDTO1);
                        movementItemDTO.ListUnitItems = movementItemDTO.ListUnitItems.Union(movementItemDTO1.ListUnitItems);
                        ter.unitInTerrain.LifeRest -= lifeDamage;
                        ter.unitInTerrain.MoralRest -= moralDamage;
                    }
                }
                showSpecialAttack = true;

                bool attackerAvoidDefender = base.attack(defender, out lifeDamage, out moralDamage, out extraMoralDamage, out bool newBool, out MovementItemDTO movementItemDTO2);
                movementItemDTO.ListUnitItems = movementItemDTO.ListUnitItems.Union(movementItemDTO2.ListUnitItems);
                return attackerAvoidDefender;                
            }
            else
                //attackerAvoidDefender
                return base.attack(defender, out lifeDamage, out moralDamage, out extraMoralDamage, out showSpecialAttack, out movementItemDTO);
        }

        public override void NextTurn()
        {
            if (this.InRage) this.FlightModifier--;
            if (!this.InRage) this.MoralRest += 13;
        }

    }

    [Serializable()]
    public class HeavyInfantry : ModelUnit
    {
        public HeavyInfantry()
        {
            Movementbasis = 2;
            MovementRest = Movementbasis;
            Strength = 5;
        }
    }

    [Serializable()]
    public class Light_InfantryV : ModelUnit
    {
        public Light_InfantryV()
        {
            Movementbasis = 3;
            MovementRest = Movementbasis;
            Strength = 3;
        }
    }

    [Serializable()]
    public class Devotee : ModelUnit
    {
        public Devotee()
        {
            HasSpecialStregth = true;
            Movementbasis = 2;
            MovementRest = Movementbasis;
            Strength = 3.5;
        }

        public override bool attack(ModelUnit defender, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out bool showSpecialAttack, out MovementItemDTO movementItemDTO)
        {
            bool avoidDefender = base.attack(defender, out lifeDamage, out moralDamage, out extraMoralDamage, out showSpecialAttack, out movementItemDTO);

            extraMoralDamage = Convert.ToInt16(moralDamage * (AngstBonus - 1));

            showSpecialAttack = true;

            //attackerAvoidDefender
            return avoidDefender;
        }

        public double AngstBonus { set; get; } = 2.5;
    }
}
