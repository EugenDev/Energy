using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Energy.UI.Controls
{
    public class Link : Shape
    {
        public ControlBase From { get; }
        public ControlBase To { get; }
        public double Distance { get; private set; }

        public Link(ControlBase from, ControlBase to, double distance)
        {
            From = from;
            To = to;
            From.PositionChanged += EndPointPositionChanged;
            To.PositionChanged += EndPointPositionChanged;

            Distance = distance;
            ToolTip = new LinkToolTip(this);
            ContextMenu = CreateContextMenu();

            Panel.SetZIndex(this, -1);
        }

        public void Disconnect()
        {
            From.PositionChanged -= EndPointPositionChanged;
            To.PositionChanged -= EndPointPositionChanged;
        }

        private void EndPointPositionChanged(object sender, EventArgs eventArgs)
        {
            InvalidateVisual();
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var fromX = From.X + From.Width/2;
                var fromY = From.Y + From.Height/2;

                var toX = To.X + To.Width/2;
                var toY = To.Y + To.Height/2;
                return new LineGeometry
                {
                    StartPoint = new Point {X = fromX, Y = fromY},
                    EndPoint = new Point {X = toX, Y = toY}
                };
            }
        }

        public event EventHandler WantDelete;

        private void OnWantDelete()
        {
            WantDelete?.Invoke(this, EventArgs.Empty);
        }

        private ContextMenu CreateContextMenu()
        {
            var result = new ContextMenu();
            var deleteItem = new MenuItem { Header = "Удалить" };
            deleteItem.Click += (sender, args) => OnWantDelete();
            result.Items.Add(deleteItem);
            return result;
        }
    }
}
