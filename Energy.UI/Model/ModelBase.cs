﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Energy.UI.Annotations;

namespace Energy.UI.Model
{
    public class ModelBase :INotifyPropertyChanged
    {
        private double _x;
        public double X
        {
            get { return _x; }
            set { _x = value; OnPropertyChanged(); }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set { _y = value; OnPropertyChanged(); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); }
        }
        
        public string Name { get; set; }

        protected ModelBase()
        {
        }

        protected ModelBase(string name)
        {
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
