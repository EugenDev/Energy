using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class TaskModel
    {
        public FeaturesModel FeaturesModel { get; }
        public GraphicModel GraphicModel { get; }

        private readonly ControlsFactory _controlsFactory;

        public TaskModel()
        {
            FeaturesModel = new FeaturesModel();
            GraphicModel = new GraphicModel();

            _controlsFactory = new ControlsFactory();
        }

        public void AddParticipant(string name, ParticipantType participantType)
        {
            //TODO: Check uniqueness
            //var control = _controlsFactory.CreateControl(name, ParticipantType);

            if(participantType == ParticipantType.Station)
                GraphicModel.Elements.Add(new StationModel(name));
            else
                GraphicModel.Elements.Add(new ConsumerModel(name));

            FeaturesModel.AddParticipant(name, participantType);
        }

        public void AddFeature(string featureName)
        {
            FeaturesModel.AddFeature(featureName);
        }
    }
}
