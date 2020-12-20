using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation;

namespace ViewGame.View.OtherWindowsMain
{
    public class ListNew : TranslationList
    {
        public ListNew(NewPlayerPage newPlayer)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = newPlayer.Abrechen , Spanish = "Cancelar", German ="Abrechen", English ="Cancel"},
                new TranslationType{ Control = newPlayer.Name, Spanish = "Nombre", German ="Name", English ="Name"},
                new TranslationType{ Control = newPlayer.Bestätigen, Spanish = "Confirmar", German ="Bestätigen", English ="Confirm"},
            };
        }
    }
}
