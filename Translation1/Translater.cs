using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Controls.Primitives;
using FirstWindows.ViewModel;
using BasicElements;
using FirstWindows.View.HotSeat;

namespace Translation
{
    public static class Translater
    {
        public static Language Language { set; get; }

        public static T ÜbersetzungMeth<T>(T translationList, Action MethodAfterTranslation = null) where T : TranslationList
        {
            Language language = Translater.Language;
            switch (language)
            {
                case BasicElements.Language.Spanish:
                    foreach (TranslationType Über in translationList.List)
                    {
                        Übersetzung1(Über.Control, Über.Spanish);
                    }

                    break;
                case BasicElements.Language.English:
                    foreach (TranslationType Über in translationList.List)
                    {
                        Übersetzung1(Über.Control, Über.English);
                    }
                    break;
                case BasicElements.Language.German:
                    foreach (TranslationType Über in translationList.List)
                    {
                        Übersetzung1(Über.Control, Über.German);
                    }
                    break;
            }

            if (MethodAfterTranslation != null)
                MethodAfterTranslation();

            return translationList;

            void Übersetzung1(object Kontroll, string Text)
            {
                if (Kontroll is ButtonBase)
                {
                    ButtonBase con = Kontroll as ButtonBase;
                    con.Content = Text;
                }
                else if (Kontroll is HeaderedContentControl)
                {
                    HeaderedContentControl con = Kontroll as HeaderedContentControl;
                    con.Header = Text;
                }
                else if (Kontroll is TextBox)
                {
                    TextBox textbox = Kontroll as TextBox;
                    textbox.Text = Text;
                }
                else if (Kontroll is TextBlock)
                {
                    TextBlock textbox = Kontroll as TextBlock;
                    textbox.Text = Text;
                }
                else if (Kontroll is ComboBoxItem)
                {
                    ComboBoxItem con = Kontroll as ComboBoxItem;
                    con.Content = Text;
                }
                else if (Kontroll is StringBuilder)
                {
                    StringBuilder con = Kontroll as StringBuilder;
                    con.Remove(0, con.Length);
                    con.Append(Text);
                }
                else if (Kontroll is ItemDictionaryArmiesViewModel)
                {
                    ItemDictionaryArmiesViewModel army = Kontroll as ItemDictionaryArmiesViewModel;
                    army.TranslatableName = new StringBuilder(Text);
                }
                else if (Kontroll is ItemListUnitViewModel)
                {
                    ItemListUnitViewModel unit = Kontroll as ItemListUnitViewModel;
                    unit.TranslatableName = new StringBuilder(Text);
                }
                else if (Kontroll is TranslationElement)
                {
                    TranslationElement element = Kontroll as TranslationElement;
                    element.TranslatablePhrare = new StringBuilder(Text);
                }
            }
        }
    }

    public abstract class TranslationList
    {
        public static List<TranslationType> dinamicList;

        public List<TranslationType> List { get; set; }
    }
}
