using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using FirstWindows.View.InitialDialog;
using BasicElements;
using WpfBasicElements;
using WpfBasicElements.AbstractClasses;
using BasicElements.AbstractClasses;

namespace FirstWindows
{
    public class StartLogic: StartLogicAbstractInteceptor
    {
        public StartLogic (IPrincipalWindow window, ActionWrapperForNinject initializeAppli, ActionWrapperForNinject initializeWindow): base (window, initializeAppli, initializeWindow)
        {
        }

        public override void Start()
        {
            try
            {
                FileStream filest1 = null;
                BinaryReader BinReader = null;
                string CurrentDirectory = System.Environment.CurrentDirectory;

                try
                {
                    if (!Directory.Exists(CurrentDirectory + "\\Karten"))
                        Directory.CreateDirectory(CurrentDirectory + "\\Karten");
                    if (!Directory.Exists(CurrentDirectory + "\\Spiele"))
                        Directory.CreateDirectory(CurrentDirectory + "\\Spiele");
                    filest1 = new FileStream(CurrentDirectory + "\\Daten.dat", FileMode.Open);
                    BinReader = new BinaryReader(filest1);
                    filest1.Seek(0, SeekOrigin.Begin);
                    if (!Enum.IsDefined(typeof(Language), BinReader.ReadByte()))
                        throw new NoLanguageChoosenException();
                    BinReader.Close();
                    InitializeAppli();
                }
                catch (Exception e)
                {
                    if (e is FileNotFoundException || e is NoLanguageChoosenException)
                    {
                        if (!File.Exists(CurrentDirectory + "\\Daten.dat"))
                        {
                            FileInfo DataFile = new FileInfo(CurrentDirectory + "\\Daten.dat");
                            FileStream filestInfo = DataFile.Open(FileMode.CreateNew);
                            DataFile.Attributes = FileAttributes.Hidden;
                            filestInfo.Close();
                        }
                        FileStream filest = new FileStream(CurrentDirectory + "\\Daten.dat", FileMode.Open);
                        BinaryWriter BinWriter = new BinaryWriter(filest);
                        filest.Seek(0, SeekOrigin.Begin);
                        byte Byte = 0;
                        double MusikVolumen = 1;
                        double EffekteVolumen = 1;
                        BinWriter.Write(Byte);
                        BinWriter.Write(MusikVolumen);
                        BinWriter.Write(EffekteVolumen);
                        BinWriter.Close();
                        SpracheWählenWindow neue1 = new SpracheWählenWindow(InitializeAppli);
                        neue1.ShowDialog();
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                string CurrentDirectory = System.Environment.CurrentDirectory;
                string Directory = CurrentDirectory.Substring(0, 2);
                FileStream filest = new FileStream(Directory + "\\Miguel.txt", FileMode.Create);
                BinaryWriter BinWriter = new BinaryWriter(filest);
                BinWriter.Seek(0, SeekOrigin.Begin);
                BinWriter.Write("StartLogik \n Exception: " + e + "\nMessage: " + e.Message + "\nSource: " + e.Source + "\nTarget: " + e.TargetSite + "\nData: " + e.Data);
                BinWriter.Close();
                throw e;
            }

            InitializeWindow();
            Window.MainFrame.Navigate(new VideoPage());
        }
    }

    public class NoLanguageChoosenException : Exception
    {
        public NoLanguageChoosenException() { }
        public NoLanguageChoosenException(string message) : base(message) { }
        public NoLanguageChoosenException(string message, Exception innerException) : base(message, innerException) { }
        protected NoLanguageChoosenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
