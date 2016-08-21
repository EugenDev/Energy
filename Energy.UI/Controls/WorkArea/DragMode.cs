using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Energy.UI.Controls.WorkArea
{
    public class DragMode : WorkAreaModeBase
    {
        private readonly Point _startPoint;
        private Vector _previousVector;
        
        public DragMode(GraphControl graphControl) : base(graphControl)
        {
            _startPoint = Mouse.GetPosition(GraphControl);
            _previousVector = new Vector();
            GraphControl.MouseMove += MouseMoveHandler;
        }

        private void MouseMoveHandler(object sender, MouseEventArgs mouseEventArgs)
        {
            var mousePosition = Mouse.GetPosition(GraphControl);
            var newVector = new Vector(mousePosition.X - _startPoint.X, mousePosition.Y - _startPoint.Y);
            var result = newVector - _previousVector;
            foreach (var control in GraphControl.SelectedElements)
            {
                control.Y += result.Y;
                control.X += result.X;
            }
            _previousVector = newVector;
        }

        public override void MouseLeftButtonUp()
        {
            GraphControl.MouseMove -= MouseMoveHandler;
            GraphControl.SetStartMode();
        }
    }
}
