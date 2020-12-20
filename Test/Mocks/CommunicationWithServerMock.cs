using System;
using System.Collections.Generic;
using DTO_Models;
using System.Net;
using System.IO;
using BasicElements;
using BasicElements.AbstractServerInternetCommunication;
using Ninject;
using System.Net.Sockets;
using System.Linq;

namespace Test
{
    public class CommunicationWithServerMock : ICommunicationWithServer
    {
        public bool DeleteGame(int currentGameID)
        {
            return true;
        }

        public bool EditUser(EditUserDTO editUserDTO)
        {
            return true;
        }

        public IEnumerable<OnlineGameDTO> GetPoolGames(PlayerDTO loggedPlayer)
        {
            List<OnlineGameDTO> OnlineGamesStorage = new List<OnlineGameDTO>();
            OnlineGamesStorage.Add(new OnlineGameDTO(new PlayerDTO("Alberto", 2.2F, "1") { GlobalIP = "192.168.200.86" })
            {
                ID = 1,
                Description = "Una gran batalla",
                CreationTime = new DateTime(2020, 05, 17, 22, 33, 50)
            });
            OnlineGamesStorage.Add(new OnlineGameDTO(new PlayerDTO("Ana", 3.7F, "2") { GlobalIP = "192.168.200.87" })
            {
                ID = 2,
                Description = "Una batalla pequennita",
                CreationTime = new DateTime(2020, 05, 17, 21, 25, 33),
            });
            OnlineGamesStorage.Add(new OnlineGameDTO(new PlayerDTO("Jesus", 1.3F, "3"))
            {
                ID = 3,
                Description = "Otra batalla",
                CreationTime = new DateTime(2020, 05, 17, 23, 15, 47)
            });
            return OnlineGamesStorage;
        }

        public bool InternetLogin(LoginModel details, out IEnumerable<OnlineGameDTO> gamesPool, out bool connectionSucceeded)
        {
            IInternetCommunicationMainModelView firstMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();

            firstMV.LoggedPlayer = new UserPlayer(new PlayerDTO("Miguel", 4.7F, "4"));
            //IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress IPadress = host.AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            //if (IPadress == null)
            //    IPadress = host.AddressList.FirstOrDefault();

            //firstMV.LoggedPlayer.Player.LocalIPs = IPadress.ToString();

            List<OnlineGameDTO> OnlineGamesStorage = new List<OnlineGameDTO>();
            OnlineGamesStorage.Add(new OnlineGameDTO(new PlayerDTO("Alberto", 2.2F, "1"))
            {
                Description = "Una gran batalla",
                CreationTime = new DateTime(2020, 05, 17, 22, 33, 50)
            });
            gamesPool = OnlineGamesStorage;


            connectionSucceeded = true;
            return true;
        }

        public int PostHostGame(OnlineGameDTO onlineGameDTO)
        {
            return 4;
        }

        public bool PutHostGame(OnlineGameDTO onlineGameDTO)
        {
            return true;
        }

        public void MaintainConnection(int currentGameID)
        {
            
        }

        public void Victory(BattleDTO battleDTO)
        {

        }
    }
}
