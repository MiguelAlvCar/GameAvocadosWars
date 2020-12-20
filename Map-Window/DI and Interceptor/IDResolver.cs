
using Ninject;
using Ninject.Modules;
using ModelGame.Actions;
using ModelGame;
using FirstWindows.Controller;
using BasicElements.AbstractForArmyInterceptor;
using Armies;
using Armies.TemplateMethods;
using ViewGame.View.Game;
using ArmyAndUnitTypes;
using Sound;
using WpfBasicElements.AbstractClasses;
using BasicElements.AbstractServerInternetCommunication;
using InternetCommunication.Server;
using FirstWindows;
using BasicElements.AbstractClasses;
using FirstWindows.Interceptors;
using WpfBasicElements.AbstractInterceptors;
using SaveLoad;
using Test;
using ViewModelOnlineGame.P2PCommunication;
using ModelGame.Abstract;
using Test.LocalP2P;
using ViewModelOnlineGame;
using ViewModelHotSeat;
using Armies.MethodsToInject;

namespace Initialization
{
    static public class IDResolver
    {
        //static bool test = false;

        internal static IKernel Kernel { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly IKernel instance = new StandardKernel(new NinjectSettings() { AllowNullInjection = true}, new ProductionModule());
        }
    }
    public class ProductionModule : NinjectModule
    {
        public override void Load()
        {
            //ModelGameActions
            Bind<AbstractCloseCombat>().To<CloseCombat>();
            Bind<IFieldsRelations>().To<FieldsRelations>();
            Bind<AbstractMovement>().To<MovementBase>();
            Bind<AbstractVisibleTerrains>().To<VisibleTerrains>();
            Bind<AbstractRangedCombat>().To<RangedCombat>();
           
            //MainModel
            Bind<IListArmiesMainModelView>().ToConstant(MainModelView.Instance);
            Bind<IInternetCommunicationMainModelView>().ToConstant(MainModelView.Instance);

            //Armies
            Bind<IModifierConverters>().To<ModifierConverters>();
            Bind<IModifierToUriConverter>().To<ModifierToUriConverter>();
            Bind<INextTurnModelInterceptor>().ToConstant(NextTurnInterceptor.Instance);
            Bind<INextTurnViewInterceptor>().ToConstant(NextTurnInterceptor.Instance);
            Bind<IShowSpecialEffectWhileSelectingUnitInterceptor>().To<ShowSpecialEffectWhileSelectingUnitInterceptor>();

            //Bind for the assemblies
            Bind<StartLogicAbstractInteceptor>().To<StartLogic>();
            Bind<IMusic>().To<Music>().InSingletonScope();
            Bind<IEffects>().To<Effects>().InSingletonScope();            
            Bind<IViewGameInterceptor>().To<ViewGameInterceptor>();
            Bind<ISaveViewGame>().To<GamePage>();
            Bind<IHotSeatViewModel>().To<HotSeatViewModel>();
            Bind<IModelGame>().To<Game>();
            Bind<IDeserializer>().To<GameDeserializer>();
            Bind<IBattleData>().To<BattleData>();
            Bind<IActionsLoader>().To<ActionsLoader>();
            Bind<ISaveLoadInterceptor>().To<SaveLoadInterceptor>();
            Bind<IFirstPageInterceptorFromSaveLoad>().To<FirstPageInterceptor>();

            Bind<P2PInterceptor>().To<ElefantsP2PInterceptor>();

            if (Properties.Settings.Default.InternetConnectionTest)
            {
                Bind<ICommunicationWithServer>().To<CommunicationWithServerMock>();
                Bind<AbstractOnlineHallListener>().To<ListenerMock>();
                Bind<IOpenedListener>().To<OpenedListenerMock>();
                Bind<AbstractOnlineHallTransmiter>().To<TransmiterMock>();
                Bind<AbstractOnlineGameListener>().To<ListenerMock>();
                Bind<AbstractOnlineGameTransmiter>().To<TransmiterMock>();
            }
            else
            {
                Bind<ICommunicationWithServer>().To<CommunicationWithServer>();
                Bind<AbstractOnlineHallListener>().To<Listener>();
                Bind<IOpenedListener>().To<OpenedListener>();
                Bind<AbstractOnlineHallTransmiter>().To<Transmiter>();
                Bind<AbstractOnlineGameListener>().To<Listener>();
                Bind<AbstractOnlineGameTransmiter>().To<Transmiter>();
            }

        }
    }
}