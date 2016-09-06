using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class ConsumerModel : ModelBase
    {
        public ConsumerModel(string name, ObservableCollection<string> featuresNames) 
            : base(name, featuresNames)
        {
            ParticipantType = ParticipantType.Consumer;
            Zones = new List<StationModel>();
        }

        public string ZonesDescription => Zones.Count == 0 
            ? "Не находится ни в чьей зоне влияния"
            : "Находится в зоне влияния:\n" + string.Join("\n", Zones.Select(i => i.Name));

        public List<StationModel> Zones { get; }
    }
}
