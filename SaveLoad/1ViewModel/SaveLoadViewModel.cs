using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.IO;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using Translation;
using WpfBasicElements;
using WpfBasicElements.AbstractClasses;
using BasicElements.AbstractClasses;
using BasicElements;
using Ninject;
using Ninject.Parameters;

namespace SaveLoad
{
    public class SaveLoadViewModel
    {
        string CurrentDirectory;
        int PathLenght;
        string _DirectoryPath;

        public ObservableCollection<SaveFile> ListeGespeicherte { get; set; } = new ObservableCollection<SaveFile>();
        private bool _IsWorkingWithMap;
        public bool IsWorkingWithMap
        {
            get => _IsWorkingWithMap;
            set
            {
                _IsWorkingWithMap = value;
                CurrentDirectory = System.Environment.CurrentDirectory;
                _DirectoryPath = value ? CurrentDirectory + "\\Karten" : CurrentDirectory + "\\Spiele";
                PathLenght = _DirectoryPath.Length;

                culture = BasicMechanisms.ConvertLanguageToCulture(Translater.Language);

                string[] savePaths = Directory.GetFileSystemEntries(_DirectoryPath);

                List<SaveFile> ListeGespeicherte1 = new List<SaveFile>();
                foreach (string path in savePaths)
                {
                    if (Convert.ToString(System.IO.Path.GetExtension(path)) == ".spl")
                    {
                        FileInfo file = new FileInfo(path);
                        DateTime time = file.LastWriteTime;
                        string timeString = String.Format(culture, "{0:f}", time);
                        string name = System.IO.Path.GetFileNameWithoutExtension(path);
                        SaveFile saveFile = new SaveFile() { datetime = time, Name = name, DateString = timeString };
                        ListeGespeicherte1.Add(saveFile);
                    }
                }

                ListeGespeicherte = new ObservableCollection<SaveFile>(ListeGespeicherte1.OrderBy(p => p.datetime));
            }
        }
        private bool _IsSaving;
        public bool IsSaving
        {
            get => _IsSaving;
            set
            {
                _IsSaving = value;
            }
        }
        public string TitelMV { set; get; }
        public string NameGame { set; get; }
        public bool FromFirstWindow { set; get; }

        CultureInfo culture;

        IHotSeatViewModel _HotSeatViewModel;

        SaveLoadVMList TranslationList;

        public SaveLoadViewModel(IHotSeatViewModel hotSeatViewModel)
        {
            _HotSeatViewModel = hotSeatViewModel;
            Confirm = new RelayCommand(Confirm_Execute, Confirm_CanExecute);
            Delete = new RelayCommand(Delete_Execute, Delete_CanExecute);

            TranslationList = Translater.ÜbersetzungMeth(new SaveLoadVMList());
        }

        public event Action<string, bool, Action<string, bool>> OverwriteBySavingConfirmation;
        public event Action<Action<object[]>, object[]> PopUpConfirmationDelete;
        public event Action<IHotSeatViewModel> CloseSaveLoad;
        public event Action PopUpPleaseName;

        public void OnClose()
        {
            CloseSaveLoad(null);
        }

        public event Action <string> NotificateException;
        public ICommand Confirm { get; set; }
        private bool Confirm_CanExecute(object obj)
        {
            return true;
        }
        private void Confirm_Execute(object obj)
        {
            if (NameGame.Trim() == "")
            {
                if (!IsSaving)
                {
                    CloseSaveLoad(null);                    
                    return;
                }
                else
                {
                    PopUpPleaseName();
                    return;
                }                
            }
            else
            {
                IModelGame game1 = null;
                if (_HotSeatViewModel != null)
                    game1 = _HotSeatViewModel.IGame;
                DateiReader dateiReader = new DateiReader(this, game1);
                if (IsSaving)
                {
                    foreach (SaveFile datei in ListeGespeicherte)
                    {
                        if (NameGame == datei.Name)
                        {                            
                            OverwriteBySavingConfirmation(_DirectoryPath + "\\" + NameGame + ".spl", IsWorkingWithMap, dateiReader.InDateiSchreiben);                            
                            return;
                        }
                    }
                    try
                    {
                        dateiReader.InDateiSchreiben(_DirectoryPath + "\\" + NameGame + ".spl", IsWorkingWithMap);
                    }
                    catch (SerializationException e)
                    {
                        NotificateException(TranslationList.NoSave.ToString() + "\n" + e.Message);
                    }
                    catch (ArgumentException e)
                    {
                        NotificateException(TranslationList.NoSave.ToString() + "\n" + e.Message);
                    }
                }
                else
                {
                    try
                    {
                        IModelGame game = dateiReader.VonDateiLesen(_DirectoryPath + "\\" + NameGame + ".spl", IsWorkingWithMap);
                        CloseSaveLoad(BasicMechanisms.Kernel.Get<IHotSeatViewModel>(
                            new[] {
                            new ConstructorArgument ("game", game) }
                        ));
                    }
                    catch (SerializationException e)
                    {
                        NotificateException(TranslationList.NoLoad.ToString() + "\n" + e.Message);
                    }
                }
            }
        }

        public void OnAfterDeserialization(ISaveViewGame hotseat)
        {
            hotseat.IViewModel.IGame.AfterDeserialization(IsWorkingWithMap);
        }

        public ICommand Delete { get; set; }
        private bool Delete_CanExecute(object obj)
        {
            return true;
        }
        private void Delete_Execute(object obj)
        {
            void delete(object[] para1)
            {
                File.Delete((string)para1[0]);

                for (int i = 0; i < ListeGespeicherte.Count; i++)
                {
                    SaveFile datei = (SaveFile)ListeGespeicherte[i];
                    if (datei.Name == Path.GetFileNameWithoutExtension((string)para1[0]))
                    {
                        ListeGespeicherte.Remove(ListeGespeicherte[i]);
                        break;
                    }
                }
            }
            object[] para = new object[] { _DirectoryPath + "\\" + NameGame + ".spl"};

            PopUpConfirmationDelete(delete, para);
        }
    }

    public class SaveFile
    {
        public DateTime datetime { set; get; }
        public string Name { set; get; }
        public string DateString { set; get; }
    }
}
