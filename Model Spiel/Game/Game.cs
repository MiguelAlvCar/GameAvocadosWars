
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ModelGame.Actions;
using System.ComponentModel;
using BasicElements;
using System.Collections;
using Ninject;
using Ninject.Parameters;
using BasicElements.AbstractClasses;
using System.Runtime.Serialization.Formatters.Binary;
using ModelGame.Abstract;

namespace ModelGame
{
    [Serializable()]
    public class Game : ModelBase, IModelGame
    {
        [NonSerialized()]
        private ActionsLoader _ActionsLoader;
        public ActionsLoader ActionsLoader { get => _ActionsLoader; set => _ActionsLoader = value; }
        public IActionsLoader IActionsloader{ set
            {
                ActionsLoader = (ActionsLoader)value;
            }
            get
            {
                return ActionsLoader;
            }
        }

        public BattleData battledata { set; get; }
        public IBattleData IBattledata { 
            set 
            {
                battledata = (BattleData)value;
            }            
            get 
            {
                return battledata;
            } 
        }

        public Game()
        {
            ActionsLoader = new ActionsLoader(this);                      
            IsChangingToNextTurn = false;
            RedUnitsStorage = new Stack<UnitItemDTO>();
            BlueUnitsStorage = new Stack<UnitItemDTO>();
            battledata = new BattleData(this);
            colorTurn = ArmyColor.Blue;
            Gamestate = GamestateModel.CreatingMap;

            Map = new List<Terrain>();      
        }
         
        #region Unit and armies

        public ListArmy<ModelUnit> ListReds { get => battledata.RedsList; }
        public ListArmy<ModelUnit> ListBlues { get => battledata.BluesList; }

        [NonSerialized()]
        public List<UnitID> GivenID = new List<UnitID>();
        public void NewID(ModelUnit unit)
        {
            UnitID lastUnitID = GivenID.Where(x => x.ArmyColor == unit.ArmyAffiliation).OrderByDescending(x => x.ID).FirstOrDefault();
            if (lastUnitID.ID == int.MaxValue)
                throw new InvalidDataException();

            UnitID newUnitID;
            if (GivenID.Count == 0)
                newUnitID = new UnitID() { ArmyColor = unit.ArmyAffiliation, ID = 0 };
            else
                newUnitID = new UnitID() { ArmyColor = unit.ArmyAffiliation, ID = lastUnitID.ID + 1 };
            GivenID.Add(newUnitID);
            unit.ID = newUnitID;
        }

        [NonSerialized()]
        private Action<ModelUnit> _UnitCreated;
        public event Action<ModelUnit> UnitCreated
        {
            add { _UnitCreated += value; }
            remove { _UnitCreated -= value; }
        }

        public void OnUnitCreated(ModelUnit unit)
        {
            _UnitCreated(unit);
        }

        [NonSerialized()]
        private Action _selectNoUnit;
        public event Action selectNoUnit
        {
            add { _selectNoUnit += value; }
            remove { _selectNoUnit -= value; }
        }
        [NonSerialized()]
        private Action _OpenPanelUnitInfoSelectingUnit;
        public event Action OpenPanelUnitInfoSelectingUnit
        {
            add { _OpenPanelUnitInfoSelectingUnit += value; }
            remove { _OpenPanelUnitInfoSelectingUnit -= value; }
        }
        [NonSerialized()]
        private Action<ModelUnit> _ShowSpecialEffectWhileSelecting;
        public event Action<ModelUnit> ShowSpecialEffectWhileSelecting
        {
            add { _ShowSpecialEffectWhileSelecting += value; }
            remove { _ShowSpecialEffectWhileSelecting -= value; }
        }
        public void OnShowSpecialEffectWhileSelecting(ModelUnit unitToSelect)
        {
            _ShowSpecialEffectWhileSelecting(unitToSelect);
        }
        [NonSerialized()]
        private ModelUnit _selectedUnit;
        public ModelUnit selectedUnit
        {
            get => _selectedUnit;
        }
        public void SelectUnit (ModelUnit unitToSelect, bool isLocalPlayerInHisOwnTurn)
        {
            ActionsLoader.Movement.ListMovement.ClearTerrains();
            if (selectedUnit is RangeUnit)
            {
                ActionsLoader.RangedCombat.FireList.ClearTerrains();
            }
            if (unitToSelect == null)
            {
                _selectNoUnit();
                _selectedUnit = unitToSelect;
            }
            else
            {
                if (selectedUnit == null)
                {
                    _OpenPanelUnitInfoSelectingUnit();
                }
                if (isLocalPlayerInHisOwnTurn && Gamestate == GamestateModel.Battle)
                {
                    if (colorTurn == ArmyColor.Red && unitToSelect.ArmyAffiliation == ArmyColor.Red)
                    {
                        ActionsLoader.Movement.ShowMovementPosible(unitToSelect);
                    }
                    else if (colorTurn == ArmyColor.Blue && unitToSelect.ArmyAffiliation == ArmyColor.Blue)
                    {
                        ActionsLoader.Movement.ShowMovementPosible(unitToSelect);
                    }
                }
                _selectedUnit = unitToSelect;
                unitToSelect.ShowSpecialEffectWhileSelecting();
            }
        }
        
        [NonSerialized()]
        private Action<ModelUnit> _DefenderEffect;
        public event Action<ModelUnit> DefenderEffect
        {
            add { _DefenderEffect += value; }
            remove { _DefenderEffect -= value; }
        }
        public void OnDefenderEffect(ModelUnit unit)
        {
            _DefenderEffect(unit);
        }

        [NonSerialized()]
        private Action<ModelUnit, bool> _AttackerEffect;
        public event Action<ModelUnit, bool> AttackerEffect
        {
            add { _AttackerEffect += value; }
            remove { _AttackerEffect -= value; }
        }
        public void OnAttackerEffect(ModelUnit unit, bool showSpecialAttack)
        {
            _AttackerEffect(unit, showSpecialAttack);
        }

        public void DisposeUnitModel(ModelUnit uni)
        {
            if (uni.ArmyAffiliation == ArmyColor.Red)
                ListReds.Remove(uni);
            else if (uni.ArmyAffiliation == ArmyColor.Blue)
                ListBlues.Remove(uni);
            foreach (Terrain terr in Map)
            {
                if (terr.unitInTerrain == uni)
                {
                    terr.unitInTerrain = null;
                }
            }
            if (selectedUnit == uni)
                SelectUnit(null, false);
            if (colorTurn == uni.ArmyAffiliation || Gamestate != GamestateModel.Battle)
                ActionsLoader.VisibleTerrains.listVisilibity.HideTerrainsOfUnit(uni);
            if (uni.InTerrain != null)
                ActionsLoader.VisibleTerrains.listVisilibity.ClearTerrainsOfUnit(uni);
            uni.OnDisposeUnitViewModel();

            uni.Game = null;
            uni.InTerrain = null;

            if (GivenID != null)
                GivenID.Remove(uni.ID);
        }

        public void DisposeTerrain(Terrain terr)
        {
            Map.Remove(terr);

            terr.OnDisposeViewModelTerrain();
        }

        #endregion

        public InitialValues InitialValues { set; get; } = new InitialValues();

        #region Map

        [NonSerialized()]
        private Action<short, short, bool> _LenghtWidthChanged;
        public event Action<short, short, bool> LenghtWidthChanged
        {
            add { _LenghtWidthChanged += value; }
            remove { _LenghtWidthChanged -= value; }
        }
        public void OnChangeLenghtAndWidthMap(short length, short width, bool fromSaved)
        {
            mapLenght = length;
            mapWidth = width;
            _LenghtWidthChanged(length, width, fromSaved);
        }

        [NonSerialized()]
        private Action<int, int, Terrain> _TerrainCreated;
        public event Action<int, int, Terrain> TerrainCreated
        {
            add { _TerrainCreated += value; }
            remove { _TerrainCreated -= value; }
        }
        public void OnTerrainCreated(int y, int x, Terrain terr)
        {
            _TerrainCreated(y, x, terr);
        }

        public short mapLenght { get; set; }
        public short mapWidth { get; set; }

        public List<Terrain> Map { set; get; }

        #endregion 

        #region Turn and Victory

        [NonSerialized()]
        public Action<string, bool> _Win;
        public event Action<string, bool> Win
        {
            add { _Win += value; }
            remove { _Win -= value; }
        }
        
        [NonSerialized()]
        private Stack<UnitItemDTO> _redUnitsStorage;
        public Stack<UnitItemDTO> RedUnitsStorage {
            set => _redUnitsStorage = value;
            get => _redUnitsStorage; }
        [NonSerialized()]
        private Stack<UnitItemDTO> _blueUnitsStorage;
        public Stack<UnitItemDTO> BlueUnitsStorage
        {
            set => _blueUnitsStorage = value;
            get => _blueUnitsStorage;
        }
        public bool IsInBattleAfterWaitingForLateToBattleSignal = false;
        private GamestateModel _gamestate;
        public GamestateModel Gamestate
        {
            get => _gamestate;
            set
            {
                SetProperty(ref _gamestate, value);
                switch (value)
                {
                    case GamestateModel.DeployingBlueUnits:
                        foreach (ModelUnit unit in ListReds)
                        {
                            unit.OnHideUnit();
                            UnitItemDTO unitItemDTO = new UnitItemDTO(unit);
                            unitItemDTO.XX = unit.InTerrain.XX;
                            unitItemDTO.YY = unit.InTerrain.YY;
                            RedUnitsStorage.Push(unitItemDTO);
                            unit.InTerrain.unitInTerrain = null;
                        }
                        ActionsLoader.VisibleTerrains.listVisilibity.HideTerrains(ArmyColor.Red);
                        break;
                    case GamestateModel.Battle:
                        ActionsLoader.VisibleTerrains.listVisilibity.ClearTerrains(ArmyColor.Red);
                        foreach (ModelUnit unit in ListReds)
                        {
                            if (RedUnitsStorage != null && RedUnitsStorage.Count > 0)
                            {
                                UnitItemDTO UnitItem = RedUnitsStorage.Pop();
                                Map.FirstOrDefault(x => x.XX == UnitItem.XX && x.YY == UnitItem.YY).unitInTerrain = UnitItem.Unit;
                                unit.OnRevealUnit();
                            }
                            else
                                break;
                        }
                        foreach (ModelUnit unit in ListBlues)
                        {
                            if (BlueUnitsStorage != null && BlueUnitsStorage.Count > 0)
                            {
                                UnitItemDTO UnitItem = BlueUnitsStorage.Pop();
                                Map.FirstOrDefault(x => x.XX == UnitItem.XX && x.YY == UnitItem.YY).unitInTerrain = UnitItem.Unit;
                                unit.OnRevealUnit();
                            }
                            else
                                break;
                        }
                        ActionsLoader.VisibleTerrains.listVisilibity.HideTerrains(ArmyColor.Blue);
                        ActionsLoader.VisibleTerrains.listVisilibity.ShowTerrains(ArmyColor.Red);
                        foreach (Terrain terr in Map)
                        {
                            terr.deploymentArea = ArmyColor.None;
                        }
                        foreach (Terrain ter in Map)
                        {
                            ter.Hide = true;
                        }
                        NextTurn(true, ColorArmyToShow, out object ob);

                        System.Timers.Timer aTimer1 = new System.Timers.Timer();
                        aTimer1.Elapsed += (a, b) =>
                        {
                            IsInBattleAfterWaitingForLateToBattleSignal = true;
                        };
                        aTimer1.Interval = 4000;
                        aTimer1.AutoReset = false; ;
                        aTimer1.Start();
                        
                        break;
                }
                if (value != GamestateModel.CreatingMap && value != GamestateModel.Battle)
                {
                    foreach (Terrain ter in Map)
                    {
                        ter.Hide = true;
                    }
                }
            }
        }

        public int turn
        {
            get => battledata.turn;
            private set
            {
                SetProperty(ref battledata._turn, value);
            }
        }
        public ArmyColor colorTurn
        {
            get => battledata.colorTurn;
            set
            {
                SetProperty(ref battledata._colorTurn, value);
            }
        }
        [NonSerialized()]
        public ArmyColor ColorArmyToShow;

        [NonSerialized()]
        private Action _ChangedToNextTurnInGame;
        public event Action ChangedToNextTurnInGame
        {
            add { _ChangedToNextTurnInGame += value; }
            remove { _ChangedToNextTurnInGame -= value; }
        }

        public void NextTurn(bool isNeededToRecalculateVisibility, ArmyColor colorArmyToShow, out object parameter)
        {
            ColorArmyToShow = colorArmyToShow;

            SelectUnit(null, false);

            #region Elefants

            if (BasicMechanisms.Kernel.CanResolve<INextTurnModelInterceptor>())
                BasicMechanisms.Kernel.Get<INextTurnModelInterceptor>().OnChangingToNextTurnInModelGame(this, out parameter);
            else
                parameter = null;

            #endregion

            foreach (ModelUnit unit in ListBlues.Union(ListReds))
            {
                unit.NextTurn();
            }

            if (isNeededToRecalculateVisibility)
            {
                ArmyColor colorToHide;
                if (isNeededToRecalculateVisibility)
                    colorToHide = colorTurn;
                else
                    colorToHide = colorArmyToShow;

                ActionsLoader.VisibleTerrains.listVisilibity.HideTerrains(colorToHide);
                ActionsLoader.VisibleTerrains.listVisilibity.ClearTerrains(colorArmyToShow);
            }

            if (colorArmyToShow == ArmyColor.Red)
            {
                foreach (ModelUnit unit in ListReds)
                {
                    unit.NextTurnToOwn();
                    if (isNeededToRecalculateVisibility)
                        ActionsLoader.VisibleTerrains.ShowVisibles(unit);
                }
            }
            else if (colorArmyToShow == ArmyColor.Blue)
            {
                foreach (ModelUnit unit in ListBlues)
                {
                    unit.NextTurnToOwn();
                    if (isNeededToRecalculateVisibility)
                        ActionsLoader.VisibleTerrains.ShowVisibles(unit);
                }
            }            
            foreach (ModelUnit unit in ListBlues.Union(ListReds))
            {
                if (unit is INextTurnAfterNextTurnUnit)
                {
                    ((INextTurnAfterNextTurnUnit)unit).NextTurnAfterNextTurn();
                }
            }
            ++colorTurn;
            if (!Enum.IsDefined(typeof(ArmyColor), colorTurn))
            {
                colorTurn = ArmyColor.Red;
                turn++;
            }
            if (isNeededToRecalculateVisibility)
                ActionsLoader.VisibleTerrains.listVisilibity.ShowTerrains(ColorArmyToShow);

            _ChangedToNextTurnInGame();
        }

        public void NextTurnAfterReceivedSignal(ArmyColor colorArmyToShow)
        {
            bool isNeededToRecalculateVisibility = true;

            ColorArmyToShow = colorArmyToShow;

            SelectUnit(null, false);

            if (isNeededToRecalculateVisibility)
            {
                ActionsLoader.VisibleTerrains.listVisilibity.HideTerrains(colorArmyToShow);
                ActionsLoader.VisibleTerrains.listVisilibity.ClearTerrains(colorArmyToShow);
            }

            if (colorArmyToShow == ArmyColor.Red)
            {
                foreach (ModelUnit unit in ListReds)
                {
                    if (isNeededToRecalculateVisibility)
                        ActionsLoader.VisibleTerrains.ShowVisibles(unit);
                }
            }
            else if (colorArmyToShow == ArmyColor.Blue)
            {
                foreach (ModelUnit unit in ListBlues)
                {
                    if (isNeededToRecalculateVisibility)
                        ActionsLoader.VisibleTerrains.ShowVisibles(unit);
                }
            }

            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(colorTurn)));

            if (isNeededToRecalculateVisibility)
                ActionsLoader.VisibleTerrains.listVisilibity.ShowTerrains(ColorArmyToShow);

            _ChangedToNextTurnInGame();
        }

        private bool _elephantPhase;
        public bool IsChangingToNextTurn
        {
            get => _elephantPhase;
            set
            {
                SetProperty(ref _elephantPhase, value);
            }
        }

        #endregion

        #region Deserialization

        public void AfterDeserialization(bool isWorkingWithMap, bool isDeserializingIntoOnline = false)
        {
            SubAfterDeserialization1();

            if (!isDeserializingIntoOnline)
                ColorArmyToShow = colorTurn;

            foreach (ModelUnit unit in ListBlues)
            {
                unit.Game = this;
                OnUnitCreated(unit);
                unit.AfterDeserialization();
            }
            foreach (ModelUnit unit in ListReds)
            {
                unit.Game = this;
                OnUnitCreated(unit);
                unit.AfterDeserialization();
            }

            OnChangeLenghtAndWidthMap(mapLenght, mapWidth, true);
            foreach (Terrain terr in Map)
            {
                OnTerrainCreated((terr.YY / Terrain.YYY) + 1, (terr.XX / Terrain.XXX) + 1, terr);
                terr.AfterDeserialization();
            }
            if (Gamestate != GamestateModel.CreatingMap)
                _PropertyChanged(this, new PropertyChangedEventArgs(nameof(Gamestate)));

            if (!isWorkingWithMap)
            {
                foreach (ModelUnit unit in ListBlues)
                {
                    ActionsLoader.VisibleTerrains.ShowVisibles(unit);
                }
                foreach (ModelUnit unit in ListReds)
                {
                    ActionsLoader.VisibleTerrains.ShowVisibles(unit);
                }
                foreach (Terrain ter in Map)
                {
                    ter.Hide = true;
                }
                ActionsLoader.VisibleTerrains.listVisilibity.ShowTerrains(ColorArmyToShow);
            }
        }

        private void SubAfterDeserialization1()
        {
            GivenID = new List<UnitID>();
            foreach (ModelUnit unit in ListBlues.Union(ListReds))
            {
                if (unit._DisposeFromGame == null)
                {
                    unit.DisposeFromGame += (uni) => DisposeUnitModel(uni);
                }
            }
            RedUnitsStorage = new Stack<UnitItemDTO>();
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(colorTurn)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(turn)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(InitialValues.Army2)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(InitialValues.Army1)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(InitialValues.player2Points)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(InitialValues.player1Points)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(InitialValues.player2)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(InitialValues.player1)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(Gamestate)));
        }


        #endregion
    }
    
}