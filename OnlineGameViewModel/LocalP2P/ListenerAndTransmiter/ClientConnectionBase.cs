using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel.Design;

namespace ViewModelOnlineGame.P2PCommunication
{
    public class ClientConnectionBase: IDisposable
    {
        public TcpClient Client { set; get; }
        public NetworkStream Stream { set; get; }

        public void Dispose()
        {
            if (Stream != null)
                Stream.Close();
            if (Client != null)
                Client.Close();
        }

        ~ClientConnectionBase()
        {
            Dispose();
        }
    }
}
