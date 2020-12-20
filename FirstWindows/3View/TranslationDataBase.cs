using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using FirstWindows.View.Start;
using FirstWindows.View.InitialDialog;
using FirstWindows.View.OnlineHall;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Translation;

namespace FirstWindows.View.Translation
{     

    public class StartList : TranslationList
    {
        public StartList(StartPage start)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = start.BackToMenu, Spanish = "Menu principal", German ="Hauptmenü", English ="Main menu"},
                new TranslationType{ Control = start.MapSaveButton, Spanish = "Salvar mapa", German ="Karte speichern", English ="Save map"},
                new TranslationType{ Control = start.MapLoadButton, Spanish = "Cargar mapa", German ="Karte laden", English ="Load map"},
                new TranslationType{ Control = start.GameSaveButton, Spanish = "Salvar juego", German ="Spiel speichern", English ="Save game"},
                new TranslationType{ Control = start.GameLoadButton, Spanish = "Cargar juego", German ="Spiel laden", English ="Load game"},
                new TranslationType{ Control = start.Optionen, Spanish = "Opciones", German ="Optionen", English ="Options"},
                new TranslationType{ Control = start.Spiel_verlassen, Spanish = "Abandonar juego", German ="Spiel verlassen", English ="Quit game"},
                new TranslationType{ Control = start.Zurück_zum_Spiel, Spanish = "Volver al juego", German ="Zurück zum Spiel", English ="Back to the game"},
            };
        }
    }     

    #region First Window

    public class ListVideo : TranslationList
    {
        public ListVideo()
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = NoMusik, Spanish = "Ha surgido un problema con Windows Media Player y no se podrá poner la música", German ="Ein Problem mit Windows Media Player ist entstanden sodass die Musik nicht abgepielt werden kann.", English ="There was a problem with Windows Media Player and it won't be possible to play the music."},
            };
        }
        
        public StringBuilder NoMusik { set; get; } = new StringBuilder();
    }

    public class ListEditUser : TranslationList
    {
        public ListEditUser(ref EditUserWindow editUser)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = WrongOldPass, Spanish = "La contraseña antigua no es correcta", German ="Das alte Passwort ist nicht richtig", English ="The old password isn't right"},
                new TranslationType{ Control = OldPassNeed, Spanish = "Introduzca la contraseña actual", German ="Tragen Sie das aktuelle Passwort ein", English ="Enter the current password"},
                new TranslationType{ Control = WrongNewPass, Spanish = "Las dos versiones de la nueva contraseña no concuerdan", German ="Die zwei Versionen des neuen Passworts stimmen nicht überein", English ="The two versions of the password don't match"},
                new TranslationType{ Control = editUser.UserName, Spanish = "Nombre", German ="Name", English ="Name"},
                new TranslationType{ Control = editUser.Email, Spanish = "Email", German ="Email", English ="Email"},
                new TranslationType{ Control = editUser.OldPass, Spanish = "Contraseña antigua", German ="Altes Passwort", English ="Old password"},
                new TranslationType{ Control = editUser.NewPass, Spanish = "Contraseña nueva", German ="Neues Passwort", English ="New password"},
                new TranslationType{ Control = editUser.NewPass2, Spanish = "Repetir la contraseña nueva", German ="Wiederholung des neuen Passworts", English ="Repeat the new password"},
                new TranslationType{ Control = InvalidEmail, Spanish = "El formato del email es incorrecto", German ="Das Email ist nicht korrekt formatiert", English ="The format of the email isn't right"},
            };
        }

        public StringBuilder WrongNewPass { set; get; } = new StringBuilder();
        public StringBuilder OldPassNeed { set; get; } = new StringBuilder();
        public StringBuilder WrongOldPass { set; get; } = new StringBuilder();
        public StringBuilder InvalidEmail { set; get; } = new StringBuilder();
    }

    public class ListFirst : TranslationList
    {
        public ListFirst(ButtonsPage instance)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = instance.Quit, Spanish = "Salir del juego", German ="Spiel verlassen", English ="Exit game"},
                new TranslationType{ Control = instance.Load, Spanish = "Cargar juego", German ="Spiel laden", English ="Load game"},
                new TranslationType{ Control = instance.Online, Spanish = "Juego en línea", German ="Netzspiel", English ="Online game"},
                new TranslationType{ Control = instance.HotSeat, Spanish = "Hot seat", German ="Hot seat", English ="Hot seat"},
                new TranslationType{ Control = instance.Options, Spanish = "Opciones", German ="Optionen", English ="Options"},
                new TranslationType{ Control = LoadGame, Spanish = "Cargar juego", German ="Spiel laden", English ="Load game"},
            };
        }

        public StringBuilder LoadGame { set; get; } = new StringBuilder();
    }

    public class ListOptions : TranslationList
    {
        public ListOptions(ref OptionsPage options)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = options.Sound, Spanish = "Sonido", German ="Sound", English ="Sound"},
                new TranslationType{ Control = options.Sprache, Spanish = "Idioma", German ="Sprache", English ="Language"},
                new TranslationType{ Control = options.Musik, Spanish = "Música", German ="Musik", English ="Music"},
                new TranslationType{ Control = options.Umgebung, Spanish = "Efectos", German ="Effekte", English ="Effects"},
                new TranslationType{ Control = options.Confirm, Spanish = "Confirmar", German ="Bestätigen", English ="Confirm"},
                new TranslationType{ Control = options.Sprache_wählen, Spanish = "Elección de idioma", German ="Sprache wählen", English ="Choose your language"},
                new TranslationType{ Control = options.Spanisch, Spanish = "Español", German ="Spanisch", English ="Spanish"},
                new TranslationType{ Control = options.Deutsch, Spanish = "Alemán", German ="Deutsch", English ="German"},
                new TranslationType{ Control = options.Englisch, Spanish = "Inglés", German ="Englisch", English ="English"},
                new TranslationType{ Control = options.LogOutNoti, Spanish = "No hay ningún usuario identificado", German ="Kein Benutzer ist angemeldet", English ="No player is logged in"},
                new TranslationType{ Control = options.LogIn, Spanish = "Identificarse", German ="Anmelden", English ="Log in"},
                new TranslationType{ Control = options.LogOut, Spanish = "Desconectarse", German ="Abmelden", English ="Log out"},
                new TranslationType{ Control = options.UserName, Spanish = "Nombre", German ="Name", English ="Name"},
                new TranslationType{ Control = options.Ability, Spanish = "Habilidad", German ="Fähigkeit", English ="Ability"},
                new TranslationType{ Control = options.Battles, Spanish = "Batallas", German ="Schlachten", English ="Battles"},
                new TranslationType{ Control = options.WonBattles, Spanish = "Batallas ganadas", German ="Gewonnene Schlachten", English ="Won battles"},
                new TranslationType{ Control = options.Email, Spanish = "Email", German ="Email", English ="Email"},
                new TranslationType{ Control = options.Edit, Spanish = "Editar", German ="Bearbeiten", English ="Edit"},
                new TranslationType{ Control = options.Player, Spanish = "Jugador", German ="Spieler", English ="Player"},
            };
        }
    }

    #endregion

    #region Online Hall

    public class ListOnlineHall : TranslationList
    {
        public ListOnlineHall(ref OnlineHallPage onlineHall)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = onlineHall.ConfirmAdv, Spanish = "Confirmar\nadversario", German ="Gegner\nbestätigen", English ="Confirm\nadversary"},
                new TranslationType{ Control = onlineHall.Back, Spanish = "Volver", German ="Zurück", English ="Back"},
                new TranslationType{ Control = onlineHall.HostGame, Spanish = "Organizar", German ="Veranstalten", English ="Host"},
                new TranslationType{ Control = onlineHall.Titel, Spanish = "Sala online", German ="Online Halle", English ="Online Hall"},
                new TranslationType{ Control = onlineHall.EnterMessage, Spanish = "Ok", German ="Ok", English ="Ok"},
                new TranslationType{ Control = ConfirmAdversaryButton, Spanish = "Confirmar\nadversario", German ="Gegner\nbestätigen", English ="Confirm\nadversary"},
                new TranslationType{ Control = DontConfirmAdversaryButton, Spanish = "Cancelar\nconfirmación", German ="Bestätigung\nwiderrufen", English ="Cancel\nconfirmation"},
                new TranslationType{ Control = AdversaryHasConfirmed, Spanish = "El adversarion\nestá listo", German ="Der Gegner\nist bereit", English ="The adversary\nis ready"},
            };
        }

        public StringBuilder ConfirmAdversaryButton { set; get; } = new StringBuilder();
        public StringBuilder DontConfirmAdversaryButton { set; get; } = new StringBuilder();
        public StringBuilder AdversaryHasConfirmed { set; get; } = new StringBuilder();
    }

    public class ListGuest : TranslationList
    {
        public ListGuest(ref Guest guest)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = guest.HeaderPlayerName, Spanish = "Jugador", German ="Spieler", English ="Player"},
                new TranslationType{ Control = guest.HeaderPlayerCapacity, Spanish = "Capacidad", German ="Fähigkeit", English ="Capacity"},
                new TranslationType{ Control = guest.HeaderTime, Spanish = "Hora de creación", German ="Erstellungszeit", English ="Creation time"},
                new TranslationType{ Control = guest.DescriptionTextBox, Spanish = "Descripción", German ="Beschreibung", English ="Description"},
                new TranslationType{ Control = guest.UpdateButton, Spanish = "Actualizar", German ="Aktualisieren", English ="Update"},
                new TranslationType{ Control = guest.Join, Spanish = "Unirse", German ="Eintreten", English ="Join"},
                new TranslationType{ Control = Join, Spanish = "Unirse", German ="Eintreten", English ="Join"},
                new TranslationType{ Control = GoOut , Spanish = "Salir", German ="Verlassen", English ="Abandon"},
            };
        }

        public StringBuilder Join { set; get; } = new StringBuilder();
        public StringBuilder GoOut { set; get; } = new StringBuilder();
    }    

    public class ListHost : TranslationList
    {
        public ListHost(ref Host host)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = host.Description, Spanish = "Descripción", German ="Beschreibung", English ="Description"},
                new TranslationType{ Control = host.ConfirmDescription, Spanish = "Confirmar\ndescripción", German ="Beschreibung\nbestätigen", English ="Confirm\nDescription"},
                new TranslationType{ Control = host.HeaderPlayerName, Spanish = "Jugador", German ="Spieler", English ="Player"},
                new TranslationType{ Control = host.HeaderPlayerCapacity, Spanish = "Capacidad", German ="Fähigkeit", English ="Capacity"},
                new TranslationType{ Control = host.OnlineGameNotConfirmed, Spanish = "Para crear un nuevo partida en línea debe introducir una descripción.", German ="Zur Erstellung eines online Spiels müssen Sie eine Beschreibung eintragen.", English ="In order to create a new online game, you must enter a description."},
                new TranslationType{ Control = host.OnlineGameConfirmed, Spanish = "Se ha creado una partida en línea, que se ha hecho pública.", German ="Ein online Spiel ist erstellt und öffentlich gemacht.", English ="A new game was created and made public."},
            };
        }
    }

    public class ListUserLogin : TranslationList
    {
        public ListUserLogin(ref UserLogin userLogin)
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = userLogin.Login, Spanish = "Identificación", German ="Anmeldung", English ="User login"},
                new TranslationType{ Control = userLogin.Name, Spanish = "Nombre", German ="Name", English ="Name"},
                new TranslationType{ Control = userLogin.Password, Spanish = "Contraseña", German ="Kennwort", English ="Passwort"},
                new TranslationType{ Control = userLogin.Cancel, Spanish = "Cancelar", German ="Abbrechen", English ="Cancel"},
                new TranslationType{ Control = userLogin.Confirm, Spanish = "Confirmar", German ="Bestätigen", English ="Confirm"},
                new TranslationType{ Control = InvalidEmail, Spanish = "El formato del email es incorrecto", German ="Das Email ist nicht korrekt formatiert", English ="The format of the email isn't right"},
                new TranslationType{ Control = ValidationMessage, Spanish = "Su nombre o su contraseña no es correcta", German ="Ihr Name oder Ihre Passwort ist nicht richtig", English ="Your name or your password isn't right"},
                new TranslationType{ Control = ConnectionToWebFailed, Spanish = "No se ha podido conectar al servidor Web", German ="Die Verbindung zum Webserver wurde fehlgeschlagen", English ="The connection to the webserver has failed"},
            };
        }

        public StringBuilder ConnectionToWebFailed { set; get; } = new StringBuilder();
        public StringBuilder ValidationMessage { set; get; } = new StringBuilder();
        public StringBuilder InvalidEmail { set; get; } = new StringBuilder();
    }

    #endregion


    
}
