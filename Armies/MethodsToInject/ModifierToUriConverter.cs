using System;
using System.Collections.Generic;
using System.Text;
using BasicElements;
using BasicElements.AbstractForArmyInterceptor;

namespace Armies.TemplateMethods
{
    public class ModifierToUriConverter: IModifierToUriConverter
    {
        public Uri GetUri(Modifier modifier) 
        {
            Uri uri = null;
            switch (modifier)
            {
                case Modifier.Charge:
                    uri = new Uri("pack://siteoforigin:,,,/Units/Images/Modifiers/Charge.jpg");
                    break;
                case Modifier.Flight:
                    uri = new Uri("pack://siteoforigin:,,,/Units/Images/Modifiers/Flight.jpg");
                    break;
                case Modifier.Formation:
                    uri = new Uri("pack://siteoforigin:,,,/Units/Images/Modifiers/Formation.jpg");
                    break;
                case Modifier.Fright:
                    uri = new Uri("pack://siteoforigin:,,,/Units/Images/Modifiers/Fright.jpg");
                    break;
                case Modifier.Rage:
                    uri = new Uri("pack://siteoforigin:,,,/Units/Images/Modifiers/Rage.jpg");
                    break;
            }
            return uri;
        }
    }
}
