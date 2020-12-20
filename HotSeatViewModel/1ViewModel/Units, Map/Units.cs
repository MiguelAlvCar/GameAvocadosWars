using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using Map_Window;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections;
using Map_Window.Converter_und_Karte;
using Map_Window.View.Main;
using Map_Window.Model;

namespace Map_Window.Einheiten
{
    public enum HeerZugehoerigkeit
    {
        None, Rot, Blau
    }

    public abstract class Unit : Canvas
    {
        public HeerZugehoerigkeit heerzugeh { get; set; }
        public static string Bezeichner { set; get; }
        public virtual string VerzeichnisFürsBildRot {set; get;}
        public virtual string VerzeichnisFürsBildBlau { set; get; }
        public Unit()
        {
            MoralRest = Moralbasis;
            LebenRest = Lebenbasis;
            fliehend = false;
        }

        public virtual int Bewegungsbasis { get; set; }

        private int _BewegungRest;
        virtual public int BewegungRest
        {
            get { return _BewegungRest; }
            set
            {
                if (value < 0) _BewegungRest = 0;
                else _BewegungRest = value;
                BewegungsindikatorAktualisieren();
            }
        }

        protected void BewegungsindikatorAktualisieren()
        {
            foreach (Shape poly in this.Children)
            {
                if (poly.Uid == "Bewegung")
                {
                    if (BewegungRest == Bewegungsbasis) poly.Fill = Brushes.Green;
                    else if (BewegungRest == 0) poly.Fill = Brushes.Red;
                    else poly.Fill = Brushes.Yellow;
                }                
            }
        }

        public double Moralbasis { get; set; } = 100;

        protected double _MoralRest1;
        protected double _MoralRest
        {
            get { return _MoralRest1; }
            set
            {
                _MoralRest1 = value;
                foreach (UIElement Uiele in this.Children)
                {
                    if (Uiele.Uid == "Moral")
                    {
                        Rectangle rec = Uiele as Rectangle;
                        rec.Height = Math.Floor((((8 * 5) - 4) * _MoralRest1) / 100);
                        rec.SetValue(Canvas.TopProperty, (((double)4 * 5) + 2) + (((8 * 5) - 4) - rec.Height));
                    }
                }
            }
        }
        virtual public double MoralRest
        {
            get { return _MoralRest; }
            set
            {
                if (value < 0) _MoralRest = 0;
                else if (value > 100) _MoralRest = 100;
                else _MoralRest = value;
            }
        }

        public double Lebenbasis { get; set; } = 100;

        private double _LebenRest;        
        public double LebenRest
        {
            get { return _LebenRest; }
            set
            {
                if (value < 0) _LebenRest = 0;
                else _LebenRest = value;
                foreach (UIElement Uiele in this.Children)
                {
                    if (Uiele.Uid == "Leben")
                    {
                        Rectangle rec = Uiele as Rectangle;
                        rec.Height = Math.Floor((((8 * 5) - 4) * this.LebenRest) / 100);
                        rec.SetValue(Canvas.TopProperty, (((double)4 * 5) + 2) + (((8 * 5) - 4) - rec.Height));
                    }
                }
            }
        }

        public virtual double Kraft { get; set; }

        protected bool _fliehend;

        virtual public bool fliehend
        {
            get { return _fliehend; }
            set
            {
                _fliehend = value;
                if (value)
                {
                    FluchtModifikator = 5;
                }
                if (!value)
                {
                    foreach (UIElement rec in this.Children)
                    {
                        if (rec.Uid == "Flucht")
                        {
                            MainWindow.toremove = rec;
                        }
                    }
                    this.Children.Remove(MainWindow.toremove);
                    MainWindow.toremove = null;
                }
            }
        }

        private int _FluchtModifikator;

        virtual public int FluchtModifikator
        {
            get { return _FluchtModifikator; }
            set
            {
                
                if (value == 5)
                {
                    Rectangle rec = new Rectangle();
                    rec.Height = 22.5;
                    rec.Width = 6 * 5;
                    rec.SetValue(Canvas.LeftProperty, 7 * 5.0);
                    rec.SetValue(Canvas.TopProperty, 0.0);
                    ImageBrush Flagge = new ImageBrush();
                    Flagge.ImageSource = new BitmapImage(new Uri("pack://application:,,,/3View/Resources/Main/Images/Modifiers/Weissepflagge.jpg"));
                    rec.Fill = Flagge;
                    rec.Uid = "Flucht";
                    this.Children.Add(rec);
                }

                if (value == 0 && _FluchtModifikator != 0)
                {
                    this.fliehend = false;
                    this.MoralRest = 7;
                }
                _FluchtModifikator = value;
            }
        }

        internal virtual void NaechteRundeJedeZweite()
        {
            this.BewegungRest = this.Bewegungsbasis;
        }

        protected virtual void NaechteRundeJede2()
        {
            if (this.fliehend) this.FluchtModifikator--;
            if (this.MoralRest == 0 && !this.fliehend) this.fliehend = true;
            if (!this.fliehend) this.MoralRest += 13;
        }
        internal virtual void NaechteRundeJede1()
        {
            NaechteRundeJede2();
        }
    }

    public abstract class Nahkampf : Unit
    {
    }

    interface IFernkampf
    {
        int Reichweite { get; set; }

        double Fernkampf { get; set; }
    }

    public abstract class Kavallerie : Nahkampf
    {
        abstract public double AngriffStaerke { get; set; }

        private bool _angriffbereit = false;
        public bool Angriffbereit
        {
            get
            {

                return _angriffbereit;
            }
            set
            {
                if (value && !_angriffbereit)
                {
                    Rectangle rec = new Rectangle();
                    rec.Height = 20;
                    rec.Width = 27;
                    rec.SetValue(Canvas.LeftProperty, 36.5);
                    rec.SetValue(Canvas.TopProperty, -7.0);
                    ImageBrush Galopp = new ImageBrush();
                    Galopp.ImageSource = new BitmapImage(new Uri("pack://application:,,,/3View/Resources/Main/Images/Modifiers/Galopp.jpg"));
                    rec.Fill = Galopp;
                    rec.Uid = "Galopp";
                    this.Children.Add(rec);
                }
                if (!value)
                {
                    foreach (UIElement rec in this.Children)
                    {
                        if (rec.Uid == "Galopp")
                        {
                            MainWindow.toremove = rec;
                        }
                    }
                    this.Children.Remove(MainWindow.toremove);
                    MainWindow.toremove = null;
                }
                _angriffbereit = value;
            }
        }

        internal override void NaechteRundeJede1()
        {            
            NaechteRundeJede2();
            List<Unit> arrayFeinde = MainWindow.UnmittelbareFeinde(this);
            if (arrayFeinde.Count > 0) Angriffbereit = false;
            else if (!this.fliehend) Angriffbereit = true;
        }

    }

    public abstract class MoralModifizierer : Nahkampf
    {
        internal abstract double MoralFaktor { get; set; }
        internal void DirekterZufriffMoral(double moralrest)
        {
            _MoralRest = moralrest;
        }
    }

    public class Schwertkaempfer : Nahkampf
    {
        new public static string Bezeichner { set; get; } = "Schwertkaempfer";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://siteoforigin:,,,/3View/Resources/Main/Images/Armies/Red/RotSchwert.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/Schwertblau.jpg";
        public Schwertkaempfer()
        {
            Bewegungsbasis = 2;
            BewegungRest = Bewegungsbasis;
            Kraft = 4;
        }
    }

    public class Reiter : Kavallerie
    {
        new public static string Bezeichner { set; get; } = "Reiter";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/Pferderot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/Pferdeblau.jpg";
        public Reiter()
        {
            Bewegungsbasis = 4;
            BewegungRest = Bewegungsbasis;
            Kraft = 3;
        }
        override public double AngriffStaerke { get; set; } = 1.8;
    }
    
    public class Bogenschutze : Unit, IFernkampf
    {
        new public static string Bezeichner { set; get; } = "Bogenschutze";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/Bogenrot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/Bogenblau.jpg";
        public Bogenschutze()
        {
            Bewegungsbasis = 2;
            BewegungRest = Bewegungsbasis;
            Kraft = 1;
            Reichweite = 5;
            Fernkampf = 1.7;
        }
        public int Reichweite { get; set; }
        public double Fernkampf { get; set; }
    }

    public class Ritter : Kavallerie
    {
        new public static string Bezeichner { set; get; } = "Ritter";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/Ritterrot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/Ritterblau.jpg";
        public Ritter()
        {
            Bewegungsbasis = 3;
            BewegungRest = Bewegungsbasis;
            Kraft = 6;
        }
        override public double AngriffStaerke { get; set; } = 2.2;
    }

    public class Berseker : MoralModifizierer
    {
        new public static string Bezeichner { set; get; } = "Berseker";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/Bersekerrot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/Bersekerblau.jpg";
        public Berseker()
        {
            Bewegungsbasis = 2;
            BewegungRest = Bewegungsbasis;
            Kraft = 5;
        }

        override public bool fliehend
        {
            get { return _fliehend; }
            set
            {
                wuetend = value;                
            }
        }

        private bool _wuetend = false;

        public bool wuetend
        {
            get { return _wuetend; }
            set
            {                
                if (value && !_wuetend)
                {
                    FluchtModifikator = 5;
                    Rectangle rec = new Rectangle();
                    rec.Height = 22.5;
                    rec.Width = 6 * 5;
                    rec.SetValue(Canvas.LeftProperty, 7 * 5.0);
                    rec.SetValue(Canvas.TopProperty, 0.0);
                    ImageBrush Flagge = new ImageBrush();
                    Flagge.ImageSource = new BitmapImage(new Uri("pack://application:,,,/3View/Resources/Main/Images/Modifiers/wuetend.jpg"));
                    rec.Fill = Flagge;
                    rec.Uid = "Flucht";
                    this.Children.Add(rec);
                }
                if (!value)
                {
                    foreach (UIElement rec in this.Children)
                    {
                        if (rec.Uid == "Flucht")
                        {
                            MainWindow.toremove = rec;
                        }
                    }
                    this.Children.Remove(MainWindow.toremove);
                    MainWindow.toremove = null;
                }
                _wuetend = value;
            }
        }

        internal override double MoralFaktor { get; set; }=  1.3;
        
        override public double MoralRest
        {
            get { return _MoralRest; }
            set
            {
                if (value < 0)
                {
                    _MoralRest = 100;
                    if (!this.wuetend)
                        this.wuetend = true;
                }
                else if (!this.wuetend)
                {
                    double moralrest = _MoralRest - (MoralFaktor * (_MoralRest - value));
                    if (moralrest < 0) moralrest = 0;
                    if (moralrest > 100) moralrest = 100;
                    _MoralRest = moralrest;
                }
                if (value > 100) _MoralRest = 100;
            }
        }

        private int _FluchtModifikator;

        override public int FluchtModifikator
        {
            get { return _FluchtModifikator; }
            set
            {
                _FluchtModifikator = value;

                if (value == 0)
                {
                    this.wuetend = false;
                    this.MoralRest += 7;
                }
            }
        }

        protected override void NaechteRundeJede2()
        {
            if (this.wuetend) this.FluchtModifikator--;
            if (!this.wuetend) this.MoralRest += 13;
        }
        
    }

    public class Schwere_Infanterie : Nahkampf
    {
        new public static string Bezeichner { set; get; } = "Schwere_Infanterie";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/Schwerrot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/Schwerblau.jpg";
        public Schwere_Infanterie()
        {
            Bewegungsbasis = 2;
            BewegungRest = Bewegungsbasis;
            Kraft = 5;
        }
    }

    public class Leichte_InfanterieV : Nahkampf
    {
        new public static string Bezeichner { set; get; } = "Leichte_InfanterieV";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/LeichtVrot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/LeichtVblau.jpg";
        public Leichte_InfanterieV()
        {
            Bewegungsbasis = 3;
            BewegungRest = Bewegungsbasis;
            Kraft = 3;
        }
    }

    public class Anbeter : Nahkampf
    {
        new public static string Bezeichner { set; get; } = "Anbeter_des_Todes";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/Anbeterrot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/Anbeterblau.jpg";
        public Anbeter()
        {
            Bewegungsbasis = 2;
            BewegungRest = Bewegungsbasis;
            Kraft = 3.5;
        }

        public double AngstBonus { set; get; } = 2;
    }

    public class Hoplit : Nahkampf
    {
        new public static string Bezeichner { set; get; } = "Hoplit";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/Hoplitrot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/Hoplitblau.jpg";
        private bool _verteidungsfaehig = false;
        public bool Verteidungsfaehig
        {
            get
            {
                return _verteidungsfaehig;
            }
            set
            {                
                if (value && !_verteidungsfaehig)
                {
                    Rectangle rec = new Rectangle();
                    rec.Height = 20;
                    rec.Width = 27;
                    rec.SetValue(Canvas.LeftProperty, 36.5);
                    rec.SetValue(Canvas.TopProperty, -7.0);
                    ImageBrush Lanzen = new ImageBrush();
                    Lanzen.ImageSource = new BitmapImage(new Uri("pack://application:,,,/3View/Resources/Main/Images/Modifiers/Lanzen.jpg"));
                    rec.Fill = Lanzen;
                    rec.Uid = "Lanzen";
                    this.Children.Add(rec);
                }
                if (!value)
                {
                    foreach (UIElement rec in this.Children)
                    {
                        if (rec.Uid == "Lanzen")
                        {
                            MainWindow.toremove = rec;
                        }
                    }
                    this.Children.Remove(MainWindow.toremove);
                    MainWindow.toremove = null;
                }
                _verteidungsfaehig = value;
            }
        }
        
        public Hoplit()
        {
            Bewegungsbasis = 2;
            BewegungRest = Bewegungsbasis;
            Kraft = 3.5;
        }

        internal override void NaechteRundeJede1()
        {            
            NaechteRundeJede2();

            Gelaende gela = this.Parent as Gelaende;
            List<Unit> listfeinde = MainWindow.UnmittelbareFeinde(this);
            if (listfeinde.Count != 0)
                this.Verteidungsfaehig = false;
            else if (!this.fliehend)
                this.Verteidungsfaehig = true;
        }
    }

    public class Begleitk : Kavallerie
    {
        new public static string Bezeichner { set; get; } = "Begleitkavallerie";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/Begleitrot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/Begleitblau.jpg";
        public Begleitk()
        {
            Bewegungsbasis = 4;
            BewegungRest = Bewegungsbasis;
            Kraft = 4;
        }
        override public double AngriffStaerke { get; set; } = 2;
    }

    public class Elefant : MoralModifizierer
    {
        new public static string Bezeichner { set; get; } = "Elefant";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/Elefantrot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/Elefantblau.jpg";
        public Elefant()
        {
            Bewegungsbasis = 3;
            BewegungRest = Bewegungsbasis;
            Kraft = 7;
        }

        private int _BewegungRest;
        override public int BewegungRest
        {
            get { return _BewegungRest; }
            set
            {
                if (value < 0) _BewegungRest = 0;
                else if (!this.tobend || MainWindow.TobendesEle) _BewegungRest = value;
                BewegungsindikatorAktualisieren();
            }
        }

        override public bool fliehend
        {
            get { return _fliehend; }
            set
            {
                tobend = value;
            }
        }

        private bool _tobend;

        public bool tobend
        {
            get { return _tobend; }
            set
            {
                if (value && !_tobend)
                {
                    FluchtModifikator = 5;
                    _MoralRest = 100;
                    BewegungRest = 0;

                    Rectangle rec = new Rectangle();
                    rec.Height = 22.5;
                    rec.Width = 6 * 5;
                    rec.SetValue(Canvas.LeftProperty, 7 * 5.0);
                    rec.SetValue(Canvas.TopProperty, 0.0);
                    ImageBrush Flagge = new ImageBrush();
                    Flagge.ImageSource = new BitmapImage(new Uri("pack://application:,,,/3View/Resources/Main/Images/Modifiers/Aufgescheucht.jpg"));
                    rec.Fill = Flagge;
                    rec.Uid = "Flucht";
                    this.Children.Add(rec);
                }                
                if (!value)
                {
                    foreach (UIElement rec in this.Children)
                    {
                        if (rec.Uid == "Flucht")
                        {
                            MainWindow.toremove = rec;
                        }
                    }
                    this.Children.Remove(MainWindow.toremove);
                    MainWindow.toremove = null;
                }
                _tobend = value;
            }
        }

        private int _FluchtModifikator;

        override public int FluchtModifikator
        {
            get { return _FluchtModifikator; }
            set
            {                
                if (value == 0 && _FluchtModifikator != 0)
                {
                    this.tobend = false;
                    this.DirekterZufriffMoral(7);
                    this.BewegungRest = this.Bewegungsbasis;
                }
                _FluchtModifikator = value;
            }
        }

        internal override double MoralFaktor { get; set; } = 2;
        override public double MoralRest
        {
            get { return _MoralRest; }
            set
            {
                if (value < 0)
                {
                    _MoralRest = 0;
                }
                else if (!this.tobend)
                {
                    double moralrest = _MoralRest - (MoralFaktor * (_MoralRest - value));
                    if (moralrest < 0) moralrest = 0;
                    if (moralrest > 100) moralrest = 100;
                    _MoralRest = moralrest;
                }
                if (value > 100) _MoralRest = 100;
            }
        }

        protected override void NaechteRundeJede2()
        {
            if (this.tobend) this.FluchtModifikator--;
            if (this.MoralRest == 0 && !this.tobend) this.fliehend = true;
            if (!this.tobend) this.MoralRest += 13;
        }
    }

    public class Leichte_InfanterieH : Nahkampf
    {
        new public static string Bezeichner { set; get; } = "Leichte_InfanterieH";
        public override string VerzeichnisFürsBildRot { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Red/LeichtHrot.jpg";
        public override string VerzeichnisFürsBildBlau { set; get; } = "pack://application:,,,/3View/Resources/Main/Images/Armies/Blue/LeichtHblau.jpg";
        public Leichte_InfanterieH()
        {
            Bewegungsbasis = 2;
            BewegungRest = Bewegungsbasis;
            Kraft = 3;
        }
    }
}

