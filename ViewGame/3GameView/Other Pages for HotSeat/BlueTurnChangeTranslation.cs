using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation;

namespace ViewGame.View.OtherWindowsMain
{
    public class ListBlueTurn : TranslationList
    {
        public ListBlueTurn(ref BlueTurnChange blueTurn)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = blueTurn.Runde_für, Spanish = "Turno para el jugador azul", German ="Runde für den blauen Spieler", English ="Turn for the blue player"},
                new TranslationType{ Control = blueTurn.Spielen, Spanish = "Jugar", German ="Spielen", English ="Play"},
            };
        }
    }
}
