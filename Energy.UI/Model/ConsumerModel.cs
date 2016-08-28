using System.Collections.ObjectModel;
using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class ConsumerModel : ModelBase
    {
        public ConsumerModel(string name, ObservableCollection<string> featuresNames) 
            : base(name, featuresNames)
        {
            ParticipantType = ParticipantType.Consumer;
        }
    }
}
