using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameFrontend;
using Moq;
using GameFrontend.ViewModel;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameFrontend.View.Main;
using GameFrontend.View;
using GameFrontend.Controller.HotSeat;
using Armies;
using ModelBaseElements;
using GameFrontend.View.Resources;

namespace UnitTest
{
    [TestClass]
    public class TerrainTest
    {
        [TestMethod]
        public void CreateTerrain()
        {

            HotSeatModelView Main = new HotSeatModelView();
            
            TerrainView terrainview = new TerrainView(1, 1);

            terrainview.MVterrain.terrain.deploymentArea = ArmyColor.Red;
            
            TerrainView terrainview2 = new TerrainView(1,2);;
            terrainview2.MVterrain.terrain.typeTerrain = TypeTerrainClass.forest;
            terrainview2.MVterrain.terrain.deploymentArea = ArmyColor.Blue;

            Assert.AreEqual(Game.game.Map.Count, 2);

            Assert.AreEqual(terrainview.typeTerrainView, TerrainType.Plain);            
            Assert.AreEqual(terrainview.deploymentAreaView, ArmyColor.Red);
            Assert.AreEqual(terrainview.MVterrain.terrain.Y, 1);

            Assert.AreEqual(terrainview2.typeTerrainView, TerrainType.Forest);            
            Assert.AreEqual(terrainview2.deploymentAreaView, ArmyColor.Blue);
            Assert.AreEqual(terrainview2.MVterrain.terrain.Y, 2);
        }
    }

    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void CreateUnit()
        {
            string Unittype;
            Unittype = BasicMechanisms.RemovePrefixType(Convert.ToString(typeof(Elephant)));
            Uri uri = new Uri(@"https://elpais.com/");
            ViewUnitType.unitTypes.Add(Unittype, new ViewUnitType(uri,
                Unittype));

            HotSeatModelView Main = new HotSeatModelView();

            Elephant ele = new Elephant() { heerzugeh = HeerZugehoerigkeit.Blue };
            UnitViewModel unitModelView = new UnitViewModel(ele);
            UnitView unitview = new UnitView() { unit = unitModelView };

            Assert.AreEqual(unitview.UriUnitType, uri);
            Assert.AreEqual(unitview.affilationUnitView, ArmyAffilation.Blue);
            Assert.AreEqual(unitModelView.lifeUnitViewModel, 100);
            Assert.AreEqual(unitview.lifeUnitView, 100 - 100);
            Assert.AreEqual(unitview.moralUnitView, 100 - 100);
        }

        [TestMethod]
        public void CreateUnitFromSave()
        {
            string Unittype;
            Unittype = BasicMechanisms.RemovePrefixType(Convert.ToString(typeof(Elephant)));
            Uri uri = new Uri(@"https://elpais.com/");
            ViewUnitType.unitTypes.Add(Unittype, new ViewUnitType(uri,
                Unittype));

            HotSeatModelView Main = new HotSeatModelView();

            Elephant ele = new Elephant() { heerzugeh = HeerZugehoerigkeit.Blue };
            ele.heerzugeh = HeerZugehoerigkeit.Blue;
            ele.LebenRest = 80;
            UnitViewModel unitModelView = new UnitViewModel(ele);
            UnitView unitview = new UnitView() { unit = unitModelView };

            Assert.AreEqual(unitview.UriUnitType, uri);
            Assert.AreEqual(unitview.affilationUnitView, ArmyAffilation.Blue);
            Assert.AreEqual(unitview.lifeUnitView, 100- ele.LebenRest);
        }

        [TestMethod]
        public void PropertiesUnit()
        {
            string Unittype;
            Unittype = BasicMechanisms.RemovePrefixType(Convert.ToString(typeof(Elephant)));
            Uri uri = new Uri(@"https://elpais.com/");
            ViewUnitType.unitTypes.Add(Unittype, new ViewUnitType(uri,
                Unittype));

            HotSeatModelView Main = new HotSeatModelView();

            Elephant ele = new Elephant() { heerzugeh = HeerZugehoerigkeit.Blue };
            UnitViewModel unitModelView = new UnitViewModel(ele);
            UnitView unitview = new UnitView() { unit = unitModelView };

            ele.LebenRest = 80;
            Assert.AreEqual(unitview.lifeUnitView, 100- ele.LebenRest);
            ele.MoralRest = 85;
            Assert.IsTrue(unitview.moralUnitView > 100- 85);
            ele.BewegungRest = (byte)2;
            Assert.AreEqual(unitview.movementUnitView, ViewMovement.Yellow);
            ele.fliehend = true;
            Assert.AreEqual(unitview.modifierUnitView, Modifier.Fright);
        }

        [TestMethod]
        public void MoveUnit()
        {
            string Unittype;
            Unittype = BasicMechanisms.RemovePrefixType(Convert.ToString(typeof(Elephant)));
            Uri uri = new Uri(@"https://elpais.com/");
            ViewUnitType.unitTypes.Add(Unittype, new ViewUnitType(uri,
                Unittype));

            HotSeatModelView Main = new HotSeatModelView();

            Elephant ele = new Elephant() { heerzugeh = HeerZugehoerigkeit.Blue };
            UnitViewModel unitModelView = new UnitViewModel(ele);
            UnitView unitview = new UnitView() { unit = unitModelView };
            Game game = new Game();
            TerrainView terrainview1 = new TerrainView(1, 1);
            TerrainView terrainview2 = new TerrainView(2, 2);

            terrainview1.MVterrain.terrain.unitInTerrain = ele;
            Assert.AreEqual(terrainview1.MVterrain.unit, unitModelView);
            Assert.AreEqual(terrainview1.MVterrain.terrain.unitInTerrain, ele);

            terrainview2.MVterrain.terrain.unitInTerrain = ele;
            Assert.AreEqual(terrainview2.MVterrain.unit, unitModelView);
            Assert.AreNotEqual(terrainview1.MVterrain.unit, unitModelView);
            Assert.AreEqual(terrainview2.MVterrain.terrain.unitInTerrain, ele);
            Assert.AreNotEqual(terrainview1.MVterrain.terrain.unitInTerrain, ele);
        }

        [TestMethod]
        public void DisposeFromModel()
        {
            string Unittype;
            Unittype = BasicMechanisms.RemovePrefixType(Convert.ToString(typeof(Elephant)));
            Uri uri = new Uri(@"https://elpais.com/");
            ViewUnitType.unitTypes.Add(Unittype, new ViewUnitType(uri,
                Unittype));

            HotSeatModelView mainWindowMV = new HotSeatModelView();
            
            TerrainView terrainview = new TerrainView(1,1);
                    
            TerrainView terrainview1 = new TerrainView(1,2);

            UnitView unitview = new UnitView() { unit = new UnitViewModel(new Elephant() { heerzugeh = HeerZugehoerigkeit.Red }) };
            terrainview1.MVterrain.terrain.unitInTerrain = unitview.unit.unit;

            Assert.AreEqual(terrainview1.MVterrain.unit, unitview.unit);
            Assert.AreEqual(Game.game.ListReds.Count, 1);

            UnitModel.DisposeUnitModel(unitview.unit.unit);

            Assert.AreEqual(Game.game.ListReds.Count, 0);
            Assert.IsNull(terrainview1.MVterrain.unit);
            Assert.IsNull(terrainview1.MVterrain.unit);
        }

        [TestMethod]
        public void DisposeFromViewModel()
        {
            string Unittype;
            Unittype = BasicMechanisms.RemovePrefixType(Convert.ToString(typeof(Elephant)));
            Uri uri = new Uri(@"https://elpais.com/");
            ViewUnitType.unitTypes.Add(Unittype, new ViewUnitType(uri,
                Unittype));

            HotSeatModelView mainWindowMV = new HotSeatModelView();
            
            TerrainView terrainview = new TerrainView(1,1);
            
            TerrainView terrainview1 = new TerrainView(1,2);

            UnitView unitview = new UnitView() { unit = new UnitViewModel(new Elephant() { heerzugeh = HeerZugehoerigkeit.Red }) };
            terrainview1.MVterrain.terrain.unitInTerrain = unitview.unit.unit;

            Assert.AreEqual(terrainview1.MVterrain.unit, unitview.unit);
            Assert.AreEqual(Game.game.ListReds.Count, 1);

            unitview.unit.DisposeUnitViewModelFromViewModel();

            Assert.AreEqual(Game.game.ListReds.Count, 0);
            Assert.IsNull(terrainview1.MVterrain.unit);
            Assert.IsNull(terrainview1.MVterrain.unit);
        }
    }
}
