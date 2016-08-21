using System.Linq;
using System.Windows.Input;
using Energy.UI.Model;

namespace Energy.UI.Controls.WorkArea
{
    public class StartMode : WorkAreaModeBase
    {
        public StartMode(GraphControl graphControl) : base(graphControl)
        {
        }

        public override void MouseLeftButtonUp()
        {
            if(GraphControl.SelectedElements.Count != 0)
                GraphControl.ToggleElementSelection(hitElement);
        }

        public override void MouseLeftButtonDown()
        {
            HitTest();
        }

        public override void ProcessKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                GraphControl.SetMode(new MultiselectMode(GraphControl));
        }

        private ModelBase hitElement;
        protected override void ProcessHitTest(ModelBase element)
        {
            hitElement = element;
            GraphControl.ClearSelection();
            GraphControl.ToggleElementSelection(element);
            GraphControl.SetMode(new DragMode(GraphControl));
        }
    }
}
