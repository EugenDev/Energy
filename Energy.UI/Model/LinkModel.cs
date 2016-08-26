using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using Energy.UI.Annotations;

namespace Energy.UI.Model
{
    public class LinkModel : INotifyPropertyChanged
    {
        private double _distance;
        private double _conduction;

        public double Distance
        {
            get { return _distance; }
            set { _distance = value; OnPropertyChanged(); }
        }

        public double Conduction
        {
            get { return _conduction; }
            set { _conduction = value; OnPropertyChanged(); }
        }
        
        public ModelBase From { get; }
        public ModelBase To { get; }

        public LinkModel(ModelBase from, ModelBase to, double distance, double conduction)
        {
            From = from;
            To = to;
            _distance = distance;
            _conduction = conduction;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
