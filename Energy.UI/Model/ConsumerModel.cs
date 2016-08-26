using System.Collections.ObjectModel;

namespace Energy.UI.Model
{
    public class ConsumerModel : ModelBase
    {
        public ConsumerModel(string name, ObservableCollection<string> featuresNames) 
            : base(name, featuresNames)
        {
        }
    }
}
