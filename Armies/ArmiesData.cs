using System;
using System.Collections.Generic;
using System.Text;
using ArmyAndUnitTypes;
using BasicElements;
using Sound;
using ViewModelHotSeat;

namespace Armies
{
    public class ArmiesData
    {
        public List<ArmyType> ArmyList = new List<ArmyType>();

        public ArmiesData()
        {
            Dictionary<Language, string> cavalryDic = new Dictionary<Language, string>();
            cavalryDic.Add(Language.English, "Charge: multiplies the force,\nif they begin the turn\nwithout adjacent enemies");
            cavalryDic.Add(Language.German, "Kavallerieangriff: vervielfacht\ndie Kraft, wenn sie die Runde\nohne angrenzende Feind anfängt");
            cavalryDic.Add(Language.Spanish, "Carga: multiplica la fuerza\ncuando se inicia el turno\nsin enemigos adyacentes");



            ArmyType knights = new ArmyType(ArmyTypeEnum.Knights);
            knights.LanguageNameDictionary.Add(Language.English, "Knights of Bretonnia");
            knights.LanguageNameDictionary.Add(Language.German, "Ritter von Bretonnia");
            knights.LanguageNameDictionary.Add(Language.Spanish, "Caballeros de Bretonia");

            UnitType<SwordFighter> schwertkbri = new UnitType<SwordFighter>() { Points = 4};
            schwertkbri.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/Swordman.png");
            schwertkbri.UnitNameDictionary.Add(Language.English, "Swordmans");
            schwertkbri.UnitNameDictionary.Add(Language.German, "Schwertkämpfer");
            schwertkbri.UnitNameDictionary.Add(Language.Spanish, "Espadachines");
            knights.Units.Add(schwertkbri);

            UnitType<Rider> reiterbri = new UnitType<Rider>() { Points = 4.5 };
            reiterbri.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/Rider.png");
            reiterbri.UnitNameDictionary.Add(Language.English, "Novice Riders");
            reiterbri.UnitNameDictionary.Add(Language.German, "Junge Reiter");
            reiterbri.UnitNameDictionary.Add(Language.Spanish, "Jinetes noveles");
            reiterbri.SpecialStrengthDictionary = cavalryDic;
            reiterbri.AttackEffect = StandardEffects.Horse;
            knights.Units.Add(reiterbri);

            UnitType<Knight> ritterbri = new UnitType<Knight>() { Points = 7 };
            ritterbri.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/Knight.png");
            ritterbri.UnitNameDictionary.Add(Language.English, "Knights");
            ritterbri.UnitNameDictionary.Add(Language.German, "Ritter");
            ritterbri.UnitNameDictionary.Add(Language.Spanish, "Caballeros");
            ritterbri.SpecialStrengthDictionary = cavalryDic;
            ritterbri.AttackEffect = StandardEffects.Horse;
            knights.Units.Add(ritterbri);

            UnitType<Archer> bogenbri = new UnitType<Archer>() { Points = 3 };
            bogenbri.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/Archer.png");
            bogenbri.UnitNameDictionary.Add(Language.English, "Archers");
            bogenbri.UnitNameDictionary.Add(Language.German, "Bogenschützen");
            bogenbri.UnitNameDictionary.Add(Language.Spanish, "Arqueros");
            bogenbri.AttackEffect = new UriEffect("Pfeil", "\\Units\\Effects"); 
            knights.Units.Add(bogenbri);



            ArmyType vikings = new ArmyType(ArmyTypeEnum.Vikings);
            vikings.LanguageNameDictionary.Add(Language.English, "Nordic looters");
            vikings.LanguageNameDictionary.Add(Language.German, "Norkische Räuber");
            vikings.LanguageNameDictionary.Add(Language.Spanish, "Saqueadores nórdicos");

            UnitType<Berseker> berserker = new UnitType<Berseker>() { Points = 7 };
            berserker.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/Berseker.png");
            berserker.UnitNameDictionary.Add(Language.English, "Bersekers");
            berserker.UnitNameDictionary.Add(Language.German, "Berseker");
            berserker.UnitNameDictionary.Add(Language.Spanish, "Bersequers");
            berserker.SpecialStrengthDictionary.Add(Language.English, "Blood rage: when the\nmoral is lost, they\nattack every adjacent enemy");
            berserker.SpecialStrengthDictionary.Add(Language.German, "Blutstrage: wenn die Moral\nauf Null geht, greifen\nsie alle angrenzenden Feinde an");
            berserker.SpecialStrengthDictionary.Add(Language.Spanish, "Trance de sangre: al perder\nla moral, en vez de huir, puede atacar a\ntodos los enemigos adyacentes");
            berserker.AttackEffect = new UriEffect("Berse", "\\Units\\Effects");
            vikings.Units.Add(berserker);

            UnitType<HeavyInfantry> sinfanterie = new UnitType<HeavyInfantry>() { Points = 5 };
            sinfanterie.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/HInfantry.png");
            sinfanterie.UnitNameDictionary.Add(Language.English, "Heavy infantry");
            sinfanterie.UnitNameDictionary.Add(Language.German, "Schwere Infanterie");
            sinfanterie.UnitNameDictionary.Add(Language.Spanish, "Infanteria pesada");
            vikings.Units.Add(sinfanterie);

            UnitType<Light_InfantryV> leichinfanV = new UnitType<Light_InfantryV>() { Points = 3 };
            leichinfanV.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/LInfantryV.png");
            leichinfanV.UnitNameDictionary.Add(Language.English, "Light infantry");
            leichinfanV.UnitNameDictionary.Add(Language.German, "Leichte Infanterie");
            leichinfanV.UnitNameDictionary.Add(Language.Spanish, "Infanteria ligera");
            vikings.Units.Add(leichinfanV);

            UnitType<Devotee> anbeter = new UnitType<Devotee>() { Points = 5 };
            anbeter.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/Devotee.png");
            anbeter.UnitNameDictionary.Add(Language.English, "Devotees of the Death");
            anbeter.UnitNameDictionary.Add(Language.German, "Anbeter des Todes");
            anbeter.UnitNameDictionary.Add(Language.Spanish, "Adoradores de la Muerte");
            anbeter.SpecialStrengthDictionary.Add(Language.English, "Invocation of the Death:\nbonus for the moral damage");
            anbeter.SpecialStrengthDictionary.Add(Language.German, "Anrufung des Todes: Bonus\nfür die Schädigung der Moral");
            anbeter.SpecialStrengthDictionary.Add(Language.Spanish, "Invocación de la muerte:\nbonus para dañar la moral");
            anbeter.AttackEffect = new UriEffect("Horro", "\\Units\\Effects");
            vikings.Units.Add(anbeter);



            ArmyType hoplits = new ArmyType(ArmyTypeEnum.Hoplits);
            hoplits.LanguageNameDictionary.Add(Language.English, "Tileanic phalanxes");
            hoplits.LanguageNameDictionary.Add(Language.German, "Tileanische Phalangen");
            hoplits.LanguageNameDictionary.Add(Language.Spanish, "Falanges tileanas");

            UnitType<Hoplit> hoplit = new UnitType<Hoplit>() { Points = 4 };
            hoplit.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/Hoplite.png");
            hoplit.UnitNameDictionary.Add(Language.English, "Hoplites");
            hoplit.UnitNameDictionary.Add(Language.German, "Hopliten");
            hoplit.UnitNameDictionary.Add(Language.Spanish, "Hoplitas");
            hoplit.SpecialStrengthDictionary.Add(Language.English, "Closed formation: when they start\nthe turn without adjacent enemies,\nthey form a phalanx until next combat");
            hoplit.SpecialStrengthDictionary.Add(Language.German, "Geschlossene Reihe: wenn sie die Runde\nohne anschliessenden Feinde anfängt, bilden\nsie bis den nächsten Kampf eine Phalanx");
            hoplit.SpecialStrengthDictionary.Add(Language.Spanish, "Formación cerrada: al empezar el\nturno sin enemigos adyacentes forman\nen falange hasta el proximo combate");
            hoplit.DefenseEffect = new UriEffect("Verte", "\\Units\\Effects");
            hoplits.Units.Add(hoplit);

            UnitType<Elephant> elefant = new UnitType<Elephant>() { Points = 8 };
            elefant.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/Elephant.png");
            elefant.UnitNameDictionary.Add(Language.English, "Elephants");
            elefant.UnitNameDictionary.Add(Language.German, "Elefanten");
            elefant.UnitNameDictionary.Add(Language.Spanish, "Elefantes");
            elefant.SpecialStrengthDictionary.Add(Language.English, "Animal fright: when the moral is\nlost, they move in an aleatory\ndirection attacking every unit");
            elefant.SpecialStrengthDictionary.Add(Language.German, "Aufscheuchen: wenn die Moral auf Null\ngeht, bewegen sie sich jede Runde in\neiner zufälligen Richtung, wobei sie\nalle getroffenen Einheiten angreifen");
            elefant.SpecialStrengthDictionary.Add(Language.Spanish, "Espanto: al perder la moral, se\nmueve cada turno en una dirección\naleatoria atacando a todas las unidades");
            elefant.AttackEffect = new UriEffect("Eleph", "\\Units\\Effects");
            hoplits.Units.Add(elefant);

            UnitType<LightInfantryH> leichinfanH = new UnitType<LightInfantryH>() { Points = 3 };
            leichinfanH.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/LInfantryH.png");
            leichinfanH.UnitNameDictionary.Add(Language.English, "Light infantry");
            leichinfanH.UnitNameDictionary.Add(Language.German, "Leichte Infanterie");
            leichinfanH.UnitNameDictionary.Add(Language.Spanish, "Infanteria ligera");
            hoplits.Units.Add(leichinfanH);

            UnitType<Companion> begleit = new UnitType<Companion>() { Points = 6 };
            begleit.ImageUri = new Uri("pack://siteoforigin:,,,/Units/Images/Companion.png");
            begleit.UnitNameDictionary.Add(Language.English, "Companion cavalry");
            begleit.UnitNameDictionary.Add(Language.German, "Begleitkavallerie");
            begleit.UnitNameDictionary.Add(Language.Spanish, "Caballeria de compañeros");
            begleit.SpecialStrengthDictionary = cavalryDic;
            begleit.AttackEffect = StandardEffects.Horse;
            hoplits.Units.Add(begleit);


            ArmyList.Add(hoplits);
            ArmyList.Add(knights);
            ArmyList.Add(vikings);
        }        
    }
}
