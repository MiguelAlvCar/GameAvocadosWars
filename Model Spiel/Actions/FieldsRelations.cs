using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ModelGame;
using System.ComponentModel;
using static System.Math;

namespace ModelGame.Actions
{
    public class FieldsRelations : IFieldsRelations
    {
        public IEnumerable<Terrain> AdjacentTerrains(Terrain terr, IEnumerable<Terrain> listTerrains)
        {
            return listTerrains.Where(a =>
            {
                return ((a.YY == terr.YY) && (a.XX + Terrain.XXX == terr.XX || a.XX - Terrain.XXX == terr.XX)) ||
                ((a.YY + Terrain.YYY == terr.YY || a.YY - Terrain.YYY == terr.YY) && (a.XX + Terrain.XXX / 2 == terr.XX || a.XX - Terrain.XXX / 2 == terr.XX));
            });
        }

        public List<Terrain> TerrainsInRange(List<Terrain> newTerrainsInRange, List<Terrain> oldTerrainsInRange, IEnumerable<Terrain> listTerrains, int range)
        {
            if (range > 0)
            {
                List<Terrain> newTerrainsInRangeCopy = newTerrainsInRange.ToList();
                foreach (Terrain ter in newTerrainsInRange)
                {
                    foreach (Terrain terra in ter.Adjacents.Except(oldTerrainsInRange))
                    {
                        oldTerrainsInRange.Add(terra);
                        newTerrainsInRangeCopy.Add(terra);
                    }
                }
                return TerrainsInRange(newTerrainsInRangeCopy, oldTerrainsInRange, listTerrains, --range);
            }
            else
                return newTerrainsInRange;
        }

        public List<Terrain> TerrainsInRangeOfOne(Terrain origin, IEnumerable<Terrain> listTerrains, int range)
        {
            List<Terrain> ListTerrainUnit = new List<Terrain>();
            ListTerrainUnit.Add(origin);
            List<Terrain> ListTerrainUnit1 = ListTerrainUnit.ToList();
            return TerrainsInRange(ListTerrainUnit, ListTerrainUnit1, listTerrains, range);
        }

        public IEnumerable<Terrain> TerrainsBehind(Terrain obstacle, Terrain origin, IEnumerable<Terrain> listTerrains, int rangeVisibility)
        {
            List<Terrain> CollectionBehind = new List<Terrain>();

            CalculateVectors(out double mx1, out double my1, out double mx2, out double my2, obstacle, origin);
            List<Terrain> ToProof = new List<Terrain>();
            ToProof.Add(obstacle);
            NextLevel(ToProof, rangeVisibility, new List<Terrain>());

            void NextLevel(IEnumerable<Terrain> ToProof1, int remainedVisibility, IEnumerable<Terrain> LastAdjacentCollection)
            {
                List<Terrain> newToProof = new List<Terrain>();
                List<Terrain> NewLastAdjacentCollection = new List<Terrain>();
                foreach (Terrain terrbehind in ToProof1)
                {
                    NewLastAdjacentCollection = NewLastAdjacentCollection.Union(terrbehind.Adjacents).ToList();
                    IEnumerable<Terrain> adjacentsBehind = terrbehind.Adjacents.Where(x => TerrainBehind1First(x, mx1, my1, mx2, my2, origin, obstacle)).Except(CollectionBehind);                    
                    foreach (Terrain terra in adjacentsBehind)
                    {
                        if (terra != obstacle)
                        {
                            CollectionBehind.Add(terra);
                            newToProof.Add(terra);
                        }
                    }
                }                
                if (newToProof.Count > 0 && --remainedVisibility > 0)
                    NextLevel(newToProof, remainedVisibility, NewLastAdjacentCollection);
            }            

            return CollectionBehind;
        }

        public void CalculateVectors(out double mx1, out double my1, out double mx2, out double my2, Terrain obstacle, Terrain orig)
        {
            double a = - Pow(Terrain.XXX/1.995, 2) + Pow(orig.XX, 2) + Pow(obstacle.XX, 2) - 2 * obstacle.XX * orig.XX;
            double b = -2 * orig.XX * orig.YY + 2 * orig.XX * obstacle.YY + 2 * obstacle.XX * orig.YY - 2 * obstacle.XX * obstacle.YY;
            double c = - Pow(Terrain.XXX / 1.995, 2) + Pow(orig.YY, 2) + Pow(obstacle.YY, 2) - 2 * orig.YY * obstacle.YY;
            if (a == 0)
                a = 1;
            // gradient (pendiente)
            double m1 = (-b + Pow((Pow(b, 2) - 4 * a * c), 0.5)) / (2 * a);
            double m2 = (-b - Pow((Pow(b, 2) - 4 * a * c), 0.5)) / (2 * a);
            // Vectors

            if (obstacle.XX + Terrain.XXX / 2 < orig.XX || obstacle.XX - Terrain.XXX / 2 > orig.XX)
            {
                if (obstacle.XX > orig.XX)
                {
                    mx1 = 1;
                    mx2 = 1;
                    my1 = m1;
                    my2 = m2;
                }
                else
                {
                    mx1 = -1;
                    mx2 = -1;
                    my1 = -m1;
                    my2 = -m2;
                }
            }
            else
            {
                if (obstacle.YY > orig.YY)
                {
                    mx1 = -1;
                    mx2 = 1;
                    my1 = -m1;
                    my2 = m2;
                }
                else
                {
                    mx1 = 1;
                    mx2 = -1;
                    my1 = m1;
                    my2 = -m2;
                }
            }
        }

        public bool TerrainBehind1First(Terrain terr, double mx1, double my1, double mx2, double my2, Terrain origin, Terrain obstacle)
        {
            if ( ((origin.XX <= terr.XX && terr.XX <= obstacle.XX) || (origin.XX >= terr.XX && terr.XX >= obstacle.XX)) &&
                ((origin.YY <= terr.YY && terr.YY <= obstacle.YY) || (origin.YY >= terr.YY && terr.YY >= obstacle.YY))  )
            {
                return false;
            }
            else
                return TerrainBehind1(terr, mx1, my1, mx2, my2, origin);
        }

        public bool TerrainBehind1(Terrain terr, double mx1, double my1, double mx2, double my2, Terrain origin)
        {
            double v = (my2 * (terr.XX - origin.XX) + mx2 * (origin.YY - terr.YY)) / (-my1 * mx2 + mx1 * my2);
            double b = (terr.XX - origin.XX - v * mx1) / mx2;
            if (v > 0 && b > 0)
                return true;
            else
                return false;
        }
    }

    public interface IFieldsRelations
    {
        IEnumerable<Terrain> AdjacentTerrains(Terrain terr, IEnumerable<Terrain> listTerrains);

        List<Terrain> TerrainsInRange(List<Terrain> newTerrainsInRange, List<Terrain> oldTerrainsInRange, IEnumerable<Terrain> listTerrains, int range);

        List<Terrain> TerrainsInRangeOfOne(Terrain origin, IEnumerable<Terrain> listTerrains, int range);

        IEnumerable<Terrain> TerrainsBehind(Terrain obstacle, Terrain origin, IEnumerable<Terrain> listTerrains, int rangeVisibility);

        void CalculateVectors(out double mx1, out double my1, out double mx2, out double my2, Terrain obstacle, Terrain orig);

        bool TerrainBehind1First(Terrain terr, double mx1, double my1, double mx2, double my2, Terrain origin, Terrain obstacle);

        bool TerrainBehind1(Terrain terr, double mx1, double my1, double mx2, double my2, Terrain origin);
    }
}
