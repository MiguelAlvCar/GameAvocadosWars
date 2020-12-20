using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using BasicElements;

namespace FirstWindows.View.InitialDialog
{
    /// <summary>
    /// Interaktionslogik für SpracheWählen.xaml
    /// </summary>
    public partial class SpracheWählenWindow : Window
    {
        public SpracheWählenWindow(Action continueMeth)
        {
            Continue = continueMeth;
            try
            {
                InitializeComponent();
                Sprache_Wählen.Text = "Bitte, wählen Sie ihre Sprache\nPor favor, elija su idioma\nPlease choose your language";
            }
            catch (Exception e)
            {
                string CurrentDirectory = System.Environment.CurrentDirectory;
                string Directory = CurrentDirectory.Substring(0, 2);
                FileStream filest = new FileStream(Directory + "\\Miguel.txt", FileMode.Create);
                BinaryWriter BinWriter = new BinaryWriter(filest);
                BinWriter.Seek(0, SeekOrigin.Begin);
                BinWriter.Write("Sprache Wählen \n Exception: " + e + "\nMessage: " + e.Message + "\nSource: " + e.Source + "\nTarget: " + e.TargetSite + "\nData: " + e.Data);
                BinWriter.Close();
                throw e;
            }
        }

        Action Continue;

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            byte Byte = 0;
            switch (ComboboxSprache.Text)
            {
                case "English":
                    Byte = Convert.ToByte(BasicElements.Language.English);
                    break;
                case "Deutsch":
                    Byte = Convert.ToByte(BasicElements.Language.German);
                    break;
                case "Español":
                    Byte = Convert.ToByte(BasicElements.Language.Spanish);
                    break;
            }

            FileStream filest = new FileStream(System.Environment.CurrentDirectory + "\\Daten.dat", FileMode.Open);
            BinaryWriter BinWriter = new BinaryWriter(filest);
            BinWriter.Seek(0, SeekOrigin.Begin);
            BinWriter.Write(Byte);
            BinWriter.Close();
                        
            this.Close();

            Continue();
        }
    }
}
