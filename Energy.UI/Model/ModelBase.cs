using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Energy.UI.Annotations;
using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class ModelBase : FeaturedItem, INotifyPropertyChanged
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

        public ParticipantType ParticipantType { get; set; }

        protected ModelBase(string name, ObservableCollection<string>featuresNames)
            :base(name, featuresNames)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
