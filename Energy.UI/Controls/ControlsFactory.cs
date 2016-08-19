using System;
using System.Windows.Media;

namespace Energy.UI.Controls
{
    public class ControlsFactory
    {
        public event EventHandler<ObjectEventArgs<ControlBase>> WantDeleteControl;
        public event EventHandler<ObjectEventArgs<Link>> WantDeleteLink;
        
        public ControlBase CreateControl(ControlType controlType, string name)
        {
            ControlBase result;

            switch (controlType)
            {
                case ControlType.Station:
                    result = new Station(name);
                    ConfigureControl(result);
                    return result;
                case ControlType.Consumer:
                    result = new Consumer(name);
                    ConfigureControl(result);
                    return result;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void ConfigureControl(ControlBase control)
        {
            control.WantDelete += Control_WantDelete;
        }

        public Link CreateLink(ControlBase from, ControlBase to, double distance)
        {
            var result = new Link(from, to, distance);
            result.WantDelete += Link_WantDelete;
            result.Stroke = Brushes.DarkRed;
            result.StrokeThickness = 2;
            return result;
        }

        private void Link_WantDelete(object sender, EventArgs e)
        {
            WantDeleteLink?.Invoke(this, new ObjectEventArgs<Link>(sender as Link));
        }

        private void Control_WantDelete(object sender, EventArgs e)
        {
            WantDeleteControl?.Invoke(this, new ObjectEventArgs<ControlBase>(sender as ControlBase));
        }
    }
}
