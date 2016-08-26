using System.Collections.ObjectModel;

namespace Energy.UI.Model
{
    public class StationModel : ModelBase
    {
        public StationModel(string name, ObservableCollection<string> featuresNames) 
            : base(name, featuresNames)
        {
        }
    }
}
