using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewGame.View.Game
{
    public interface INextTurnViewInterceptor
    {
        void OnChangedToNextTurnInHotSeat(GamePage hotSeatPage);
    }
}
