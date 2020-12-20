using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FirstWindows.View.Translation;
using BasicElements;
using Translation;
using Sound;
using Ninject;
using WpfBasicElements.AbstractClasses;

namespace GameFrontEnd.View
{
    internal static class DatenDat
    {
        public static void OpenDatenDat()
        {
            FileStream filest = new FileStream(System.Environment.CurrentDirectory + "\\Daten.dat", FileMode.Open);
            BinaryReader BinReader = new BinaryReader(filest);
            //filest.Seek(0, SeekOrigin.Begin);
            Translater.Language = (Language)BinReader.ReadByte();
            BasicMechanisms.Kernel.Get<IMusic>().MusicVolume = BinReader.ReadDouble();
            BasicMechanisms.Kernel.Get<IEffects>().VolumenEffekte = BinReader.ReadDouble();
            
            BinReader.Close();
        }
    }
}
