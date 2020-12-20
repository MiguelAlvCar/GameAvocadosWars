using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using BasicElements;
using BasicElements.AbstractClasses;

namespace WpfBasicElements.AbstractClasses
{
    public interface ISaveViewGame
    {
        void Translate();
        IHotSeatViewModel IViewModel { set; get; }
    }
}
