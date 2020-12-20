using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BasicElements

{
    public enum Language : byte
    {
        English, German, Spanish
    }

    //public class ViewUnitType
    //{
    //    public ViewUnitType (Uri uri, string name)
    //    {
    //        Uri = uri;
    //        UnitName = name;
    //    }

    //    static public Dictionary<string, ViewUnitType> unitTypes = new Dictionary<string, ViewUnitType>();
    //    public Uri Uri { get; set; }
    //    public string UnitName { get; set; }
    //}

    public enum ArmyColor : byte
    {
        None, Red, Blue
    }

    public class ViewTerrainType
    {
        static ViewTerrainType()
        {
            terrainTypes = new Dictionary<TerrainType, Uri>();
            ViewTerrainType.terrainTypes.Add(TerrainType.Plain, new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/Sechseck.jpg"));
            ViewTerrainType.terrainTypes.Add(TerrainType.See, new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/See.jpg"));
            ViewTerrainType.terrainTypes.Add(TerrainType.Forest, new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/Wald.jpg"));
            ViewTerrainType.terrainTypes.Add(TerrainType.Hill, new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/Huegel.jpg"));
            ViewTerrainType.terrainTypes.Add(TerrainType.HillCity, new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/CityHill.jpg"));
            ViewTerrainType.terrainTypes.Add(TerrainType.HillForest, new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/HuegelWald.jpg"));
            ViewTerrainType.terrainTypes.Add(TerrainType.PlainCity, new Uri("pack://siteoforigin:,,,/3GameView/Resources/Main/Images/Terrain/CityGrass.jpg"));
        }

        public ViewTerrainType(Uri uri, TerrainType name)
        {
            Uri = uri;
            terrainName = name;
        }

        static public Dictionary<TerrainType, Uri> terrainTypes;
        public Uri Uri { get; set; }
        public TerrainType terrainName { get; set; }
    }
    
    public enum TerrainType
    {
        Plain, Forest, PlainCity, See, HillCity, Hill, HillForest
    }

    public enum Modifier
    {
        None, Charge, Flight, Formation, Fright, Rage
    }

    public enum GamestateModel
    {
        CreatingMap, DeployingRedUnits, DeployingBlueUnits, Battle
    }
}


