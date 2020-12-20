using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BasicElements
{
    public delegate void PropertyChangingEventHandler<T>(object sender, T oldvalue, PropertyChangedEventArgs e);

    [Serializable()]
    public abstract class ModelBase: INotifyPropertyChanged, INotifyPropertyChanging
    {
        [NonSerialized()]
        protected PropertyChangedEventHandler _PropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _PropertyChanged += value; }
            remove { _PropertyChanged -= value; }
        }

        [NonSerialized()]
        protected PropertyChangingEventHandler _PropertyChanging;
        public event PropertyChangingEventHandler PropertyChanging
        {
            add { _PropertyChanging += value; }
            remove { _PropertyChanging -= value; }
        }

        protected void OnPropertyChanging([CallerMemberName] string property = null)
        {
            _PropertyChanging(this, new PropertyChangingEventArgs(property));
        }

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string property = null)
        {
            if (Object.Equals(storage, value)) return;
            storage = value;
            if (_PropertyChanged != null)
                _PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}