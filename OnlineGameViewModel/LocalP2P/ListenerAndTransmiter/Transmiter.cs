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
using ModelGame.Abstract;
using BasicElements;
using System.Diagnostics;
using System.Threading;

namespace ViewModelOnlineGame.P2PCommunication
{
    public class Transmiter: AbstractOnlineGameTransmiter
    {
        public override IOnlineGameEncoder OnlineGameEncoder { 
            set => OnlineHallEncoder = value; 
            get => (IOnlineGameEncoder)OnlineHallEncoder; }
        private ClientConnectionBase _ClientConnection;
        public Transmiter(string addresse, int port, int maintainConnectionPoolingMiliseconds)
        {
            Stopwatch timer = new Stopwatch();
            try
            {
                _ClientConnection = new ClientConnectionBase();
                OnlineGameEncoder = new Encoder(this, maintainConnectionPoolingMiliseconds);
                IPAddress ipA = IPAddress.Parse(addresse);
                IPEndPoint ipEP = new IPEndPoint(ipA, port);
                _ClientConnection.Client = new TcpClient();
                //BasicMechanisms.Log("Connecting transmitter, addresse: " + addresse + "; port: " + port);
                timer.Start();
                _ClientConnection.Client.Connect(ipEP);
                // BasicMechanisms.Log("Transmitter connected");
                _ClientConnection.Stream = _ClientConnection.Client.GetStream();
            }
            catch (Exception e)
            {
                timer.Stop();
                //BasicMechanisms.Log("Transmitter exception, time: " + timer.ElapsedMilliseconds + " ms");
                throw e;
            }
            
        }

        public override void Write(byte method, MemoryStream stream)
        {
            if (IsClosed)
            {
                return;
            }
            MemoryStream stream1 = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(stream1);
            binWriter.Write(method);
            binWriter.Write((int)stream.Length);

            stream.Seek(0, SeekOrigin.Begin);
            byte[] buffer1 = new byte[stream.Length];
            stream.Read(buffer1, 0, (int)stream.Length);
            stream1.Write(buffer1, 0, buffer1.Length);
            byte[] buffer = stream1.ToArray();
            _ClientConnection.Client.SendBufferSize = buffer1.Length;
            try
            {
                //BasicMechanisms.Log("Before Network stream write");
                //BasicMechanisms.ReadBuffer(buffer1);
                _ClientConnection.Stream.Write(buffer, 0, buffer.Length);
                //BasicMechanisms.Log("After Network stream write");
            }
            catch (IOException e)
            {
                //Throw exception when exception code not the same
            }
        }

        bool IsClosed = false;

        // 127.0.0.0
        // port 6112
        public override void Close()
        {
            IsClosed = true;
            OnlineHallEncoder.aTimer.Stop();
            OnlineHallEncoder.aTimer.Dispose();
            //BasicMechanisms.Log("Transmitter closed");
            _ClientConnection.Dispose();
        }
    }
}
