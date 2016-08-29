using System.Collections.ObjectModel;
using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class ModelBase : FeaturedItem
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
    }
}
