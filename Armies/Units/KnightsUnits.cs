using System;
using System.Collections.Generic;
using System.Text;
using ModelGame;

namespace Armies
{
    [Serializable()]
    public class SwordFighter : ModelUnit
    {
        public SwordFighter()
        {
            Movementbasis = 2;
            MovementRest = Movementbasis;
            Strength = 4;
        }
    }

    [Serializable()]
    public class Rider : Cavalry
    {
        public Rider()
        {
            Movementbasis = 4;
            MovementRest = Movementbasis;
            Strength = 3;
        }
        override public double AngriffStaerke { get; set; } = 1.8;
    }

    [Serializable()]
    public class Archer : RangeUnit
    {
        public Archer()
        {
            Movementbasis = 2;
            MovementRest = Movementbasis;
            Strength = 1;
            Reichweite = 5;
            FernStrength = 1.7;
        }                

        public override double calculateRangedLifeDamage(ModelUnit defender)
        {
            return (12 * FernStrength * (LifeRest + 1)) / (defender.Strength * (defender.LifeRest + 3));
        }
    }

    [Serializable()]
    public class Knight : Cavalry
    {
        public Knight()
        {
            Movementbasis = 3;
            MovementRest = Movementbasis;
            Strength = 5;
        }
        override public double AngriffStaerke { get; set; } = 2.3;
    }
}