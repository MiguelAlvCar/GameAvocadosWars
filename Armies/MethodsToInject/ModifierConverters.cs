using System;
using System.Collections.Generic;
using System.Text;
using ModelGame;
using ModelGame.Actions;
using System.Linq;
using BasicElements;
using BasicElements.AbstractForArmyInterceptor;

namespace Armies
{
    public class ModifierConverters : IModifierConverters
    {
        public Modifier GetModifierFromModel(IUnitModel uni)
        {
            Modifier modifier = Modifier.None;
            if (((ModelUnit)uni).Fleeing)
                modifier = Modifier.Flight;
            else if (uni is Berseker && ((Berseker)uni).InRage)
                modifier = Modifier.Rage;
            else if (uni is Cavalry && ((Cavalry)uni).ReadyToCharge)
                modifier = Modifier.Charge;
            else if (uni is Hoplit && ((Hoplit)uni).Verteidungsfaehig)
                modifier = Modifier.Formation;
            else if (uni is Elephant && ((Elephant)uni).tobend)
                modifier = Modifier.Fright;
            else
                modifier = Modifier.None;

            return modifier;
        }
    }
}
