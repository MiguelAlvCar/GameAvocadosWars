using System;
using System.Collections.Generic;
using DTO_Models;

namespace BasicElements.AbstractServerInternetCommunication
{
    public interface ICommunicationWithServer
    {
        bool InternetLogin(LoginModel details, out IEnumerable<OnlineGameDTO> gamesPool, out bool connectionSucceeded);
        bool EditUser(EditUserDTO editUserDTO);
        int PostHostGame(OnlineGameDTO onlineGameDTO);
        IEnumerable<OnlineGameDTO> GetPoolGames(PlayerDTO loggedPlayer);
        bool DeleteGame(int currentGameID);
        bool PutHostGame(OnlineGameDTO onlineGameDTO);
        void MaintainConnection(int currentGameID);
        void Victory(BattleDTO battleDTO);
    }
}
