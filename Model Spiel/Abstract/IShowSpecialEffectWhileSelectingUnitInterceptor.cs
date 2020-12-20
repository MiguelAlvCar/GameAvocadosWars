using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGame.Abstract
{
    public interface IShowSpecialEffectWhileSelectingUnitInterceptor
    {
        void SpecialEffect(ModelUnit unitToSelect);
    }
}
