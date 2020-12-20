using System.Collections.Generic;
using System.Text;
using Translation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ViewGame.View.Game
{
    public class HotSeatList : TranslationList
    {
        public HotSeatList(GamePage hotseat)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = hotseat.Start, Spanish = "Inicio", German = "Start", English = "Start" },
                new TranslationType{ Control = hotseat.chkbox, Spanish = "Mismos puntos", German ="Gleiche Punktzahl", English ="Same points"},
                new TranslationType{ Control = hotseat.Neuer_Spieler, Spanish = "Nuevo jugador", German ="Neuer Spieler", English ="New player"},
                new TranslationType{ Control = hotseat.Raster_Zeichnen, Spanish = "Dibujar tablero", German ="Raster Zeichnen", English ="Draw board"},
                new TranslationType{ Control = hotseat.Wald, Spanish = "Bosque", German ="Wald", English ="Forest"},
                new TranslationType{ Control = hotseat.Huegel, Spanish = "Colina", German ="Huegel", English =" Hill "},
                new TranslationType{ Control = hotseat.See, Spanish = " Mar ", German =" See ", English =" See "},
                new TranslationType{ Control = hotseat.City, Spanish = "Ciudad", German ="Stadt", English ="City"},
                new TranslationType{ Control = hotseat.River, Spanish = " Río ", German ="Fluss", English ="River"},
                new TranslationType{ Control = hotseat.Bridge, Spanish = "Puente", German ="Brücke", English ="Bridge"},
                new TranslationType{ Control = hotseat.Karte, Spanish = "Mapa", German ="Karte", English ="Map"},
                new TranslationType{ Control = hotseat.Heer_1, Spanish = "Ejército 1", German ="Heer 1", English ="Army 1"},
                new TranslationType{ Control = hotseat.Heer_2, Spanish = "Ejército 2", German ="Heer 2", English ="Army 2"},
                new TranslationType{ Control = hotseat.Gemacht_von, Spanish = "Hecho por Miguel Alvarez Caro", German ="Gemacht von Miguel Alvarez Caro", English ="Made by Miguel Alvarez Caro"},
                new TranslationType{ Control = hotseat.Heer1, Spanish = "Ejército 1", German ="Heer 1", English ="Army 1"},
                new TranslationType{ Control = hotseat.Heer2, Spanish = "Ejército 2", German ="Heer 2", English ="Army 2"},
                new TranslationType{ Control = hotseat.Punktzahl, Spanish = "Puntos", German ="Punktzahl", English ="Points"},
                //new TranslationType{ Control = hotseat.NextTurn, Spanish = "Siguiente turno", German ="Nächste Runde", English ="Next turn"},
                new TranslationType{ Control = hotseat.Fire, Spanish = "Disparar", German ="Feuern", English ="Fire"},
                new TranslationType{ Control = hotseat.Spieler, Spanish = "Jugador", German ="Spieler", English ="Player"},
                new TranslationType{ Control = hotseat.Zum_Heer_2 , Spanish = "Al\nejército 2", German ="Zum\nHeer 2", English ="To the\n2nd army"},
                new TranslationType{ Control = hotseat.Fraktion, Spanish = "Facción", German ="Fraktion", English ="Faction"},
                new TranslationType{ Control = ErklaerungAnbeter, Spanish = "Invocación de la muerte:\nbonus para dañar la moral", German ="Anrufung des Todes: Bonus\nfür die Schädigung der Moral", English ="Invocation of the Death:\nbonus for the moral damage"},
                new TranslationType{ Control = ErklaerungBerserker, Spanish = "Trance de sangre: al perder la\nmoral, en vez de huir, puede atacar\na todos los enemigos adyacentes", German ="Blutstrage: wenn die Moral\nauf Null geht, greifen sie\nalle angrenzenden Feinde an", English ="Blood rage: when the\nmoral is lost, they\nattack every adjacent enemy"},
                new TranslationType{ Control = ErklaerungElefant, Spanish = "Espanto: al perder la moral, se\nmueve cada turno en una dirección\naleatoria atacando a todas las unidades", German ="Aufscheuchen: wenn die Moral auf Null\ngeht, bewegen sie sich jede Runde in\neiner zufälligen Richtung, wobei sie\nalle getroffenen Einheiten angreifen", English ="Animal fright: when the moral is\nlost, they move in an aleatory\ndirection attacking every unit"},
                new TranslationType{ Control = ErklaerungHoplit, Spanish = "Formación cerrada: al empezar el\nturno sin enemigos adyacentes forman\nen falange hasta el proximo combate", German ="Geschlossene Reihe: wenn sie die Runde\nohne anschliessenden Feinde anfängt, bilden\nsie bis den nächsten Kampf eine Phalanx", English ="Closed formation: when they start\nthe turn without adjacent enemies,\nthey form a phalanx until next combat"},
                new TranslationType{ Control = ErklaerungKavallerie, Spanish = "Carga: multiplica la fuerza\ncuando se inicia el turno\nsin enemigos adyacentes", German ="Kavallerieangriff: vervielfacht\ndie Kraft, wenn sie die Runde\nohne angrenzende Feinde anfängt", English ="Charge: multiplies the force,\nif they begin the turn\nwithout adjacent enemies"},
                new TranslationType{ Control = hotseat.RotToggleB, Spanish = "Rojo", German ="Rot", English ="Red"},
                new TranslationType{ Control = hotseat.BlauToggleB, Spanish = "Azul", German ="Blau", English ="Blue"},
                new TranslationType{ Control = PleaseNoUnits, Spanish = "Por favor, despliegue alguna unidad en el mapa.", German ="Schwärmen Sie bitte mindestens eine Einheit in die Karte aus.", English ="Please, deploy at least a unit in the map."},
                new TranslationType{ Control = PleasePointsNumber, Spanish = "Por favor, introduzca un numero de puntos.", German ="Tragen Sie bitte eine gültige Zahl von Pünkte", English ="Please, insert a number of points"},
                new TranslationType{ Control = BitteRaster, Spanish = "Por favor, dibuje el tablero", German ="Bitte, zeichnen Sie einen Raster", English ="Please, draw the board"},
                new TranslationType{ Control = PleaseDeploy, Spanish = "Por favor, despliegue todas sus tropas", German ="Bitte, lassen Sie alle ihre Truppen ausschwärmen", English ="Please, deploy all your troops"},
                new TranslationType{ Control = GloriousVictory1, Spanish = "¡¡Una victoria gloriosa!!\n", German ="Ein glorreicher Sieg!!\n", English ="A glorious victory!!\n"},
                new TranslationType{ Control = GloriousVictory2, Spanish = " ha vencido", German =" hat besiegt", English =" has triumphed"},
                new TranslationType{ Control = HideFlowDocument, Spanish = "Esconder", German ="Verstecken", English ="Hide"},
                new TranslationType{ Control = ShowFlowDocument, Spanish = "Mostrar", German ="Zeigen", English ="Show"},
                new TranslationType{ Control = AdversaryOffline, 
                    Spanish = "El adversario se ha desconectado.\nEl juego se guardará en la base de datos al pasar el turno y el adversario será notificado por email", 
                    German ="Der Gegner ist offline gegangen.\nDas Spiel wird in die Datenbank gespeichert und der Gegner wird über Email benachrichtigt, wenn Sie die Runde beenden", 
                    English ="The adversary has disconnected.\nBy passing the turn the game will be saved in the database and the adversary will be notified with an email."},

                new TranslationType{ Control = hotseat.ToCreateArmies, Spanish = "Construcción\n de los \nejércitos", German ="Aufbau\nder Heere", English ="Building\nof the\narmies"},
                new TranslationType{ Control = ToCreateArmies, Spanish = "Construcción\n de los \nejércitos", German ="Aufbau\nder Heere", English ="Building\nof the\narmies"},
                new TranslationType{ Control = IsConfirmed, Spanish = "Esperando\nconfirmación", German ="Warten auf die\nBestätigung", English ="Waiting for the\nconfirmation"},
                new TranslationType{ Control = AdversaryHasConfirmed, Spanish = "El adversarion\nestá listo", German ="Der Gegner\nist bereit", English ="The adversary\nis ready"},

                 new TranslationType{ Control = hotseat.ToBattle, Spanish = "A la\nbatalla", German ="Zum\nSchlacht", English ="To the\nbattle"},
                 new TranslationType{ Control = ToBattle, Spanish = "A la\nbatalla", German ="Zum\nSchlacht", English ="To the\nbattle"},
                
            };
        }

        public StringBuilder ErklaerungAnbeter { set; get; } = new StringBuilder();
        public StringBuilder ErklaerungBerserker { set; get; } = new StringBuilder();
        public StringBuilder ErklaerungElefant { set; get; } = new StringBuilder();
        public StringBuilder BitteNameDatei { set; get; } = new StringBuilder();
        public StringBuilder SicherDelete { set; get; } = new StringBuilder();
        public StringBuilder SicherUeberschreiben { set; get; } = new StringBuilder();
        public StringBuilder Abbrechen { set; get; } = new StringBuilder();
        public StringBuilder Bestaetigen { set; get; } = new StringBuilder();
        public StringBuilder ErklaerungHoplit { set; get; } = new StringBuilder();
        public StringBuilder ErklaerungKavallerie { set; get; } = new StringBuilder();
        public StringBuilder PleaseNoUnits { set; get; } = new StringBuilder();
        public StringBuilder PleasePointsNumber { set; get; } = new StringBuilder();
        public StringBuilder BitteRaster { set; get; } = new StringBuilder();
        public StringBuilder PleaseDeploy { set; get; } = new StringBuilder();
        public StringBuilder GloriousVictory1 { set; get; } = new StringBuilder();
        public StringBuilder GloriousVictory2 { set; get; } = new StringBuilder();
        public StringBuilder HideFlowDocument { set; get; } = new StringBuilder();
        public StringBuilder ShowFlowDocument { set; get; } = new StringBuilder();
        public StringBuilder ToCreateArmies { set; get; } = new StringBuilder();
        public StringBuilder MapIsConfirmed { set; get; } = new StringBuilder();
        public StringBuilder AdversaryHasMapConfirmed { set; get; } = new StringBuilder();
        public StringBuilder ToBattle { set; get; } = new StringBuilder();
        public StringBuilder IsConfirmed { set; get; } = new StringBuilder();
        public StringBuilder AdversaryHasConfirmed { set; get; } = new StringBuilder(); 
        public StringBuilder AdversaryOffline { set; get; } = new StringBuilder();
    }

    public static class BinderUpdater
    {
        public static void UpdateTranslationHotSeat(GamePage hotseat)
        {
            hotseat.Länge.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            hotseat.Breite.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            BindingOperations.GetMultiBindingExpression(hotseat.MovementText, TextBlock.TextProperty).UpdateTarget();
            hotseat.LifeText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            hotseat.MoralText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            hotseat.StregthText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            hotseat.RangeText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            hotseat.RangedStregthText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            hotseat.SpecialStrengthText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            hotseat.UnitNameText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
    }
}
