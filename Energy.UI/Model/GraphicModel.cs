using System;
using System.Collections.ObjectModel;
using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class GraphicModel
    {
        public ObservableCollection<ModelBase> Elements { get; }

        public GraphicModel()
        {
            Elements = new ObservableCollection<ModelBase>();
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
