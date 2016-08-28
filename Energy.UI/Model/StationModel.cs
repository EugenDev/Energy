using System.Collections.ObjectModel;
using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class StationModel : ModelBase
    {
        public StationModel(string name, ObservableCollection<string> featuresNames) 
            : base(name, featuresNames)
        {
            ParticipantType = ParticipantType.Station;
        }
    }
}
