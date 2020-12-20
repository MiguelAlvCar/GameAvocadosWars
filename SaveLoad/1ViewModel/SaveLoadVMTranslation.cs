using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation;

namespace SaveLoad
{
    public class SaveLoadVMList : TranslationList
    {
        public SaveLoadVMList()
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = NoLoad, Spanish = "Ha surgido un problema al cargar una partida.\nProbablemente la versión de la partida no concuerda con la del programa", German ="Ein Problem mit ist entstanden, als versucht wurde, ein\nSpielzustand zu laden. Wahrscheinlich die Version des\nSpielzustandes stimmt nicht mit derjenigen von des Programmes", English ="There was a problem by loading the saved game.\nProbably the version doesn't match with that of the program."},
                new TranslationType{ Control = NoSave, Spanish = "Ha surgido un problema al salvar una partida.", German ="Ein Problem mit ist entstanden, als versucht wurde, ein Spielzustand zu speichen.", English ="There was a problem by saving the game."},
            };
        }

        public StringBuilder NoLoad { set; get; } = new StringBuilder();
        public StringBuilder NoSave { set; get; } = new StringBuilder();
    }
}
