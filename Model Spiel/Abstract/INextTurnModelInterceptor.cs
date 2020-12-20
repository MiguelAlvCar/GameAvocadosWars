using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGame.Abstract
{
    public interface INextTurnModelInterceptor
    {
        void OnChangingToNextTurnInModelGame(Game game, out object objectForTheView);
    }
}
