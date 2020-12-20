using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using BasicElements.AbstractClasses;
using DTO_Models;
using ModelGame;
using ModelGame.Abstract;
using BasicElements;

namespace ViewModelOnlineGame.P2PCommunication
{
    public class Encoder: IOnlineGameEncoder
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
        public Encoder(AbstractOnlineGameTransmiter transmiter, int maintainConnectionPoolingMiliseconds)
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
        public void ConnectIPAndPlayer (PlayerDTO player)
        {
            SerializablePlayerDTO serializable = SerializablePlayerDTO.ConvertDTOToSerializable(player);

            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, serializable);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.ConnectIPAndPlayer, stream);
        }
        //2
        public void NewChatMessage(string message)
        {
            
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(message);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.NewChatMessage, stream);
        }
        //3
        public void ToMap(bool isConfirmed, bool hasAdversaryConfirmed, string ID)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(stream);
            binWriter.Write(isConfirmed);
            binWriter.Write(hasAdversaryConfirmed);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(ID);
            writer.Flush();

            stream.Seek(0, SeekOrigin.Begin);
            BinaryReader binReader = new BinaryReader(stream);
            bool isConfirmed1 = binReader.ReadBoolean();
            bool hasAdversaryConfirmed1 = binReader.ReadBoolean();
            StreamReader sReader = new StreamReader(stream);
            string ID1 = sReader.ReadToEnd();

            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.ToMap, stream);
        }
        //4
        public void GoFromGame()
        {
            MemoryStream stream = new MemoryStream();
            Transmiter.Write((byte)P2PMethod.GoFromGame, stream);
        }
        //5
        public void ChangeDescription(string description)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(description);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.ChangeDescription, stream);
            //SetMethodeAndTransmit(, Encoding.UTF8.GetBytes(description));
        }
        //6
        public void MaintainP2PConnection()
        {
            MemoryStream stream = new MemoryStream();
            Transmiter.Write((byte)P2PMethod.MaintainP2PConnection, stream);
        }
        //7
        public void ChangeInitialValues(InitialValues initialValues)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, initialValues);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.ChangeInitialValues, stream);
        }
        //8
        public void AdjustWidthAndLenghMap(int x, int y)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(stream);
            binWriter.Write(x);
            binWriter.Write(y);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.AdjustWidthAndLenghMap, stream);
        }
        //9
        public void NewTerrain(TerrainType terrainType, int x, int y) 
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(stream);
            binWriter.Write((int)terrainType);
            binWriter.Write(x);
            binWriter.Write(y);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.NewTerrain, stream);
        }
        //10
        public void AddRiverEnd(int riverEnd, int x, int y)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(stream);
            binWriter.Write(riverEnd);
            binWriter.Write(x);
            binWriter.Write(y);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.AddRiverEnd, stream);
        }
        //11
        public void ClearRiverEnds(int x, int y)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(stream);
            binWriter.Write(x);
            binWriter.Write(y);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.ClearRiverEnds, stream);
        }
        //12
        public void AddBridge(bool isBridge, int x, int y)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(stream);
            binWriter.Write(isBridge);
            binWriter.Write(x);
            binWriter.Write(y);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.AddBridge, stream);
        }
        //13
        public void AddDeploymentArea(ArmyColor deploymentColor, int x, int y)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(stream);
            binWriter.Write((byte)deploymentColor);
            binWriter.Write(x);
            binWriter.Write(y);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.AddDeploymentArea, stream);
        }
        //14
        public void ToDeployment(bool isConfirmed, bool hasAdversaryConfirmed)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(stream);
            binWriter.Write(isConfirmed);
            binWriter.Write(hasAdversaryConfirmed);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.ToDeployment, stream);
        }
        //15
        public void ToBattle(bool isConfirmed, bool hasAdversaryConfirmed, List<UnitItemDTO> units)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(stream);
            binWriter.Write(isConfirmed);
            binWriter.Write(hasAdversaryConfirmed);

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, units);

            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.ToBattle, stream);
        }
        //16
        public void NextTurn(IModelGame game)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, game);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.NextTurn, stream);
        }
        //17
        public void Victory(PlayerDTO player)
        {
            SerializablePlayerDTO serializable = SerializablePlayerDTO.ConvertDTOToSerializable(player);

            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, serializable);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.Victory, stream);
        }
        //18
        public void NewMovement(MovementItemDTO movementItemDTO)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, movementItemDTO);
            stream.Seek(0, SeekOrigin.Begin);

            Transmiter.Write((byte)P2PMethod.NewMovement, stream);
        }
    }
}
