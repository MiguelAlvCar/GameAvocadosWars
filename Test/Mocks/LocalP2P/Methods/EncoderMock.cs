using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BasicElements.AbstractClasses;
using DTO_Models;
using ModelGame.Abstract;
using ViewModelOnlineGame.P2PCommunication;
using ModelGame;
using BasicElements;

namespace Test.LocalP2P
{
    public class EncoderMock : IOnlineGameEncoder
    {
        AbstractOnlineGameTransmiter Transmiter;
        private int _MaintainConnectionPoolingMiliseconds;
        public int MaintainConnectionPoolingMiliseconds
        {
            get
            {
                return _MaintainConnectionPoolingMiliseconds;
            }
            set
            {
                aTimer.Interval = value;
                _MaintainConnectionPoolingMiliseconds = value;
            }
        }
        public System.Timers.Timer aTimer { set; get; } = new System.Timers.Timer();

        public EncoderMock(AbstractOnlineGameTransmiter transmiter, int maintainConnectionPoolingMiliseconds)
        {
            Transmiter = transmiter;
            MaintainConnectionPoolingMiliseconds = maintainConnectionPoolingMiliseconds;
            aTimer.Elapsed += (a, b) =>
            {
                MaintainP2PConnection();
            };
            aTimer.AutoReset = true;
            aTimer.Start();
        }

        //1
        public void ConnectIPAndPlayer(PlayerDTO player)
        {
            SerializablePlayerDTO serializable = SerializablePlayerDTO.ConvertDTOToSerializable(player);

            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, serializable);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
        }
        //2
        public void NewChatMessage(string message)
        {
            //SetMethode(P2PMethod.ConnectIPAndPlayer, Encoding.UTF8.GetBytes(message));
        }
        //3
        public void ToMap(bool isConfirm, bool hasAdversaryConfirmed, string ID) 
        {

        }
        //4
        public void GoFromGame()
        {

        }
        //5
        public void ChangeDescription(string description)
        {
            //SetMethodeAndTransmit(P2PMethod.ChangeDescription, Encoding.UTF8.GetBytes(description));
        }
        //6
        public void MaintainP2PConnection()
        {
            Transmiter.Write((byte)P2PMethod.MaintainP2PConnection, new MemoryStream());
        }
        //7
        public void ChangeInitialValues(InitialValues initialValues)
        {
            
        }
        //8
        public void AdjustWidthAndLenghMap(int x, int y)
        {
            
        }
        //9
        public void NewTerrain(TerrainType terrainType, int x, int y)
        {
            
        }
        //10
        public void AddRiverEnd(int riverEnd, int x, int y)
        {

        }
        //11
        public void ClearRiverEnds(int x, int y)
        {
            
        }
        //12
        public void AddBridge(bool isBridge, int x, int y)
        {
            
        }
        //13
        public void AddDeploymentArea(ArmyColor deploymentColor, int x, int y)
        {

        }
        //14
        public void ToDeployment(bool isConfirmed, bool hasAdversaryConfirmed)
        {
            
        }

        //15
        public void ToBattle(bool isConfirmed, bool hasAdversaryConfirmed, List<UnitItemDTO> units)
        {

        }
        //16
        public void NextTurn(IModelGame game)
        {
            
        }
        //17
        public void Victory(PlayerDTO player)
        {
            
        }
        //18
        public void NewMovement(MovementItemDTO unitItems)
        {
            
        }
    }
}
