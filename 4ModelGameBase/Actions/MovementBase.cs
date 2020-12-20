using System;
using System.Collections.Generic;
using System.Text;
using BasicElements;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ModelGameBase;
using ModelGameBase.Actions;

namespace Model.Actions
{
    public class MovementBase : AbstractMovementBase
    {
        public MovementBase (ActionsLoader gameBase): base (gameBase) { }

        public override void showMovementPosible(UnitModel unit, bool posibleAttackAgainsOwn = false)
        {
            if (unit.MovementRest > 0)
            {
                List<TerrainVisibleItem> Visibles = _GameBase.VisibleTerrains.listVisilibity.CalculateWhichList(unit.heerzugeh);
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
                            (!(terr.unitInTerrain != null && terr.unitInTerrain.heerzugeh == unit.heerzugeh) || posibleAttackAgainsOwn))
                        {
                            IEnumerable<Terrain> transverseTerrains = EnuTerrains.Intersect(terr.Adjacents);

                            Terrain TerrainToStop = null;
                            short movementcosttothere;
                            if (transverseTerrains.Any(t => t.unitInTerrain != null && Visibles.Any(x => x.TerrainVisible == t) 
                                && t.unitInTerrain.heerzugeh != unit.heerzugeh && !t.unitInTerrain.Fleeing) 
                                ||
                                (terr.unitInTerrain != null && Visibles.Any(x => x.TerrainVisible == terr) 
                                && ((terr.unitInTerrain.heerzugeh != unit.heerzugeh) || posibleAttackAgainsOwn)))
                            {
                                movementcosttothere = Convert.ToInt16(9 + initialMovementCost);
                            }
                            else if (transverseTerrains.Any(t => t.unitInTerrain != null && !Visibles.Any(x => x.TerrainVisible == t)
                                && t.unitInTerrain.heerzugeh != unit.heerzugeh)
                                ||
                                (terr.unitInTerrain != null && !Visibles.Any(x => x.TerrainVisible == terr)
                                && terr.unitInTerrain.heerzugeh != unit.heerzugeh && !terr.unitInTerrain.Fleeing )  )
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

                            ListMovementItem ItemWithTerrainAlready = _GameBase.MovementBase.ListMovement.Where(item => item.Terrain == terr).FirstOrDefault();

                            ListMovementItem ItemWhereThisComeFrom = _GameBase.MovementBase.ListMovement.Where(item => item.Terrain == terrainItem.Terrain).FirstOrDefault();
                            if (ItemWhereThisComeFrom != null && ItemWhereThisComeFrom.TerrainToStop != null)
                                TerrainToStop = ItemWhereThisComeFrom.TerrainToStop;

                            
                            if (ItemWithTerrainAlready == null)
                            {
                                _GameBase.MovementBase.ListMovement.AddTerrain(new ListMovementItem(terr, terrainItem, movementcosttothere, TerrainToStop));
                                if (movementcosttothere < unit.MovementRest)
                                    TerrainsToContinueSearch.Add(new ListMovementItem(terr, terrainItem, movementcosttothere, TerrainToStop));
                            }
                            else if (ItemWithTerrainAlready.MovementCostToThere > movementcosttothere)
                            {
                                _GameBase.MovementBase.ListMovement.RemoveTerrain(ItemWithTerrainAlready);
                                _GameBase.MovementBase.ListMovement.AddTerrain(new ListMovementItem(terr, terrainItem, movementcosttothere, TerrainToStop));
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

        public override void move(Terrain terrTarget, UnitModel unit, bool posibleAttackAgainsOwn)
        {
            ListMovementItem ItemOfTarget = ListMovement.Where(x => x.Terrain == terrTarget).FirstOrDefault();
            if (ItemOfTarget != null)
            {
                if (ItemOfTarget.TerrainToStop != null)
                {
                    if (ItemOfTarget.TerrainToStop.unitInTerrain == null)
                    {
                        ItemOfTarget.TerrainToStop.unitInTerrain = unit;
                        ListMovement.ClearTerrains();
                    }
                    else if (!unit.Fleeing && (ItemOfTarget.TerrainToStop.unitInTerrain.heerzugeh != unit.heerzugeh || posibleAttackAgainsOwn))
                    {
                        ListMovementItem ItemTerrainToThere_NotToStop = _GameBase.MovementBase.ListMovement.
                            Where(item => item.TerrainToStop == ItemOfTarget.TerrainToStop && item.TerrainItemToThere.TerrainToStop == null).FirstOrDefault();

                        CombatMove(ItemOfTarget.TerrainToStop, unit, ItemTerrainToThere_NotToStop.TerrainItemToThere.Terrain);
                    }
                    if (unit != null)
                    {
                        unit.MovementRest = 0;
                        _GameBase.VisibleTerrains.listVisilibity.HideTerrainsOfUnit(unit);
                        _GameBase.VisibleTerrains.listVisilibity.ClearTerrainsOfUnit(unit);
                        _GameBase.VisibleTerrains.ShowVisibles(unit);
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
                    else if (!unit.Fleeing && (terrTarget.unitInTerrain.heerzugeh != unit.heerzugeh || posibleAttackAgainsOwn))
                    {
                        CombatMove(terrTarget, unit, ItemOfTarget.TerrainItemToThere.Terrain);
                    }
                    if (unit != null)
                    {
                        _GameBase.VisibleTerrains.listVisilibity.HideTerrainsOfUnit(unit);
                        _GameBase.VisibleTerrains.listVisilibity.ClearTerrainsOfUnit(unit);
                        _GameBase.VisibleTerrains.ShowVisibles(unit);
                        showMovementPosible(unit);
                    }
                }

                void CombatMove(Terrain enemyTerrain, UnitModel unit5, Terrain enemyNextTerrain)
                {
                    enemyNextTerrain.unitInTerrain = unit5;
                    unit5.MovementRest = 0;
                    bool defenderKilled = _GameBase.CloseCombat.Combat(unit5, enemyTerrain.unitInTerrain);
                    if (defenderKilled)
                    {
                        if (unit5 != null)
                            enemyTerrain.unitInTerrain = unit5;
                    }
                    if (unit5 != null)
                    {                        
                        ListMovement.ClearTerrains();
                    }
                }


            }            
        }
    }

    public abstract class AbstractMovementBase
    {
        protected ActionsLoader _GameBase;
        public AbstractMovementBase (ActionsLoader gameBase)
        {
            _GameBase = gameBase;
            ListMovement = new ListMovement<ListMovementItem>();
        }

        public ListMovement<ListMovementItem> ListMovement { get; }

        abstract public void showMovementPosible(UnitModel unit, bool posibleAttackAgainsOwn = false);

        abstract public void move(Terrain terrTarget, UnitModel unit, bool posibleAttackAgainsOwn);
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
