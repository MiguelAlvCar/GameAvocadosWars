using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation;

namespace ViewGame.View.Game
{
    public class TranslationValidation : TranslationList
    {
        public static TranslationValidation GetTranslation()
        {
            return Translater.ÜbersetzungMeth(new TranslationValidation());
        }

        private TranslationValidation()
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = PleasePoints, Spanish = "Por favor, dé a cada jugador por lo menos tres puntos", German ="Bitte, geben Sie beiden Heeren mehr als drei Pünkte", English ="Please, give each player at least three points"},
                new TranslationType{ Control = PleaseNumber, Spanish = "Por favor, introduzca un numero de puntos con cifras", German ="Bitte, tragen Sie für die Punktzahl eine Zahl mit Ziffern ein", English ="Please, enter a digit for the number of points"},
                new TranslationType{ Control = PleasePlayer, Spanish = "Por favor, seleccione un jugador distinto para cada ejército", German ="Bitte, wählen Sie einen verschiedenen Spieler für jedes Heer", English ="Please, select a different player for each army"},
                new TranslationType{ Control = PleaseArmy, Spanish = "Por favor, seleccione una facción para cada jugador", German ="Bitte, wählen Sie eine Fraktion für jeden Spieler", English = "Please, choose a faction for each player"},
            };
        }
        public StringBuilder PleaseNumber { set; get; } = new StringBuilder();
        public StringBuilder PleasePoints { set; get; } = new StringBuilder();
        public StringBuilder PleasePlayer { set; get; } = new StringBuilder();
        public StringBuilder PleaseArmy { set; get; } = new StringBuilder();
    }
}
