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

    public class RangedCombat : AbstractRangedCombat
    {
        public RangedCombat (ActionsLoader gameBase): base (gameBase) { }

        public static ListFire<Terrain> FireList = new ListFire<Terrain>();

        public override void ShowFirePosible(RangeUnit un, byte range)
        {
            if (un.InTerrain.terrainType == TerrainType.Hill || un.InTerrain.terrainType == TerrainType.HillCity || un.InTerrain.terrainType == TerrainType.HillForest)
                range++;
            List<Terrain> ListTerrainUnit = new List<Terrain>();
            ListTerrainUnit.Add(un.InTerrain);
            List<Terrain> ListTerrainUnit1 = ListTerrainUnit.ToList();
            List<Terrain> TerrainsInFire = _GameBase.FieldsRelations.TerrainsInRange(ListTerrainUnit, ListTerrainUnit1, (List<Terrain>)UnitModel.OnGetGameMap(), range);

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

                while (Obstacles.Count > 0)
                {
                    Terrain obstacle = Obstacles[Obstacles.Count - 1];

                    if (obstacle.terrainType == TerrainType.Plain && obstacle.unitInTerrain != null)
                    {
                        IEnumerable<Terrain> TerrainsBehind = _GameBase.FieldsRelations.TerrainsBehind(obstacle, un.InTerrain, TerrainsInFire, range)
                            .Where(x => x.terrainType != TerrainType.Hill && x.terrainType != TerrainType.HillCity && x.terrainType != TerrainType.HillForest);
                        TerrainsInFire = TerrainsInFire.Except(TerrainsBehind).ToList();
                    }
                    else
                        TerrainsInFire = TerrainsInFire.Except(_GameBase.FieldsRelations.TerrainsBehind(obstacle, un.InTerrain, TerrainsInFire, range)).ToList();

                    if (obstacle.terrainType == TerrainType.HillCity || obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.Forest || obstacle.terrainType == TerrainType.PlainCity)
                        TerrainsInFire.Remove(obstacle);
                    Obstacles.RemoveAt(Obstacles.Count - 1);
                }
                
            }

            // if the unit is in high terrain
            else
            {
                TerrainsInFire = ObstacleLevel(un, 0, ListTerrainUnit, TerrainsInFire);

                List<Terrain> ObstacleLevel(RangeUnit uni, int level, List<Terrain> ListTerrainLevel, List<Terrain> TerrainsInFire1)
                {
                    if (uni.Reichweite > level)
                    {
                        List<Terrain> ListTerrainLevel1 = ListTerrainLevel.ToList();
                        List<Terrain> ListTerrainLevelPlus1 = _GameBase.FieldsRelations.TerrainsInRange(ListTerrainLevel, ListTerrainLevel1, (List<Terrain>)UnitModel.OnGetGameMap(), 1).Except(ListTerrainLevel).ToList();
                        
                        List<Terrain> Obstacles = new List<Terrain>();
                        foreach (Terrain ter in ListTerrainLevelPlus1)
                        {
                            if (!(ter.terrainType == TerrainType.Plain || ter.terrainType == TerrainType.See) || ter.unitInTerrain != null)
                            {
                                Obstacles.Add(ter);
                            }
                        }

                        while (Obstacles.Count > 0)
                        {
                            Terrain obstacle = Obstacles[Obstacles.Count - 1];
                            if (obstacle.terrainType == TerrainType.PlainCity || obstacle.terrainType == TerrainType.Forest || (obstacle.terrainType == TerrainType.Hill && obstacle.unitInTerrain == null))
                            {
                                TerrainsInFire1 = TerrainsInFire1.Except(_GameBase.FieldsRelations.TerrainsBehind(obstacle, uni.InTerrain, TerrainsInFire1, level + 1)).ToList();
                                if (obstacle.terrainType == TerrainType.HillCity || obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.Forest || obstacle.terrainType == TerrainType.PlainCity)
                                    TerrainsInFire1.Remove(obstacle);
                            }
                            else if (obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.HillCity || (obstacle.terrainType == TerrainType.Hill && obstacle.unitInTerrain != null) )
                            {
                                TerrainsInFire1 = TerrainsInFire1.Except(_GameBase.FieldsRelations.TerrainsBehind(obstacle, uni.InTerrain, TerrainsInFire1, uni.Reichweite)).ToList();
                                if (obstacle.terrainType == TerrainType.HillCity || obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.Forest || obstacle.terrainType == TerrainType.PlainCity)
                                    TerrainsInFire1.Remove(obstacle);
                            }
                            // Plain with unit
                            else
                            {
                                if (obstacle.terrainType == TerrainType.Plain && obstacle.unitInTerrain != null)
                                {
                                    IEnumerable<Terrain> TerrainsBehind = _GameBase.FieldsRelations.TerrainsBehind(obstacle, un.InTerrain, TerrainsInFire, 1)
                                        .Where(x => x.terrainType != TerrainType.Hill && x.terrainType != TerrainType.HillCity && x.terrainType != TerrainType.HillForest);
                                    TerrainsInFire1 = TerrainsInFire1.Except(TerrainsBehind).ToList();
                                }
                                else
                                    TerrainsInFire1 = TerrainsInFire1.Except(_GameBase.FieldsRelations.TerrainsBehind(obstacle, un.InTerrain, TerrainsInFire, range)).ToList();

                                if (obstacle.terrainType == TerrainType.HillCity || obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.Forest || obstacle.terrainType == TerrainType.PlainCity)
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
                FireList.AddTerrain(ter);
            }
        }

        public override void Combat(RangeUnit attacker, Terrain defenderTerr)
        {
            if (defenderTerr.unitInTerrain != null && defenderTerr.unitInTerrain.heerzugeh != attacker.heerzugeh)
            {
                attacker.RangeAttack(defenderTerr.unitInTerrain, out short defenderLifedamage, out short defenderMoraldamage, out Type defenderEffectType);
                defenderTerr.unitInTerrain.MoralRest -= defenderMoraldamage;
                defenderTerr.unitInTerrain.LifeRest -= defenderLifedamage;

                _GameBase.CloseCombat.OnAttackerEffect(BasicMechanisms.RemovePrefixType(defenderEffectType.ToString()));
                FireList.ClearTerrains();
            }           
        }
    }

    public class ListFire<T> : List<T> where T : Terrain
    {
        public void AddTerrain(T item)
        {
            item.fireposible = true;
            base.Add(item);
        }

        public void RemoveTerrain(T item)
        {
            item.fireposible = false;
            base.Remove(item);
        }

        public void ClearTerrains()
        {
            foreach (T item in this)
            {
                item.fireposible = false;
            }
            base.Clear();
        }
    }

    public abstract class AbstractRangedCombat
    {
        protected ActionsLoader _GameBase;
        public AbstractRangedCombat (ActionsLoader gameBase)
        {
            _GameBase = gameBase;
        }
        public abstract void ShowFirePosible(RangeUnit un, byte range);

        public abstract void Combat(RangeUnit attacker, Terrain defenderTerr);
    }
}
