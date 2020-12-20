using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Runtime.Serialization;
using FirstWindows.View.InitialDialog;
using Armies;
using FirstWindows.View.Translation;
using BasicElements;
using BasicElements.ViewModel;
using WpfBasicElements;
using FirstWindows.Controller;
using ArmyAndUnitTypes;
using System.Collections.ObjectModel;
using Ninject;
using WpfBasicElements.AbstractClasses;
using Ninject.Parameters;
using BasicElements.AbstractClasses;

namespace Initialization
{    
    public partial class StartWindow : Window, IPrincipalWindow
    {
        private Frame _MainFrame;
        public Frame MainFrame { 
            get {

                if (!_MainFrame.CanGoBack && !_MainFrame.CanGoForward)
                {
                    return _MainFrame;
                }
                var entry = _MainFrame.RemoveBackEntry();
                while (entry != null)
                {
                    entry = _MainFrame.RemoveBackEntry();
                }

                return _MainFrame;
            } 
            set => _MainFrame = value; 
        }

        public StartWindow()
        {
            MainFrame = new Frame();
            MainFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            this.Content = MainFrame;

            IDResolver.Kernel.Get<StartLogicAbstractInteceptor>(
                new[] {
                new ConstructorArgument ("window", this),
                new ConstructorArgument ("initializeAppli",new  ActionWrapperForNinject(Initialize)),
                new ConstructorArgument ("initializeWindow",new  ActionWrapperForNinject( 
                    () => InitializeComponent()   
                )) }).Start();
        }

        private static void Initialize()
        {
            BasicMechanisms.Kernel = IDResolver.Kernel;

            BasicMechanisms.Kernel.Get<IListArmiesMainModelView>().listArmiesObSave = new ObservableCollection<ArmyType>(new ArmiesData().ArmyList);

            BasicMechanisms.Test = Properties.Settings.Default.InternetConnectionTest;
        }
    }
}
