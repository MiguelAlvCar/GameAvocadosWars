using DTO_Models;
using System;
using System.IO;
using ModelGame;
using BasicElements.AbstractClasses;
using BasicElements;
using System.Collections.Generic;

namespace ViewModelOnlineGame.P2PCommunication
{
    public interface IOnlineGameEncoder: IOnlineHallEncoder
    {
        // 7500 and 10500 Milise

        void ChangeInitialValues(InitialValues initialValues);
        void AdjustWidthAndLenghMap(int x, int y);
        void NewTerrain(TerrainType terrainType, int x, int y);
        void AddRiverEnd(int riverEnd, int x, int y);
        void ClearRiverEnds(int x, int y);
        void AddBridge(bool isBridge, int x, int y);
        void AddDeploymentArea(ArmyColor deploymentColor, int x, int y);
        void ToDeployment(bool isConfirmed, bool hasAdversaryConfirmed);
        void ToBattle(bool isConfirmed, bool hasAdversaryConfirmed, List<UnitItemDTO> units);
        void NextTurn(IModelGame game);
        void Victory(PlayerDTO player);
        void NewMovement(MovementItemDTO unitItems);
    }

    public interface IOnlineGameDecoder : IOnlineHallDecoder
    {
        // 3000 and 4000 Milise

        event Action<InitialValues> ChangeInitialValues;
        event Action<int, int> AdjustWidthAndLenghMap;
        event Action<TerrainType, int, int> NewTerrain;
        event Action<int, int, int> AddRiverEnd;
        event Action<int, int> ClearRiverEnds;
        event Action<bool, int, int> AddBridge;
        event Action<ArmyColor, int, int> AddDeploymentArea;
        event Action<bool, bool> ToDeployment;
        event Action<bool, bool, List<UnitItemDTO>> ToBattle;
        event Action<IModelGame> NextTurn;
        event Action<PlayerDTO> Victory;
        event Action<MovementItemDTO> NewMovement;
        event Action<object> Injectable255;
    }

}