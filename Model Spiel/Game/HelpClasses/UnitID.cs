using System;
using System.Collections.Generic;
using System.Text;
using BasicElements;

namespace ModelGame
{
    [Serializable()]
    public struct UnitID
    {
        public ArmyColor ArmyColor { set; get; }
        public int ID { set; get; }
    }
}
