using System;
using System.Collections.Generic;
using BasicElements;
using System.Linq;
using System.Collections;
using ModelGame;
using ViewGame.View.Game;
using ViewGame.View.Resources;
using GameFrontEnd.View;
using System.Threading;
using Sound;
using WpfBasicElements.AbstractClasses;
using Ninject;
using ModelGame.Abstract;
using ViewModelHotSeat;
using System.Diagnostics;

namespace Armies.TemplateMethods
{
    public class NextTurnInterceptor : INextTurnModelInterceptor, INextTurnViewInterceptor
    {
        private static NextTurnInterceptor _Instance;
        public static NextTurnInterceptor Instance {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new NextTurnInterceptor();
                }
                return _Instance;
            } 
        }

        private NextTurnInterceptor() {
            ElephantSound = new UriEffect("Eleph", "\\Units\\Effects");
        }

        private UriEffect ElephantSound;
        public FrightenElefantStorage frightenElefantStorage { get; set; }

        public void OnChangingToNextTurnInModelGame(Game game, out object objectForTheView)
        {
            game.IsChangingToNextTurn = true;
            Stack elefantStack = new Stack();
            // Random throws the same values if random() is in a loop
            Stack RandomDirections = new Stack();
            Random directionRandom = new Random();
            foreach (Elephant ele in game.ListBlues.Union(game.ListReds).Where(x => x is Elephant))
            {
                elefantStack.Push(ele);
                for (byte i = 0; i < 2; i++)
                {
                    RandomDirections.Push(directionRandom.Next(0, 6));
                }
            }
            FrightenElefantStorage listElefantAttackStorage = new FrightenElefantStorage();
            while (elefantStack.Count > 0)
            {
                Stack RandomDirectionsParticular = new Stack();
                for (byte i = 0; i < 2; i++)
                {
                    RandomDirectionsParticular.Push(RandomDirections.Pop());
                }
                Elephant ele = (Elephant)elefantStack.Pop();
                ele.NextTurnElefant(RandomDirectionsParticular, listElefantAttackStorage, game.ActionsLoader);
            }
            game.IsChangingToNextTurn = false;
            frightenElefantStorage = listElefantAttackStorage;
            objectForTheView = listElefantAttackStorage;
        }

        public void OnChangedToNextTurnInHotSeat(GamePage hotSeatPage)
        {
            if (frightenElefantStorage == null)
                return;

            Thread thread = new Thread(() => {
                hotSeatPage.autoResetEventEle.WaitOne();
                Stack<UnitsToRelocate> listUnitsRelocate = new Stack<UnitsToRelocate>();
                Stack<ViewUnit> fakeUnits = new Stack<ViewUnit>();
                hotSeatPage.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    
                    IEnumerable<ViewTerrain> listWithUnitsToHide = hotSeatPage.ListTerrains.Where(a => (frightenElefantStorage.Units.Any(b => b.X == a.X && b.Y == a.Y) ||
                        frightenElefantStorage.states.Any(b => b.XfinalEle == a.X && b.YfinalEle == a.Y)) && a.Unit != null);
                    foreach (ViewTerrain terr in listWithUnitsToHide)
                    {
                        listUnitsRelocate.Push(new UnitsToRelocate(terr.X, terr.Y, terr.unitview));
                        terr.Unit = null;
                    }
                    foreach (ElefantAttackUnit uni in frightenElefantStorage.Units)
                    {
                        ViewModelUnit viewModelUnit = new ViewModelUnit(null, hotSeatPage.ViewModel);
                        viewModelUnit.UnitType = hotSeatPage.ViewModel.listArmiesOb.SelectMany(x => x.Units)
                            .FirstOrDefault(x => x.GetUnitType() == uni.UnitType);

                        ViewUnit unit = new ViewUnit(hotSeatPage.ViewModel, viewModelUnit);
                        unit.AffilationUnitView = uni.Color;
                        unit.LifeUnitView = (byte)(100 - uni.Life);
                        unit.MoralUnitView = (byte)(100 - uni.Moral);

                        //unit.UriUnitType = ViewUnitType.unitTypes[uni.UnitType].Uri;
                        unit.ModifierUnitView = uni.modifier;
                        ViewTerrain terrain = hotSeatPage.ListTerrains.FirstOrDefault(b => 
                            b.X == uni.X && b.Y == uni.Y);
                        terrain.unitview = unit;
                        fakeUnits.Push(unit);
                    }
                });

                Debug.WriteLine("elefantAttackStorage.states.Count: " + frightenElefantStorage.states.Count);
                while (frightenElefantStorage.states.Count > 0)
                {
                    ParticularEleAttack partAtt = frightenElefantStorage.states.Dequeue();

                    ElefantAttackUnit currentEle = partAtt.Elefant;
                    while (partAtt.states.Count > 0)
                    {
                        State sta = partAtt.states.Dequeue();

                        Thread.Sleep(300);

                        Debug.WriteLine("currentEle.X: " + currentEle.X + "; currentEle.Y: " + currentEle.Y);

                        ViewTerrain terrain = hotSeatPage.ListTerrains.FirstOrDefault(b => b.X == currentEle.X && b.Y == currentEle.Y);
                        List<ViewTerrain> adjacents = FieldsRelationsView.AdjacentTerrains(terrain, hotSeatPage);
                        hotSeatPage.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            foreach (ViewTerrain terr in adjacents)
                            {
                                terr.movementposible = true;
                            }
                            terrain.Unit.Selected = true;
                        });
                        Thread.Sleep(80);
                        ViewTerrain terrBefore = null;
                        foreach (ViewTerrain terr in adjacents)
                        {
                            hotSeatPage.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                            {
                                if (terrBefore != null)
                                    terrBefore.focusedmovement = false;
                                terr.focusedmovement = true;
                                terrBefore = terr;
                            });
                            Thread.Sleep(100);
                        }
                        ViewTerrain terrainselected = null;
                        hotSeatPage.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            terrBefore.focusedmovement = false;
                            terrainselected = hotSeatPage.ListTerrains.FirstOrDefault(b => b.X == sta.X && b.Y == sta.Y);
                            terrainselected.focusedmovement = true;
                        });
                        Thread.Sleep(500);
                        hotSeatPage.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            terrainselected.focusedmovement = false;
                            foreach (ViewTerrain terr in adjacents)
                            {
                                terr.movementposible = false;
                            }

                            if (terrainselected.Unit != null)
                            {
                                IEffects iEffects = BasicMechanisms.Kernel.Get<IEffects>();
                                iEffects.Play(ElephantSound.EffectUri, iEffects.AttackEffectsMediaPlayer);
                            }

                            ViewUnit ele = terrain.Unit;
                            ele.LifeUnitView = (byte)(100 - sta.Elefant.Life);
                            ele.MoralUnitView = (byte)(100 - sta.Elefant.Moral);
                            if (partAtt.Elefant.X != sta.Elefant.X || partAtt.Elefant.Y != sta.Elefant.Y)
                            {
                                ViewTerrain terraintoMove = hotSeatPage.ListTerrains.FirstOrDefault(b => b.X == sta.Elefant.X && b.Y == sta.Elefant.Y);
                                terraintoMove.unitview = null;
                                terrain.unitview = null;
                                terraintoMove.unitview = ele;
                            }

                            if (sta.Enemy != null)
                            {
                                ViewUnit ene = (ViewUnit)hotSeatPage.ListTerrains.FirstOrDefault(b => b.X == sta.Enemy.X && b.Y == sta.Enemy.Y).Unit;
                                if (ene != null)
                                {
                                    ene.LifeUnitView = (byte)(100 - sta.Enemy.Life);
                                    ene.MoralUnitView = (byte)(100 - sta.Enemy.Moral);
                                    ene.ModifierUnitView = sta.Enemy.modifier;
                                }
                            }
                        });

                        Thread.Sleep(250);

                        currentEle = sta.Elefant;
                    }
                }

                hotSeatPage.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    Debug.WriteLine("fakeUnits.Count: " + fakeUnits.Count);
                    while (fakeUnits.Count > 0)
                    {
                        ViewUnit unit = fakeUnits.Pop();
                        if (unit.Terrain != null)
                        {
                            unit.Terrain.unitview = null;
                        }
                    }
                    while (listUnitsRelocate.Count > 0)
                    {
                        UnitsToRelocate unitRelocate = listUnitsRelocate.Pop();
                        ViewTerrain terrain = hotSeatPage.ListTerrains.FirstOrDefault(b => b.X == unitRelocate.X && b.Y == unitRelocate.Y);
                        terrain.unitview = unitRelocate.uni;
                    }
                });

                frightenElefantStorage = null;
            });
            hotSeatPage.autoResetEventEle.Reset();                        
            thread.Start();
        }
    }
}
