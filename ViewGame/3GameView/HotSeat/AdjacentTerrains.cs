
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ViewGame.View.Resources;

namespace ViewGame.View.Game
{
    public static class FieldsRelationsView
    {
        public static List<ViewTerrain> AdjacentTerrains(ViewTerrain terr, GamePage hotseat)
        {
            List<ViewTerrain> arr = new List<ViewTerrain>();
            foreach (ViewTerrain gel in hotseat.ListTerrains)
            {
                if ((gel.Y == terr.Y) && (gel.X + 1 == terr.X || gel.X - 1 == terr.X)) arr.Add(gel);
                if ((terr.Y * 10) % 20 == 10)
                {
                    if ((((gel.Y + 1 == terr.Y) || (gel.Y - 1 == terr.Y)) && (gel.X == terr.X || gel.X + 1 == terr.X)))
                    {
                        arr.Add(gel);
                    }
                }
                else
                {
                    if ((((gel.Y + 1 == terr.Y) || (gel.Y - 1 == terr.Y)) && (gel.X == terr.X || gel.X - 1 == terr.X)))
                    {
                        arr.Add(gel);
                    }
                }
            }
            return arr;
        }
    }

    public class UnitsToRelocate
    {
        public UnitsToRelocate (int x, int y, ViewUnit un)
        {
            X = x;
            Y = y;
            uni = un;
        }
        public int X;
        public int Y;
        public ViewUnit uni;
    }
}