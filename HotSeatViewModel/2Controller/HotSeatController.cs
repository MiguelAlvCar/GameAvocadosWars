using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ModelGame;
using System.Windows.Controls;
using System.IO;
using BasicElements;
using ModelGame.Actions;
using WpfBasicElements.AbstractClasses;
using BasicElements.AbstractClasses;
using Ninject;
using BasicElements.AbstractServerInternetCommunication;
using DTO_Models;

namespace ViewModelHotSeat
{
    public partial class HotSeatViewModel : ModelBase, IHotSeatViewModel
    {
        #region Map-1st zone

        public void RemoveModelMap()
        {
            while (Game.Map.Count > 0)
            {
                Game.DisposeTerrain(Game.Map[0]);
            }
        }

        public void RemoveAllUnits()
        {
            while (Game.ListBlues.Count > 0)
            {
                Game.DisposeUnitModel(Game.ListBlues[0]);
            }
            while (Game.ListReds.Count > 0)
            {
                Game.DisposeUnitModel(Game.ListReds[0]);
            }
        }

        public virtual void CreateMap(short length, short width)
        {
            BasisCreateMap(length, width);
        }

        protected void BasisCreateMap(short length, short width)
        {
            Game.OnChangeLenghtAndWidthMap(length, width, false);
            for (short len = 1; len <= length; len++)
            {
                for (short wid = 1; wid <= width; wid++)
                {
                    TerrainFabric.Create(wid, len, _Game);
                }
                foreach (Terrain terr in Game.Map)
                {
                    terr.Adjacents = Game.ActionsLoader.FieldsRelations.AdjacentTerrains(terr, Game.Map).ToList();
                }
            }
            
            foreach (Terrain terr in Game.Map)
            {
                if (terr.YY / Terrain.YYY < 3)
                    terr.deploymentArea = ArmyColor.Red;
                if (terr.YY / Terrain.YYY > Game.mapLenght - 4)
                    terr.deploymentArea = ArmyColor.Blue;
            }
        }
        
        #endregion

        #region Commands Creating Armies

        public ICommand DeleteComboItem { get; set; }
        private bool DeleteComboItemCanExecute(object obj)
        {
            return true;
        }
        private void DeleteComboItemExecute(object obj)
        {
            ComboBoxItem comboItem = obj as ComboBoxItem;
            listPlayersOb.Remove((string)comboItem.Content);

            FileInfo PlayerFile = new FileInfo("Spieler.dat");
            PlayerFile.Delete();
            FileStream FStr = PlayerFile.Create();
            PlayerFile.Attributes = FileAttributes.Hidden;
            FStr.Close();
            FileStream FStre = new FileStream("Spieler.dat", FileMode.Open);
            StreamWriter StWriter = new StreamWriter(FStre);
            foreach (string Player in listPlayersOb)
            {
                StWriter.WriteLine(Player);
            }
            StWriter.Close();
            FStre.Close();
        }

        protected void OnNotAllRedUnitDeployed()
        {
            NotAllRedUnitDeployed();
        }
        public event Action NotAllRedUnitDeployed;
        protected void OnNoUnitsDeployed()
        {
            NoUnitsDeployed();
        }
        public event Action NoUnitsDeployed;
        public event Action OpenPanelUnitInfoSelectingUnit;
        public ICommand ToPlayer2 { get; set; }
        private bool ToPlayer2CanExecute(object obj)
        {
            return true;
        }
        protected virtual void ToPlayer2Execute(object obj)
        {
            ChangeGamestate(GamestateMV.DeployingBlueUnits);
        }

        public ICommand ToBattle { get; set; }
        private bool ToBattle_CanExecute(object obj)
        {
            return true;
        }
        protected virtual void ToBattle_Execute(object obj)
        {
            ChangeGamestate(GamestateMV.Battle);
        }

        #endregion

        #region Army

        public Action<string> Win;

        #endregion        

        public ICommand NextTurn { get; set; }
        private bool NextTurn_CanExecute(object obj)
        {
            return Gamestate == GamestateMV.Battle;
        }
        protected virtual void NextTurn_Execute(object obj)
        {
            //Debugg
            //BattleDTO MakeBattleDTOFake(double result)
            //{
            //    BattleDTO battleDTO = new BattleDTO();
            //    battleDTO.Army1 = Game.InitialValues.Army1 ?? -1;
            //    battleDTO.Army2 = Game.InitialValues.Army2 ?? -1;
            //    battleDTO.Length = Game.mapLenght;
            //    battleDTO.Width = Game.mapWidth;
            //    battleDTO.Points1 = Game.InitialValues.player1Points ?? -1;
            //    battleDTO.Points2 = Game.InitialValues.player2Points ?? -1;
            //    battleDTO.Result = result;
            //    battleDTO.Player1ID = "fa2895b6-bd16-407a-ab5b-1c6788afa873";
            //    battleDTO.Player2ID = "edf2d38a-732f-4d39-92cc-c16044f483ce";
            //    return battleDTO;
            //}
            //double result1 = 10;
            //BattleDTO battleDTO1 = MakeBattleDTOFake(result1);
            //BasicMechanisms.Kernel.Get<IInternetCommunicationWithServer>().Victory(battleDTO1);
            //Debugg

            int colorNextTurn = (int)ColorTurn + 1;
            if (!Enum.IsDefined(typeof(ArmyColor), (ArmyColor)colorNextTurn))
            {
                colorNextTurn = (int)ArmyColor.Red;
            }

            Game.NextTurn(true, (ArmyColor)colorNextTurn, out object ob);
        }

        internal void selectNoUnit()
        {
            Game.SelectUnit(null, false);
        }

        #region Unit Commands
        
        public void SelectUnit(ViewModelUnit uni)
        {
            if (!IsChangingToNextTurn)
                Game.SelectUnit(uni.Unit, IsLocalPlayerInHisOwnTurn);
        }

        public ICommand ShowFire { get; set; }
        protected bool ShowFireCanExecute(object obj)
        {
            return Game.selectedUnit != null && !Game.selectedUnit.Fleeing && Game.selectedUnit.MovementRest > 0;
        }
        private void ShowFireExecute(object obj)
        {
            if ((bool)obj)
            {
                Game.ActionsLoader.Movement.ListMovement.ClearTerrains();
                Game.ActionsLoader.RangedCombat.ShowFirePosible((RangeUnit)Game.selectedUnit, ((RangeUnit)Game.selectedUnit).Reichweite);
            }
            else
            {
                Game.ActionsLoader.Movement.ShowMovementPosible(Game.selectedUnit);
                Game.ActionsLoader.RangedCombat.FireList.ClearTerrains();
            }
        }

        #endregion

        public virtual void SendTheListUnitItemsThroughNet(MovementItemDTO listUnitItems)
        {

        }
    }
}
