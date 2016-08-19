using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Energy.UI.Controls.WorkArea
{
    public abstract class WorkAreaModeBase
    {
        protected readonly GraphControl GraphControl;
        protected Canvas Canvas => GraphControl.Canvas;

        protected Point GetCurrentPoint()
        {
            return Mouse.GetPosition(Canvas);
        }

        protected WorkAreaModeBase(GraphControl graphControl)
        {
            GraphControl = graphControl;
        }

        protected void HitTest()
        {
            var canvasHitPoint = Mouse.GetPosition(Canvas);
            VisualTreeHelper.HitTest(Canvas,
                null,
                result =>
                {
                    if (!(result.VisualHit is ControlBase))
                        return HitTestResultBehavior.Continue;
                    ProcessHitTest(result.VisualHit as ControlBase);
                    return HitTestResultBehavior.Stop;
                },
                new PointHitTestParameters(canvasHitPoint));
        }

        protected virtual void ProcessHitTest(ControlBase hitElement) { }

        public virtual void ProcessKeyUp(KeyEventArgs args) { }

        public virtual void ProcessKeyDown(KeyEventArgs args) { }

        public virtual void MouseLeftButtonUp() { }

        public virtual void MouseLeftButtonDown() { }

        public virtual void Cleanup() { }
    }
}