using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Energy.UI.Model
{
    public class GraphicModel
    {
        public CompositeCollection Elements { get; }
        public ObservableCollection<ModelBase> Models { get; }
        public ObservableCollection<LinkModel> Links { get; }

        public GraphicModel()
        {
            Models = new ObservableCollection<ModelBase>();
            Links = new ObservableCollection<LinkModel>();

            Elements = new CompositeCollection
            {
                new CollectionContainer {Collection = Links},
                new CollectionContainer {Collection = Models}
            };
        }


        //private void OnControlAdded(ControlBase control, ParticipantType ParticipantType)
        //{
        //    ControlAdded?.Invoke(this, new ControlAddedEventArgs(control, ParticipantType));
        //}

        //public void OnControlRemoved(ControlBase control, ParticipantType ParticipantType)
        //{
        //    ControlRemoved?.Invoke(this, new ControlRemovedEventArgs(control, ParticipantType));
        //}

        //public event EventHandler<ControlAddedEventArgs> ControlAdded;
        //public event EventHandler<ControlRemovedEventArgs> ControlRemoved;
    }
}
