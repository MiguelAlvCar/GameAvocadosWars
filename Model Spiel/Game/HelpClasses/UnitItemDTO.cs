using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGame
{
    [Serializable()]
    public class UnitItemDTO
    {
        public UnitItemDTO(ModelUnit unit)
        {
            Unit = unit;
            isDead = false;
        }
        public ModelUnit Unit { set; get; }
        public int XX { set; get; }
        public int YY { set; get; }
        public bool isDead { set; get; }
        public Terrain OriginalTerrain { set; get; }
    }

    [Serializable()]
    public class MovementItemDTO
    {
        public IEnumerable<UnitItemDTO> ListUnitItems { set; get; } = new List<UnitItemDTO>();
        public bool showSpecialAttack { set; get; } = false;
        public bool showSpecialDefense { set; get; } = false;
        public ModelUnit DefenderForEffect { set; get; }
        public ModelUnit Attacker { set; get; }
        public bool WasThereCombat { set; get; } = false;
    }
}
