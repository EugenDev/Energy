using System.Collections.ObjectModel;
using System.Windows.Media;
using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class StationModel : ModelBase
    {
        public Color Color { get; }

        public StationModel(string name, ObservableCollection<string> featuresNames, Color color) 
            : base(name, featuresNames)
        {
            Color = color;
            ParticipantType = ParticipantType.Station;
        }

        private Brush _tagBrush;
        public Brush TagBrush
        {
            get
            {
                if(_tagBrush == null)
                    _tagBrush = new SolidColorBrush(Color);
                return _tagBrush;
            }
        }
    }
}
