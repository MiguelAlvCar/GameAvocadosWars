﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ArmyAndUnitTypes
{
    public interface IListArmiesMainModelView
    {
        ObservableCollection<ArmyType> listArmiesObSave { set; get; }
    }
}