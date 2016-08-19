using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class ControlRemovedEventArgs : ObjectEventArgs<ControlBase>
    {
        public ControlType ControlType { get; }

        public ControlRemovedEventArgs(ControlBase item, ControlType controlType) 
            : base(item)
        {
            ControlType = controlType;
        }
    }
}