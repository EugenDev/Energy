using System;
using System.Windows.Media;

namespace Energy.UI.Controls
{
    public class ControlsFactory
    {
        public event EventHandler<WantDeleteControlEventArgs> WantDeleteControl;
        public event EventHandler<WantDeleteLinkEventArgs> WantDeleteLink;
        
        public ControlBase CreateControl(ElementType elementType, string name)
        {
            ControlBase result;

            switch (elementType)
            {
                case ElementType.Station:
                    result = new Station(name);
                    ConfigureControl(result);
                    return result;
                case ElementType.Consumer:
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
            WantDeleteLink?.Invoke(this, new WantDeleteLinkEventArgs(sender as Link));
        }

        private void Control_WantDelete(object sender, EventArgs e)
        {
            WantDeleteControl?.Invoke(this, new WantDeleteControlEventArgs(sender as ControlBase));
        }
    }

    public class WantDeleteControlEventArgs : EventArgs
    {
        public ControlBase Control { get; }

        public WantDeleteControlEventArgs(ControlBase control)
        {
            Control = control;
        }
    }

    public class WantDeleteLinkEventArgs : EventArgs
    {
        public Link Link { get; }

        public WantDeleteLinkEventArgs(Link link)
        {
            Link = link;
        }
    }
}
