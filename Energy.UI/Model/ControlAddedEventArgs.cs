using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class ControlAddedEventArgs : ObjectEventArgs<ControlBase>
    {
        public ControlType ControlType { get; }

        public ControlAddedEventArgs(ControlBase item, ControlType controlType) 
            : base(item)
        {
            ControlType = controlType;
        }
    }
}