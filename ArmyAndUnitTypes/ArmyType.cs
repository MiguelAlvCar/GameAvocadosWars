using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicElements;
using Translation;

namespace ArmyAndUnitTypes
{
    public class ArmyType
    {
        public ArmyType(ArmyTypeEnum armyID)
        {
            ArmyID = armyID;
        }

        public ArmyTypeEnum ArmyID;

        public Dictionary<Language, string> LanguageNameDictionary { set; get; } = new Dictionary<Language, string>();

        public List<AbstractUnitType> Units { set; get; } = new List<AbstractUnitType>();

        public string Name {
            get
            {
                return LanguageNameDictionary[Translater.Language];
            }
        }
    }

    public enum ArmyTypeEnum
    {
        Knights, Vikings, Hoplits
    }
}
