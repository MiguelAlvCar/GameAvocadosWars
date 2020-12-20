using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicElements;
using Translation;
using ModelGame;
using System.Collections;
using Sound;

namespace ArmyAndUnitTypes
{
    public class UnitType<F> : AbstractUnitType where F : ModelUnit, new()
    {
        public Dictionary<Language, string> UnitNameDictionary { set; get; } = new Dictionary<Language, string>();

        public Dictionary<Language, string> SpecialStrengthDictionary { set; get; } = new Dictionary<Language, string>();

        public override string Name
        {
            get
            {
                return UnitNameDictionary[Translater.Language];
            }
        }

        public override string SpecialStrength
        {
            get
            {
                if (SpecialStrengthDictionary.ContainsKey(Translater.Language))
                    return SpecialStrengthDictionary[Translater.Language];
                else
                    return null;
            }
        }

        public override double Points { set; get; }

        public override Type GetUnitType ()
        {
            return typeof(F);
        }

        public override void CreateModelUnitInGame(Game game, int NumberUnitsRequested, ArrayList ListUnitsToRenderInCamp, ArmyColor armyAffilation)
        {
            ListArmy<ModelUnit> ListArmy;

            if (armyAffilation == ArmyColor.Red)
                ListArmy = game.ListReds;
            else
                ListArmy = game.ListBlues;

            int unitsInMap = ListArmy.Where(c => c is F && c.InTerrain != null).Count();
            int unitsInGame = ListArmy.Where(c => c is F).Count();
            if (unitsInMap <= NumberUnitsRequested)
            {
                if (unitsInGame <= NumberUnitsRequested)
                {
                    while (ListArmy.Where(c => c is F).Count() < NumberUnitsRequested)
                    {
                        F unit = CreateModelUnit(game, armyAffilation);
                    }
                    foreach (ModelUnit uni in ListArmy)
                    {
                        if (uni.InTerrain == null && uni is F)
                            ListUnitsToRenderInCamp.Add(uni.OnGetViewModelUnitFromModelUnit());
                    }
                }
                else
                {
                    List<ModelUnit> UnitToDispose = ListArmy.Where(n => n is F && n.InTerrain == null).Take(unitsInGame - NumberUnitsRequested).ToList();
                    foreach (ModelUnit unim in UnitToDispose)
                    {
                        game.DisposeUnitModel(unim);
                    }
                    foreach (ModelUnit uni in ListArmy)
                    {
                        if (uni.InTerrain == null && uni is F)
                            ListUnitsToRenderInCamp.Add(uni.OnGetViewModelUnitFromModelUnit());
                    }
                }
            }
            else
            {
                // First at all it takes all units that are not in the map, and then the rest necessary from the map
                List<ModelUnit> UnitToDisposeNotFromMap = ListArmy.Where(n => n is F && n.InTerrain == null).ToList();
                int UnitToRemoveFromMap = unitsInGame - NumberUnitsRequested - UnitToDisposeNotFromMap.Count;
                List<ModelUnit> UnitToDisposeFromMap = ListArmy.Where(n => !UnitToDisposeNotFromMap.Contains(n) && n is F).Take(UnitToRemoveFromMap).ToList();
                foreach (ModelUnit unim in UnitToDisposeNotFromMap)
                {
                    game.DisposeUnitModel(unim);
                }
                foreach (ModelUnit unim in UnitToDisposeFromMap)
                {
                    game.DisposeUnitModel(unim);
                }
            }
        }

        private F CreateModelUnit(Game game, ArmyColor armyAffilation)
        {
            F unit = new F();
            game.OnUnitCreated(unit);
            unit.DisposeFromGame += (uni) => game.DisposeUnitModel(uni);
            unit.Game = game;
            unit.ArmyAffiliation = armyAffilation;
            return unit;
        }

        public override Uri ImageUri { set; get; }
        public override UriEffect DefenseEffect { set; get; }
        public override UriEffect AttackEffect { set; get; }
    }

    public abstract class AbstractUnitType 
    {
        //public abstract Dictionary<Language, string> UnitNameDictionary { set; get; }
        //public abstract Dictionary<Language, string> SpecialStrengthDictionary { set; get; }
        public abstract string Name { get; }
        public abstract string SpecialStrength { get; }
        public abstract double Points { set; get; }
        public abstract Type GetUnitType();
        public abstract void CreateModelUnitInGame(Game game, int NumberUnitsRequested, ArrayList ListUnitsToRenderInCamp, ArmyColor armyAffilation);
        public abstract Uri ImageUri { set; get; }
        public abstract UriEffect DefenseEffect { set; get; }
        public abstract UriEffect AttackEffect { set; get; }
    }
}
