using System;
using System.Collections.Generic;
using System.Text;
using ModelGame;
using BasicElements;
using System.Linq;

namespace ModelGame.Actions
{
    public class VisibleTerrains : AbstractVisibleTerrains
    {
        public VisibleTerrains (Game game, ActionsLoader actionsLoader) : base (game, actionsLoader) {
            VisibilityRange = 6;
            MaxVisibilityBonus = 1;
            listVisilibity = new ListVisibility(actionsLoader, VisibilityRange);
        }

        public override List<Terrain> ShowVisibles(ModelUnit un)
        {
            byte range = VisibilityRange;
            if (un.InTerrain.terrainType == TerrainType.Hill || un.InTerrain.terrainType == TerrainType.HillCity || un.InTerrain.terrainType == TerrainType.HillForest)
                range++;
            List<Terrain> ListTerrainUnit = new List<Terrain>();
            ListTerrainUnit.Add(un.InTerrain);
            List<Terrain> ListTerrainUnit1 = ListTerrainUnit.ToList();
            List<Terrain> TerrainsInFire = _ActionsLoader.FieldsRelations.TerrainsInRange(ListTerrainUnit, ListTerrainUnit1, _Game.Map, range);

            // if the unit isn´t in high terrain
            if (un.InTerrain.terrainType != TerrainType.Hill && un.InTerrain.terrainType != TerrainType.HillCity && un.InTerrain.terrainType != TerrainType.HillForest)
            {
                List<Terrain> Obstacles = new List<Terrain>();
                foreach (Terrain ter in TerrainsInFire)
                {
                    if (!(ter.terrainType == TerrainType.Plain || ter.terrainType == TerrainType.See) || ter.unitInTerrain != null)
                    {
                        Obstacles.Add(ter);
                    }
                }

                List<Terrain> adja = un.InTerrain.Adjacents;
                List<Terrain> Terrains2Range = _ActionsLoader.FieldsRelations.TerrainsInRange(adja, adja.ToList(), _Game.Map, 1);
                while (Obstacles.Count > 0)
                {                    
                    Terrain obstacle = Obstacles[Obstacles.Count - 1];

                    if (obstacle.terrainType == TerrainType.Plain && obstacle.unitInTerrain != null)
                    {
                        IEnumerable<Terrain> TerrainsBehind = _ActionsLoader.FieldsRelations.TerrainsBehind(obstacle, un.InTerrain, TerrainsInFire, range)
                            .Where(x => x.terrainType != TerrainType.Hill && x.terrainType != TerrainType.HillCity && x.terrainType != TerrainType.HillForest);
                        TerrainsInFire = TerrainsInFire.Except(TerrainsBehind).ToList();
                    }
                    else
                        TerrainsInFire = TerrainsInFire.Except(_ActionsLoader.FieldsRelations.TerrainsBehind(obstacle, un.InTerrain, TerrainsInFire, range)).ToList();

                    if (  (obstacle.terrainType == TerrainType.HillCity || obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.Forest || obstacle.terrainType == TerrainType.PlainCity)
                        && !Terrains2Range.Contains(obstacle))
                        TerrainsInFire.Remove(obstacle);
                    Obstacles.RemoveAt(Obstacles.Count - 1);
                }

            }

            // if the unit is in high terrain
            else
            {
                TerrainsInFire = ObstacleLevel(un, 0, ListTerrainUnit, TerrainsInFire);

                List<Terrain> ObstacleLevel(ModelUnit uni, int level, List<Terrain> ListTerrainLevel, List<Terrain> TerrainsInFire1)
                {
                    if (range > level)
                    {
                        List<Terrain> ListTerrainLevel1 = ListTerrainLevel.ToList();
                        List<Terrain> ListTerrainLevelPlus1 = _ActionsLoader.FieldsRelations.TerrainsInRange(ListTerrainLevel, ListTerrainLevel1, _Game.Map, 1).Except(ListTerrainLevel).ToList();

                        List<Terrain> Obstacles = new List<Terrain>();
                        foreach (Terrain ter in ListTerrainLevelPlus1)
                        {
                            if (!(ter.terrainType == TerrainType.Plain || ter.terrainType == TerrainType.See))
                            {
                                Obstacles.Add(ter);
                            }
                        }

                        while (Obstacles.Count > 0)
                        {
                            Terrain obstacle = Obstacles[Obstacles.Count - 1];
                            if (obstacle.terrainType == TerrainType.PlainCity || obstacle.terrainType == TerrainType.Forest)
                            {
                                TerrainsInFire1 = TerrainsInFire1.Except(_ActionsLoader.FieldsRelations.TerrainsBehind(obstacle, uni.InTerrain, TerrainsInFire1, level + 1)).ToList();
                                if (  (obstacle.terrainType == TerrainType.Forest || obstacle.terrainType == TerrainType.PlainCity)
                                    && level > 1)
                                    TerrainsInFire1.Remove(obstacle);
                            }

                            else if (obstacle.terrainType == TerrainType.Hill && obstacle.unitInTerrain == null)
                            {
                                IEnumerable<Terrain> TerrainsBehindHill = _ActionsLoader.FieldsRelations.TerrainsBehind(obstacle, uni.InTerrain, TerrainsInFire1, level + 1)
                                    .Where(x => x.terrainType != TerrainType.Hill && x.terrainType != TerrainType.HillCity && x.terrainType != TerrainType.HillForest);
                                TerrainsInFire1 = TerrainsInFire1.Except(TerrainsBehindHill).ToList();

                                if ((obstacle.terrainType == TerrainType.HillCity || obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.Forest || obstacle.terrainType == TerrainType.PlainCity)
                                    && level > 1)
                                    TerrainsInFire1.Remove(obstacle);
                            }

                            else if (obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.HillCity || (obstacle.terrainType == TerrainType.Hill && obstacle.unitInTerrain != null))
                            {
                                if (obstacle.terrainType == TerrainType.Plain && obstacle.unitInTerrain != null)
                                {
                                    IEnumerable<Terrain> TerrainsBehind = _ActionsLoader.FieldsRelations.TerrainsBehind(obstacle, un.InTerrain, TerrainsInFire, 1)
                                        .Where(x => x.terrainType != TerrainType.Hill && x.terrainType != TerrainType.HillCity && x.terrainType != TerrainType.HillForest);
                                    TerrainsInFire1 = TerrainsInFire1.Except(TerrainsBehind).ToList();
                                }
                                else
                                    TerrainsInFire1 = TerrainsInFire1.Except(_ActionsLoader.FieldsRelations.TerrainsBehind(obstacle, un.InTerrain, TerrainsInFire, range)).ToList();

                                if (  (obstacle.terrainType == TerrainType.HillCity || obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.Forest || obstacle.terrainType == TerrainType.PlainCity)
                                    && level > 1)
                                    TerrainsInFire1.Remove(obstacle);
                            }
                            Obstacles.RemoveAt(Obstacles.Count - 1);
                        }
                        return ObstacleLevel(un, ++level, ListTerrainLevelPlus1.Union(ListTerrainLevel).ToList(), TerrainsInFire1);
                    }
                    else
                        return TerrainsInFire1;
                }
            }

            foreach (Terrain ter in TerrainsInFire)
            {
                _ActionsLoader.VisibleTerrains.listVisilibity.AddTerrain(new TerrainVisibleItem(ter, un), un.ArmyAffiliation);
            }
            return TerrainsInFire;
        }

        public override void RecalculateVisibility(ModelUnit unit)
        {
            listVisilibity.HideTerrainsOfUnit(unit);
            listVisilibity.ClearTerrainsOfUnit(unit);
            ShowVisibles(unit);
        }

        public override void RecalculateVisibilityDifferentEnvironment(ModelUnit unitToShow, ModelUnit unitToHide)
        {
            listVisilibity.HideTerrainsOfUnit(unitToHide);
            listVisilibity.ClearTerrainsOfUnit(unitToHide);
            ShowVisibles(unitToShow);
        }
    }

    public class ListVisibility
    {
        ActionsLoader _ActionLoader;

        byte _VisibilityRange;
        public ListVisibility (ActionsLoader actionLoader, byte visibilityRange)
        {
            _VisibilityRange = visibilityRange;
            _ActionLoader = actionLoader;
        }
        public List<TerrainVisibleItem> RedVisibles { set; get; } = new List<TerrainVisibleItem>();
        public List<TerrainVisibleItem> BlueVisibles { set; get; } = new List<TerrainVisibleItem>();

        public void AddTerrain(TerrainVisibleItem item, ArmyColor turnColor)
        {
            List<TerrainVisibleItem> ListVisible = CalculateWhichList(turnColor);
            //item.TerrainVisible.Hide = false;
            if (!ListVisible.Any(x => x.TerrainVisible == item.TerrainVisible))
                ListVisible.Add(item);
        }

        public void RemoveTerrain(TerrainVisibleItem item, ArmyColor turnColor)
        {
            List<TerrainVisibleItem> ListVisible = CalculateWhichList(turnColor);
            ListVisible.Remove(item);
        }

        public void ClearTerrains(ArmyColor turnColor)
        {
            List<TerrainVisibleItem> ListVisible = CalculateWhichList(turnColor);
            ListVisible.Clear();
        }

        public void ClearTerrainsOfUnit(ModelUnit unit)
        {
            List<TerrainVisibleItem> ListVisible = CalculateWhichList(unit.ArmyAffiliation);
            IEnumerable<TerrainVisibleItem> ItemsVisibleOfUnit = ListVisible.Where(x => x.Unit == unit);
            List<TerrainVisibleItem> ItemsVisibleOfUnit1 = ItemsVisibleOfUnit.ToList();
            Stack<TerrainVisibleItem> ToRemove = new Stack<TerrainVisibleItem>(ItemsVisibleOfUnit);
            while (ToRemove.Count > 0)
            {
                ListVisible.Remove(ToRemove.Pop());
            }

            int RealVisibilityRange = _VisibilityRange;
            if (unit.InTerrain.terrainType == TerrainType.Hill || unit.InTerrain.terrainType == TerrainType.HillCity || unit.InTerrain.terrainType == TerrainType.HillForest)
                RealVisibilityRange++;

            List<Terrain> TerrainsVisiblesOfUnit = ItemsVisibleOfUnit1.Select(x => x.TerrainVisible).ToList();
            while (TerrainsVisiblesOfUnit.Count > 0)
            {
                List<Terrain> ListTerrain = new List<Terrain>();
                ListTerrain.Add(TerrainsVisiblesOfUnit[0]);
                List<Terrain> ListTerrain1 = ListTerrain.ToList();

                FindUnitToTerrain(ListTerrain, ListTerrain1, 1);

                void FindUnitToTerrain(List<Terrain> ListTerrainOrigin, List<Terrain> oldTerrainsInLevel, int level)
                {                    
                    List<Terrain> LevelPlus1 = oldTerrainsInLevel.SelectMany(x => x.Adjacents).Distinct().ToList();
                    IEnumerable<Terrain> LevelPlus = LevelPlus1.Except(oldTerrainsInLevel);
                    IEnumerable<ModelUnit> UnitsInLevel = LevelPlus.Where(x => x.unitInTerrain != null && x.unitInTerrain.ArmyAffiliation == unit.ArmyAffiliation).
                        Select(x => x.unitInTerrain);
                    foreach (ModelUnit un in UnitsInLevel)
                    {
                        List<Terrain> NewVisibles = _ActionLoader.VisibleTerrains.ShowVisibles(un);
                        if (NewVisibles.Contains(TerrainsVisiblesOfUnit[0]))
                        {
                            return;
                        }
                    }
                    if (level < RealVisibilityRange)
                        FindUnitToTerrain(ListTerrainOrigin, LevelPlus1, ++level);
                }
                TerrainsVisiblesOfUnit.RemoveAt(0);
                TerrainsVisiblesOfUnit = TerrainsVisiblesOfUnit.Except(ListVisible.Select(x => x.TerrainVisible)).ToList();
            }
        }

        public void HideTerrainsOfUnit(ModelUnit unit)
        {
            List<TerrainVisibleItem> ListVisible = CalculateWhichList(unit.ArmyAffiliation);
            foreach (TerrainVisibleItem item in ListVisible)
            {
                if (item.Unit == unit)
                    item.TerrainVisible.Hide = true;
            }
        }

        public void ShowTerrains(ArmyColor turnColor)
        {
            List<TerrainVisibleItem> ListVisible = CalculateWhichList(turnColor);
            foreach (TerrainVisibleItem item in ListVisible)
            {
                item.TerrainVisible.Hide = false;
            }
        }

        public void HideTerrains(ArmyColor turnColor)
        {
            List<TerrainVisibleItem> ListVisible = CalculateWhichList(turnColor);
            if (ListVisible != null)
            {
                foreach (TerrainVisibleItem item in ListVisible)
                {
                    item.TerrainVisible.Hide = true;
                }
            }
        }

        public List<TerrainVisibleItem> CalculateWhichList(ArmyColor color)
        {
            if (color == ArmyColor.Red)
            {
                return RedVisibles;
            }
            else if (color == ArmyColor.Blue)
            {
                return BlueVisibles;
            }
            return null;
        }
    }

    public class TerrainVisibleItem
    {
        public TerrainVisibleItem(Terrain terrainVisible, ModelUnit unit)
        {
            TerrainVisible = terrainVisible;
            Unit = unit;
        }
        public Terrain TerrainVisible;
        public ModelUnit Unit;
    }

    public abstract class AbstractVisibleTerrains
    {
        protected ActionsLoader _ActionsLoader;
        protected Game _Game;

        public byte VisibilityRange;
        public byte MaxVisibilityBonus;

        public AbstractVisibleTerrains(Game game, ActionsLoader actionsLoader)
        {
            _Game = game;
            _ActionsLoader = actionsLoader;            
        }
        public abstract List<Terrain> ShowVisibles(ModelUnit un);

        public ListVisibility listVisilibity { get; protected set; }

        public abstract void RecalculateVisibility(ModelUnit unit);

        public abstract void RecalculateVisibilityDifferentEnvironment(ModelUnit unitToShow, ModelUnit unitToHide);
    }
}

