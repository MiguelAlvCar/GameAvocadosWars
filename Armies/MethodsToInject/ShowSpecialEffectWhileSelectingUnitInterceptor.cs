using ModelGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelHotSeat;
using ModelGame.Abstract;

namespace Armies.MethodsToInject
{
    public class ShowSpecialEffectWhileSelectingUnitInterceptor : IShowSpecialEffectWhileSelectingUnitInterceptor
    {
        public void SpecialEffect (ModelUnit unitToSelect)
        {
            if (unitToSelect is Berseker)
            {
                Berseker berseker = unitToSelect as Berseker;
                if (berseker.InRage && berseker.ArmyAffiliation == berseker.Game.colorTurn && berseker.MovementRest > 0)
                {
                    IEnumerable<Terrain> EnuTerrains = unitToSelect.InTerrain.Adjacents.ToList().
                                    Where(x => x.unitInTerrain != null && x.unitInTerrain.ArmyAffiliation != unitToSelect.ArmyAffiliation);
                    foreach (Terrain ter in EnuTerrains)
                    {
                        ((ViewModelTerrain)ter.OnGetViewModelTerrain()).MultipleAttack = true;
                    }
                }
            }
        }
    }
}
