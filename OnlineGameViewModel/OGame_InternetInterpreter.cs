
using System;
using ViewModelHotSeat;
using OnlineGameChatAndStore;
using System.Windows.Input;
using BasicElements;
using Ninject;
using BasicElements.AbstractServerInternetCommunication;
using DTO_Models;
using ModelGame;
using System.Collections.Generic;
using ArmyAndUnitTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using BasicElements.AbstractClasses;
using System.Security.Cryptography.X509Certificates;

namespace ViewModelOnlineGame
{
    public partial class OnlineGameViewModel : HotSeatViewModel
    {
        private void NewChatMessageFromAdversary(string message)
        {
            PlayerDTO adversary = !OnlineManager.IsGuest ? OnlineManager.ChatGame.Guest : OnlineManager.ChatGame.Host;
            AddMessageToChat(message, adversary, !OnlineManager.IsGuest);
        }

        public event Action NotificationAdversaryOffline;
        private void AdversaryGoesFromGame()
        {
            OnlineManager.Listener.Close();
            OnlineManager.Transmiter.Close();
            IsAdverdaryOnline = false;
            NotificationAdversaryOffline();
        }

        private void SetInitialValues(InitialValues initialValues)
        {
            if (initialValues.Army1 != null && (Army1 == null || initialValues.Army1 != (int)Army1.ArmyID))
            {
                foreach (ArmyType item in BasicMechanisms.Kernel.Get<IListArmiesMainModelView>().listArmiesObSave)
                {
                    if ((int)item.ArmyID == initialValues.Army1)
                    {
                        _Army1 = item;
                        break;
                    }
                }
            }
            if (initialValues.Army2 != null && (Army2 == null || initialValues.Army2 != (int)Army2.ArmyID))
            {
                foreach (ArmyType item in BasicMechanisms.Kernel.Get<IListArmiesMainModelView>().listArmiesObSave)
                {
                    if ((int)item.ArmyID == initialValues.Army2)
                    {
                        _Army2 = item;
                        break;
                    }
                }
            }

            if (initialValues.player1 != null && (Player1 == null || initialValues.player1 != Player1))
            {
                Player1 = initialValues.player1;
            }
            if (initialValues.player2 != null && (Player2 == null || initialValues.player2 != Player2))
            {
                Player2 = initialValues.player2;
            }

            if (initialValues.player1Points != null && (Player1Points == null || initialValues.player1Points != Player1Points))
            {
                _Player1Points = initialValues.player1Points;
            }
            if (initialValues.player2Points != null && (Player2Points == null || initialValues.player2Points != Player2Points))
            {
                _Player2Points = initialValues.player2Points;
            }
        }

        private void AdjustWidthAndLenghMap(int x, int y)
        {
            BasisCreateMap((short)y, (short)x);
        }

        private void AddRiverEnd(int riverEnd, int x, int y)
        {
            ViewModelTerrain terr = SearchTerrain(x, y);
            if (terr != null)
                terr.BasisCreateRiver(riverEnd);
        }
        private void ClearRiverEnds(int x, int y)
        {
            ViewModelTerrain terr = SearchTerrain(x, y);
            if (terr != null)
                terr.BasisClearRiverEnds();
        }
        private void AddBridge(bool isBridge, int x, int y)
        {
            ViewModelTerrain terr = SearchTerrain(x, y);
            if (terr != null)
                terr.BasisBridge = isBridge;
        }
        private void AddDeploymentArea(ArmyColor deployColor, int x, int y)
        {
            ViewModelTerrain terr = SearchTerrain(x, y);
            if (terr != null)
                terr.BasisDeploymentArea = deployColor;
        }
        private void NewTerrain(TerrainType terrainType, int x, int y)
        {
            ViewModelTerrain terr = SearchTerrain(x, y);
            if (terr != null)
            {
                ((OnlineViewModelTerrain)terr).BasisTerrainTypeClass = TypeTerrainClass.ListStaticTerrains.FirstOrDefault(z => z.terrainType == terrainType);                
            }
        }
        private ViewModelTerrain SearchTerrain (int x, int y)
        {

            Terrain terr = _Game.Map.FirstOrDefault(z => z.XX == x && z.YY == y );
            if (terr != null)
                return (ViewModelTerrain)terr.OnGetViewModelTerrain();
            else
                return null;
        }

        private void ToDeployment (bool hasAdverdaryConfirmed, bool isConfirmed)
        {
            if (hasAdverdaryConfirmed && isConfirmed)
            {
                if (OnlineManager.IsGuest)
                    ChangeGamestate(GamestateMV.DeployingBlueUnits);
                else
                    ChangeGamestate(GamestateMV.DeployingRedUnits);
            }
            else
                HasAdversaryConfirmedMap = hasAdverdaryConfirmed;
        }

        private void InternetToBattle(bool hasAdverdaryConfirmed, bool isConfirmed, List<UnitItemDTO> listDeployed)
        {
            if (hasAdverdaryConfirmed)
            {
                if (listDeployed.First().Unit.ArmyAffiliation != ArmyColor.Red)
                {
                    while (Game.ListBlues.Count > 0)
                    {
                        Game.DisposeUnitModel(Game.ListBlues[0]);
                    }
                }
                else
                {
                    while (Game.ListReds.Count > 0)
                    {
                        Game.DisposeUnitModel(Game.ListReds[0]);
                    }
                }

                foreach (UnitItemDTO deployedUItem in listDeployed)
                {
                    if (OnlineManager.IsGuest)
                        Game.ListReds.Add(deployedUItem.Unit);
                    else
                        Game.ListBlues.Add(deployedUItem.Unit);
                    deployedUItem.Unit.Game = Game;
                    Game.OnUnitCreated(deployedUItem.Unit);
                    deployedUItem.Unit.AfterDeserialization();
                    deployedUItem.Unit.DisposeFromGame += (uni) => Game.DisposeUnitModel(uni);
                }
                if (IsArmyConfirmed)
                {
                    foreach (UnitItemDTO deployedUItem in listDeployed)
                    {
                        Terrain terrain = Game.Map.First(x => x.XX == deployedUItem.XX && x.YY == deployedUItem.YY);
                        terrain.unitInTerrain = deployedUItem.Unit;
                    }
                }
                else
                {
                    foreach (UnitItemDTO deployedUItem in listDeployed)
                    {
                        if (OnlineManager.IsGuest)
                            Game.RedUnitsStorage.Push(deployedUItem);
                        else
                            Game.BlueUnitsStorage.Push(deployedUItem);
                    }
                }
            }

            if (Game.Gamestate == GamestateModel.Battle)
            {
                if (listDeployed.First().Unit.ArmyAffiliation == ArmyColor.Red)
                {
                    foreach (ModelUnit unit1 in Game.ListBlues)
                    {
                        Game.ActionsLoader.VisibleTerrains.RecalculateVisibility(unit1);
                    }
                }
                else
                {
                    foreach (ModelUnit unit1 in Game.ListReds)
                    {
                        Game.ActionsLoader.VisibleTerrains.RecalculateVisibility(unit1);
                    }
                }
                Game.ActionsLoader.VisibleTerrains.listVisilibity.ShowTerrains(Game.ColorArmyToShow);
            }

            if (hasAdverdaryConfirmed && isConfirmed && Game.Gamestate != GamestateModel.Battle)
                ChangeGamestate(GamestateMV.Battle);
            else
                HasAdversaryConfirmedArmy = hasAdverdaryConfirmed;

        }

        private void InternetNextTurn (IModelGame game)
        {
            Game = (Game)game;
            ArmyColor armyForVisibility = IsLocalPlayerHost() ? ArmyColor.Red : ArmyColor.Blue;
            Game.ColorArmyToShow = armyForVisibility;
            Game.AfterDeserialization(false, true);
            Game.NextTurnAfterReceivedSignal(armyForVisibility);
        }

        private void Victory(PlayerDTO player)
        {
            if (player.ID == OnlineManager.ChatGame.Guest.ID)
                Game._Win(player.Name, true);
            else if (player.ID == OnlineManager.ChatGame.Host.ID)
                Game._Win(player.Name, true);
        }

        private void NewMovement(MovementItemDTO movementItemDTO)
        {
            IEnumerable<ModelUnit> unitsInGame = Game.ListBlues.Union(Game.ListReds);

            foreach (UnitItemDTO unitItem in movementItemDTO.ListUnitItems)
            {
                // Here there is a exception thrown by the MockListener when it go to battle and the user havenn't pressed the confirm battle
                ModelUnit originalUnit = unitsInGame.First(x => x.ID.ArmyColor == unitItem.Unit.ID.ArmyColor && x.ID.ID == unitItem.Unit.ID.ID);
                unitItem.OriginalTerrain = originalUnit.InTerrain;
                originalUnit.InTerrain.unitInTerrain = null;
            }

            foreach (UnitItemDTO unitItem in movementItemDTO.ListUnitItems)
            {
                if (unitItem.isDead)
                {
                    // Here there is a exception thrown by the MockListener when it go to battle and the user havenn't pressed the confirm battle
                    ModelUnit originalUnit = unitsInGame.First(x => x.ID.ArmyColor == unitItem.Unit.ID.ArmyColor && x.ID.ID == unitItem.Unit.ID.ID);
                    Terrain terrainOfOriginalUnit = unitItem.OriginalTerrain;
                    Game.DisposeUnitModel(originalUnit);
                }
            }

            foreach (UnitItemDTO unitItem in movementItemDTO.ListUnitItems)
            {
                if (!unitItem.isDead)
                {
                    // Here there is a exception thrown by the MockListener when it go to battle and the user havenn't pressed the confirm battle
                    ModelUnit originalUnit = unitsInGame.First(x => x.ID.ArmyColor == unitItem.Unit.ID.ArmyColor && x.ID.ID == unitItem.Unit.ID.ID);
                    Terrain terrainOfOriginalUnit = unitItem.OriginalTerrain;

                    if (unitItem.Unit.ArmyAffiliation == ArmyColor.Red)
                        Game.ListReds.Add(unitItem.Unit);
                    else
                        Game.ListBlues.Add(unitItem.Unit);
                    unitItem.Unit.Game = Game;
                    Game.OnUnitCreated(unitItem.Unit);
                    unitItem.Unit.AfterDeserialization();
                    unitItem.Unit.DisposeFromGame += (uni) => Game.DisposeUnitModel(uni);

                    Terrain newLocationTerrain = Game.Map.First(x => x.XX == unitItem.XX && x.YY == unitItem.YY);
                    newLocationTerrain.unitInTerrain = unitItem.Unit;

                    if (unitItem.Unit.ArmyAffiliation == Game.ColorArmyToShow)
                        Game.ActionsLoader.VisibleTerrains.RecalculateVisibilityDifferentEnvironment(unitItem.Unit, originalUnit);

                    int range = Game.ActionsLoader.VisibleTerrains.MaxVisibilityBonus + Game.ActionsLoader.VisibleTerrains.VisibilityRange;
                    IEnumerable<Terrain> rangeFromOriginal = Game.ActionsLoader.FieldsRelations.TerrainsInRangeOfOne(terrainOfOriginalUnit, Game.Map, range);
                    IEnumerable<Terrain> rangeFromNewLocation = Game.ActionsLoader.FieldsRelations.TerrainsInRangeOfOne(newLocationTerrain, Game.Map, range);
                    IEnumerable<ModelUnit> allUnitToracalculateVisi = rangeFromOriginal.Union(rangeFromNewLocation).Where(x => x.unitInTerrain != null &&
                        x.unitInTerrain.ArmyAffiliation == Game.ColorArmyToShow).Select(x => x.unitInTerrain);
                    foreach (ModelUnit unit2 in allUnitToracalculateVisi)
                    {
                        Game.ActionsLoader.VisibleTerrains.RecalculateVisibility(unit2);
                    }

                    Game.DisposeUnitModel(originalUnit);
                }

                
            }

            Game.ActionsLoader.VisibleTerrains.listVisilibity.ShowTerrains(Game.ColorArmyToShow);

            if (movementItemDTO.WasThereCombat)
            {
                ModelUnit attacker = unitsInGame.First(x => x.ID.ArmyColor == movementItemDTO.Attacker.ID.ArmyColor && x.ID.ID == movementItemDTO.Attacker.ID.ID);
                Game.OnAttackerEffect(attacker, movementItemDTO.showSpecialAttack);
            }               
                
            if (movementItemDTO.showSpecialDefense)
            {
                ModelUnit defender = unitsInGame.First(x => x.ID.ArmyColor == movementItemDTO.DefenderForEffect.ID.ArmyColor &&
                    x.ID.ID == movementItemDTO.DefenderForEffect.ID.ID);
                Game.OnDefenderEffect(defender);
            }
                
        }
    }
}
