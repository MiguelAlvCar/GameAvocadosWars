using FirstWindows.Controller;
using BasicElements;
using Ninject;
using BasicElements.AbstractServerInternetCommunication;

namespace FirstWindows.View.Start
{
    public class EditUserModel
    {
        public EditUserModel()
        {
            IInternetCommunicationMainModelView mainMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            Name = mainMV.LoggedPlayer.Player.Name;
            Email = mainMV.LoggedPlayer.Player.Email;
        }
        public string Name { set; get; }
        public string Email { set; get; }
        public string PropertyForPasswordBinding { set; get; }
    }
}
