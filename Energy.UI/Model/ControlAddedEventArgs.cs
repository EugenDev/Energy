using Energy.UI.Controls;

namespace Energy.UI.Model
{
    public class ControlAddedEventArgs : ObjectEventArgs<ControlBase>
    {
        public ParticipantType ParticipantType { get; }

        public ControlAddedEventArgs(ControlBase item, ParticipantType participantType) 
            : base(item)
        {
            ParticipantType = participantType;
        }
    }
}