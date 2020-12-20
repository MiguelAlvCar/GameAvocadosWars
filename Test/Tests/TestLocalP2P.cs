using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModelOnlineGame.P2PCommunication;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Timers;
using System.Threading.Tasks;
using DTO_Models;

namespace Test
{
    [TestClass]
    public class TestLocalP2P
    {
        [TestMethod]
        public void ListenerLoopback()
        {
            Listener listener = new Listener(7500);
            listener.OnlineGameDecoder.ConnectIPAndPlayer += (data) =>
            {
                //TestMethod to debug and to set a red point to get the result after the testmethod is run.
                //ip must be equal to Miguel
            };
            listener.Listen(/*IPAddress.Any*/);
            new Transmiter("127.0.0.1", 6112, 3000).OnlineGameEncoder.ConnectIPAndPlayer(new PlayerDTO("Miguel", 4.7F, "4"));
        }
    }
}
