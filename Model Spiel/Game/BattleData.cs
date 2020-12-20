
using System;
using System.Collections.Generic;
using BasicElements;
using BasicElements.AbstractClasses;

namespace ModelGame
{
    [Serializable()]
    public class BattleData : ICloneable, IBattleData
    {
        public BattleData(Game game)
        {
            turn = 0;

            RedsList = new ListArmy<ModelUnit>(game);
            BluesList = new ListArmy<ModelUnit>(game);
        }

        public ListArmy<ModelUnit> RedsList { set; get; }
        public ListArmy<ModelUnit> BluesList { set; get; }

        internal int _turn;
        public int turn { set => _turn = value; get => _turn; }

        internal ArmyColor _colorTurn;
        public ArmyColor colorTurn { set => _colorTurn = value; get => _colorTurn; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    [Serializable()]
    public class ListArmy<T> : List<T> where T : ModelUnit
    {
        private Game game;
        public ListArmy(Game gam)
        {
            game = gam;
        }
        public new void Remove(T item)
        {
            base.Remove(item);
            if (this.Count <= 0 && game.colorTurn == game.ColorArmyToShow)
            {
                if (game.IsInBattleAfterWaitingForLateToBattleSignal)
                {
                    if (item.ArmyAffiliation == ArmyColor.Blue)
                        game._Win(game.InitialValues.player1, false);
                    if (item.ArmyAffiliation == ArmyColor.Red)
                        game._Win(game.InitialValues.player2, false);
                }
            }
        }
    }
}
