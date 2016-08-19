using System;
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

            GraphicModel.ControlAdded += GraphicModelOnControlAdded;
            GraphicModel.ControlRemoved += GraphicModelOnControlRemoved;
        }
        
        private void GraphicModelOnControlAdded(object sender, ControlAddedEventArgs controlAddedEventArgs)
        {
            switch (controlAddedEventArgs.ControlType)
            {
                case ControlType.Station:
                    FeaturesModel.AddStation(controlAddedEventArgs.Item.ControlName);
                    break;

                case ControlType.Consumer:
                    FeaturesModel.AddConsumer(controlAddedEventArgs.Item.ControlName);
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private void GraphicModelOnControlRemoved(object sender, ControlRemovedEventArgs controlRemovedEventArgs)
        {
            switch (controlRemovedEventArgs.ControlType)
            {
                case ControlType.Station:
                    FeaturesModel.RemoveStation(controlRemovedEventArgs.Item.ControlName);
                    break;

                case ControlType.Consumer:
                    FeaturesModel.RemoveConsumer(controlRemovedEventArgs.Item.ControlName);
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
