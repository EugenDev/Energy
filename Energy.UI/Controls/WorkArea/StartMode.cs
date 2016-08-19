using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Energy.UI.Controls.WorkArea
{
    public class StartMode : WorkAreaModeBase
    {
        public StartMode(GraphControl graphControl) : base(graphControl)
        {
        }

        public override void MouseLeftButtonUp()
        {
            GraphControl.ClearSelection();
        }

        public override void MouseLeftButtonDown()
        {
            HitTest();
        }

        public override void ProcessKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                GraphControl.SetMode(new MultiselectMode(GraphControl));

            if (e.Key == Key.Delete)
                GraphControl.DeleteSelected();
        }

        protected override void ProcessHitTest(ControlBase element)
        {
            if(!(element is ISelectable))
                return;
            
            var selectedElements = GraphControl.SelectedControls.Count != 0
                ? GraphControl.SelectedControls.Cast<ControlBase>().ToList()
                : new List<ControlBase> {element };

            GraphControl.SetMode(new DragMode(GraphControl, selectedElements));
        }
    }
}
