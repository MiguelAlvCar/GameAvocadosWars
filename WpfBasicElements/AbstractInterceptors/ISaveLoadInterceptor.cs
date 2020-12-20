using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfBasicElements.AbstractInterceptors
{
    public interface ISaveLoadInterceptor
    {
        void ReturnFirstWindow(Page page);
    }
}
