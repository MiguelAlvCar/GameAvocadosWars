using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BasicElements;
using System.ComponentModel;
using System.Linq;

namespace Model
{
    [Serializable()]
    public class Terrain : ModelBase, ITerrainTypeInterface
    {
        static public Action<Terrain> _addMap;
        static public event Action<Terrain> AddMap
        {
            add { _addMap += value; }
            remove { _addMap -= value; }
        }

        public static int XXX = 106;
        public static int YYY = 92;

        public Terrain()
        {
            _addMap(this);
        }

        [NonSerialized()]
        private Func<object> _GetViewModelTerrain;
        public event Func<object> GetViewModelTerrain
        {
            add { _GetViewModelTerrain += value; }
            remove { _GetViewModelTerrain -= value; }
        }
        public object OnGetViewModelTerrain()
        {
            if (_GetViewModelTerrain != null)
                return _GetViewModelTerrain();
            else return null;
        }

        private UnitModel _unitInTerrain;
        public UnitModel unitInTerrain
        {
            get => _unitInTerrain;
            set
            {
                if (value != null)
                {
                    if (value.InTerrain != null)
                        value.InTerrain.unitInTerrain = null;
                    value.InTerrain = this;
                }
                SetProperty(ref _unitInTerrain, value);
            }
        }

        private ArmyColor _deploymentArea;
        public ArmyColor deploymentArea { get => _deploymentArea; set => SetProperty(ref _deploymentArea, value); }
        
        private TypeTerrainClass _typeTerrain;
        public TypeTerrainClass typeTerrain { get => _typeTerrain;
            set {
                if (value == TypeTerrainClass.see)
                {
                    bridge = false;
                    riverEndsClear();
                }
                SetProperty(ref _typeTerrain, value);
            }
        }

        public TerrainType terrainType { get { return _typeTerrain.terrainType; }
            set
            {
                _typeTerrain.terrainType = value;
                if (value == TerrainType.See)
                {
                    bridge = false;
                    riverEndsClear();
                }
            }
        }

        internal short? alteredMovementCost;
        public short? MovementCost {
            get
            {
                if (alteredMovementCost == null)
                    return _typeTerrain.MovementCost;
                else
                    return alteredMovementCost;
            }
        }

        internal double? alteredDefendBonus;
        public double? defendBonus {
            get
            {
                if (alteredDefendBonus != null)
                {
                    if (_typeTerrain.defendBonus == null)
                        return 1 + alteredDefendBonus;
                    else
                        return _typeTerrain.defendBonus + alteredDefendBonus;
                }
                return _typeTerrain.defendBonus;
            }
        }

        internal double? alteredAttackBonus;
        public double? attackBonus
        {
            get
            {
                if (alteredAttackBonus != null)
                {
                    return _typeTerrain.attackBonus + alteredAttackBonus;
                }
                return _typeTerrain.attackBonus;
            }
        }

        public byte ID { get { return _typeTerrain.ID; } set { _typeTerrain.ID = value; } }
          
        public short X { get; set; }
        public short Y { get; set; }
        public int XX { get; set; }
        public int YY { set; get; }

        private List<Terrain> _adjacents;
        public List<Terrain> Adjacents
        {
            set => _adjacents = value;
            get => _adjacents.ToList();
        }

        [NonSerialized()]
        private bool _focusedmovement;
        public bool focusedmovement
        {
            get => _focusedmovement;
            set
            {
                SetProperty(ref _focusedmovement, value);
            }
        }

        [NonSerialized()]
        private bool _focusedfire;
        public bool focusedfire
        {
            get => _focusedfire;
            set
            {
                SetProperty(ref _focusedfire, value);
            }
        }

        [NonSerialized()]
        private bool _fireposible;
        public bool fireposible
        {
            get => _fireposible;
            set
            {
                SetProperty(ref _fireposible, value);
            }
        }

        [NonSerialized()]
        private bool _movementposible;
        public bool movementposible
        {
            get => _movementposible;
            set
            {                
                SetProperty(ref _movementposible, value);
            }
        }

        [NonSerialized()]
        private bool _hide;
        public bool Hide
        {
            get => _hide;
            set
            {
                SetProperty(ref _hide, value);
            }
        }

        [NonSerialized()]
        private Action<int> _riverAdded;
        public event Action<int> riverAdded
        {
            add { _riverAdded += value; }
            remove { _riverAdded -= value; }
        }
        [NonSerialized()]
        private Action _riverCleared;
        public event Action riverCleared
        {
            add { _riverCleared += value; }
            remove { _riverCleared -= value; }
        }
        // In order to set right the river and the possible bridge
        public bool river;
        private int? _riverEnd1;
        private int? riverEnd1
        {
            get => _riverEnd1;
            set
            {
                _riverEnd1 = value;
                river = _riverEnd1 != null;
            }
        }
        private int? riverEnd2;
        private List<int> _riverEnds = new List<int>();
        public void riverEndsAdd (int num)
        {
            if (_riverEnds.Count <= 5 && !_riverEnds.Contains(num) 
                && terrainType != TerrainType.See)
            {
                if (_riverEnds.Count == 0)
                {
                    alteredAttackBonus = -0.3;
                    alteredDefendBonus = -0.3;
                    alteredMovementCost = 9;
                    riverEnd1 = num;
                }
                else if (_riverEnds.Count == 1)
                {
                    riverEnd2 = num;
                }
                _riverEnds.Add(num);
                _riverAdded(num);
            }            
        }
        public void riverEndsClear()
        {
            alteredAttackBonus = null;
            alteredDefendBonus = null;
            alteredMovementCost = null;
            _riverCleared();
            riverEnd1 = null;
            riverEnd2 = null;
            _riverEnds.Clear();
            bridge = false;
        }

        private bool _bridge;
        public bool bridge
        {
            get => _bridge;
            set
            {
                if (value && (riverEnd1 != null && riverEnd2 != null))
                {
                    alteredAttackBonus = null;
                    alteredDefendBonus = null;
                    alteredMovementCost = null;
                    SetProperty(ref _bridge, value);
                }                    
                else if (!value)
                {
                    SetProperty(ref _bridge, value);
                    if (riverEnd1 != null)
                    {
                        alteredAttackBonus = -0.3;
                        alteredDefendBonus = -0.3;
                        alteredMovementCost = 9;
                    }

                }                    
            }
        }

        public void AfterDeserialization()
        {
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(typeTerrain)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(deploymentArea)));
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(unitInTerrain)));
            foreach (int riv in _riverEnds)
            {
                _riverAdded(riv);
            }
            _PropertyChanged(this, new PropertyChangedEventArgs(nameof(bridge)));
        }
    }
    
    interface ITerrainTypeInterface
    {
        TerrainType terrainType { get; }
        short? MovementCost { get; }
        byte ID { get; }
        double? defendBonus { get; }
        double? attackBonus { get; }
    }

    [Serializable()]
    public class TypeTerrainClass : ITerrainTypeInterface
    {
        public static TypeTerrainClass plain = new TypeTerrainClass(1, 1, TerrainType.Plain, null, null);
        public static TypeTerrainClass forest = new TypeTerrainClass(9, 2, TerrainType.Forest, 1.33, null);
        public static TypeTerrainClass hill = new TypeTerrainClass(2, 3, TerrainType.Hill, null, 1.25);
        public static TypeTerrainClass hillForest = new TypeTerrainClass(9, 4, TerrainType.HillForest, 1.33, 1.10);
        public static TypeTerrainClass hillCity = new TypeTerrainClass(2, 5, TerrainType.HillCity, 1.40, 1.10);
        public static TypeTerrainClass plainCity = new TypeTerrainClass(1, 6, TerrainType.PlainCity, 1.40, null);
        public static TypeTerrainClass see = new TypeTerrainClass(null, 7, TerrainType.See, null, null);

        public TerrainType terrainType { get; set; }
        public short? MovementCost { get; set; }
        public byte ID { get; set; }
        public double? defendBonus { set; get; }
        public double? attackBonus { set; get; }

        private TypeTerrainClass(short? bewegungkost, byte id, TerrainType _terrainType, double? defendBonus, double? attackBonus)
        {
            terrainType = _terrainType;
            MovementCost = bewegungkost;
            ID = id;
            this.defendBonus = defendBonus;
            this.attackBonus = attackBonus;
        }
    }
}
