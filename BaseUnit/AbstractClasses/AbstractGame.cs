using System;
using System.Collections.Generic;
using System.Text;
using Ninject;
using System.IO;


namespace BasicElements.AbstractClasses
{
    public interface IHotSeatViewModel
    {
        bool IsInBattle { get; }

        IModelGame IGame { set; get; }

        void RemoveModelMap();
        void RemoveAllUnits();
    }

    public interface IModelGame
    {
        void AfterDeserialization(bool isWorkingWithMap, bool isDeserializingIntoOnline = false);
        IBattleData IBattledata {get; set;}
        IActionsLoader IActionsloader { set; get; }
        ArmyColor colorTurn { set; get; }
        GamestateModel Gamestate { set; get; }
    }
    public interface IDeserializer
    {
        IModelGame Deserialize(Stream fileStream);
    }

    public interface IBattleData: ICloneable
    {

    }

    public interface IActionsLoader
    {

    }
}
