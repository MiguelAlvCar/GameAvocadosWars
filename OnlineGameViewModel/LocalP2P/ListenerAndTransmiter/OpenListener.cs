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
using BasicElements;

namespace ViewModelOnlineGame.P2PCommunication
{
    public class OpenedListener : IOpenedListener
    {
        private ClientConnectionBase _ClientConnection;

        public OpenedListener()
        {
            _ClientConnection = new ClientConnectionBase();
            OnlineHallDecoder = new Decoder();
        }

        public async override void Listen()
        {
            _TcpListener = new TcpListener(IPAddress.Any, 6112);
            try
            {
                _TcpListener.Start();
            }
            catch (SocketException e)
            {
                if (e.ErrorCode != 10048)
                {
                    throw e;
                }
                else
                {
                    //BasicMechanisms.Log("SockedException while starting openedlistener\n");
                    //BasicMechanisms.Log(e.Message + "\n");
                }
            }

            try
            {
                //BasicMechanisms.Log("Opend listener opened");
                _ClientConnection.Client = await _TcpListener.AcceptTcpClientAsync();
                //BasicMechanisms.Log("Opend listener has received data");
                _ClientConnection.Stream = _ClientConnection.Client.GetStream();
                //BasicMechanisms.Log("Opend listener has read stream");

                var memStream = new MemoryStream();

                BinaryReader binReader = new BinaryReader(_ClientConnection.Stream);
                byte method = binReader.ReadByte();
                int streamLenght = binReader.ReadInt32();

                byte[] buffer = new byte[streamLenght];

                _ClientConnection.Stream.Read(buffer, 0, streamLenght);
                memStream.Write(buffer, 0, streamLenght);
                memStream.Seek(0, SeekOrigin.Begin);

                //BasicMechanisms.Log("OpenListener is decoding");

                //BasicMechanisms.ReadBuffer(buffer);
                OnlineHallDecoder.Decode(method, memStream);
            }
            catch (ObjectDisposedException)
            {

            }
            catch (Exception e)
            {
                //BasicMechanisms.Log("Exception while listenig in opened listener: " + e.Message);
            }

        }

        public override void Close()
        {
            //BasicMechanisms.Log("Opend listener closed");
            _TcpListener.Server.LingerState.Enabled = false;
            _TcpListener.Stop();
            _ClientConnection.Dispose();
        }
    }
}
