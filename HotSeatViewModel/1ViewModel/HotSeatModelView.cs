using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using ModelGame;
using ModelGame.Actions;
using BasicElements;
using System.ComponentModel;
using ArmyAndUnitTypes;
using Ninject;
using WpfBasicElements;
using WpfBasicElements.AbstractClasses;
using BasicElements.AbstractClasses;
using ModelGame.Abstract;
using System.Security.Cryptography.X509Certificates;

//[assembly: InternalsVisibleTo("Armies")]

namespace ViewModelHotSeat
{
    public partial class HotSeatViewModel : ModelBase, IHotSeatViewModel
    {
        protected Game _Game;
        public virtual Game Game {
            get { return _Game; }
            set
            {
                value.PropertyChanged += (sender, arg) =>
                {
                    Game gam = sender as Game;
                    if (arg.PropertyName == nameof(gam.Gamestate))
                    {
                        Gamestate = GamestateConverterToMV(value.Gamestate);
                    }
                    else if (arg.PropertyName == nameof(gam.turn))
                    {
                        turn = gam.turn;
                    }
                    else if (arg.PropertyName == nameof(gam.colorTurn))
                    {
                        if (gam.colorTurn == ArmyColor.Blue || gam.colorTurn == ArmyColor.Red)
                            ColorTurn = gam.colorTurn;
                    }
                    else if (arg.PropertyName == nameof(gam.InitialValues.player1Points))
                    {
                        Player1Points = gam.InitialValues.player1Points;
                    }
                    else if (arg.PropertyName == nameof(gam.InitialValues.player2Points))
                    {
                        Player2Points = gam.InitialValues.player2Points;
                    }
                    else if (arg.PropertyName == nameof(gam.InitialValues.player1))
                    {
                        Player1 = gam.InitialValues.player1;
                    }
                    else if (arg.PropertyName == nameof(gam.InitialValues.player2))
                    {
                        Player2 = gam.InitialValues.player2;
                    }
                    else if (arg.PropertyName == nameof(gam.InitialValues.Army1))
                    {
                        foreach (ArmyType item in BasicMechanisms.Kernel.Get<IListArmiesMainModelView>().listArmiesObSave)
                        {
                            if ((int)item.ArmyID == gam.InitialValues.Army1)
                            {
                                Army1 = item;
                                break;
                            }
                        }
                    }
                    else if (arg.PropertyName == nameof(gam.InitialValues.Army2))
                    {
                        foreach (ArmyType item in BasicMechanisms.Kernel.Get<IListArmiesMainModelView>().listArmiesObSave)
                        {
                            if ((int)item.ArmyID == gam.InitialValues.Army2)
                            {
                                Army2 = item;
                            }
                        }
                    }
                    else if (arg.PropertyName == nameof(gam.IsChangingToNextTurn))
                    {
                        IsChangingToNextTurn = gam.IsChangingToNextTurn;
                    }
                };
                value.ShowSpecialEffectWhileSelecting += (unitToSelect) =>
                {
                    selectedUnit = (ViewModelUnit)unitToSelect.OnGetViewModelUnitFromModelUnit();
                    if (BasicMechanisms.Kernel.CanResolve<IShowSpecialEffectWhileSelectingUnitInterceptor>())
                        BasicMechanisms.Kernel.Get<IShowSpecialEffectWhileSelectingUnitInterceptor>().SpecialEffect(unitToSelect);
                };
                value.Win += (playerName, isReceivedSignalFromInternet) => Win(playerName);
                value.selectNoUnit += () => selectedUnit = null;
                _Game = value;
                value.OpenPanelUnitInfoSelectingUnit += () =>
                    OpenPanelUnitInfoSelectingUnit();
                value.LenghtWidthChanged += (len, wid, fromSaved) =>
                {
                    mapLenght = len;
                    mapWidth = wid;
                    LenghtWidthChanged(len, wid, fromSaved);
                };
                value.TerrainCreated += TerrainCreatedHandler;
                value.UnitCreated += (unit) =>
                {
                    HotSeatViewModel viewModel = this;
                    ViewModelUnit unitModelView = new ViewModelUnit(unit, viewModel);
                    // I need to set de UnitType here and not in UnitRow because the game loads don´t use UnitRow.
                    unitModelView.UnitType = listArmiesOb.SelectMany(x => x.Units)
                        .FirstOrDefault(x => x.GetUnitType() == unit.GetType());

                    if (unitModelView.UnitType == null)
                        throw new InvalidOperationException("There is no UnitType, that match the ModelUnit. Your army-assembly is faulty");

                    UnitCreated(unitModelView);
                };
                value.ChangedToNextTurnInGame += () => ViewModelAfterNextTurn();
                value.AttackerEffect += (unit, showSpecialAttack) =>
                {
                    if (!IsChangingToNextTurn)
                    {
                        if (showSpecialAttack)
                            attackerEffect(GetUnitTypeFromModelUnit(unit).AttackEffect.EffectUri);
                        else
                            attackerEffect(StandardEffects.Sword.EffectUri);
                    }
                };
                value.DefenderEffect += (unit) =>
                {
                    if (!IsChangingToNextTurn)
                        defenderEffect(GetUnitTypeFromModelUnit(unit).DefenseEffect.EffectUri);
                };
            }
        }

        public IModelGame IGame {
            set =>
                Game = (Game)value;
            get => Game;
        }

        public HotSeatViewModel(Game game = null)
        {
            if (game == null)
                game = new Game();

            Game = game;

            Game.ColorArmyToShow = ArmyColor.Red;

            IsChangingToNextTurn = false;

            DeleteComboItem = new RelayCommand(DeleteComboItemExecute, DeleteComboItemCanExecute);
            ToPlayer2 = new RelayCommand(ToPlayer2Execute, ToPlayer2CanExecute);
            ToBattle = new RelayCommand(ToBattle_Execute, ToBattle_CanExecute);
            NextTurn = new RelayCommand(NextTurn_Execute, NextTurn_CanExecute);
            ShowFire = new RelayCommand(ShowFireExecute, ShowFireCanExecute);

            #region AddListPlayer

            if (File.Exists("Spieler.dat"))
            {
                foreach (string str in File.ReadAllLines("Spieler.dat"))
                {
                    listPlayersOb.Add(str);
                }
            }

            #endregion
        }

        public bool IsInBattle
        {
            get
            {
                return Gamestate == GamestateMV.Battle;
            }
        }

        #region Game state and turns

        public virtual void ChangeGamestate(GamestateMV gamestateMV)
        {
            GamestateModel gameStateModel = GamestateConverterToMV(gamestateMV);

            switch (gameStateModel)
            {
                case GamestateModel.DeployingBlueUnits:
                    if (Game.ListReds.Any(uni => uni.InTerrain == null))
                    {
                        NotAllRedUnitDeployed();
                        return;
                    }
                    if (Game.ListReds.Count == 0)
                    {
                        NoUnitsDeployed();
                        return;
                    }
                    break;
                case GamestateModel.Battle:
                    if (Game.ListBlues.Any(uni => uni.InTerrain == null))
                    {
                        NotAllRedUnitDeployed();
                        return;
                    }
                    if (Game.ListBlues.Count == 0)
                    {
                        NoUnitsDeployed();
                        return;
                    }
                    break;
            }

            Game.Gamestate = gameStateModel;
        }
        private GamestateMV _gamestate;
        public GamestateMV Gamestate
        {
            get => _gamestate;
            set
            {
                if (value == GamestateMV.DeployingBlueUnits)
                    selectNoUnit();
                SetProperty(ref _gamestate, value);
            }
        }
        static protected GamestateMV GamestateConverterToMV(GamestateModel gamestateMV)
        {
            switch (gamestateMV)
            {
                case GamestateModel.CreatingMap:
                    return GamestateMV.CreatingMap;
                case GamestateModel.DeployingRedUnits:
                    return GamestateMV.DeployingRedUnits;
                case GamestateModel.DeployingBlueUnits:
                    return GamestateMV.DeployingBlueUnits;
                case GamestateModel.Battle:
                    return GamestateMV.Battle;
                default:
                    return GamestateMV.CreatingMap;
            }
        }
        static protected GamestateModel GamestateConverterToMV(GamestateMV gamestateMV)
        {
            switch (gamestateMV)
            {
                case GamestateMV.CreatingMap:
                    return GamestateModel.CreatingMap;
                case GamestateMV.DeployingRedUnits:
                    return GamestateModel.DeployingRedUnits;
                case GamestateMV.DeployingBlueUnits:
                    return GamestateModel.DeployingBlueUnits;
                case GamestateMV.Battle:
                    return GamestateModel.Battle;
                default:
                    return GamestateModel.CreatingMap;
            }
        }

        private bool _IsChangingToNextTurn;
        public bool IsChangingToNextTurn
        {
            get => _IsChangingToNextTurn;
            set
            {
                _IsChangingToNextTurn = value;
            }
        }

        private int _turn;
        public int turn
        {
            get => _turn;
            set
            {
                SetProperty(ref _turn, value);
            }
        }

        private ArmyColor _ColorTurn;
        public ArmyColor ColorTurn
        {
            get => _ColorTurn;
            set
            {
                SetProperty(ref _ColorTurn, value);
            }
        }

        #endregion

        #region Properties by creating map

        public event Action<int, int, ViewModelTerrain> TerrainCreated;
        public void OnTerrainCreated (int x, int y, ViewModelTerrain terr)
        {
            TerrainCreated(x, y, terr);
        }

        public event Action<ViewModelUnit> UnitCreated;

        public ObservableCollection<string> listPlayersOb { get; } = new ObservableCollection<string>();

        public ObservableCollection<ArmyType> listArmiesOb { get => BasicMechanisms.Kernel.Get<IListArmiesMainModelView>().listArmiesObSave; }

        protected ushort? __Player1Points;
        public ushort? _Player1Points
        {
            get => __Player1Points;
            protected set
            {
                if (value != __Player1Points)
                {
                    Game.InitialValues.player1Points = value;
                    SetProperty(ref __Player1Points, value);
                }
            }
        }
        public virtual ushort? Player1Points
        {
            get => __Player1Points;
            set => _Player1Points = value;
        }
        protected ushort? __Player2Points;
        public ushort? _Player2Points
        {
            get => __Player2Points;
            protected set
            {
                if (value != __Player2Points)
                {
                    Game.InitialValues.player2Points = value;
                    SetProperty(ref __Player2Points, value);
                }
            }
        }
        public virtual ushort? Player2Points
        {
            get => __Player2Points;
            set => _Player2Points = value;
        }
        protected ArmyType __Army1;
        public ArmyType _Army1
        {
            get => __Army1;
            protected set
            {
                if ((int)value.ArmyID != _Game.InitialValues.Army1)
                {
                    Game.InitialValues.Army1 = (int)value.ArmyID;
                }
                SetProperty(ref __Army1, value);
            }
        }
        public virtual ArmyType Army1
        {
            get => __Army1;
            set => _Army1 = value;
        }
        protected ArmyType __Army2;
        public ArmyType _Army2
        {
            get => __Army2;
            protected set
            {
                if ((int)value.ArmyID != _Game.InitialValues.Army2)
                {
                    Game.InitialValues.Army2 = (int)value.ArmyID;
                }
                SetProperty(ref __Army2, value);
            }
        }
        public virtual ArmyType Army2
        {
            get => __Army2;
            set => _Army2 = value;
        }

        protected string _Player1;
        public string Player1
        {
            get => _Player1;
            set
            {
                if (value != _Player1)
                {
                    Game.InitialValues.player1 = value;
                    SetProperty(ref _Player1, value);
                }
            }
        }

        protected string _player2;
        public string Player2
        {
            get => _player2;
            set
            {
                if (value != _player2)
                {
                    Game.InitialValues.player2 = value;
                    SetProperty(ref _player2, value);
                }
            }
        }

        public event Action<short, short, bool> LenghtWidthChanged;
        public short mapLenght { set; get; }
        public short mapWidth { set; get; }

        #endregion

        protected void TerrainCreatedHandler (int y, int x, Terrain terr)
        {
            HotSeatViewModel viewModel = this;
            ViewModelTerrain VMTerr = new ViewModelTerrain(x, y, terr, viewModel);
            TerrainCreated(y, x, VMTerr);
        }

        public virtual bool IsLocalPlayerInHisOwnTurn { get => true; }

        private ViewModelTerrain _movFocusedTerrain;
        public ViewModelTerrain movFocusedTerrain
        {
            get => _movFocusedTerrain;
            set
            {
                if (movFocusedTerrain != null)
                    movFocusedTerrain.MovementFocused = false;
                if (value == null)
                {                    
                    _movFocusedTerrain = value;
                }
                else
                {
                    _movFocusedTerrain = value;
                    value.MovementFocused = true;
                }
            }
        }

        private ViewModelTerrain _fireFocusedTerrain;
        public ViewModelTerrain FireFocusedTerrain
        {
            get => _fireFocusedTerrain;
            set
            {
                if (FireFocusedTerrain != null)
                    FireFocusedTerrain.FireFocused = false;
                if (value == null)
                {
                    _fireFocusedTerrain = value;
                }
                else
                {
                    _fireFocusedTerrain = value;
                    value.FireFocused = true;
                }
            }
        }
        
        public void OnUpdateSelectedUnit()
        {
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(selectedUnit)));
        }
        public event Action<ViewModelUnit> RemoveSelectedUnit;
        private ViewModelUnit _selectedUnit;
        public ViewModelUnit selectedUnit
        {
            get => _selectedUnit;
            set
            {
                RemoveSelectedUnit(selectedUnit);
                SetProperty(ref _selectedUnit, value);
            }
        }

        public event Action ViewModelAfterNextTurn;

        public event Action<Uri> attackerEffect;
        public event Action<Uri> defenderEffect;

        private AbstractUnitType GetUnitTypeFromModelUnit (ModelUnit unit)
        {
            return ((ViewModelUnit)unit.OnGetViewModelUnitFromModelUnit()).UnitType;
        }

        public bool TogButtonForestChecked { get; set; } = false;
        public bool TogButtonHillChecked { get; set; } = false;
        public bool TogButtonRedChecked { get; set; } = false;
        public bool TogButtonBlueChecked { get; set; } = false;
        public bool TogButtonCityChecked { get; set; } = false;
        public bool TogButtonSeeChecked { get; set; } = false;
        public bool TogButtonRiverChecked { get; set; } = false;
        public bool TogButtonBridgeChecked { get; set; } = false;
    }

    public enum GamestateMV
    {
        CreatingMap, DeployingRedUnits, DeployingBlueUnits, Battle
    }
}
