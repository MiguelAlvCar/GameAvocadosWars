using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using WpfBasicElements;
using Translation;
using Styles.CommonPages;
using WpfBasicElements.AbstractClasses;
using Ninject;
using Ninject.Parameters;
using BasicElements;
using BasicElements.AbstractClasses;
using WpfBasicElements.AbstractInterceptors;

namespace SaveLoad
{
    /// <summary>
    /// Interaktionslogik für Speichern.xaml
    /// </summary>
    public partial class SaveLoadPage : Page
    {
        private SaveLoadViewModel _ModelView;
        public SaveLoadViewModel SaveLoadViewModel
        {
            set
            {
                _ModelView = value;
                value.CloseSaveLoad += CloseSaveLoad;
                value.PopUpPleaseName += () =>
                {
                    WPopup pop = new WPopup(TranslationList.BitteNameDatei.ToString());
                    SaveLoadFrame.Navigate(pop);
                };
                value.OverwriteBySavingConfirmation += (path, isWorkingWithMap, method) => 
                {
                    object[] para = {path, isWorkingWithMap };
                    Action<object[]> yesNotMethod = delegate (object[] parameter) { method((string)parameter[0], (bool)parameter[1]); };
                    PopupYesNo yesno = new PopupYesNo(TranslationList.Bestaetigen.ToString(), TranslationList.Abbrechen.ToString(),
                        TranslationList.SicherUeberschreiben.ToString(), yesNotMethod, para);
                    SaveLoadFrame.Navigate(yesno);
                };
                value.PopUpConfirmationDelete += (method, para) =>
                {
                    PopupYesNo yesno = new PopupYesNo(TranslationList.Bestaetigen.ToString(), TranslationList.Abbrechen.ToString(),
                        TranslationList.SicherDelete.ToString(), method, para);
                    SaveLoadFrame.Navigate(yesno);

                };

            }
            get
            {
                return _ModelView;
            }
        }

        public SaveList TranslationList;

        public SaveLoadPage(string title, bool isWorkingWithMap, bool isSaving, ISaveViewGame hotSeat)
        {
            HotSeat = hotSeat;
            IHotSeatViewModel hotSeatViewModel = null;
            if (HotSeat != null)
                hotSeatViewModel = HotSeat.IViewModel;
            SaveLoadViewModel saveLoadViewModel = new SaveLoadViewModel(hotSeatViewModel);
            saveLoadViewModel.NotificateException += (str) => SaveLoadFrame.Navigate(new WPopup(str));
            DataContext = saveLoadViewModel;
            SaveLoadViewModel = saveLoadViewModel;
            SaveLoadViewModel.TitelMV = title;
            SaveLoadViewModel.IsWorkingWithMap = isWorkingWithMap;
            SaveLoadViewModel.IsSaving = isSaving;

            InitializeComponent();

            SolidColorBrush color = new SolidColorBrush();
            color.Color = Colors.Black;
            color.Opacity = 0.5;
            this.Background = color;

            Titel.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            ListDateien.GetBindingExpression(ListView.ItemsSourceProperty).UpdateTarget();

            if (!isSaving)
                Textbox.Visibility = Visibility.Collapsed;

            TranslationList = Translater.ÜbersetzungMeth(new SaveList(this)/*, BinderUpdater.UpdateTranslationHotSeat*/);
        }

        ISaveViewGame HotSeat;

        private void Abbrechen_Click(object sender, RoutedEventArgs e)
        {
            if (HotSeat != null)
            {
                NavigationService.Navigate(null);
            }
            else
            {
                BasicMechanisms.Kernel.Get<ISaveLoadInterceptor>().ReturnFirstWindow(this);
            }
        }

        public void CloseSaveLoad(IHotSeatViewModel hotSeatModelView)
        {
            if (HotSeat != null)
            {
                if (hotSeatModelView == null)
                {
                    NavigationService.Navigate(null);
                }
                else
                {
                    ISaveViewGame hotseat = BasicMechanisms.Kernel.Get<ISaveViewGame>(
                        new[] {
                        new ConstructorArgument ("modelView", hotSeatModelView) });
                    SaveLoadViewModel.OnAfterDeserialization(hotseat);
                    ((IPrincipalWindow)Window.GetWindow(this)).MainFrame.Navigate(hotseat);
                }
            }                
            else
            {
                if (hotSeatModelView == null)
                {
                    BasicMechanisms.Kernel.Get<ISaveLoadInterceptor>().ReturnFirstWindow(this);
                }
                else
                {
                    ISaveViewGame hotseat = BasicMechanisms.Kernel.Get<ISaveViewGame>(
                        new[] {
                        new ConstructorArgument ("modelView", hotSeatModelView) });
                    SaveLoadViewModel.OnAfterDeserialization(hotseat);
                    ((IPrincipalWindow)Window.GetWindow(this)).MainFrame.Navigate(hotseat);
                }
                
            }
        }
    }
}
