using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation;

namespace ViewGame.View.OtherWindowsMain
{
    public class VictoryList : TranslationList
    {
        public VictoryList(ref VictoryPage victory)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = victory.Noch_ein_Kampf, Spanish = "¡A otra batalla!", German ="Noch ein Kampf!", English ="Another fight!"},
            };
        }
    }
}
