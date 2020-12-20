
using System.Net;
using System;
using System.IO;
using System.Net.Sockets;


namespace BasicElements.AbstractClasses
{
    public abstract class AbstractOnlineHallListener: AbstractListener
    {
    }
    public abstract class IOpenedListener: AbstractListener
    {
    }

    public abstract class AbstractListener: IDisposable
    {
        public IOnlineHallDecoder OnlineHallDecoder { get; set; }

        public abstract void Listen();
        public abstract void Close();

        protected TcpListener _TcpListener;

        public void Dispose()
        {
            Close();
        }
        ~AbstractListener()
        {
            Dispose();
        }
    }

    public abstract class AbstractOnlineHallTransmiter
    {
        public abstract void Close();
        public IOnlineHallEncoder OnlineHallEncoder { set; get; }
        public abstract void Write(byte method, MemoryStream stream);
    }
}
