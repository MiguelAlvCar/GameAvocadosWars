using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ModelGame;
using BasicElements;
using ModelGame.Actions;
using System.Diagnostics;

namespace ModelGame.Actions
{
    public class CloseCombat : AbstractCloseCombat
    {
        public CloseCombat(Game game): base (game) { }

        public override bool Combat (ModelUnit attacker, ModelUnit defender, out MovementItemDTO movementItemDTO)
        {
            if (!defender.Fleeing)
            {
                Debug.WriteLine("attacker: " + attacker.GetType() + "; defender: " + defender.GetType() );

                bool wasDefenderAvoidedByAttacker = defender.defend(attacker, out short attackerLifedamage, out short attackerMoraldamage, out short attackerExtraMoralDamage, out bool showSpecialDefense);
                bool wasAttackerAvoidedByDefender = attacker.attack(defender, out short defenderLifedamage, out short defenderMoraldamage, out short defenderExtraMoralDamage, out bool showSpecialAttack, out movementItemDTO);

                Debug.WriteLine("attacker.LifeRest: " + attacker.LifeRest + "; attacker.MoralRest: " + attacker.MoralRest + "; attacker.Strength: " + attacker.Strength);
                Debug.WriteLine("defender.LifeRest: " + defender.LifeRest + "; defender.MoralRest: " + defender.MoralRest + "; defender.Strength: " + defender.Strength);
                Debug.WriteLine("attackerLifedamage: " + attackerLifedamage + "; attackerMoraldamage: " + attackerMoraldamage);
                Debug.WriteLine("defenderLifedamage: " + defenderLifedamage + "; attackerMoraldamage: " + defenderMoraldamage);
                Debug.WriteLine("");

                if (!wasAttackerAvoidedByDefender)
                {                    
                    attacker.LifeRest -= attackerLifedamage;
                    attacker.MoralRest -= attackerMoraldamage;
                    if (!wasDefenderAvoidedByAttacker)
                        attacker.MoralRest -= attackerExtraMoralDamage;
                }
                else
                    attacker.MoralRest -= defenderExtraMoralDamage;
                if (!wasDefenderAvoidedByAttacker)
                {
                    defender.LifeRest -= defenderLifedamage;
                    defender.MoralRest -= defenderMoraldamage;
                    if (!wasAttackerAvoidedByDefender)
                        defender.MoralRest -= defenderExtraMoralDamage;
                }
                else
                    defender.MoralRest -= attackerExtraMoralDamage;

                movementItemDTO.showSpecialDefense = showSpecialDefense;
                movementItemDTO.showSpecialAttack = showSpecialAttack;

                _Game.OnAttackerEffect(attacker, showSpecialAttack);

                if (showSpecialDefense)
                {
                    movementItemDTO.DefenderForEffect = defender.CloneWithoutTerrain();
                    _Game.OnDefenderEffect(defender);
                }
                
                // defender killed
                if (defender.LifeRest > 0)
                    return false;
                else
                    return true;
            }
            else
            {
                movementItemDTO = new MovementItemDTO();
                UnitItemDTO unitItem = new UnitItemDTO(defender);
                List<UnitItemDTO> listUnits=  movementItemDTO.ListUnitItems.ToList();
                listUnits.Add(unitItem);
                movementItemDTO.ListUnitItems = listUnits;
                defender.LifeRest = 0;
                return true;
            }
        }
    }

    public abstract class AbstractCloseCombat
    {
        public abstract bool Combat(ModelUnit attacker, ModelUnit defender, out MovementItemDTO movementItemDTO);

        protected Game _Game;

        public AbstractCloseCombat(Game game)
        {
            _Game = game;
        }
    }
}
