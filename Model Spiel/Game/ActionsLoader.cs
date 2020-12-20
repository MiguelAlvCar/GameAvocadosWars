using System;
using System.Collections.Generic;
using System.Text;
using ModelGame.Actions;
using Ninject;
using Ninject.Parameters;
using BasicElements;
using BasicElements.AbstractClasses;

namespace ModelGame
{
    public class ActionsLoader: IActionsLoader
    {
        public AbstractCloseCombat CloseCombat { get; }
        public IFieldsRelations FieldsRelations { get; }
        public AbstractMovement Movement { get; }
        public AbstractVisibleTerrains VisibleTerrains { get; }
        public AbstractRangedCombat RangedCombat { get; }

        public ActionsLoader(Game game) 
        {
            IKernel kernel = BasicMechanisms.Kernel;
            CloseCombat = kernel.Get<AbstractCloseCombat>(
                new[] {
                new ConstructorArgument ("game", game) });
            FieldsRelations = kernel.Get<IFieldsRelations>();
            Movement = kernel.Get<AbstractMovement>(
                new[] {
                new ConstructorArgument ("game", game),
                new ConstructorArgument ("actionsLoader", this) });
            VisibleTerrains = kernel.Get<AbstractVisibleTerrains>
                (new[] {
                new ConstructorArgument ("game", game),
                new ConstructorArgument ("actionsLoader", this) });
            RangedCombat = kernel.Get<AbstractRangedCombat>(
                new[] {
                new ConstructorArgument ("game", game) });
        }
    }
}
