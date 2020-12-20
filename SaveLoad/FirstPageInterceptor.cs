using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfBasicElements.AbstractInterceptors;
using System.Windows.Navigation;
using WpfBasicElements.AbstractClasses;

namespace SaveLoad
{
    public class FirstPageInterceptor: IFirstPageInterceptorFromSaveLoad
    {
        public void NavigateToSaveLoadPage(NavigationService nagivationService, string title, bool isWorkingWithMap, bool isSaving, ISaveViewGame hotSeat) 
        {
            SaveLoadPage speichern = new SaveLoadPage(title, isWorkingWithMap, isSaving, hotSeat);
            nagivationService.Navigate(speichern);
        }
    }
}
