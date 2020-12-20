using DTO_Models;
using System;
using System.IO;

namespace BasicElements.AbstractClasses
{
    public interface IOnlineHallEncoder
    {
        // 7500 and 10500 Milise
        System.Timers.Timer aTimer { set; get; }
        int MaintainConnectionPoolingMiliseconds { get; set; }

        void ConnectIPAndPlayer(PlayerDTO player);
        void NewChatMessage(string message);
        void ToMap(bool isConfirmed, bool hasAdversaryConfirmed, string ID);
        void GoFromGame();
        void ChangeDescription(string description);
        void MaintainP2PConnection();
    }

    public interface IOnlineHallDecoder
    {
        // 3000 and 4000 Milise
        int MaintainConnectionPoolingMiliseconds { get; set; }

        event Action<PlayerDTO> ConnectIPAndPlayer;
        event Action<string> NewChatMessage;
        event Action<bool, bool, string> ToMap;
        event Action GoFromGame;
        event Action<string> ChangeDescription;
        event Action MaintainP2PConnection;
        void Decode(byte method, Stream stream);
    }


}