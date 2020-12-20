using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BasicElements.AbstractClasses;
using ModelGame.Abstract;
using DTO_Models;
using System.Runtime.Serialization.Formatters.Binary;
using ModelGame;
using System.Security.Cryptography.X509Certificates;
using BasicElements;
using Ninject;
using Ninject.Parameters;

namespace ViewModelOnlineGame.P2PCommunication
{
    public class Decoder: IOnlineGameDecoder
    {
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
                aTimer.Stop();
                aTimer.Start();
                _MaintainConnectionPoolingMiliseconds = value;
            }
        }
        private System.Timers.Timer aTimer { set; get; } = new System.Timers.Timer();

        public Decoder(int maintainConnectionPoolingMiliseconds = 0)
        {
            if (maintainConnectionPoolingMiliseconds > 0)
            {
                MaintainConnectionPoolingMiliseconds = maintainConnectionPoolingMiliseconds;
                aTimer.Elapsed += (a, b) =>
                {                    
                    MaintainP2PConnection();
                };
                aTimer.AutoReset = false;
                aTimer.Start();
            }
        }

        public event Action<PlayerDTO> ConnectIPAndPlayer;
        public event Action<string> NewChatMessage;
        public event Action<bool, bool, string> ToMap;
        public event Action GoFromGame;
        public event Action<string> ChangeDescription;
        public event Action MaintainP2PConnection;
        public event Action<InitialValues> ChangeInitialValues;
        public event Action<int, int> AdjustWidthAndLenghMap;
        public event Action<TerrainType, int, int> NewTerrain;
        public event Action<int, int, int> AddRiverEnd;
        public event Action<int, int> ClearRiverEnds;
        public event Action<bool, int, int> AddBridge;
        public event Action<ArmyColor, int, int> AddDeploymentArea;
        public event Action<bool, bool> ToDeployment;
        public event Action<bool, bool, List<UnitItemDTO>> ToBattle;
        public event Action<IModelGame> NextTurn;
        public event Action<PlayerDTO> Victory;
        public event Action<MovementItemDTO> NewMovement;
        //For the elefants theater
        public event Action<object> Injectable255;

        public void Decode(byte method, Stream stream)
        {
            switch (method)
            {
                case (byte)P2PMethod.ConnectIPAndPlayer:
                    if (ConnectIPAndPlayer != null)
                    {
                        ConnectIPAndPlayer(ConnectIPAndPlayerContentDecode(stream));
                    }
                    break;
                case (byte)P2PMethod.NewChatMessage:
                    if (NewChatMessage != null)
                    {
                        NewChatMessage(NewChatMessageContentDecode(stream));
                    }
                    break;
                case (byte)P2PMethod.ToMap:
                    if (ToMap != null)
                    {
                        ToMapContentDecode(stream, out bool isConfirmed, out bool hasAdversaryConfirmed, out string ID);
                        ToMap(isConfirmed, hasAdversaryConfirmed, ID);
                    }
                    break;
                case (byte)P2PMethod.GoFromGame:
                    if (GoFromGame != null)
                    {
                        GoFromGame();
                    }
                    break;
                case (byte)P2PMethod.ChangeDescription:
                    if (ChangeDescription != null)
                    {
                        ChangeDescription(ChangeDescriptionContentDecode(stream));
                    }
                    break;
                case (byte)P2PMethod.MaintainP2PConnection:
                    aTimer.Stop();
                    aTimer.Start();
                    break;
                case (byte)P2PMethod.ChangeInitialValues:
                    if (ChangeInitialValues != null)
                    {
                        ChangeInitialValues(ChangeInitialValuesContentDecode(stream));
                    }
                    break;
                case (byte)P2PMethod.AdjustWidthAndLenghMap:
                    if (AdjustWidthAndLenghMap != null)
                    {
                        AdjustWidthAndLenghMapContentDecode(stream, out int x, out int y);
                        AdjustWidthAndLenghMap(x, y);
                    }
                    break;
                case (byte)P2PMethod.NewTerrain:
                    if (NewTerrain != null)
                    {
                        NewTerrainContentDecode(stream, out TerrainType terrain, out int x, out int y);
                        NewTerrain(terrain, x, y);
                    }
                    break;
                case (byte)P2PMethod.AddRiverEnd:
                    if (AddRiverEnd != null)
                    {
                        AddRiverEndContentDecode(stream, out int riverEnd, out int x, out int y);
                        AddRiverEnd(riverEnd, x, y);
                    }
                    break;
                case (byte)P2PMethod.ClearRiverEnds:
                    if (ClearRiverEnds != null)
                    {
                        ClearRiverEndsContentDecode(stream, out int x, out int y);
                        ClearRiverEnds(x, y);
                    }
                    break;
                case (byte)P2PMethod.AddBridge:
                    if (AddBridge != null)
                    {
                        AddBridgeContentDecode(stream, out bool isBridge, out int x, out int y);
                        AddBridge(isBridge, x, y);
                    }
                    break;
                case (byte)P2PMethod.AddDeploymentArea:
                    if (AddDeploymentArea != null)
                    {
                        AddDeploymentAreaContentDecode(stream, out ArmyColor armyColor, out int x, out int y);
                        AddDeploymentArea(armyColor, x, y);
                    }
                    break;
                case (byte)P2PMethod.ToDeployment:
                    if (ToDeployment != null)
                    {
                        ToDeploymentContentDecode(stream, out bool isConfirmed, out bool hasAdversaryConfirmed);
                        ToDeployment(isConfirmed, hasAdversaryConfirmed);
                    }
                    break;
                case (byte)P2PMethod.ToBattle:
                    if (ToBattle != null)
                    {
                        ToBattleContentDecode(stream, out bool isConfirmed, out bool hasAdversaryConfirmed, out List<UnitItemDTO> listUnits);
                        ToBattle(isConfirmed,  hasAdversaryConfirmed, listUnits);
                    }
                    break;
                case (byte)P2PMethod.NextTurn:
                    if (NextTurn != null)
                    {
                        NextTurnContentDecode(stream, out IModelGame game);
                        NextTurn(game);
                    }
                    break;
                case (byte)P2PMethod.Victory:
                    if (Victory != null)
                    {
                        VictoryContentDecode(stream, out PlayerDTO player);
                        Victory(player);
                    }
                    break;
                case (byte)P2PMethod.NewMovement:
                    if (NewMovement != null)
                    {
                        NewMovementContentDecode(stream, out MovementItemDTO movementItemDTO);
                        NewMovement(movementItemDTO);
                    }
                    break;
                case 255:
                    if (Injectable255 != null)
                    {
                        object parameter = BasicMechanisms.Kernel.Get<P2PInterceptor>().Injectable255Decoder(stream);
                        Injectable255(parameter);
                    }
                    break;
                    
            }
        }

        //1
        private PlayerDTO ConnectIPAndPlayerContentDecode(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SerializablePlayerDTO serializable = (SerializablePlayerDTO)formatter.Deserialize(stream);
            return SerializablePlayerDTO.ConvertToPlayerDTO(serializable);
        }
        //2
        private string NewChatMessageContentDecode(Stream stream)
        {
            StreamReader sReader = new StreamReader(stream);
            string message = sReader.ReadToEnd();

            return message;
        }
        //3
        private void ToMapContentDecode(Stream stream, out bool isConfirmed, out bool hasAdversaryConfirmed, out string ID)
        {
            BinaryReader binReader = new BinaryReader(stream);
            isConfirmed = binReader.ReadBoolean();
            hasAdversaryConfirmed = binReader.ReadBoolean();
            StreamReader sReader = new StreamReader(stream);
            ID = sReader.ReadToEnd();
        }
        //4
        // No content to decode
        //5
        private string ChangeDescriptionContentDecode(Stream stream)
        {
            StreamReader sReader = new StreamReader(stream);
            return sReader.ReadToEnd();
        }
        //6
        // No content to decode
        //7
        private InitialValues ChangeInitialValuesContentDecode(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (InitialValues)formatter.Deserialize(stream);
        }
        //8
        private void AdjustWidthAndLenghMapContentDecode(Stream stream, out int x, out int y)
        {
            BinaryReader binReader = new BinaryReader(stream);
            x = binReader.ReadInt32();
            y = binReader.ReadInt32();
            return;
        }
        //9
        private void NewTerrainContentDecode(Stream stream, out TerrainType terrain, out int x, out int y) 
        {
            BinaryReader binReader = new BinaryReader(stream);
            terrain = (TerrainType)binReader.ReadInt32();
            x = binReader.ReadInt32();
            y = binReader.ReadInt32();
            return;
        }
        //10
        private void AddRiverEndContentDecode(Stream stream, out int riverEnd, out int x, out int y)
        {
            BinaryReader binReader = new BinaryReader(stream);
            riverEnd = binReader.ReadInt32();
            x = binReader.ReadInt32();
            y = binReader.ReadInt32();
            return;
        }
        //11
        private void ClearRiverEndsContentDecode(Stream stream, out int x, out int y)
        {
            BinaryReader binReader = new BinaryReader(stream);
            x = binReader.ReadInt32();
            y = binReader.ReadInt32();
            return;
        }
        //12
        private void AddBridgeContentDecode(Stream stream, out bool isBridge, out int x, out int y)
        {
            BinaryReader binReader = new BinaryReader(stream);
            isBridge = binReader.ReadBoolean();
            x = binReader.ReadInt32();
            y = binReader.ReadInt32();
            return;
        }
        //13
        private void AddDeploymentAreaContentDecode(Stream stream, out ArmyColor armyColor, out int x, out int y)
        {
            BinaryReader binReader = new BinaryReader(stream);
            armyColor = (ArmyColor)binReader.ReadByte();
            x = binReader.ReadInt32();
            y = binReader.ReadInt32();
            return;
        }
        //14        
        private void ToDeploymentContentDecode(Stream stream, out bool isConfirmed, out bool hasAdversaryConfirmed)
        {
            BinaryReader binReader = new BinaryReader(stream);
            isConfirmed = binReader.ReadBoolean();
            hasAdversaryConfirmed = binReader.ReadBoolean();
        }
        //15
        private void ToBattleContentDecode(Stream stream, out bool isConfirmed, out bool hasAdversaryConfirmed, out List<UnitItemDTO> listUnits)
        {
            BinaryReader binReader = new BinaryReader(stream);
            isConfirmed = binReader.ReadBoolean();
            hasAdversaryConfirmed = binReader.ReadBoolean();
            BinaryFormatter formatter = new BinaryFormatter();
            listUnits = (List<UnitItemDTO>)formatter.Deserialize(stream);
        }
        //16
        private void NextTurnContentDecode(Stream stream, out IModelGame game)
        {
            game = BasicMechanisms.Kernel.Get<IDeserializer>().Deserialize(stream);

            game.IActionsloader = BasicMechanisms.Kernel.Get<IActionsLoader>(
                new[] {
                    new ConstructorArgument ("game", game) }
            );
        }
        //17
        private void VictoryContentDecode(Stream stream, out PlayerDTO player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SerializablePlayerDTO serializable = (SerializablePlayerDTO)formatter.Deserialize(stream);
            player = SerializablePlayerDTO.ConvertToPlayerDTO(serializable);
        }
        //18
        private void NewMovementContentDecode(Stream stream, out MovementItemDTO unitItems)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            unitItems = (MovementItemDTO)formatter.Deserialize(stream);
        }
    }
}
