using System.Collections.ObjectModel;
using System.Windows.Data;
using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class TaskModel
    {
        public FeaturesModel FeaturesModel { get; }
        public GraphicModel GraphicModel { get; }

        public TaskModel()
        {
            FeaturesModel = new FeaturesModel();
            GraphicModel = new GraphicModel();
        }

        public void AddParticipant(string name, ParticipantType participantType)
        {
            //TODO: Check uniqueness

            if (participantType == ParticipantType.Station)
                GraphicModel.Models.Add(new StationModel(name));
            else
                GraphicModel.Models.Add(new ConsumerModel(name));

            FeaturesModel.AddParticipant(name, participantType);
        }

        public void AddFeature(string featureName)
        {
            FeaturesModel.AddFeature(featureName);
        }

        public void AddLink(ModelBase from, ModelBase to)
        {
            GraphicModel.Links.Add(new LinkModel(from, to, 10, 10));
        }

        public void DeleteStation(StationModel station)
        {
            FeaturesModel.RemoveStation(station.Name);
            GraphicModel.Models.Remove(station);
        }

        public void DeleteConsumer(ConsumerModel consumer)
        {
            FeaturesModel.RemoveConsumer(consumer.Name);
            GraphicModel.Models.Remove(consumer);
        }
    }
}
