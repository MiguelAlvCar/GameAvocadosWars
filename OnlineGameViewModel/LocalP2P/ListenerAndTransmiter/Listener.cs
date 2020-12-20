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
    public class Listener: AbstractOnlineGameListener
    {
        public override IOnlineGameDecoder OnlineGameDecoder { 
            get => (IOnlineGameDecoder)OnlineHallDecoder; 
            set => OnlineHallDecoder = value; }


        private ClientConnectionBase _ClientConnection;

        public Listener(int maintainConnectionPoolingMiliseconds)
        {
            _ClientConnection = new ClientConnectionBase();
            OnlineGameDecoder = new Decoder(maintainConnectionPoolingMiliseconds);
        }

        public override async void Listen()
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
            }
            //BasicMechanisms.Log("Listener is listening");
            _ClientConnection.Client = await _TcpListener.AcceptTcpClientAsync();
            _ClientConnection.Client.ReceiveBufferSize = 500000;
            _ClientConnection.Stream = _ClientConnection.Client.GetStream();

            bool WithoutException = true;
            while (WithoutException)
            {
                await ReadNetStream();
                
            }

            Task ReadNetStream()
            {
                Task task = Task.Run(() => {
                    try
                    {
                        var memStream = new MemoryStream();

                        BinaryReader binReader = new BinaryReader(_ClientConnection.Stream);
                        byte method = binReader.ReadByte();
                        int streamLenght = binReader.ReadInt32();

                        if (streamLenght > 0)
                        {
                            int position = 0;
                            byte[] buffer = new byte[streamLenght];

                            while (position < streamLenght)
                            {                                
                                int bytesRead = _ClientConnection.Stream.Read(buffer, position, streamLenght - position);
                                position += bytesRead;
                                if (position < streamLenght)
                                {

                                }                                
                            }
                            memStream.Write(buffer, 0, streamLenght);
                            memStream.Seek(0, SeekOrigin.Begin);
                        }
                        else
                        {

                        }

                        //BasicMechanisms.Log("Listener is decoding");
                        OnlineGameDecoder.Decode(method, memStream);

                    }
                    catch (ObjectDisposedException e)
                    {
                        //BasicMechanisms.Log("Listener has an exception: " + e.Message);
                        WithoutException = false;
                        Dispose();
                    }
                    catch (IOException e)
                    {
                        //BasicMechanisms.Log("Listener has an exception: " + e.Message);
                        WithoutException = false;
                        Dispose();
                        if (e.InnerException is SocketException && ((SocketException)e.InnerException).ErrorCode == 10054)
                        {
                            
                        }
                        
                    }
                    catch (ArgumentException e)
                    {
                        WithoutException = false;
                        Dispose();
                    }

                });
                if (!WithoutException)
                    return null;
                return task;
            }
        }

        public override void Close()
        {
            _TcpListener.Stop();
            _ClientConnection.Dispose();
        }
    }
    
}
