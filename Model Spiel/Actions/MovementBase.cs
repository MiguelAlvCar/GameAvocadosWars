using System;
using System.Collections.Generic;
using System.Text;
using BasicElements;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ModelGame;
using ModelGame.Actions;

namespace ModelGame.Actions
{
    public class MovementBase : AbstractMovement
    {
        public MovementBase (Game game, ActionsLoader actionsLoader) : base (game, actionsLoader) { }

        public override void ShowMovementPosible(ModelUnit unit, bool posibleAttackAgainsOwn = false)
        {
            if (unit.MovementRest > 0)
            {
                List<TerrainVisibleItem> Visibles = _ActionsLoader.VisibleTerrains.listVisilibity.CalculateWhichList(unit.ArmyAffiliation);
                AddAbjacentTerrainsToShow(new ListMovementItem(unit.InTerrain, null, 0), 0);

                foreach (ListMovementItem item in ListMovement)
                {
                    item.Terrain.movementposible = true;
                }

                void AddAbjacentTerrainsToShow(ListMovementItem terrainItem, short initialMovementCost)
                {
                    List<Terrain> EnuTerrains = terrainItem.Terrain.Adjacents.ToList();
                    List<ListMovementItem> TerrainsToContinueSearch = new List<ListMovementItem>();
                    foreach (Terrain terr in EnuTerrains)
                    {
                        if (terr.MovementCost != null &&
                            (!(terr.unitInTerrain != null && terr.unitInTerrain.ArmyAffiliation == unit.ArmyAffiliation) || posibleAttackAgainsOwn))
                        {
                            IEnumerable<Terrain> transverseTerrains = EnuTerrains.Intersect(terr.Adjacents);

                            Terrain TerrainToStop = null;
                            short movementcosttothere;
                            if (transverseTerrains.Any(t => t.unitInTerrain != null && Visibles.Any(x => x.TerrainVisible == t) 
                                && t.unitInTerrain.ArmyAffiliation != unit.ArmyAffiliation && !t.unitInTerrain.Fleeing) 
                                ||
                                (terr.unitInTerrain != null && Visibles.Any(x => x.TerrainVisible == terr) 
                                && ((terr.unitInTerrain.ArmyAffiliation != unit.ArmyAffiliation) || posibleAttackAgainsOwn)))
                            {
                                movementcosttothere = Convert.ToInt16(9 + initialMovementCost);
                            }
                            else if (transverseTerrains.Any(t => t.unitInTerrain != null && !Visibles.Any(x => x.TerrainVisible == t)
                                && t.unitInTerrain.ArmyAffiliation != unit.ArmyAffiliation)
                                ||
                                (terr.unitInTerrain != null && !Visibles.Any(x => x.TerrainVisible == terr)
                                && terr.unitInTerrain.ArmyAffiliation != unit.ArmyAffiliation && !terr.unitInTerrain.Fleeing )  )
                            {
                                movementcosttothere = terr.MovementCost ?? 0;
                                movementcosttothere += initialMovementCost;
                                TerrainToStop = terr;
                            }
                            else
                            {
                                movementcosttothere = terr.MovementCost ?? 0;
                                movementcosttothere += initialMovementCost;
                            }

                            ListMovementItem ItemWithTerrainAlready = _ActionsLoader.Movement.ListMovement.Where(item => item.Terrain == terr).FirstOrDefault();

                            ListMovementItem ItemWhereThisComeFrom = _ActionsLoader.Movement.ListMovement.Where(item => item.Terrain == terrainItem.Terrain).FirstOrDefault();
                            if (ItemWhereThisComeFrom != null && ItemWhereThisComeFrom.TerrainToStop != null)
                                TerrainToStop = ItemWhereThisComeFrom.TerrainToStop;

                            
                            if (ItemWithTerrainAlready == null)
                            {
                                _ActionsLoader.Movement.ListMovement.AddTerrain(new ListMovementItem(terr, terrainItem, movementcosttothere, TerrainToStop));
                                if (movementcosttothere < unit.MovementRest)
                                    TerrainsToContinueSearch.Add(new ListMovementItem(terr, terrainItem, movementcosttothere, TerrainToStop));
                            }
                            else if (ItemWithTerrainAlready.MovementCostToThere > movementcosttothere)
                            {
                                _ActionsLoader.Movement.ListMovement.RemoveTerrain(ItemWithTerrainAlready);
                                _ActionsLoader.Movement.ListMovement.AddTerrain(new ListMovementItem(terr, terrainItem, movementcosttothere, TerrainToStop));
                                if (movementcosttothere < unit.MovementRest)
                                    TerrainsToContinueSearch.Add(new ListMovementItem(terr, terrainItem, movementcosttothere, TerrainToStop));
                            }
                        }
                    }
                    foreach (ListMovementItem terraitem in TerrainsToContinueSearch)
                    {
                        AddAbjacentTerrainsToShow(terraitem, terraitem.MovementCostToThere);
                    } 
                }
            }
        }

        public override MovementItemDTO Move(Terrain terrTarget, ModelUnit unit, bool posibleAttackAgainsOwn)
        {
            MovementItemDTO movementItemDTO = new MovementItemDTO();
            ListMovementItem ItemOfTarget = ListMovement.Where(x => x.Terrain == terrTarget).FirstOrDefault();
            if (ItemOfTarget != null)
            {
                Terrain originTerrain = unit.InTerrain;
                ArmyColor colorOfUnit = unit.ArmyAffiliation;
                if (ItemOfTarget.TerrainToStop != null)
                {
                    if (ItemOfTarget.TerrainToStop.unitInTerrain == null)
                    {
                        ItemOfTarget.TerrainToStop.unitInTerrain = unit;
                        ListMovement.ClearTerrains();
                    }
                    else if (!unit.Fleeing && (ItemOfTarget.TerrainToStop.unitInTerrain.ArmyAffiliation != unit.ArmyAffiliation || posibleAttackAgainsOwn))
                    {
                        ListMovementItem ItemTerrainToThere_NotToStop = _ActionsLoader.Movement.ListMovement.
                            Where(item => item.TerrainToStop == ItemOfTarget.TerrainToStop && item.TerrainItemToThere.TerrainToStop == null).FirstOrDefault();

                        movementItemDTO = CombatMove(ItemOfTarget.TerrainToStop, unit, ItemTerrainToThere_NotToStop.TerrainItemToThere.Terrain, movementItemDTO);
                    }
                    if (unit != null)
                    {
                        unit.MovementRest = 0;
                    }
                }
                else
                {
                    if (terrTarget.unitInTerrain == null)
                    {
                        unit.MovementRest -= ItemOfTarget.MovementCostToThere;
                        terrTarget.unitInTerrain = unit;
                        ListMovement.ClearTerrains();
                    }
                    else if (!unit.Fleeing && (terrTarget.unitInTerrain.ArmyAffiliation != unit.ArmyAffiliation || posibleAttackAgainsOwn))
                    {
                        movementItemDTO = CombatMove(terrTarget, unit, ItemOfTarget.TerrainItemToThere.Terrain, movementItemDTO);
                    }
                }
                if (colorOfUnit == Game.ColorArmyToShow)
                {
                    if (unit != null)
                    {
                        _ActionsLoader.VisibleTerrains.RecalculateVisibility(unit);
                        ShowMovementPosible(unit);
                    }
                }
                else
                {
                    int range = _ActionsLoader.VisibleTerrains.MaxVisibilityBonus + _ActionsLoader.VisibleTerrains.VisibilityRange;
                    IEnumerable<Terrain> terrainsFromOrigin = _ActionsLoader.FieldsRelations.TerrainsInRangeOfOne(originTerrain, Game.Map, range);
                    IEnumerable<Terrain> terrainsFromTarget = new List<Terrain>();
                    if (unit != null)
                    {
                        terrainsFromTarget = _ActionsLoader.FieldsRelations.TerrainsInRangeOfOne(terrTarget, Game.Map, range);
                    }
                    IEnumerable<Terrain> allTerrains = terrainsFromOrigin.Union(terrainsFromTarget);
                    IEnumerable<ModelUnit> unitsToRecalculateVisibility = allTerrains.Where(x => 
                        x.unitInTerrain != null && x.unitInTerrain.ArmyAffiliation == Game.ColorArmyToShow)
                        .Select(x => x.unitInTerrain);

                    foreach (ModelUnit visibilityUnit in unitsToRecalculateVisibility)
                    {
                        _ActionsLoader.VisibleTerrains.RecalculateVisibility(visibilityUnit);
                        ShowMovementPosible(unit);
                    }
                }

                _ActionsLoader.VisibleTerrains.listVisilibity.ShowTerrains(Game.ColorArmyToShow);

                UnitItemDTO unitItem = new UnitItemDTO(unit);
                movementItemDTO.ListUnitItems = movementItemDTO.ListUnitItems.ToList();
                ((List<UnitItemDTO>)movementItemDTO.ListUnitItems).Add(unitItem);
                foreach (UnitItemDTO unitItem1 in movementItemDTO.ListUnitItems)
                {
                    if (unitItem1.Unit.LifeRest <= 0)
                        unitItem1.isDead = true;
                    if (unitItem1.Unit.InTerrain != null)
                    {
                        unitItem1.XX = unitItem1.Unit.InTerrain.XX;
                        unitItem1.YY = unitItem1.Unit.InTerrain.YY;
                    }
                    unitItem1.Unit = unitItem1.Unit.CloneWithoutTerrain();
                }



                MovementItemDTO CombatMove(Terrain enemyTerrain, ModelUnit unit5, Terrain enemyNextTerrain, MovementItemDTO movementItem3)
                {
                    enemyNextTerrain.unitInTerrain = unit5;
                    unit5.MovementRest = 0;
                    bool defenderKilled = _ActionsLoader.CloseCombat.Combat(unit5, enemyTerrain.unitInTerrain, out MovementItemDTO movementItemDTO1);
                    if (defenderKilled)
                    {
                        if (unit5 != null)
                            enemyTerrain.unitInTerrain = unit5;
                    }
                    if (unit5 != null)
                    {                        
                        ListMovement.ClearTerrains();
                    }
                    movementItemDTO1.WasThereCombat = true;

                    if (movementItemDTO1.showSpecialDefense)
                    {
                        movementItemDTO1.showSpecialAttack = false;
                    }
                    movementItemDTO1.ListUnitItems = movementItem3.ListUnitItems
                        .Union(movementItemDTO1.ListUnitItems);

                    return movementItemDTO1;
                }

            }

            movementItemDTO.Attacker = unit.CloneWithoutTerrain(); ;

            return movementItemDTO;
        }

        public override void deploy(Terrain terr)
        {
            if (Game.selectedUnit != null && terr.unitInTerrain == null)
            {
                if (Game.Gamestate == GamestateModel.DeployingRedUnits && terr.deploymentArea == ArmyColor.Red && Game.selectedUnit.ArmyAffiliation == ArmyColor.Red)
                {
                    if (terr.terrainType != TerrainType.See)
                    {
                        terr.unitInTerrain = Game.selectedUnit;
                        Game.ActionsLoader.VisibleTerrains.listVisilibity.HideTerrainsOfUnit(Game.selectedUnit);
                        Game.ActionsLoader.VisibleTerrains.listVisilibity.ClearTerrainsOfUnit(Game.selectedUnit);
                        Game.ActionsLoader.VisibleTerrains.ShowVisibles(Game.selectedUnit);
                    }
                }
                else if (Game.Gamestate == GamestateModel.DeployingBlueUnits && terr.deploymentArea == ArmyColor.Blue && Game.selectedUnit.ArmyAffiliation == ArmyColor.Blue)
                {
                    if (terr.terrainType != TerrainType.See)
                    {
                        terr.unitInTerrain = Game.selectedUnit;
                        Game.ActionsLoader.VisibleTerrains.listVisilibity.HideTerrainsOfUnit(Game.selectedUnit);
                        Game.ActionsLoader.VisibleTerrains.listVisilibity.ClearTerrainsOfUnit(Game.selectedUnit);
                        Game.ActionsLoader.VisibleTerrains.ShowVisibles(Game.selectedUnit);
                    }
                }
                
                _ActionsLoader.VisibleTerrains.listVisilibity.ShowTerrains(Game.selectedUnit.ArmyAffiliation);
            }
        }
    }

    public abstract class AbstractMovement
    {
        public Game Game;

        protected ActionsLoader _ActionsLoader;
        public AbstractMovement (Game game, ActionsLoader actionsLoader)
        {
            Game = game;
            _ActionsLoader = actionsLoader;
            ListMovement = new ListMovement<ListMovementItem>();
        }

        public ListMovement<ListMovementItem> ListMovement { get; }

        abstract public void deploy(Terrain terr);

        abstract public void ShowMovementPosible(ModelUnit unit, bool posibleAttackAgainsOwn = false);

        abstract public MovementItemDTO Move(Terrain terrTarget, ModelUnit unit, bool posibleAttackAgainsOwn);
    }

    public class ListMovement<T> : List<T> where T : ListMovementItem
    {
        public void AddTerrain(T item)
        {
            base.Add(item);
        }

        public void RemoveTerrain(T item)
        {
            base.Remove(item);
        }

        public void ClearTerrains()
        {
            foreach (T item in this)
            {
                item.Terrain.movementposible = false;
            }
            base.Clear();
        }
    }

    public class ListMovementItem
    {
        public ListMovementItem(Terrain terrain, ListMovementItem terrainItemToThere, short movementCostToThere, Terrain terrainToStop = null)
        {
            Terrain = terrain;
            TerrainItemToThere = terrainItemToThere;
            MovementCostToThere = movementCostToThere;
            TerrainToStop = terrainToStop;
        }

        public Terrain Terrain;
        public ListMovementItem TerrainItemToThere;
        public short MovementCostToThere;
        public Terrain TerrainToStop;
    }

}
