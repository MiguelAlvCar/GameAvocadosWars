using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ModelGameBase;
using BasicElements;
using ModelGameBase.Actions;
using System.Diagnostics;

namespace Model.Actions
{
    public class CloseCombat : ICloseCombat
    {
        public static event Action<string> attackerEffect;
        public static event Action<string> defenderEffect;

        public void OnAttackerEffect (string str)
        {
            attackerEffect(str);
        }

        public bool Combat (UnitModel attacker, UnitModel defender)
        {
            if (!defender.Fleeing)
            {
                Debug.WriteLine("attacker: " + attacker.GetType() + "; defender: " + defender.GetType() );

                bool defenderAvoidAttacker = defender.defend(attacker, out short attackerLifedamage, out short attackerMoraldamage, out short attackerExtraMoralDamage, out Type defenderEffectType);
                bool attackerAvoidDefender = attacker.attack(defender, out short defenderLifedamage, out short defenderMoraldamage, out short defenderExtraMoralDamage, out Type attackerEffectType);

                Debug.WriteLine("attacker.LifeRest: " + attacker.LifeRest + "; attacker.MoralRest: " + attacker.MoralRest + "; attacker.Strength: " + attacker.Strength);
                Debug.WriteLine("defender.LifeRest: " + defender.LifeRest + "; defender.MoralRest: " + defender.MoralRest + "; defender.Strength: " + defender.Strength);
                Debug.WriteLine("attackerLifedamage: " + attackerLifedamage + "; attackerMoraldamage: " + attackerMoraldamage);
                Debug.WriteLine("defenderLifedamage: " + defenderLifedamage + "; attackerMoraldamage: " + defenderMoraldamage);
                Debug.WriteLine("");

                if (!attackerAvoidDefender)
                {                    
                    attacker.LifeRest -= attackerLifedamage;
                    attacker.MoralRest -= attackerMoraldamage;
                    if (!defenderAvoidAttacker)
                        attacker.MoralRest -= attackerExtraMoralDamage;
                }
                else
                    attacker.MoralRest -= defenderExtraMoralDamage;
                if (!defenderAvoidAttacker)
                {
                    defender.LifeRest -= defenderLifedamage;
                    defender.MoralRest -= defenderMoraldamage;
                    if (!attackerAvoidDefender)
                        defender.MoralRest -= defenderExtraMoralDamage;
                }
                else
                    defender.MoralRest -= attackerExtraMoralDamage;

                if (attackerEffectType != null)
                    attackerEffect(BasicMechanisms.RemovePrefixType(attackerEffectType.ToString()));

                if (defenderEffectType != null)
                    defenderEffect(BasicMechanisms.RemovePrefixType(defenderEffectType.ToString()));
                
                // defender killed
                if (defender.LifeRest > 0)
                    return false;
                else
                    return true;
            }
            else
            {
                defender.LifeRest = 0;
                return true;
            }
        }
    }
    public interface ICloseCombat
    {
        void OnAttackerEffect(string str);

        bool Combat(UnitModel attacker, UnitModel defender);
    }
}
