using ModelGame;
using System;
using System.Collections.Generic;
using BasicElements;
using ViewModelHotSeat.Abstract;
using Ninject;
using System.Linq;

namespace ViewModelHotSeat
{
    public class ViewModelTerrain : ModelBase
    {
        public HotSeatViewModel ViewModel;        

        Terrain _terrain;
        public Terrain terrain
        {
            get => _terrain;
            set
            {
                _terrain = value;

                if (value == null)
                {
                    return;
                }

                _terrain.PropertyChanged += (sender, arg) =>
                {
                    Terrain terr = sender as Terrain;
                    if (arg.PropertyName == nameof(terr.typeTerrain))
                    {
                        TerrainType = terr.terrainType;
                        if (TerrainType == TerrainType.See)
                            riverEndsClear();
                    }
                    else if (arg.PropertyName == nameof(terr.deploymentArea))
                    {
                        DeploymentArea = terr.deploymentArea;
                        DeploymentAreaSet(DeploymentArea);
                    }
                    else if (arg.PropertyName == nameof(_terrain.unitInTerrain))
                    {
                        if (terr.unitInTerrain == null)
                            unit = null;
                        else
                            unit = terr.unitInTerrain.OnGetViewModelUnitFromModelUnit() as ViewModelUnit;
                    }
                    else if (arg.PropertyName == nameof(_terrain.bridge))
                    {
                        Bridge = value.bridge;
                        if (value.bridge)
                            addBridge(riverEnd1 ?? 0, riverEnd2 ?? 0);
                        else
                            clearBridge();
                    }
                    else if (arg.PropertyName == nameof(_terrain.focusedmovement))
                    {
                        MovementFocused = value.focusedmovement;
                    }
                    else if (arg.PropertyName == nameof(_terrain.focusedfire))
                    {
                        FireFocused = value.focusedfire;
                    }
                    else if (arg.PropertyName == nameof(_terrain.fireposible))
                    {
                        FirePosible = value.fireposible;
                    }
                    else if (arg.PropertyName == nameof(_terrain.movementposible))
                    {
                        MovementPosible = value.movementposible;
                        if (!terr.movementposible)
                            MultipleAttack = false;
                    }
                    else if (arg.PropertyName == nameof(_terrain.Hide))
                    {
                        Hide = value.Hide;
                    }
                };
                _terrain.riverAdded += (nu) => riverEndsAdd(nu);
                _terrain.riverCleared += () => riverEndsClear();
                _terrain.GetViewModelTerrain += () => { return this; } ;
                _terrain.DisposeViewModelTerrain += () => {
                    DisposeViewTerrain();
                    terrain = null;
                    ViewModel = null;
                    unit = null;
                };
                TerrainType = value.terrainType;
            }
        }

        public ViewModelTerrain(int x, int y, Terrain terr, HotSeatViewModel viewModel)
        {
            ViewModel = viewModel;
            X = x; Y = y;
            terrain = terr;
        }

        #region Methods

        public void BasisCreateRiver(int num)
        {
            terrain.riverEndsAdd(num);
        }
        public virtual void CreateRiver(int num)
        {
            BasisCreateRiver(num);
        }

        public void LeftDown()
        {
            if (ViewModel.Game.Gamestate == GamestateModel.CreatingMap)
            {                
                if (ViewModel.TogButtonForestChecked)
                {
                    if (terrain.terrainType == TerrainType.Hill || terrain.terrainType == TerrainType.HillCity)
                    {
                        TerrainTypeClass = TypeTerrainClass.HillForest;
                    }
                    else if (terrain.terrainType != TerrainType.HillForest)
                    {
                        TerrainTypeClass = TypeTerrainClass.Forest;
                    }
                }

                else if (ViewModel.TogButtonHillChecked == true)
                {
                    if (terrain.terrainType == TerrainType.Forest)
                    {
                        TerrainTypeClass = TypeTerrainClass.HillForest;
                    }
                    if (terrain.terrainType == TerrainType.PlainCity)
                    {
                        TerrainTypeClass = TypeTerrainClass.HillCity;
                    }
                    else if (terrain.terrainType != TerrainType.HillForest)
                    {
                        TerrainTypeClass = TypeTerrainClass.Hill;
                    }
                }
                else if (ViewModel.TogButtonCityChecked == true)
                {
                    if (terrain.terrainType == TerrainType.Hill || terrain.terrainType == TerrainType.HillForest)
                    {
                        TerrainTypeClass = TypeTerrainClass.HillCity;
                    }
                    else if (terrain.terrainType != TerrainType.HillCity)
                    {
                        TerrainTypeClass = TypeTerrainClass.PlainCity;
                    }
                }
                else if (ViewModel.TogButtonSeeChecked == true)
                {
                    TerrainTypeClass = TypeTerrainClass.See;
                }
                else if (ViewModel.TogButtonBridgeChecked == true)
                {
                    Bridge = true;
                }
                else if (ViewModel.TogButtonRedChecked &&
                    (DeploymentArea == ArmyColor.None || DeploymentArea == ArmyColor.Blue))
                {
                    DeploymentArea = ArmyColor.Red;
                }

                else if (ViewModel.TogButtonBlueChecked &&
                    (DeploymentArea == ArmyColor.None || DeploymentArea == ArmyColor.Red))
                {
                    DeploymentArea = ArmyColor.Blue;
                }
            }
            else if (ViewModel.Game.Gamestate != GamestateModel.CreatingMap)
            {
                selectNoUnit();
            }
        }

        public void selectNoUnit()
        {
            ViewModel.selectNoUnit();
        }       

        public void RightDown()
        {
            if (ViewModel.Game.Gamestate == GamestateModel.CreatingMap)
            {
                if (ViewModel.TogButtonRedChecked || ViewModel.TogButtonBlueChecked)
                {
                    DeploymentArea = ArmyColor.None;
                }
                else if (ViewModel.TogButtonBridgeChecked)
                {
                    Bridge = false;
                }
                else
                {
                    ClearRiverEnds();
                    if (!ViewModel.TogButtonRiverChecked && !ViewModel.TogButtonBridgeChecked)
                    {
                        TerrainTypeClass = TypeTerrainClass.Plain;
                    }
                    else if (ViewModel.TogButtonRiverChecked)
                    {
                        Bridge = false;
                    }
                }
            }
            else if (ViewModel.selectedUnit != null)
            {
                if (ViewModel.Game.Gamestate == GamestateModel.DeployingRedUnits)
                {
                    if (DeploymentArea == ArmyColor.Red)
                        ViewModel.movFocusedTerrain = this;
                }
                else if (ViewModel.Game.Gamestate == GamestateModel.DeployingBlueUnits)
                {
                    if (DeploymentArea == ArmyColor.Blue)
                        ViewModel.movFocusedTerrain = this;
                }
                else if (ViewModel.Game.Gamestate == GamestateModel.Battle)
                {
                    if (MovementPosible)
                        ViewModel.movFocusedTerrain = this;
                    else if (FirePosible)
                        ViewModel.FireFocusedTerrain = this;
                    if (BasicMechanisms.Kernel.CanResolve<IRightButtonInterceptor>())
                        BasicMechanisms.Kernel.Get<IRightButtonInterceptor>().SpecialEffectRightButton(this, ViewModel.selectedUnit);
                }
            }
        }

        public void RightUp()
        {
            ViewModel.movFocusedTerrain = null;
            if (ViewModel.FireFocusedTerrain != null)
                ViewModel.FireFocusedTerrain = null;
            if (ViewModel.Game.Gamestate == GamestateModel.DeployingRedUnits)
            {
                if (DeploymentArea == ArmyColor.Red)
                    ViewModel.Game.ActionsLoader.Movement.deploy(terrain);
            }
            else if (ViewModel.Game.Gamestate == GamestateModel.DeployingBlueUnits)
            {
                if (DeploymentArea == ArmyColor.Blue)
                    ViewModel.Game.ActionsLoader.Movement.deploy(terrain);
            }
            else if (ViewModel.Game.Gamestate == GamestateModel.Battle)
            {
                MovementItemDTO movementItemDTO = null;
                if (MovementPosible)
                    movementItemDTO = ViewModel.Game.ActionsLoader.Movement.Move(this.terrain, ViewModel.Game.selectedUnit, false);
                if (FirePosible)
                    movementItemDTO = ViewModel.Game.ActionsLoader.RangedCombat.Combat((RangeUnit)ViewModel.Game.selectedUnit, terrain);
                if (movementItemDTO != null)
                {
                    ViewModel.SendTheListUnitItemsThroughNet(movementItemDTO);
                }
            }
        }

        #endregion

        #region Properties 
        
        private ViewModelUnit _unit;
        public ViewModelUnit unit { get => _unit; set => SetProperty(ref _unit, value); }

        public int X { set; get; }
        public int Y { set; get; }

        private bool _movementFocused;
        public bool MovementFocused
        {
            get => _movementFocused;
            set
            {
                SetProperty(ref _movementFocused, value);
            }
        }

        private bool _fireFocused;
        public bool FireFocused
        {
            get => _fireFocused;
            set
            {
                SetProperty(ref _fireFocused, value);
            }
        }

        private bool _firePosible;
        public bool FirePosible
        {
            get => _firePosible;
            set
            {
                SetProperty(ref _firePosible, value);
            }
        }

        private bool _movementPosible;
        public bool MovementPosible
        {
            get => _movementPosible;
            set
            {
                SetProperty(ref _movementPosible, value);
            }
        }

        private bool _multipleAttack;
        public bool MultipleAttack
        {
            get => _multipleAttack;
            set
            {
                SetProperty(ref _multipleAttack, value);
            }
        }
        
        private bool _hide;
        public bool Hide
        {
            get => _hide;
            set
            {
                SetProperty(ref _hide, value);
            }
        }

        public bool BasisBridge
        {
            set
            {
                if (terrain.bridge != value)
                {
                    terrain.bridge = value;
                }
            }
        }
        public virtual bool Bridge
        {
            set
            {
                BasisBridge = value;
            }
        }

        public event Action<ArmyColor> DeploymentAreaSet;
        public ArmyColor BasisDeploymentArea
        {
            get => terrain.deploymentArea;
            set
            {
                if (terrain.deploymentArea != value)
                {
                    terrain.deploymentArea = value;
                }
            }
        }
        public virtual ArmyColor DeploymentArea
        {
            get => terrain.deploymentArea;
            set => BasisDeploymentArea = value;
        }

        public TypeTerrainClass BasisTerrainTypeClass
        {
            set
            {
                if (terrain.typeTerrain != value)
                {
                    terrain.typeTerrain = value;
                }
            }
        }

        public virtual TypeTerrainClass TerrainTypeClass
        {
            set
            {
                BasisTerrainTypeClass = value;
            }
        }

        protected TerrainType _terrainType;
        public TerrainType TerrainType
        {
            get => _terrainType;
            set => SetProperty(ref _terrainType, value);
        }   

        public event Action<int, int> riverChanged;
        public event Action riverCleared;
        public event Action<int, int> addBridge;
        public event Action clearBridge;
        // In order to set right the river and the possible bridge
        private int? riverEnd1;
        private int? riverEnd2;
        private List<int> _riverEnds = new List<int>();
        protected void riverEndsAdd(int num)
        {
            _riverEnds.Add(num);
            if (_riverEnds.Count == 1)
            {
                riverEnd1 = num;
            }
            else if (_riverEnds.Count == 2)
            {
                riverEnd2 = num;
            }
            riverChanged(riverEnd1 ?? 0, num);
        }
        private void riverEndsClear()
        {
            riverCleared();
            riverEnd1 = null;
            riverEnd2 = null;
            _riverEnds.Clear();
        }
        public void BasisClearRiverEnds()
        {
            terrain.riverEndsClear();
        }
        public virtual void ClearRiverEnds()
        {
            BasisClearRiverEnds();
        }

        public event Action DisposeViewTerrain;


        #endregion
    } 
}
