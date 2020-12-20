using System;
using System.Collections.Generic;
using DTO_Models;
using System.Net;
using System.IO;
using BasicElements;
using BasicElements.AbstractInternetCommunication;
using Ninject;

namespace InternetCommunicationMock
{
    public class CommunicationWithServerMock : IInternetCommunicationWithServer
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
            List<string> IPsList = new List<string>();
            IPsList.Add("192.168.200.85");
            IPsList.Add("192.168.200.86");
            OnlineGamesStorage.Add(new OnlineGameDTO(new PlayerDTO("Alberto", 2.2, "1") { LocalIPs = IPsList })
            {
                ID = 1,
                Description = "Una gran batalla",
                CreationTime = new DateTime(2020, 05, 17, 22, 33, 50)
            });
            IPsList = new List<string>();
            IPsList.Add("192.168.200.87");
            OnlineGamesStorage.Add(new OnlineGameDTO(new PlayerDTO("Ana", 3.7, "2") { LocalIPs = IPsList })
            {
                ID = 2,
                Description = "Una batalla pequennita",
                CreationTime = new DateTime(2020, 05, 17, 21, 25, 33),
            });
            OnlineGamesStorage.Add(new OnlineGameDTO(new PlayerDTO("Jesus", 1.3, "3"))
            {
                ID = 3,
                Description = "Otra batalla",
                CreationTime = new DateTime(2020, 05, 17, 23, 15, 47)
            });
            return OnlineGamesStorage;
        }

        public bool InternetLogin(LoginModel details, out IEnumerable<OnlineGameDTO> gamesPool)
        {
            IInternetCommunicationMainModelView firstMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            firstMV.LoggedPlayer = new UserPlayer(new PlayerDTO("Miguel", 4.7, "4"));

            List<OnlineGameDTO> OnlineGamesStorage = new List<OnlineGameDTO>();
            OnlineGamesStorage.Add(new OnlineGameDTO(new PlayerDTO("Alberto", 2.2, "1"))
            {
                Description = "Una gran batalla",
                CreationTime = new DateTime(2020, 05, 17, 22, 33, 50)
            });
            gamesPool = OnlineGamesStorage;

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
    }
}
