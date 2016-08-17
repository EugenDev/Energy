using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Energy.UI.Controls
{
    public class Link : Shape
    {
        private readonly ControlBase _from;
        private readonly ControlBase _to;
        public double Distance { get; private set; }

        public Link(ControlBase from, ControlBase to, double distance)
        {
            _from = from;
            _to = to;
            _from.PositionChanged += EndPointPositionChanged;
            _to.PositionChanged += EndPointPositionChanged;

            Distance = distance;
            ToolTip = new LinkToolTip(this);

            Panel.SetZIndex(this, -1);
        }

        private void EndPointPositionChanged(object sender, EventArgs eventArgs)
        {
            InvalidateVisual();
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var fromX = _from.X + _from.Width/2;
                var fromY = _from.Y + _from.Height/2;

                var toX = _to.X + _to.Width/2;
                var toY = _to.Y + _to.Height/2;
                return new LineGeometry
                {
                    StartPoint = new Point {X = fromX, Y = fromY},
                    EndPoint = new Point {X = toX, Y = toY}
                };
            }
        }
    }
}
