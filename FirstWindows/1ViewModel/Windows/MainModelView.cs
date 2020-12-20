using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.ObjectModel;
using BasicElements.ViewModel;
using ArmyAndUnitTypes;
using BasicElements.AbstractServerInternetCommunication;

namespace FirstWindows.Controller
{
    public class MainModelView: IInternetCommunicationMainModelView, IListArmiesMainModelView
    {
        public static MainModelView Instance { get { return Nested.Instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }
            internal static readonly MainModelView Instance = new MainModelView();
        }

        protected MainModelView() { }

        public UserPlayer LoggedPlayer { set; get;}

        public ObservableCollection<ArmyType> listArmiesObSave { set; get; }
    }
}
