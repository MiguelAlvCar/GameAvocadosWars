using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Map_Window.Einheiten;
using Map_Window.Converter_und_Karte;
using System.Windows;
using Map_Window.View.Main;
using Map_Window.Model;
using Model_Game;

namespace Map_Window.Konstruktoren
{
    // Weil die generischen Methoden keinen parametrisierten Konstruktor als constraint akzeptieren, muss man den Konstruktor hier verlagern
    public static class Konstruktoren
    {
        public static Einheitart ErstellungEinheit<Einheitart>(HeerZugehoerigkeit heerzuge) where Einheitart : Unit, new()
        {
            Einheitart Einheit = new Einheitart();
            Ellipse kreis = new Ellipse();

            string VerzeichnisFürsBild = heerzuge == HeerZugehoerigkeit.Rot ? Einheit.VerzeichnisFürsBildRot : Einheit.VerzeichnisFürsBildBlau;
            ImageBrush schwertk = new ImageBrush();
            schwertk.ImageSource = new BitmapImage(new Uri(VerzeichnisFürsBild));

            Einheit.Height = 16 * 5;
            Einheit.Width = 100;            
            Einheit.heerzugeh = heerzuge;

            kreis.Height = 12 * 5;
            kreis.Width = 12 * 5;
            SolidColorBrush Schwarz = new SolidColorBrush();
            Schwarz.Color = Colors.Black;
            kreis.Stroke = Schwarz;
            kreis.StrokeThickness = 3;
            kreis.SetValue(Canvas.TopProperty, (double)2 * 5);
            kreis.SetValue(Canvas.LeftProperty, (double)4 * 5);
            kreis.Fill = schwertk;

            Ellipse kreisbewegung = new Ellipse() { Uid = "Bewegung" };
            SolidColorBrush Gold = new SolidColorBrush();
            Gold.Color = Colors.Gold;
            SolidColorBrush Gruen = new SolidColorBrush();
            Gruen.Color = Colors.Green;

            kreisbewegung.Height = 2 * 5;
            kreisbewegung.Width = 2 * 5;
            kreisbewegung.Stroke = Schwarz;
            kreisbewegung.StrokeThickness = 2;
            kreisbewegung.Fill = Gruen;
            kreisbewegung.SetValue(Canvas.TopProperty, (double)12 * 5);
            kreisbewegung.SetValue(Canvas.LeftProperty, (double)9 * 5);

            Rectangle rec = new Rectangle();
            rec.Height = 8 * 5;
            rec.Width = 2 * 5;
            rec.Stroke = Gold;
            rec.StrokeThickness = 2;
            rec.Fill = Schwarz;
            rec.SetValue(Canvas.TopProperty, (double)4 * 5);
            rec.SetValue(Canvas.LeftProperty, (double)3 * 5);

            Rectangle rec1 = new Rectangle();
            rec1.Height = 8 * 5;
            rec1.Width = 2 * 5;
            rec1.Stroke = Gold;
            rec1.StrokeThickness = 2;
            rec1.Fill = Schwarz;
            rec1.SetValue(Canvas.TopProperty, (double)4 * 5);
            rec1.SetValue(Canvas.LeftProperty, (double)15 * 5);

            SolidColorBrush Blau = new SolidColorBrush();
            Blau.Color = Colors.DeepSkyBlue;

            Rectangle recleben = new Rectangle() { Uid = "Leben" };
            recleben.Height = (8 * 5) - 4;
            recleben.Width = (2 * 5) - 4;
            recleben.Fill = Gruen;
            recleben.SetValue(Canvas.TopProperty, ((double)4 * 5) + 2);
            recleben.SetValue(Canvas.LeftProperty, ((double)15 * 5) + 2);

            Rectangle recmoral = new Rectangle() { Uid = "Moral" };
            recmoral.Height = (8 * 5) - 4;
            recmoral.Width = (2 * 5) - 4;
            recmoral.Fill = Blau;
            recmoral.SetValue(Canvas.TopProperty, ((double)4 * 5) + 2);
            recmoral.SetValue(Canvas.LeftProperty, ((double)3 * 5) + 2);

            Einheit.Children.Add(kreis);
            Einheit.Children.Add(kreisbewegung);
            Einheit.Children.Add(rec);
            Einheit.Children.Add(rec1);
            Einheit.Children.Add(recleben);
            Einheit.Children.Add(recmoral);
            return Einheit;
        }
    }
}
