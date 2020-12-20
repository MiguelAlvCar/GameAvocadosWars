using System;
using System.Collections.Generic;
using System.Text;
using WpfBasicElements;
using BasicElements.AbstractClasses;

namespace WpfBasicElements.AbstractClasses
{
    public abstract class StartLogicAbstractInteceptor
    {
        protected Action InitializeAppli;
        protected Action InitializeWindow;
        protected IPrincipalWindow Window;
        public StartLogicAbstractInteceptor(IPrincipalWindow window, ActionWrapperForNinject initializeAppli, ActionWrapperForNinject initializeWindow)
        {
            InitializeWindow = initializeWindow.Action;
            InitializeAppli = initializeAppli.Action;
            Window = window;
        }

        public abstract void Start();
    }
}
