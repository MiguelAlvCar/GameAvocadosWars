
using System.Windows;
using System.IO;
using ViewGame.View.Game;
using System.Windows.Controls;
using System.Windows.Navigation;
using Translation;

namespace ViewGame.View.OtherWindowsMain
{
    /// <summary>
    /// Interaktionslogik für Neuer_Spieler.xaml
    /// </summary>
    public partial class NewPlayerPage : Page
    {
        private GamePage HotSeat;

        public NewPlayerPage(GamePage hotSeat)
        {
            HotSeat = hotSeat;
            InitializeComponent();
            NewPlayerPage newPlayer = this;
            Translater.ÜbersetzungMeth(new ListNew(newPlayer));
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(null);
        }

        private void Confirm (object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameTextbox.Text))
            {
                HotSeat.ViewModel.listPlayersOb.Add(NameTextbox.Text);
                if (!File.Exists("Spieler.dat"))
                {
                    FileInfo SpielerFile = new FileInfo("Spieler.dat");
                    FileStream FStr = SpielerFile.Create();
                    SpielerFile.Attributes = FileAttributes.Hidden;
                    FStr.Close();
                }
                FileStream FStre = new FileStream("Spieler.dat", FileMode.Open);
                FStre.Seek(0, SeekOrigin.End);
                StreamWriter StWriter = new StreamWriter(FStre);
                StWriter.WriteLine(NameTextbox.Text);
                StWriter.Close();
                FStre.Close();
            }

            NavigationService.GetNavigationService(this).Navigate(null);
        }
    }
}
