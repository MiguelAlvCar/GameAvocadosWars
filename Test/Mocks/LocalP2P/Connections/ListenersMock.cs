using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel.Design;
using System.Diagnostics;
using BasicElements.AbstractClasses;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using FirstWindows.View.OnlineHall;
using ModelGame.Abstract;
using ViewModelOnlineGame.P2PCommunication;

namespace Test.LocalP2P
{
    public class ListenerMock: AbstractOnlineGameListener
    {
        static public bool IsConnectionDisrupted = true;
        static public bool DoesAdversaryGo = true;

        public override IOnlineGameDecoder OnlineGameDecoder
        {
            get => (IOnlineGameDecoder)OnlineHallDecoder;
            set => OnlineHallDecoder = value;
        }
        public ListenerMock(int maintainConnectionPoolingMiliseconds)
        {
            OnlineGameDecoder = new DecoderMock(maintainConnectionPoolingMiliseconds);
        }

        public override void Listen()
        {
            System.Timers.Timer aTimer1 = new System.Timers.Timer();
            aTimer1.Elapsed += (a, b) =>
            {
                //MemoryStream stream;
                //byte[] buffer;

                AbstractDecode(P2PMethod.NewChatMessage);

                if (IsConnectionDisrupted)
                    Thread.Sleep(/*8000*/1500);

                AbstractDecode(P2PMethod.ChangeDescription);

                Thread.Sleep(1500);


                AbstractDecode(P2PMethod.ToMap);

                #region InMap

                Thread.Sleep(2000);

                AbstractDecode(P2PMethod.NewChatMessage);

                Thread.Sleep(1000);

                AbstractDecode(P2PMethod.ChangeInitialValues);

                Thread.Sleep(1000);

                AbstractDecode(P2PMethod.AdjustWidthAndLenghMap);

                #endregion


                #region Terrains

                Thread.Sleep(500);

                AbstractDecode(P2PMethod.NewTerrain);

                Thread.Sleep(500);

                AbstractDecode(P2PMethod.AddRiverEnd);

                Thread.Sleep(300);

                AbstractDecode(P2PMethod.AddRiverEnd);

                Thread.Sleep(300);

                AbstractDecode(P2PMethod.AddRiverEnd);

                Thread.Sleep(300);

                AbstractDecode(P2PMethod.AddRiverEnd);

                Thread.Sleep(500);

                AbstractDecode(P2PMethod.AddBridge);

                Thread.Sleep(500);

                AbstractDecode(P2PMethod.ClearRiverEnds);

                Thread.Sleep(500);

                AbstractDecode(P2PMethod.AddDeploymentArea);

                Thread.Sleep(1000);

                AbstractDecode(P2PMethod.ToDeployment);

                Thread.Sleep(8000);

                AbstractDecode(P2PMethod.ToBattle);

                Thread.Sleep(3000);

                AbstractDecode(P2PMethod.ToBattle);

                Thread.Sleep(3000);

                AbstractDecode(P2PMethod.NewMovement);

                Thread.Sleep(3000);

                AbstractDecode(P2PMethod.NextTurn);
                OnlineHallPage.InstanceForDebugger.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Stream stream = new MemoryStream();
                    OnlineGameDecoder.Decode(255, stream);
                });

                Thread.Sleep(5000);

                AbstractDecode(P2PMethod.Victory);

                Thread.Sleep(1000);

                #endregion


                if (DoesAdversaryGo)
                {
                    Thread.Sleep(2000);
                    AbstractDecode(P2PMethod.GoFromGame);
                }

                void AbstractDecode(P2PMethod method)
                {
                    OnlineHallPage.InstanceForDebugger.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        Stream stream = new MemoryStream();
                        OnlineGameDecoder.Decode((byte)method, stream);
                    });
                }

            };
            aTimer1.Interval = 1500;
            aTimer1.AutoReset = false; ;
            aTimer1.Start();
        }
        public override void Close() { }

        
    }


    public class OpenedListenerMock : IOpenedListener
    {
        public OpenedListenerMock() 
        {
            OnlineHallDecoder = new DecoderMock();
        }

        public override void Listen()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += (a, b) =>
            {
                OnlineHallPage.InstanceForDebugger.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    OnlineHallPage.InstanceForDebugger.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        MemoryStream stream = new MemoryStream();
                        OnlineHallDecoder.Decode((byte)P2PMethod.ConnectIPAndPlayer, stream);
                    });
                });
                
            };
            aTimer.Interval = 1000;
            aTimer.Enabled = true;
            aTimer.AutoReset = false;
        }
        public override void Close() { }
    }
}


