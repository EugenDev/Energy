using System;
using System.Collections.ObjectModel;
using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class GraphicModel
    {
        public ObservableCollection<ControlBase> Controls { get; }

        public GraphicModel()
        {
            Controls = new ObservableCollection<ControlBase>();
        }

        private void OnControlAdded(ControlBase control, ControlType controlType)
        {
            ControlAdded?.Invoke(this, new ControlAddedEventArgs(control, controlType));
        }

        public void OnControlRemoved(ControlBase control, ControlType controlType)
        {
            ControlRemoved?.Invoke(this, new ControlRemovedEventArgs(control, controlType));
        }

        public event EventHandler<ControlAddedEventArgs> ControlAdded;
        public event EventHandler<ControlRemovedEventArgs> ControlRemoved;
    }
}
