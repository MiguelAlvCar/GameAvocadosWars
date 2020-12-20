using System;
using ViewModelHotSeat;
using OnlineGameChatAndStore;
using System.Windows.Input;
using BasicElements;
using Ninject;
using BasicElements.AbstractServerInternetCommunication;
using DTO_Models;
using ModelGame;
using ArmyAndUnitTypes;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Design;
using BasicElements.AbstractClasses;
using System.IO;

namespace ViewModelOnlineGame
{
    public partial class OnlineGameViewModel : HotSeatViewModel, IConnectionCloseable
    {
        static private OnlineGameViewModel _InstanceForDebugger;
        static public OnlineGameViewModel InstanceForDebugger
        {
            get
            {
                return _InstanceForDebugger;
            }
            set
            {
                if (BasicMechanisms.Test)
                    _InstanceForDebugger = value;
            }
        }

        public override Game Game
        {
            get { return base._Game; }
            set
            {
                base.Game = value;
                value.TerrainCreated -= TerrainCreatedHandler;
                value.TerrainCreated += (y, x, terr) =>
                {
                    HotSeatViewModel viewModel = this;
                    OnlineViewModelTerrain VMTerr = new OnlineViewModelTerrain(x, y, terr, viewModel);
                    OnTerrainCreated(y, x, VMTerr);
                };
                value.Win += (playerName, isReceivedSignalFromInternet) => {
                    if (!isReceivedSignalFromInternet)
                    {
                        OnlineManager.Transmiter.OnlineGameEncoder.Victory(MainMV.LoggedPlayer.Player);
                        float result;
                        if (OnlineManager.IsGuest)
                            result = -10;
                        else
                            result = 10;
                        BattleDTO battleDTO = MakeBattleDTO(result);
                        BasicMechanisms.Kernel.Get<ICommunicationWithServer>().Victory(battleDTO);
                    }
                };
            }
        }

        private OnlineManager _OnlineManager;
        public OnlineManager OnlineManager
        {
            get => _OnlineManager;
            set
            {
                _OnlineManager = value;

                value.Listener.OnlineGameDecoder.NewChatMessage += NewChatMessageFromAdversary;
                value.Listener.OnlineGameDecoder.GoFromGame += AdversaryGoesFromGame;
                value.Listener.OnlineGameDecoder.MaintainP2PConnection += AdversaryGoesFromGame;
                value.Listener.OnlineGameDecoder.ChangeInitialValues += SetInitialValues;
                value.Listener.OnlineGameDecoder.AdjustWidthAndLenghMap += AdjustWidthAndLenghMap;
                value.Listener.OnlineGameDecoder.NewTerrain += NewTerrain;
                value.Listener.OnlineGameDecoder.AddRiverEnd += AddRiverEnd;
                value.Listener.OnlineGameDecoder.AddBridge += AddBridge;
                value.Listener.OnlineGameDecoder.AddDeploymentArea += AddDeploymentArea;
                value.Listener.OnlineGameDecoder.ClearRiverEnds += ClearRiverEnds;
                value.Listener.OnlineGameDecoder.ToDeployment += ToDeployment;
                value.Listener.OnlineGameDecoder.ToBattle += InternetToBattle;
                value.Listener.OnlineGameDecoder.NextTurn += InternetNextTurn;
                value.Listener.OnlineGameDecoder.Victory += Victory;
                value.Listener.OnlineGameDecoder.NewMovement += NewMovement;
                BasicMechanisms.Kernel.Get<P2PInterceptor>().Subscribe(this);

                
            }
        }

        IInternetCommunicationMainModelView MainMV;

        public OnlineGameViewModel(OnlineManager onlineManager)
        {
            InstanceForDebugger = this;

            MainMV = BasicMechanisms.Kernel.Get<IInternetCommunicationMainModelView>();
            OnlineManager = onlineManager;

            Game.ColorArmyToShow = IsLocalPlayerHost() ? ArmyColor.Red : ArmyColor.Blue;
        }

        private bool _hasAdversaryConfirmedMap;
        public bool HasAdversaryConfirmedMap
        {
            get => _hasAdversaryConfirmedMap;
            set
            {
                SetProperty(ref _hasAdversaryConfirmedMap, value);
            }
        }

        private bool _isMapConfirmed;
        public bool IsMapConfirmed
        {
            get => _isMapConfirmed;
            set
            {
                OnlineManager.Transmiter.OnlineGameEncoder.ToDeployment(value, HasAdversaryConfirmedMap);
                if (value && HasAdversaryConfirmedMap)
                {
                    if (OnlineManager.IsGuest)
                        ChangeGamestate(GamestateMV.DeployingBlueUnits);
                    else
                        ChangeGamestate(GamestateMV.DeployingRedUnits);
                }
                else
                    SetProperty(ref _isMapConfirmed, value);
                
            }
        }

        private bool _hasAdversaryConfirmedArmy;
        public bool HasAdversaryConfirmedArmy
        {
            get => _hasAdversaryConfirmedArmy;
            set => SetProperty(ref _hasAdversaryConfirmedArmy, value);
        }

        private bool _isArmyConfirmed;
        public bool IsArmyConfirmed
        {
            get => _isArmyConfirmed;
            set
            {
                if (value)
                {
                    Game.GivenID.Clear();
                    if (OnlineManager.IsGuest)
                    {
                        foreach (ModelUnit unit in Game.ListBlues)
                        {
                            Game.NewID(unit);
                        }
                    }
                    else
                    {
                        foreach (ModelUnit unit in Game.ListReds)
                        {
                            Game.NewID(unit);
                        }
                    }
                }
                OnlineManager.Transmiter.OnlineGameEncoder.ToBattle(value, HasAdversaryConfirmedArmy, GetListOfDeployedUnits());

                if (value && HasAdversaryConfirmedArmy)
                    ChangeGamestate(GamestateMV.Battle);
                SetProperty(ref _isArmyConfirmed, value);
            }
        }
        private List<UnitItemDTO> GetListOfDeployedUnits()
        {
            List<UnitItemDTO> list = new List<UnitItemDTO>();
            ListArmy<ModelUnit> listArmy = null;
            if (OnlineManager.IsGuest)
                listArmy = Game.ListBlues;
            else
                listArmy = Game.ListReds;

            foreach (ModelUnit unit in listArmy)
            {
                UnitItemDTO unitItemDTO = new UnitItemDTO(unit.CloneWithoutTerrain());
                unitItemDTO.XX = unit.InTerrain.XX;
                unitItemDTO.YY = unit.InTerrain.YY;
                list.Add(unitItemDTO);
            }

            return list;
        }

        public override bool IsLocalPlayerInHisOwnTurn
        {
            get
            {
                return Gamestate != GamestateMV.Battle || MainMV.LoggedPlayer.Player.ID == PlayerInTurn.ID;
            }
        }

        private bool IsLocalPlayerHost()
        {
            return MainMV.LoggedPlayer.Player.ID == OnlineManager.ChatGame.Host.ID;
        }

        private PlayerDTO PlayerInTurn
        {
            get
            {
                PlayerDTO player = null;
                if (Game.colorTurn == ArmyColor.Red)
                    player = OnlineManager.ChatGame.Host;
                else if (Game.colorTurn == ArmyColor.Blue)
                    player = OnlineManager.ChatGame.Guest;
                return player;
            }
        }

        private bool IsAdverdaryOnline = true;

        public void CloseConnection()
        {
            OnlineManager.Transmiter.OnlineHallEncoder.GoFromGame();
            OnlineManager.Transmiter.Close();
            OnlineManager.Listener.Close();
        }

        private InitialValues GetInitialValues()
        {
            InitialValues initial = new InitialValues();
            if (Army1 != null)
                initial.Army1 = (int)Army1.ArmyID;
            if (Army2 != null)
                initial.Army2 = (int)Army2.ArmyID;
            initial.player1 = Player1;
            initial.player2 = Player2;
            initial.player1Points = Player1Points;
            initial.player2Points = Player2Points;

            return initial;
        }

        public override ushort? Player1Points
        {
            get => __Player1Points;
            set
            {
                if (value != __Player1Points)
                {
                    Game.InitialValues.player1Points = value;
                    SetProperty(ref __Player1Points, value);
                    OnlineManager.Transmiter.OnlineGameEncoder.ChangeInitialValues(GetInitialValues());
                }
            }
        }
        public override ushort? Player2Points
        {
            get => __Player2Points;
            set
            {
                if (value != __Player2Points)
                {
                    Game.InitialValues.player2Points = value;
                    SetProperty(ref __Player2Points, value);
                    OnlineManager.Transmiter.OnlineGameEncoder.ChangeInitialValues(GetInitialValues());
                }
            }
        }
        public override ArmyType Army1
        {
            get => __Army1;
            set
            {
                if ((int)value.ArmyID != _Game.InitialValues.Army1)
                {
                    Game.InitialValues.Army1 = (int)value.ArmyID;
                }
                SetProperty(ref __Army1, value);
                OnlineManager.Transmiter.OnlineGameEncoder.ChangeInitialValues(GetInitialValues());
            }
        }
        public override ArmyType Army2
        {
            get => __Army2;
            set
            {
                if ((int)value.ArmyID != _Game.InitialValues.Army2)
                {
                    Game.InitialValues.Army2 = (int)value.ArmyID;
                }
                SetProperty(ref __Army2, value);
                OnlineManager.Transmiter.OnlineGameEncoder.ChangeInitialValues(GetInitialValues());
            }
        }

        public override void CreateMap(short length, short width)
        {
            BasisCreateMap(length, width);
            OnlineManager.Transmiter.OnlineGameEncoder.AdjustWidthAndLenghMap(width, length);
        }

        public override void ChangeGamestate(GamestateMV gamestateMV)
        {
            Game.Gamestate = GamestateConverterToMV(gamestateMV);
        }

        public override void SendTheListUnitItemsThroughNet(MovementItemDTO movementItemDTO)
        {
            OnlineManager.Transmiter.OnlineGameEncoder.NewMovement(movementItemDTO);
        }

        #region Commands

        public void AddMessageToChat(string message, PlayerDTO player, bool isHostPlayer)
        {
            ChatEntry newChatEntry = new ChatEntry(message, player, isHostPlayer);
            OnlineManager.ChatGame.Chat.AddChatEntry(newChatEntry);
        }
        public event Action MessageSubmitted;
        public RoutedCommand SubmitMessage { get; } = new RoutedCommand();
        public void SubmitMessageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !String.IsNullOrEmpty((string)e.Parameter);
        }
        public void SubmitMessageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OnlineManager.Transmiter.OnlineHallEncoder.NewChatMessage((string)e.Parameter);
            AddMessageToChat((string)e.Parameter, MainMV.LoggedPlayer.Player, OnlineManager.IsGuest);
            MessageSubmitted();
        }

        protected override void ToBattle_Execute(object obj)
        {
            if (Game.ListBlues.Any(uni => uni.InTerrain == null))
            {
                OnNotAllRedUnitDeployed();
                return;
            }
            if (Game.ListBlues.Count == 0)
            {
                OnNoUnitsDeployed();
                return;
            }
            IsArmyConfirmed = !IsArmyConfirmed;
        }

        protected override void ToPlayer2Execute(object obj)
        {
            if (Game.ListReds.Any(uni => uni.InTerrain == null))
            {
                OnNotAllRedUnitDeployed();
                return;
            }
            if (Game.ListReds.Count == 0)
            {
                OnNoUnitsDeployed();
                return;
            }
            IsArmyConfirmed = !IsArmyConfirmed;
        }

        protected override void NextTurn_Execute(object obj)
        {
            ArmyColor armyForVisibility = IsLocalPlayerHost() ? ArmyColor.Red : ArmyColor.Blue;
            Game.NextTurn(false, armyForVisibility, out object objectForTheView);
            MemoryStream stream = BasicMechanisms.Kernel.Get<P2PInterceptor>().Method255Encoder(objectForTheView);

            OnlineManager.Transmiter.Write(255, stream);
            OnlineManager.Transmiter.OnlineGameEncoder.NextTurn(Game);
        }

        #endregion

        private BattleDTO MakeBattleDTO(float result)
        {
            BattleDTO battleDTO = new BattleDTO();
            battleDTO.Army1 = Game.InitialValues.Army1 ?? -1;
            battleDTO.Army2 = Game.InitialValues.Army2 ?? -1;
            battleDTO.Length = Game.mapLenght;
            battleDTO.Width = Game.mapWidth;
            battleDTO.Points1 = Game.InitialValues.player1Points ?? -1;
            battleDTO.Points2 = Game.InitialValues.player2Points ?? -1;
            battleDTO.Result = result;
            battleDTO.Player1ID = OnlineManager.ChatGame.Host.ID;
            battleDTO.Player2ID = OnlineManager.ChatGame.Guest.ID;
            return battleDTO;
        }
    }
}
