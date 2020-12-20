using ModelGame;
using System;
using System.Collections.Generic;
using BasicElements;
using ViewModelHotSeat.Abstract;
using Ninject;
using ViewModelHotSeat;

namespace ViewModelOnlineGame
{
    class OnlineViewModelTerrain: ViewModelTerrain
    {
        private OnlineGameViewModel OnlineViewModel
        {
            get
            {
                return (OnlineGameViewModel)ViewModel;
            }
        }

        public OnlineViewModelTerrain(int x, int y, Terrain terr, HotSeatViewModel viewModel): 
            base (x, y, terr, viewModel) { }
                
        public override bool Bridge
        {
            set
            {
                if (terrain.bridge != value)
                {
                    base.Bridge = value;
                    OnlineViewModel.OnlineManager.Transmiter.OnlineGameEncoder.AddBridge(value, this.terrain.XX, this.terrain.YY);
                }
            }
        }
        public override ArmyColor DeploymentArea
        {
            get =>
                terrain.deploymentArea;
            set
            {
                if (terrain.deploymentArea != value)
                {
                    terrain.deploymentArea = value;
                    OnlineViewModel.OnlineManager.Transmiter.OnlineGameEncoder.AddDeploymentArea(value, this.terrain.XX, this.terrain.YY);
                }
            }

        }
        public override TypeTerrainClass TerrainTypeClass
        {
            set
            {
                if (terrain.typeTerrain != value)
                {
                    terrain.typeTerrain = value;
                    OnlineViewModel.OnlineManager.Transmiter.OnlineGameEncoder.NewTerrain(value.terrainType, this.terrain.XX, this.terrain.YY);
                }
            }
        }
        

        public override void CreateRiver(int num)
        {
            base.CreateRiver(num);
            OnlineViewModel.OnlineManager.Transmiter.OnlineGameEncoder.AddRiverEnd(num, this.terrain.XX, this.terrain.YY);
        }

        public override void ClearRiverEnds()
        {
            base.ClearRiverEnds();
            OnlineViewModel.OnlineManager.Transmiter.OnlineGameEncoder.ClearRiverEnds(this.terrain.XX, this.terrain.YY);
        }
    }
}
