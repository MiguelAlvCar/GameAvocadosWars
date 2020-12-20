using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation;

namespace ViewGame.View.Game
{

    public class TranslationConverter : TranslationList
    {
        public static TranslationConverter GetTranslation()
        {
            return Translater.ÜbersetzungMeth(new TranslationConverter());
        }

        private TranslationConverter()
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = Turn, Spanish = "Turno: ", German ="Runde: ", English ="Turn: "},
                new TranslationType{ Control = Leben, Spanish = "Vida: ", German ="Leben: ", English ="Live: "},
                new TranslationType{ Control = Moral, Spanish = "Moral: ", German ="Moral: ", English ="Morale: "},
                new TranslationType{ Control = Bewegung, Spanish = "Movimiento: ", German ="Bewegung: ", English ="Move: "},
                new TranslationType{ Control = Kraft, Spanish = "Fuerza: ", German ="Kraft: ", English ="Stregth: "},
                new TranslationType{ Control = Range, Spanish = "Alcance: ", German ="Reichweite: ", English ="Range: "},
                new TranslationType{ Control = Fernkampf, Spanish = "Fuerza a distancia: ", German ="Fernkampf: ", English ="Ranged stregth: "},
                new TranslationType{ Control = Length, Spanish = "Largo: ", German ="Länge: ", English ="Legth: "},
                new TranslationType{ Control = Width, Spanish = "Ancho: ", German ="Breite: ", English ="Width: "},
            };
        }
        public StringBuilder Moral { set; get; } = new StringBuilder();
        public StringBuilder Leben { set; get; } = new StringBuilder();
        public StringBuilder Turn { set; get; } = new StringBuilder();
        public StringBuilder Fernkampf { set; get; } = new StringBuilder();
        public StringBuilder Range { set; get; } = new StringBuilder();
        public StringBuilder Kraft { set; get; } = new StringBuilder();
        public StringBuilder Bewegung { set; get; } = new StringBuilder();
        public StringBuilder Length { set; get; } = new StringBuilder();
        public StringBuilder Width { set; get; } = new StringBuilder();
    }
}
