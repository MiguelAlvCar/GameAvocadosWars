using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation;

namespace ViewGame.View.Resources
{
    public class UnitSelecterList : TranslationList
    {
        public UnitSelecterList(UnitSelecterPanel unitSelecter)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = unitSelecter.RestPointsText, Spanish = "Puntos\nrestantes", German ="Restpunkte", English ="Remaining\nPoints"},
                new TranslationType{ Control = unitSelecter.CreateUnitsButton, Spanish = "Construir\nunidades", German ="Einheiten\nschaffen", English ="Build\nunits"},
            };
        }
    }
}
