using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation
{
    public abstract class TranslationList
    {
        public List<TranslationType> List { get; set; }
    }

    public class TranslationType
    {
        public object Control;
        public string Spanish;
        public string German;
        public string English;
    }
}
