using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation;

namespace SaveLoad
{
    public class SaveList : TranslationList
    {
        public SaveList(SaveLoadPage speichern)
        {
            List = new List<TranslationType>()
            {
                //new TranslationType{ Control = Speichern.speichernInstanz.HeaderName, Spanish = "Nombre", German ="Name", English ="Name"},
                //new TranslationType{ Control = Speichern.speichernInstanz.HeaderDatum, Spanish = "Fecha", German ="Datum", English ="Date"},
                new TranslationType{ Control = speichern.Abbrechen, Spanish = "Cancelar", German ="Abbrechen", English ="Cancel"},
                new TranslationType{ Control = speichern.Loeschen, Spanish = "Eliminar", German ="Löschen", English ="Delete"},
                new TranslationType{ Control = speichern.Bestaetigen, Spanish = "Confirmar", German ="Bestätigen", English ="Confirm"},
                new TranslationType{ Control = BitteNameDatei, Spanish = "Por favor, introduzca un nombre valido", German ="Tragen Sie bitte einen gültigen Namen", English ="Please, enter a valid name"},
                new TranslationType{ Control = SicherDelete, Spanish = "Está a punto de eliminar un archivo", German ="Sie sind im Begriff, eine Datei zu löschen", English ="You are about to delete a file"},
                new TranslationType{ Control = SicherUeberschreiben, Spanish = "Está a punto de sobreescribir un archivo", German ="Sie sind im Begriff, eine Datei zu überschreiben", English ="You are about to overwrite a file"},
                new TranslationType{ Control = Abbrechen, Spanish = "Cancelar", German ="Abbrechen", English ="Cancel"},
                new TranslationType{ Control = Bestaetigen, Spanish = "Confirmar", German ="Bestätigen", English ="Confirm"},
                new TranslationType{ Control = NoLoad, Spanish = "Ha surgido un problema al cargar una partida.\nProbablemente la versión de la partida no concuerda con la del programa", German ="Ein Problem mit ist entstanden, als versucht wurde, ein\nSpielzustand zu laden. Wahrscheinlich die Version des\nSpielzustandes stimmt nicht mit derjenigen von des Programmes", English ="There was a problem by loading the saved game.\nProbably the version doesn't match with that of the program."},
                new TranslationType{ Control = NoSave, Spanish = "Ha surgido un problema al salvar una partida.", German ="Ein Problem mit ist entstanden, als versucht wurde, ein Spielzustand zu speichen.", English ="There was a problem by saving the game."},
            };
        }

        public StringBuilder BitteNameDatei { set; get; } = new StringBuilder();
        public StringBuilder SicherDelete { set; get; } = new StringBuilder();
        public StringBuilder SicherUeberschreiben { set; get; } = new StringBuilder();
        public StringBuilder Abbrechen { set; get; } = new StringBuilder();
        public StringBuilder Bestaetigen { set; get; } = new StringBuilder();
        public StringBuilder NoLoad { set; get; } = new StringBuilder();
        public StringBuilder NoSave { set; get; } = new StringBuilder();
    }
}
