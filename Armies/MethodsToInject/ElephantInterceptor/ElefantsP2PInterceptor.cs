using ViewModelOnlineGame;
using System.IO;
using Armies.TemplateMethods;
using BasicElements.AbstractClasses;
using System.Runtime.Serialization.Formatters.Binary;

namespace Armies
{
    public class ElefantsP2PInterceptor : P2PInterceptor
    {
        public object Injectable255Decoder(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FrightenElefantStorage elefantAttackStorage = (FrightenElefantStorage)formatter.Deserialize(stream);
            return elefantAttackStorage;
        }

        public MemoryStream Method255Encoder(object objectForTheView)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objectForTheView);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public void Subscribe (OnlineGameViewModel viewModelOnlineGame)
        {
            viewModelOnlineGame.OnlineManager.Listener.OnlineGameDecoder.Injectable255 += (objectForTheViewUntypified) =>
            {
                NextTurnInterceptor.Instance.frightenElefantStorage = (FrightenElefantStorage)objectForTheViewUntypified;
            };
        }
    }
}
