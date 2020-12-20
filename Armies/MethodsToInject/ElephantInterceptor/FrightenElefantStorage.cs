using System;
using System.Collections.Generic;
using System.Text;
using BasicElements;
using ModelGame;

namespace Armies.TemplateMethods
{
    [Serializable()]
    public class FrightenElefantStorage
    {
        public Queue<ElefantAttackUnit> Units = new Queue<ElefantAttackUnit>();

        public Queue<ParticularEleAttack> states = new Queue<ParticularEleAttack>();
    }

    [Serializable()]
    public class ElefantAttackUnit
    {
        public ElefantAttackUnit(int x, int y, byte life, byte moral, Type unitType, ArmyColor color, Modifier mod)
        {
            X = (x / Terrain.XXX) + 1;
            Y = (y / Terrain.YYY) + 1;
            Life = life;
            Moral = moral;
            UnitType = unitType;
            Color = color;
            modifier = mod;
        }

        public int X { private set; get; }
        public int Y { private set; get; }
        public byte Life;
        public byte Moral;
        public Type UnitType;
        public ArmyColor Color;
        public Modifier modifier;
    }

    [Serializable()]
    public class ParticularEleAttack
    {
        public ElefantAttackUnit Elefant;
        public Queue<State> states = new Queue<State>();
        private int _XfinalEle;
        public int XfinalEle
        {
            set => _XfinalEle = (value / Terrain.XXX) + 1;
            get => _XfinalEle;
        }
        private int _YfinalEle;
        public int YfinalEle
        {
            set => _YfinalEle = (value / Terrain.YYY) + 1;
            get => _YfinalEle;
        }
    }

    [Serializable()]
    public class State
    {
        public ElefantAttackUnit Elefant;
        public ElefantAttackUnit Enemy;
        private int _X;
        public int X
        {
            set => _X = (value / Terrain.XXX) + 1;
            get => _X;
        }
        private int _Y;
        public int Y
        {
            set => _Y = (value / Terrain.YYY) + 1;
            get => _Y;
        }
    }
}
