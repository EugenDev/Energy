using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Energy.UI.Controls.WorkArea
{
    public class DragMode : WorkAreaModeBase
    {
        private readonly Point _startPoint;
        private readonly List<ControlBase> _controls;

        public DragMode(WorkArea workArea, Point startPoint, List<ControlBase> controls) : base(workArea)
        {
            _startPoint = startPoint;
            _controls = controls;
            Canvas.MouseMove += MouseMoveHandler;
        }

        private void MouseMoveHandler(object sender, MouseEventArgs mouseEventArgs)
        {
            var mousePosition = GetCurrentPoint();
            foreach (var control in _controls)
            {
                control.Y += mousePosition.Y - _startPoint.Y;
                control.X += mousePosition.X - _startPoint.X;
            }
        }

        public override void MouseLeftButtonUp()
        {
            Canvas.MouseMove -= MouseMoveHandler;
            WorkArea.SetMode(new StartMode(WorkArea));
        }
    }
}
