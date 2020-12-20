using System;
using System.Collections.Generic;
using System.Text;

namespace BasicElements.AbstractClasses
{
    public class ActionWrapperForNinject
    {
        public Action Action;
        public ActionWrapperForNinject(Action action)
        {
            Action = action;
        }
    }
}
