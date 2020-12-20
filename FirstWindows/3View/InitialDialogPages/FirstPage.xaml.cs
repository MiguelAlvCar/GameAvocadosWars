
using System.Windows.Controls;
using GameFrontEnd.View;
using FirstWindows.View.Translation;
using Translation;
using Sound;
using BasicElements;
using Ninject;
using WpfBasicElements.AbstractClasses;
using System.Windows;
using Styles.CommonPages;



namespace FirstWindows.View.InitialDialog
{
    /// <summary>
    /// Interaktionslogik für FirstWindow.xaml
    /// </summary>
    public partial class FirstPage : Page
    {
        ListVideo TranslationList;
        public FirstPage()
        {
            DatenDat.OpenDatenDat();

            TranslationList = Translater.ÜbersetzungMeth(new ListVideo()/*, BinderUpdater.UpdateTranslationHotSeat*/);

            InitializeComponent();

            BasicMechanisms.Kernel.Get<IMusic>().Load("\\3View\\Resources\\Music", (exception) => {
                WPopup pop = new WPopup(TranslationList.NoMusik.ToString() + "\n" + exception.Message);
                this.FirstFrame.Navigate(pop);
            });

            this.Loaded += (sender, e) =>
            {
                Window.GetWindow(this).Background = GetCommonElements.GetWallpaper();
            };
            
        }
    }
}
