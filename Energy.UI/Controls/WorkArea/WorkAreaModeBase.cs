using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Energy.UI.Model;

namespace Energy.UI.Controls.WorkArea
{
    public abstract class WorkAreaModeBase
    {
        protected readonly GraphControl GraphControl;
        
        protected WorkAreaModeBase(GraphControl graphControl)
        {
            GraphControl = graphControl;
        }

        protected void HitTest()
        {
            var hitPoint = Mouse.GetPosition(GraphControl);
            VisualTreeHelper.HitTest(GraphControl,
                null,
                result =>
                {
                    var element = GetHittedItem(result.VisualHit);
                    if(element != null)
                        ProcessHitTest(element);
                    return HitTestResultBehavior.Stop;
                },
                new PointHitTestParameters(hitPoint));
        }

        private ModelBase GetHittedItem(DependencyObject currentElement)
        {
            while (true)
            {
                var parent = VisualTreeHelper.GetParent(currentElement);

                if (parent == null)
                    return null;

                if (parent is ContentPresenter)
                    return (parent as ContentPresenter).DataContext as ModelBase;

                currentElement = parent;
            }
        }

        protected virtual void ProcessHitTest(ModelBase element) { }

        public virtual void ProcessKeyUp(KeyEventArgs args) { }

        public virtual void ProcessKeyDown(KeyEventArgs args) { }

        public virtual void MouseLeftButtonUp() { }

        public virtual void MouseLeftButtonDown() { }

        public virtual void Cleanup() { }
    }
}