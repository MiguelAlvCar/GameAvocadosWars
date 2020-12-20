using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicElements.AbstractClasses
{
    public enum P2PMethod : byte
    {
        ConnectIPAndPlayer, 
        NewChatMessage, 
        ToMap, 
        GoFromGame, 
        ChangeDescription, 
        MaintainP2PConnection, 
        ChangeInitialValues, 
        AdjustWidthAndLenghMap, 
        NewTerrain,
        AddRiverEnd,
        ClearRiverEnds,
        AddBridge,
        AddDeploymentArea,
        ToDeployment,
        ToBattle,
        NewMovement,
        NextTurn,
        Victory
    }
}
