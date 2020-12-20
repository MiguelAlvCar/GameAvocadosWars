
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ModelGame.Actions;
using System.ComponentModel;
using BasicElements;
using System.Collections;
using Ninject;
using Ninject.Parameters;
using BasicElements.AbstractClasses;
using System.Runtime.Serialization.Formatters.Binary;
using ModelGame.Abstract;

namespace ModelGame
{
    [Serializable()]
    public class InitialValues : ModelBase
    {

        private string _player1;
        public string player1
        {
            get => _player1;
            set => SetProperty(ref _player1, value);
        }

        private string _player2;
        public string player2
        {
            get => _player2;
            set => SetProperty(ref _player2, value);
        }

        private ushort? _player1Points;
        public ushort? player1Points
        {
            get => _player1Points;
            set => SetProperty(ref _player1Points, value);
        }

        private ushort? _player2Points;
        public ushort? player2Points { get => _player2Points; set => SetProperty(ref _player2Points, value); }

        private int? _army1;
        public int? Army1 { get => _army1; set => SetProperty(ref _army1, value); }

        private int? _army2;
        public int? Army2 { get => _army2; set => SetProperty(ref _army2, value); }

    }
}
