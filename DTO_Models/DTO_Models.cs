using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace DTO_Models
{
    public class LoginDTO
    {
        public LoginDTO (bool loggedIn)
        {
            LoggedIn = loggedIn;
        }
        public bool LoggedIn { set; get; }
        public PlayerDTO User { set; get; }
        public IEnumerable<OnlineGameDTO> GamesPool { set; get; }

    }


    public class PlayerDTO: ICloneable
    {
        public PlayerDTO() { }
        public PlayerDTO(string name, double ability, string id)
        {
            ID = id;
            Name = name;
            Ability = ability;
        }
        public string Email { set; get; }
        public string ID { set; get; }
        public string Name { set; get; }
        public double Ability { set; get; }
        public int Battles { set; get; }
        public int WonBattles { set; get; }
        public string GlobalIP { set; get; }
        public ICollection<LocalIPDTO> LocalIPs { set; get; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class LocalIPDTO
    {
        public int ID { set; get; }
        public string IP { set; get; }
    }

    public class OnlineGameDTO: ICloneable
    {
        public OnlineGameDTO() { }
        public OnlineGameDTO(PlayerDTO host)
        {
            Host = host;
        }

        public int ID { set; get; }

        public PlayerDTO Host { set; get; }
        public PlayerDTO Guest { set; get; }

        public string Description { set; get; }
        public DateTime CreationTime { set; get; }

        public object Clone()
        {
            OnlineGameDTO onlineGame = (OnlineGameDTO)MemberwiseClone();
            if (Host != null)
                onlineGame.Host = (PlayerDTO)Host.Clone();
            if (Guest != null)
                onlineGame.Guest = (PlayerDTO)Guest.Clone();
            return onlineGame;
        }
    }

    public class EditUserDTO
    {
        public string ID { get; set; }
        public string NewName { get; set; }
        public string NewEmail { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
