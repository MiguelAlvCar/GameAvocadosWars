
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
//using Model.Actions;
using BasicElements;
using Ninject;
using Ninject.Parameters;
using BasicElements.AbstractClasses;


namespace SaveLoad
{
    public class DateiReader
    {
        SaveLoadViewModel ModelView;

        IModelGame _Game;

        internal DateiReader (SaveLoadViewModel modelView, IModelGame game)
        {
            _Game = game;

            ModelView = modelView;
        }

        internal void InDateiSchreiben(string path, bool isWorkingWithMap)
        {
            FileStream fileStr;
            fileStr = new FileStream(path, FileMode.Create);
            IBattleData battledata = null;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                
                if (isWorkingWithMap)
                {
                    battledata = (IBattleData)_Game.IBattledata.Clone();
                    _Game.IBattledata = null;
                }
                formatter.Serialize(fileStr, _Game);
                fileStr.Close();
                if (isWorkingWithMap)
                {
                    _Game.IBattledata = battledata;
                }
                ModelView.OnClose();
            }
            catch (SerializationException e)
            {
                fileStr.Close();
                if (isWorkingWithMap)
                {
                    _Game.IBattledata = battledata;
                }
                throw e;
            }
        }

        internal IModelGame VonDateiLesen(string path, bool isWorkingWithMap)
        {
            FileStream fileStr = new FileStream(path, FileMode.Open);
            //BinaryFormatter formatter = new BinaryFormatter();
            IModelGame gam = null;
            try
            {
                gam = BasicMechanisms.Kernel.Get<IDeserializer>().Deserialize(fileStr);
                //gam = (Game)formatter.Deserialize(fileStr);
                //fileStr.Close();

                gam.IActionsloader = BasicMechanisms.Kernel.Get<IActionsLoader>(
                    new[] {
                    new ConstructorArgument ("game", gam) }
                );

                //gam.IActionsloader = new ActionsLoader(gam);

                if (isWorkingWithMap)
                {
                    gam.IBattledata = BasicMechanisms.Kernel.Get<IBattleData>(
                        new[] {
                        new ConstructorArgument ("game", gam) }
                    );

                    //gam.IBattledata = new BattleData(gam);
                    gam.colorTurn = ArmyColor.Blue;
                    gam.Gamestate = GamestateModel.CreatingMap;
                }

            }
            catch (SerializationException e)
            {
                fileStr.Close();
                throw e;
            }

            return gam;
        }
            
    }
}
