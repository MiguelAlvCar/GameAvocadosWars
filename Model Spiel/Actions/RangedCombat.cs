using System;
using System.Collections.Generic;
using System.Text;
using BasicElements;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ModelGame;

namespace ModelGame.Actions
{
    public class RangedCombat : AbstractRangedCombat
    {
        public RangedCombat (Game game): base (game) { }

        public override ListFire<Terrain> FireList { set; get; } = new ListFire<Terrain>();

        public override void ShowFirePosible(RangeUnit un, byte range)
        {
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
                        List<Terrain> ListTerrainLevelPlus1 = _ActionsLoader.FieldsRelations.TerrainsInRange(ListTerrainLevel, ListTerrainLevel1, _Game.Map, 1).Except(ListTerrainLevel).ToList();
                        
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
                                TerrainsInFire1 = TerrainsInFire1.Except(_ActionsLoader.FieldsRelations.TerrainsBehind(obstacle, uni.InTerrain, TerrainsInFire1, level + 1)).ToList();
                                if (obstacle.terrainType == TerrainType.HillCity || obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.Forest || obstacle.terrainType == TerrainType.PlainCity)
                                    TerrainsInFire1.Remove(obstacle);
                            }
                            else if (obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.HillCity || (obstacle.terrainType == TerrainType.Hill && obstacle.unitInTerrain != null) )
                            {
                                TerrainsInFire1 = TerrainsInFire1.Except(_ActionsLoader.FieldsRelations.TerrainsBehind(obstacle, uni.InTerrain, TerrainsInFire1, uni.Reichweite)).ToList();
                                if (obstacle.terrainType == TerrainType.HillCity || obstacle.terrainType == TerrainType.HillForest || obstacle.terrainType == TerrainType.Forest || obstacle.terrainType == TerrainType.PlainCity)
                                    TerrainsInFire1.Remove(obstacle);
                            }
                            // Plain with unit
                            else
                            {
                                if (obstacle.terrainType == TerrainType.Plain && obstacle.unitInTerrain != null)
                                {
                                    IEnumerable<Terrain> TerrainsBehind = _ActionsLoader.FieldsRelations.TerrainsBehind(obstacle, un.InTerrain, TerrainsInFire, 1)
                                        .Where(x => x.terrainType != TerrainType.Hill && x.terrainType != TerrainType.HillCity && x.terrainType != TerrainType.HillForest);
                                    TerrainsInFire1 = TerrainsInFire1.Except(TerrainsBehind).ToList();
                                }
                                else
                                    TerrainsInFire1 = TerrainsInFire1.Except(_ActionsLoader.FieldsRelations.TerrainsBehind(obstacle, un.InTerrain, TerrainsInFire, range)).ToList();

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

        public override MovementItemDTO Combat(RangeUnit attacker, Terrain defenderTerr)
        {
            MovementItemDTO movementItemDTO = new MovementItemDTO();

            if (defenderTerr.unitInTerrain != null && defenderTerr.unitInTerrain.ArmyAffiliation != attacker.ArmyAffiliation)
            {
                List<UnitItemDTO> listUnitItems = new List<UnitItemDTO>();

                attacker.RangeAttack(defenderTerr.unitInTerrain, out short defenderLifedamage, out short defenderMoraldamage, out Type defenderEffectType);
                defenderTerr.unitInTerrain.MoralRest -= defenderMoraldamage;
                defenderTerr.unitInTerrain.LifeRest -= defenderLifedamage;

                UnitItemDTO unitItem = new UnitItemDTO(defenderTerr.unitInTerrain.CloneWithoutTerrain());
                unitItem.XX = defenderTerr.XX;
                unitItem.YY = defenderTerr.YY;

                listUnitItems.Add(unitItem);
                movementItemDTO.ListUnitItems = listUnitItems;

                if (defenderTerr.unitInTerrain.LifeRest <= 0)
                    unitItem.isDead = true;

                movementItemDTO.showSpecialAttack = true;
                movementItemDTO.Attacker = attacker.CloneWithoutTerrain();
                movementItemDTO.WasThereCombat = true;

                _Game.OnAttackerEffect(attacker, true);
                FireList.ClearTerrains();
            }
            return movementItemDTO;
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
        protected Game _Game;
        protected ActionsLoader _ActionsLoader { 
            get 
            {
                return _Game.ActionsLoader;
            } 
        }

        public abstract ListFire<Terrain> FireList { set; get; }
        public AbstractRangedCombat (Game game)
        {
            _Game = game;
        }
        public abstract void ShowFirePosible(RangeUnit un, byte range);

        public abstract MovementItemDTO Combat(RangeUnit attacker, Terrain defenderTerr);
    }
}
