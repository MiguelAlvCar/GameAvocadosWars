using System;
using System.Collections.Generic;
using System.Text;
using Ninject;
using System.Globalization;
using System.IO;

namespace BasicElements
{
    public class BasicMechanisms
    {
        public static CultureInfo ConvertLanguageToCulture(Language language)
        {
            CultureInfo culture;
            switch (language)
            {
                case BasicElements.Language.English:
                    culture = new CultureInfo("en-US");
                    break;
                case BasicElements.Language.German:
                    culture = new CultureInfo("de-DE");
                    break;
                case BasicElements.Language.Spanish:
                    culture = new CultureInfo("es-ES");
                    break;
                default:
                    culture = new CultureInfo("en-US");
                    break;
            }
            return culture;
        }

        public static void Log(string message, Exception e = null)
        {
            string CurrentDirectory = System.Environment.CurrentDirectory;
            FileStream filest = new FileStream(CurrentDirectory + "\\Miguel.txt", FileMode.Append);
            BinaryWriter BinWriter = new BinaryWriter(filest);
            //BinWriter.Seek(0, SeekOrigin.Begin);
            BinWriter.Write("\n\n" + message);
            BinWriter.Close();
            filest.Close();
            if (e != null)
                throw e;
        }

        public static void ReadBuffer(byte[] buffer)
        {
            foreach (byte byte1 in buffer)
            {
                BasicMechanisms.Log(byte1.ToString() + "\n");
            }
        }

        

        static public IKernel Kernel { get; set; }
        static public bool Test { set; get; }
    }
}
