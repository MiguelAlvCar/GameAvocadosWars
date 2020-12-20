using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel.Design;
using BasicElements.AbstractClasses;
using ViewModelOnlineGame.P2PCommunication;
using ModelGame.Abstract;

namespace Test.LocalP2P
{
    public class TransmiterMock : AbstractOnlineGameTransmiter
    {
        public override IOnlineGameEncoder OnlineGameEncoder
        {
            set => OnlineHallEncoder = value;
            get => (IOnlineGameEncoder)OnlineHallEncoder;
        }
        private ClientConnectionBase _ClientConnection;
        public TransmiterMock(string addresse, int port, int maintainConnectionPoolingMiliseconds)
        {
            _ClientConnection = new ClientConnectionBase();
            OnlineGameEncoder = new EncoderMock(this, maintainConnectionPoolingMiliseconds);

            if (ListenerMock.IsConnectionDisrupted)
            {
                System.Timers.Timer aTimer1 = new System.Timers.Timer();
                aTimer1.Elapsed += (a, b) =>
                {
                    OnlineGameEncoder.MaintainConnectionPoolingMiliseconds = 8500;
                };
                aTimer1.Interval = 7000;
                aTimer1.AutoReset = false;
                aTimer1.Start();
            }
        }



        public override void Write(byte method, MemoryStream stream)
        {            
            DecoderMock.Instance.Decode(method, stream);
        }

        public override void Close()
        {
            
        }
    }
}
