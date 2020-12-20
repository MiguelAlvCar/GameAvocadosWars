using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using Map_Window;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections;
using Map_Window.Converter_und_Karte;
using Map_Window.View.Main;

namespace Map_Window.Einheiten
{

    #region Listen von Einheiten und von Heeren

    public class ListeHeere
    {
        public Dictionary<string, ItemDictionaryHeeren> ListeHeeren;
        public ListeHeere()
        {
            ListeUnitsHeere listeunits = new ListeUnitsHeere();
            ListeHeeren = new Dictionary<string, ItemDictionaryHeeren>();
            ItemDictionaryHeeren Ritter = new ItemDictionaryHeeren("Ritter von Bretonnia", "Ritter von Bretonnia", listeunits.RittervonBretonniaUnits, (byte)1);
            ListeHeeren.Add(Ritter.Bezeichner, Ritter);
            ItemDictionaryHeeren Raeuber = new ItemDictionaryHeeren("Norkische Räuber", "Norkische Räuber", listeunits.NorkischeRaeuberUnits, (byte)2);
            ListeHeeren.Add(Raeuber.Bezeichner, Raeuber);
            ItemDictionaryHeeren Phalangen = new ItemDictionaryHeeren("Tileanische Phalangen", "Tileanische Phalangen", listeunits.TileaPhalangenUnits, (byte)3);
            ListeHeeren.Add(Phalangen.Bezeichner, Phalangen);
        }
    }

    public class ItemDictionaryHeeren
    {
        public string Bezeichner { set; get; }
        public StringBuilder Name { set; get; }
        public Dictionary<string, ItemListUnit> UnitList { set; get; }
        public byte IDHeerDicti { set; get; }
        public ItemDictionaryHeeren(string bezeichner, string name, Dictionary<string, ItemListUnit> unititem, byte idHeer)
        {
            Bezeichner = bezeichner;
            Name = new StringBuilder(name);
            UnitList = unititem;
            IDHeerDicti = idHeer;
        }
    }

    public class ListeUnitsHeere
    {
        public static ListeUnitsHeere listeunits;
        public Dictionary<string, ItemListUnit> RittervonBretonniaUnits;
        public Dictionary<string, ItemListUnit> NorkischeRaeuberUnits;
        public Dictionary<string, ItemListUnit> TileaPhalangenUnits;
        public ListeUnitsHeere()
        {
            listeunits = this;
            RittervonBretonniaUnits = new Dictionary<string, ItemListUnit>();
            NorkischeRaeuberUnits = new Dictionary<string, ItemListUnit>();
            TileaPhalangenUnits = new Dictionary<string, ItemListUnit>();

            ItemListUnit schwertkbri = new ItemListUnit(4, Schwertkaempfer.Bezeichner);
            RittervonBretonniaUnits.Add(schwertkbri.BezeichnerUnit, schwertkbri);
            ItemListUnit reiterbri = new ItemListUnit(4.5, Reiter.Bezeichner);
            RittervonBretonniaUnits.Add(reiterbri.BezeichnerUnit, reiterbri);
            ItemListUnit bogenbri = new ItemListUnit(3, Bogenschutze.Bezeichner);
            RittervonBretonniaUnits.Add(bogenbri.BezeichnerUnit, bogenbri);
            ItemListUnit ritterbri = new ItemListUnit(7, Ritter.Bezeichner);
            RittervonBretonniaUnits.Add(ritterbri.BezeichnerUnit, ritterbri);

            ItemListUnit berserker = new ItemListUnit(7, Berseker.Bezeichner);
            NorkischeRaeuberUnits.Add(berserker.BezeichnerUnit, berserker);
            ItemListUnit sinfanterie = new ItemListUnit(5, Schwere_Infanterie.Bezeichner);
            NorkischeRaeuberUnits.Add(sinfanterie.BezeichnerUnit, sinfanterie);
            ItemListUnit leichinfanV = new ItemListUnit(3, Leichte_InfanterieV.Bezeichner);
            NorkischeRaeuberUnits.Add(leichinfanV.BezeichnerUnit, leichinfanV);
            ItemListUnit anbeter = new ItemListUnit(5, Anbeter.Bezeichner);
            NorkischeRaeuberUnits.Add(anbeter.BezeichnerUnit, anbeter);

            ItemListUnit hoplit = new ItemListUnit(4, Hoplit.Bezeichner);
            TileaPhalangenUnits.Add(hoplit.BezeichnerUnit, hoplit);
            ItemListUnit elefant = new ItemListUnit(8, Elefant.Bezeichner);
            TileaPhalangenUnits.Add(elefant.BezeichnerUnit, elefant);
            ItemListUnit leichinfanH = new ItemListUnit(3, Leichte_InfanterieH.Bezeichner);
            TileaPhalangenUnits.Add(leichinfanH.BezeichnerUnit, leichinfanH);
            ItemListUnit begleit = new ItemListUnit(6.5, Begleitk.Bezeichner);
            TileaPhalangenUnits.Add(begleit.BezeichnerUnit, begleit);
        }
    }

    public class ItemListUnit
    {
        public double Punkte { set; get; }
        public string BezeichnerUnit { set; get; }
        public StringBuilder Name { set; get; }
        public ItemListUnit(double punkte, string typeunit)
        {
            Punkte = punkte;
            BezeichnerUnit = typeunit;
            Name = new StringBuilder(typeunit);
        }
    }

    public class ListeVonEinheiten : ArrayList
    {
        public event SiegEventHandler OhneEinheiten;
        public void RemoveVerändert(object obj)
        {
            Remove(obj);
            if (this.Count <= 0) OhneEinheiten(this);
        }
    }

    public class ComboBoxItemHeer : ComboBoxItem
    {
        public ItemDictionaryHeeren Heer;
        public byte IDHeer;
    }

    #endregion

}
