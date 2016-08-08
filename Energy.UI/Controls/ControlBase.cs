using System.Windows.Controls;

namespace Energy.UI.Controls
{
    public class ControlBase : UserControl, IControl
    {
        public ControlBase()
        {
            X = Y = 0;
        }

        public double X
        {
            get { return Canvas.GetLeft(this); }
            set { Canvas.SetLeft(this, value); }
        }

        public double Y
        {
            get { return Canvas.GetTop(this); }
            set { Canvas.SetTop(this, value); }
        }
    }
}
