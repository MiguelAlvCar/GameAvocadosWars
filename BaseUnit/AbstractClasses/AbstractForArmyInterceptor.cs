using System;
using System.Collections.Generic;
using System.Text;

namespace BasicElements.AbstractForArmyInterceptor
{
    public interface IModifierConverters
    {
        Modifier GetModifierFromModel(IUnitModel uni);
    }

    public interface IModifierToUriConverter
    {
        Uri GetUri(Modifier modifier);
    }

    public interface IUnitModel { }
}
