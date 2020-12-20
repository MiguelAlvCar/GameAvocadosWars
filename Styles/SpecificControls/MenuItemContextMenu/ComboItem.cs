using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Translation;

namespace Styles.SpecificControls 
{ 
    partial class ComboItem : ResourceDictionary
    {
        public ListComboItem TranslationList;

        public ComboItem()
        {
            TranslationList = Translater.ÜbersetzungMeth(new ListComboItem()/*, BinderUpdater.UpdateTranslationHotSeat*/);
            InitializeComponent();            
        }
        
        void OnOpened(object sender, RoutedEventArgs e)
        {
            ContextMenu contextmenu = sender as ContextMenu;            
            foreach (MenuItem menuitem in contextmenu.Items)
                menuitem.Header = Convert.ToString(TranslationList.DeleteText);
        }
    }

    public class ListComboItem : TranslationList
    {
        public ListComboItem()
        {
            List = new List<TranslationType>()
            {
                new TranslationType{ Control = DeleteText, Spanish = "Eliminar", German ="Löschen", English ="Delete"},

            };
        }

        public StringBuilder DeleteText { set; get; } = new StringBuilder();
    }
}
