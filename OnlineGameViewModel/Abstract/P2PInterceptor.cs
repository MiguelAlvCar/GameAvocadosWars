using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ViewModelOnlineGame
{
    public interface P2PInterceptor
    {
        object Injectable255Decoder(Stream stream);

        MemoryStream Method255Encoder(object objectForTheView);

        void Subscribe(OnlineGameViewModel viewModelOnlineGame);
    }
}
