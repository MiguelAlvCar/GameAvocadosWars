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
using System.Threading;
using FirstWindows.View.OnlineHall;
using ViewModelOnlineGame.P2PCommunication;
using ModelGame;
using BasicElements;
using System.Security.Cryptography.X509Certificates;
using Armies;
using ViewModelOnlineGame;
using Ninject;
using Ninject.Parameters;
using Armies.TemplateMethods;

namespace Test.LocalP2P
{
    public class DecoderMock : IOnlineGameDecoder
    {
        static public DecoderMock Instance;
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

        public DecoderMock (int maintainConnectionPoolingMiliseconds = 0)
        {
            if (maintainConnectionPoolingMiliseconds > 0)
            {
                // In order to put the decoder of the dedicated listener
                Instance = this;
                MaintainConnectionPoolingMiliseconds = maintainConnectionPoolingMiliseconds;
                aTimer.Elapsed += (a, b) =>
                {
                    OnlineHallPage.InstanceForDebugger.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        MaintainP2PConnection();
                    });
                   
                };
                aTimer.AutoReset = false;
                aTimer.Start();
            }

            FullDeployList();
            FullMovementList();
            ChangeCoordinates(4, 4, out int x1, out int y1);
            ChangeCoordinates(4, 5, out int x2, out int y2);
            riverEnds.Push(new RiverEnds() {x = x1, y = y1, riverEnd = 0 });
            riverEnds.Push(new RiverEnds() { x = x1, y = y1, riverEnd = 120 });
            riverEnds.Push(new RiverEnds() { x = x2, y = y2, riverEnd = 180 });
            riverEnds.Push(new RiverEnds() { x = x2, y = y2, riverEnd = 240 });
        }

        private void ChangeCoordinates(int x, int y, out int xx, out int yy)
        {
            yy = (y - 1) * Terrain.YYY;
            if ((y % 2 < -0.1 || y % 2 < 0.1))
                xx = (x - 1) * Terrain.XXX + Terrain.XXX / 2;
            else
                xx = (x - 1) * Terrain.XXX;
        }

        private void FullDeployList()
        {
            Game game = new Game();

            ToBattleContent toBattleContent = new ToBattleContent();
            List<UnitItemDTO> listUnits = new List<UnitItemDTO>();
            ChangeCoordinates(3, 3, out int x, out int y);
            Knight ritter = new Knight();
            ritter.Game = game;
            ritter.ArmyAffiliation = ArmyColor.Red;
            ritter.Game = null;
            ritter.ID = new UnitID() { ID = 0, ArmyColor = ArmyColor.Red };
            //listUnits.Add(new UnitItemDTO(ritter, x, y));
            ChangeCoordinates(4, 3, out x, out y);
            Archer bogen = new Archer();
            bogen.Game = game;
            bogen.ArmyAffiliation = ArmyColor.Red;
            bogen.Game = null;
            bogen.ID = new UnitID() { ID = 1, ArmyColor = ArmyColor.Red };
            //listUnits.Add(new UnitItemDTO(bogen, x, y));
            toBattleContent.listUnits = listUnits;
            toBattleContent.isConfirmed = true;
            toBattleContent.hasAdversaryConfirmed = false;
            RedDeployList.Enqueue(toBattleContent);

            toBattleContent = new ToBattleContent();
            listUnits = new List<UnitItemDTO>();
            ChangeCoordinates(2, 3, out x, out y);
            Elephant ele = new Elephant();
            ele.Game = game;
            ele.ArmyAffiliation = ArmyColor.Red;
            ele.Game = null;
            ele.ID = new UnitID() { ID = 2, ArmyColor = ArmyColor.Red };
            //listUnits.Add(new UnitItemDTO(ele, x, y));
            ChangeCoordinates(6, 3, out x, out y);
            Hoplit hop = new Hoplit();
            hop.Game = game;
            hop.ArmyAffiliation = ArmyColor.Red;
            hop.Game = null;
            hop.ID = new UnitID() { ID = 3, ArmyColor = ArmyColor.Red };
            //listUnits.Add(new UnitItemDTO(hop, x, y));
            toBattleContent.listUnits = listUnits;
            toBattleContent.isConfirmed = true;
            toBattleContent.hasAdversaryConfirmed = true;
            RedDeployList.Enqueue(toBattleContent);

            toBattleContent = new ToBattleContent();
            listUnits = new List<UnitItemDTO>();
            ChangeCoordinates(3, 7, out x, out y);
            Knight ritter1 = new Knight();
            ritter1.Game = game;
            ritter1.ArmyAffiliation = ArmyColor.Blue;
            ritter1.Game = null;
            ritter1.ID = new UnitID() { ID = 0, ArmyColor = ArmyColor.Blue };
            //listUnits.Add(new UnitItemDTO(ritter1, x, y));
            ChangeCoordinates(6, 7, out x, out y);
            Archer bogen1 = new Archer();
            bogen1.Game = game;
            bogen1.ArmyAffiliation = ArmyColor.Blue;
            bogen1.Game = null;
            bogen1.ID = new UnitID() { ID = 1, ArmyColor = ArmyColor.Blue };
            //listUnits.Add(new UnitItemDTO(bogen1, x, y));
            toBattleContent.listUnits = listUnits;
            toBattleContent.isConfirmed = true;
            toBattleContent.hasAdversaryConfirmed = false;
            BlueDeployList.Enqueue(toBattleContent);

            toBattleContent = new ToBattleContent();
            listUnits = new List<UnitItemDTO>();
            ChangeCoordinates(4, 7, out x, out y);
            Elephant ele1 = new Elephant();
            ele1.Game = game;
            ele1.ArmyAffiliation = ArmyColor.Blue;
            ele1.Game = null;
            ele1.ID = new UnitID() { ID = 2, ArmyColor = ArmyColor.Blue };
            //listUnits.Add(new UnitItemDTO(ele1, x, y));
            ChangeCoordinates(5, 7, out x, out y);
            Companion com = new Companion();
            com.Game = game;
            com.ArmyAffiliation = ArmyColor.Blue;
            com.Game = null;
            com.ID = new UnitID() { ID = 3, ArmyColor = ArmyColor.Blue };
            //listUnits.Add(new UnitItemDTO(com, x, y));
            toBattleContent.listUnits = listUnits;
            toBattleContent.isConfirmed = true;
            toBattleContent.hasAdversaryConfirmed = true;
            BlueDeployList.Enqueue(toBattleContent);
        }

        private void FullMovementList()
        {
            MovementItemDTO movementItemDTO = new MovementItemDTO();

            List<UnitItemDTO> listunitItems = movementItemDTO.ListUnitItems.ToList();
            Game game = new Game();

            Elephant ele = new Elephant();
            ele.Game = new Game();
            ele.ArmyAffiliation = ArmyColor.Red;
            ele.Game = null;
            ele.LifeRest = 90;
            ele.MoralRest = 85;
            ele.ID = new UnitID() { ID = 2, ArmyColor = ArmyColor.Red };
            ele.InTerrain = new Terrain(1, 1, game);
            ChangeCoordinates(2, 3, out int xx, out int yy);
            //listunitItems.Add(new UnitItemDTO(ele, xx, yy));

            movementItemDTO.showSpecialAttack = true;
            movementItemDTO.Attacker = ele.CloneWithoutTerrain();

            Devotee dev = new Devotee();
            dev.Game = new Game();
            dev.ArmyAffiliation = ArmyColor.Blue;
            dev.Game = null;
            dev.LifeRest = 70;
            dev.MoralRest = 60;
            dev.ID = new UnitID() { ID = 0, ArmyColor = ArmyColor.Blue };
            dev.InTerrain = new Terrain(1, 1, game);
            ChangeCoordinates(2, 4, out int xx1, out int yy1);
            //listunitItems.Add(new UnitItemDTO(dev, xx1, yy1));

            movementItemDTO.ListUnitItems = listunitItems;

            RedMovementItemDTOs.Enqueue(movementItemDTO);
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
        public event Action<object> Injectable255;

        public void Decode(byte method, Stream stream)
        {
            switch (method)
            {
                case (byte)P2PMethod.ConnectIPAndPlayer:
                    if (ConnectIPAndPlayer != null)
                    {
                        ConnectIPAndPlayer(SendIPContentDecode());
                    }
                    break;
                case (byte)P2PMethod.NewChatMessage:
                    if (NewChatMessage != null)
                    {
                        NewChatMessage(NewChatMessageContentDecode());
                    }
                    break;
                case (byte)P2PMethod.ToMap:
                    if (ToMap != null)
                    {
                        ToMapContentDecode(out bool isConfirmed, out bool hasAdversaryConfirmed, out string ID);
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
                        ChangeDescription(ChangeDescriptionContentDecode());
                    }
                    break;
                case (byte)P2PMethod.MaintainP2PConnection:
                    aTimer.Stop();
                    aTimer.Start();
                    break;
                case (byte)P2PMethod.ChangeInitialValues:
                    if (ChangeInitialValues != null)
                    {
                        ChangeInitialValues(ChangeInitialValuesContentDecode());
                    }
                    break;
                case (byte)P2PMethod.AdjustWidthAndLenghMap:
                    if (AdjustWidthAndLenghMap != null)
                    {
                        AdjustWidthAndLenghMapContentDecode(out int x, out int y);
                        AdjustWidthAndLenghMap(y, x);
                    }
                    break;

                case (byte)P2PMethod.NewTerrain:
                    if (NewTerrain != null)
                    {
                        NewTerrainContentDecode(out TerrainType terrain, out int x, out int y);
                        NewTerrain(terrain, x, y);
                    }
                    break;
                case (byte)P2PMethod.AddRiverEnd:
                    if (AddRiverEnd != null)
                    {
                        AddRiverEndContentDecode(out int riverEnd, out int x, out int y);
                        AddRiverEnd(riverEnd, x, y);
                    }
                    break;
                case (byte)P2PMethod.ClearRiverEnds:
                    if (ClearRiverEnds != null)
                    {
                        ClearRiverEndsContentDecode(out int x, out int y);
                        ClearRiverEnds(x, y);
                    }
                    break;
                case (byte)P2PMethod.AddBridge:
                    if (AddBridge != null)
                    {
                        AddBridgeContentDecode(out bool isBridge, out int x, out int y);
                        AddBridge(isBridge, x, y);
                    }
                    break;
                case (byte)P2PMethod.AddDeploymentArea:
                    if (AddDeploymentArea != null)
                    {
                        AddDeploymentAreaContentDecode(out ArmyColor armyColor, out int x, out int y);
                        AddDeploymentArea(armyColor, x, y);
                    }
                    break;
                case (byte)P2PMethod.ToDeployment:
                    if (ToDeployment != null)
                    {
                        ToDeploymentContentDecode(out bool isConfirmed, out bool hasAdversaryConfirmed);
                        ToDeployment(isConfirmed, hasAdversaryConfirmed);
                    }
                    break;
                case (byte)P2PMethod.ToBattle:
                    if (ToBattle != null)
                    {
                        ToBattleContentDecode(out bool isConfirmed, out bool hasAdversaryConfirmed, out List<UnitItemDTO> listUnits);
                        ToBattle(isConfirmed, hasAdversaryConfirmed, listUnits);
                    }
                    break;
                case (byte)P2PMethod.NextTurn:
                    if (NextTurn != null)
                    {
                        NextTurnContentDecode(out IModelGame game);
                        NextTurn(game);
                    }
                    break;
                case (byte)P2PMethod.Victory:
                    if (Victory != null)
                    {
                        VictoryContentDecode(out PlayerDTO player);
                        Victory(player);
                    }
                    break;
                case (byte)P2PMethod.NewMovement:
                    if (NewMovement != null)
                    {
                        NewMovementContentDecode(out MovementItemDTO movementItemDTO);
                        NewMovement(movementItemDTO);
                    }
                    break;
                case 255:
                    if (Injectable255 != null)
                    {
                        Injectable255ContentDecode(out object parameter);
                        Injectable255(parameter);
                    }
                    break;
            }
        }

        //1
        private PlayerDTO SendIPContentDecode()
        {
            return new PlayerDTO("Ana", 3.7F, "2") { GlobalIP = "192.168.200.85" };
        }
        //2
        private string NewChatMessageContentDecode()
        {
            return "Hola feo";
        }
        //3
        private void ToMapContentDecode(out bool isConfirmed, out bool hasAdversaryConfirmed, out string ID)
        {
            ID = "2";
            isConfirmed = true;
            hasAdversaryConfirmed = true;
        }
        //4
        // No content to decode
        //5
        private string ChangeDescriptionContentDecode()
        {
            return "nueva descripción";
        }
        //6
        // No content to decode
        //7
        private InitialValues ChangeInitialValuesContentDecode()
        {
            return new InitialValues() { Army1 = 1, /*army2 = 2,*/ player2Points = 45 };
        }
        //8
        private void AdjustWidthAndLenghMapContentDecode(out int x, out int y)
        {
            x = 14;
            y = 9;
            return;
        }
        //9
        private void NewTerrainContentDecode(out TerrainType terrain, out int x, out int y)
        {
            terrain = TerrainType.HillCity;
            ChangeCoordinates(4, 4, out x, out y);
            return;
        }
        private Stack<RiverEnds> riverEnds = new Stack<RiverEnds>();
        struct RiverEnds
        {
            public int riverEnd { get; set; }
            public int x { get; set; }
            public int y { get; set; }
        }
        //10
        private void AddRiverEndContentDecode(out int riverEnd, out int x, out int y)
        {
            RiverEnds rEnd =  riverEnds.Pop();
            riverEnd = rEnd.riverEnd;
            x = rEnd.x;
            y = rEnd.y;
            return;
        }
        //11
        private void ClearRiverEndsContentDecode(out int x, out int y)
        {
            ChangeCoordinates(4, 4, out x, out y);
            return;
        }
        //12
        private void AddBridgeContentDecode(out bool isBridge, out int x, out int y)
        {
            isBridge = true;
            ChangeCoordinates(4, 4, out x, out y);
            return;
        }
        //13
        private void AddDeploymentAreaContentDecode(out ArmyColor armyColor, out int x, out int y)
        {
            armyColor = ArmyColor.Blue;
            ChangeCoordinates(5, 3, out x, out y);
            return;
        }
        //14        
        private void ToDeploymentContentDecode(out bool isConfirmed, out bool hasAdversaryConfirmed)
        {
            isConfirmed = true;
            hasAdversaryConfirmed = true;
        }
        private Queue<ToBattleContent> RedDeployList = new Queue<ToBattleContent>();
        private Queue<ToBattleContent> BlueDeployList = new Queue<ToBattleContent>();
        struct ToBattleContent
        {
            public bool isConfirmed;
            public bool hasAdversaryConfirmed;
            public List<UnitItemDTO> listUnits;
        }
        //15
        private void ToBattleContentDecode(out bool isConfirmed, out bool hasAdversaryConfirmed, out List<UnitItemDTO> listUnits)
        {
            if (OnlineHallPage.InstanceForDebugger.ModelView.IsGuest)
            {
                ToBattleContent content = RedDeployList.Dequeue();
                isConfirmed = content.isConfirmed;
                hasAdversaryConfirmed = content.hasAdversaryConfirmed;
                listUnits = content.listUnits;
            }
            else
            {
                ToBattleContent content = BlueDeployList.Dequeue();
                isConfirmed = content.isConfirmed;
                hasAdversaryConfirmed = content.hasAdversaryConfirmed;
                listUnits = content.listUnits;
            }
        }
        //16
        private void NextTurnContentDecode(out IModelGame game)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            OnlineGameViewModel.InstanceForDebugger.Game.battledata.colorTurn = ArmyColor.Blue;
            formatter.Serialize(stream, OnlineGameViewModel.InstanceForDebugger.Game);
            stream.Seek(0, SeekOrigin.Begin);

            game = BasicMechanisms.Kernel.Get<IDeserializer>().Deserialize(stream);
            ChangeCoordinates(2, 2, out int xx, out int yy);
            ((Game)game).Map.First(x => x.XX == xx && x.YY == yy).unitInTerrain = ((Game)game).ListReds[0];
            //((Game)game).ListReds[0].InTerrain = ((Game)game).Map.First(x => x.XX == 0 && x.YY == 0).unitInTerrain;
            game.IActionsloader = BasicMechanisms.Kernel.Get<IActionsLoader>(
                new[] {
                    new ConstructorArgument ("game", game) }
            );
        }
        //17
        private void VictoryContentDecode(out PlayerDTO player)
        {
            player = new PlayerDTO("Ana", 3.7F, "2") { GlobalIP = "192.168.200.85" };
        }
        //18
        private Queue<MovementItemDTO> RedMovementItemDTOs = new Queue<MovementItemDTO>();
        private Queue<MovementItemDTO> BlueMovementItemDTOs = new Queue<MovementItemDTO>();
        private void NewMovementContentDecode(out MovementItemDTO unitItems)
        {
            if (OnlineGameViewModel.InstanceForDebugger.Game.colorTurn == ArmyColor.Red)
            {
                unitItems = RedMovementItemDTOs.Dequeue();
            }
            else
            {
                unitItems = new MovementItemDTO();
            }
        }
        //19
        private void Injectable255ContentDecode(out object objectForTheView)
        {
            objectForTheView = new FrightenElefantStorage();
        }

    }
}
