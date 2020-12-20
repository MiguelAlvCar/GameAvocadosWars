using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BasicElements;
using System.ComponentModel;
using System.Linq;

namespace ModelGame
{
    interface ITerrainTypeInterface
    {
        TerrainType terrainType { get; }
        short? MovementCost { get; }
        byte ID { get; }
        double? defendBonus { get; }
        double? attackBonus { get; }
    }

    [Serializable()]
    public class TypeTerrainClass : ITerrainTypeInterface
    {
        public static TypeTerrainClass Plain = new TypeTerrainClass(1, 1, TerrainType.Plain, null, null);
        public static TypeTerrainClass Forest = new TypeTerrainClass(9, 2, TerrainType.Forest, 1.33, null);
        public static TypeTerrainClass Hill = new TypeTerrainClass(2, 3, TerrainType.Hill, null, 1.25);
        public static TypeTerrainClass HillForest = new TypeTerrainClass(9, 4, TerrainType.HillForest, 1.33, 1.10);
        public static TypeTerrainClass HillCity = new TypeTerrainClass(2, 5, TerrainType.HillCity, 1.40, 1.10);
        public static TypeTerrainClass PlainCity = new TypeTerrainClass(1, 6, TerrainType.PlainCity, 1.40, null);
        public static TypeTerrainClass See = new TypeTerrainClass(null, 7, TerrainType.See, null, null);

        public static List<TypeTerrainClass> ListStaticTerrains = new List<TypeTerrainClass>();

        static TypeTerrainClass()
        {
            ListStaticTerrains.Add(Plain);
            ListStaticTerrains.Add(Forest);
            ListStaticTerrains.Add(Hill);
            ListStaticTerrains.Add(HillForest);
            ListStaticTerrains.Add(HillCity);
            ListStaticTerrains.Add(PlainCity);
            ListStaticTerrains.Add(See);
        }



        public TerrainType terrainType { get; set; }
        public short? MovementCost { get; set; }
        public byte ID { get; set; }
        public double? defendBonus { set; get; }
        public double? attackBonus { set; get; }

        private TypeTerrainClass(short? bewegungkost, byte id, TerrainType _terrainType, double? defendBonus, double? attackBonus)
        {
            terrainType = _terrainType;
            MovementCost = bewegungkost;
            ID = id;
            this.defendBonus = defendBonus;
            this.attackBonus = attackBonus;
        }
    }
}
