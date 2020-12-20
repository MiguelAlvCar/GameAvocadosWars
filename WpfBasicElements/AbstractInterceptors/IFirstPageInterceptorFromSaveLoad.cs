using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using WpfBasicElements.AbstractClasses;

namespace WpfBasicElements.AbstractInterceptors
{
    public interface IFirstPageInterceptorFromSaveLoad
    {
        void NavigateToSaveLoadPage(NavigationService nagivationService, string title, bool isWorkingWithMap, bool isSaving, ISaveViewGame hotSeat);
    }
}
