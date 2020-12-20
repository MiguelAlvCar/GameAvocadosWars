using System;
using System.Collections.Generic;
using System.Text;
using ModelGameBase.Actions;
using Ninject;
using Ninject.Parameters;
using BasicElements;

namespace Model
{
    public class ActionsLoader
    {
        public ICloseCombat CloseCombat { get; }
        public IFieldsRelations FieldsRelations { get; }
        public AbstractMovementBase MovementBase { get; }
        public AbstractVisibleTerrains VisibleTerrains { get; }
        public AbstractRangedCombat RangedCombat { get; }

        public ActionsLoader() 
        {
            IKernel kernel = BasicMechanisms.GetKernel();
            CloseCombat = kernel.Get<ICloseCombat>();
            FieldsRelations = kernel.Get<IFieldsRelations>();
            MovementBase = kernel.Get<AbstractMovementBase>(
                new[] {
                new ConstructorArgument ("gameBase", this) });
            VisibleTerrains = kernel.Get<AbstractVisibleTerrains>
                (new[] {
                new ConstructorArgument ("gameBase", this) });
            RangedCombat = kernel.Get<AbstractRangedCombat>(
                new[] {
                new ConstructorArgument ("gameBase", this) });
        }
    }
}
