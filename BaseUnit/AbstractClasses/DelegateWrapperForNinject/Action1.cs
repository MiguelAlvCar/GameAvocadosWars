using System;
using System.Collections.Generic;
using System.Text;

namespace BasicElements.AbstractClasses
{
    public class Action1WrapperForNinject<T>
    {
        public Action1WrapperForNinject(Action<T> action1)
        {
            Action1 = action1;
        }

        public Action<T> Action1 { get; set; }
    }
}
