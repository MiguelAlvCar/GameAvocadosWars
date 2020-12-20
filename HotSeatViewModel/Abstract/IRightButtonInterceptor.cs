using ModelGame;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModelHotSeat.Abstract
{
    interface IRightButtonInterceptor
    {
        void SpecialEffectRightButton(ViewModelTerrain viewModelTerrain, ViewModelUnit viewModelUnit);
    }
}
