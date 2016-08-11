using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Energy.UI.Controls.WorkArea
{
    public class DragMode : WorkAreaModeBase
    {
        private readonly Point _startPoint;
        private readonly List<ControlBase> _controls;
        private Vector _previousVector;

        public DragMode(WorkArea workArea, List<ControlBase> controls) : base(workArea)
        {
            _startPoint = GetCurrentPoint();
            _previousVector = new Vector();
            _controls = controls;
            Canvas.MouseMove += MouseMoveHandler;
        }

        private void MouseMoveHandler(object sender, MouseEventArgs mouseEventArgs)
        {
            var mousePosition = GetCurrentPoint();
            var newVector = new Vector(mousePosition.X - _startPoint.X, mousePosition.Y - _startPoint.Y);
            var result = newVector - _previousVector;
            foreach (var control in _controls)
            {
                control.Y += result.Y;
                control.X += result.X;
            }
            _previousVector = newVector;
        }

        public override void MouseLeftButtonUp()
        {
            Canvas.MouseMove -= MouseMoveHandler;
            WorkArea.SetStartMode();
        }
    }
}
