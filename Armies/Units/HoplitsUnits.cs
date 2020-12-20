using System;
using System.Collections.Generic;
using System.Text;
using ModelGame;
using ModelGame.Actions;
using System.Linq;
using BasicElements;
using Armies.TemplateMethods;

namespace Armies
{
    [Serializable()]
    public class Hoplit : ModelUnit, INextTurnAfterNextTurnUnit
    {
        private bool _verteidungsfaehig = false;
        public bool Verteidungsfaehig
        {
            get
            {
                return _verteidungsfaehig;
            }
            set
            {
                SetProperty(ref _verteidungsfaehig, value);
            }
        }

        public Hoplit()
        {
            HasSpecialStregth = true;
            Movementbasis = 2;
            MovementRest = Movementbasis;
            Strength = 4;
        }

        public override bool attack(ModelUnit defender, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out bool showSpecialAttack, out MovementItemDTO movementItemDTO)
        {
            Verteidungsfaehig = false;
            return base.attack(defender, out lifeDamage, out moralDamage, out extraMoralDamage, out showSpecialAttack, out movementItemDTO);
        }

        public override bool defend(ModelUnit attacker, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out bool showSpecialDefense)
        {
            if (Verteidungsfaehig)
            {
                attacker.attack(attacker, out lifeDamage, out moralDamage, out extraMoralDamage, out showSpecialDefense, out MovementItemDTO movementItemDTO);
                Verteidungsfaehig = false;

                showSpecialDefense = true;
                // defenderAvoidAttacker
                return true;
            }
            else
                return base.defend(attacker, out lifeDamage, out moralDamage, out extraMoralDamage, out showSpecialDefense);
        }
        
        public void NextTurnAfterNextTurn()
        {
            if (!Fleeing)
            {
                IEnumerable<Terrain> list = InTerrain.Adjacents.
                Where(x => x.unitInTerrain != null && x.unitInTerrain.ArmyAffiliation != this.ArmyAffiliation && x.unitInTerrain.MoralRest > 0);
                if (list.Count() > 0)
                    this.Verteidungsfaehig = false;
                else if (!this.Fleeing && LifeRest > 33)
                    this.Verteidungsfaehig = true;
            }
        }
    }

    [Serializable()]
    public class Companion : Cavalry
    {
        public Companion()
        {
            Movementbasis = 4;
            MovementRest = Movementbasis;
            Strength = 4;
        }
        override public double AngriffStaerke { get; set; } = 2;
    }

    [Serializable()]
    public class Elephant : MoralModifier
    {
        public Elephant()
        {
            HasSpecialStregth = true;
            Movementbasis = 3;
            MovementRest = Movementbasis;
            Strength = 7;
        }
          
        private bool _tobend;

        public bool tobend
        {
            get { return _tobend; }
            set
            {
                if (value && !_tobend)
                {
                    FlightModifier = 5;
                    SetProperty<short>(ref _moralRest, 100, nameof(MoralRest));
                    //_moralRest = 100;
                    MovementRest = 0;
                }
                SetProperty(ref _tobend, value);
            }
        }

        private int _FluchtModifikator;
        override public int FlightModifier
        {
            get { return _FluchtModifikator; }
            set
            {
                if (value == 0 && _FluchtModifikator != 0)
                {
                    this.tobend = false;
                    this.DirectAccessMoral(7);
                    this.MovementRest = this.Movementbasis;
                }
                _FluchtModifikator = value;
            }
        }

        public override double MoralFactor { get; set; } = 1.8;
        override public short MoralRest
        {
            get { return _moralRest; }
            set
            {
                short moralrest = Convert.ToInt16(Math.Round(_moralRest - (MoralFactor * (_moralRest - value))));
                if (moralrest > 100) moralrest = 100;
                if (moralrest < 0)
                {
                    SetProperty<short>(ref _moralRest, 0);
                }
                else if (!this.tobend)
                {
                    if (moralrest < 0) moralrest = 0;
                    if (moralrest > LifeRest) moralrest = LifeRest;
                    SetProperty(ref _moralRest, moralrest);
                }/*
                if (value > 100) _MoralRest = 100;*/
            }
        }

        public override bool attack(ModelUnit defender, out short lifeDamage, out short moralDamage, out short extraMoralDamage, out bool showSpecialAttack, out MovementItemDTO movementItemDTO)
        {
            bool avoidDefender = base.attack(defender, out lifeDamage, out moralDamage, out extraMoralDamage, out showSpecialAttack, out movementItemDTO);

            showSpecialAttack = true;

            //attackerAvoidDefender
            return avoidDefender;
        }

        public override void NextTurnToOwn()
        {
            if (!tobend)
            {
                base.NextTurnToOwn();
            }            
        }

        public override void NextTurn()
        {
            if (!this.tobend)
            {
                MoralRest += 5;
                MoralRest += Convert.ToInt16(LifeRest / 20);
            }
        }
                
        public void NextTurnElefant(System.Collections.Stack RandomDirectionsParticular, FrightenElefantStorage listStorage, ActionsLoader actionsLoader)
        {
            if (this.tobend) this.FlightModifier--;
            if (this.MoralRest == 0 && !this.tobend) this.tobend = true;
            if (tobend)
            {
                ElefantAttackUnit ele = new ElefantAttackUnit(InTerrain.XX, InTerrain.YY, (byte)LifeRest, (byte)MoralRest, this.GetType(), ArmyAffiliation, new ModifierConverters().GetModifierFromModel(this));
                listStorage.Units.Enqueue(ele);
                ParticularEleAttack parEleAttack = new ParticularEleAttack();
                listStorage.states.Enqueue(parEleAttack);
                parEleAttack.Elefant = ele;

                for (byte i = 0; i < 2; i++)
                {                    
                    if (this.LifeRest > 0)
                    {
                        State state = new State();
                        this.MovementRest = 1;
                        actionsLoader.Movement.ShowMovementPosible(this, true);
                        List<Terrain> adjacents = InTerrain.Adjacents.ToList();
                        int direction = (int)RandomDirectionsParticular.Pop();
                        if (direction >= adjacents.Count())
                        {
                            direction = new Random().Next(0, adjacents.Count());
                        }
                        if (adjacents[direction].unitInTerrain != null)
                        {
                            ModelUnit un = adjacents[direction].unitInTerrain;
                            listStorage.Units.Enqueue(new ElefantAttackUnit(adjacents[direction].XX, adjacents[direction].YY, (byte)un.LifeRest, (byte)un.MoralRest, un.GetType(), un.ArmyAffiliation, new ModifierConverters().GetModifierFromModel(un)));
                        }

                        actionsLoader.Movement.Move(adjacents[direction], this, true);
                        state.X = adjacents[direction].XX;
                        state.Y = adjacents[direction].YY;
                        parEleAttack.XfinalEle = InTerrain.XX;
                        parEleAttack.YfinalEle = InTerrain.YY;
                        state.Elefant = new ElefantAttackUnit(InTerrain.XX, InTerrain.YY, (byte)LifeRest, (byte)MoralRest, this.GetType(), ArmyAffiliation, new ModifierConverters().GetModifierFromModel(this));
                        if (adjacents[direction].unitInTerrain != null && adjacents[direction].unitInTerrain != this)
                        {
                            ModelUnit un = adjacents[direction].unitInTerrain;
                            state.Enemy = new ElefantAttackUnit(adjacents[direction].XX, adjacents[direction].YY, (byte)un.LifeRest, (byte)un.MoralRest, un.GetType(), un.ArmyAffiliation, new ModifierConverters().GetModifierFromModel(un));
                        }
                        parEleAttack.states.Enqueue(state);
                    }
                }
            }
        }
    }

    [Serializable()]
    public class LightInfantryH : ModelUnit
    {
        public LightInfantryH()
        {
            Movementbasis = 2;
            MovementRest = Movementbasis;
            Strength = 3;
        }
    }
}
