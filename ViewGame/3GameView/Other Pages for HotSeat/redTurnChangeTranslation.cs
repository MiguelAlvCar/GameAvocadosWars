using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation;

namespace ViewGame.View.OtherWindowsMain
{
    public class ListRedTurn : TranslationList
    {
        public ListRedTurn(ref RedTurnChange redTurn)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = redTurn.Runde_für_den_roten, Spanish = "Turno para el jugador rojo", German ="Runde für den roten Spieler", English ="Turn for the red player"},
                new TranslationType{ Control = redTurn.Spielen, Spanish = "Jugar", German ="Spielen", English ="Play"},
            };
        }
    }
}
