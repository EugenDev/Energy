using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Energy.UI.Controls.WorkArea
{
    public class StartMode : WorkAreaModeBase
    {
        public StartMode(WorkArea workArea) : base(workArea)
        {
        }

        public override void MouseLeftButtonUp()
        {
            WorkArea.ClearSelection();
        }

        public override void MouseLeftButtonDown()
        {
            HitTest();
        }

        public override void ProcessKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                WorkArea.SetMode(new MultiselectMode(WorkArea));
        }

        protected override void ProcessHitTest(ControlBase element)
        {
            if(!(element is ISelectable))
                return;

            var elementHitPoint = Mouse.GetPosition(element);
            var selectedElements = WorkArea.SelectedControls.Count != 0
                ? WorkArea.SelectedControls.Cast<ControlBase>().ToList()
                : new List<ControlBase> {element };
            WorkArea.SetMode(new DragMode(WorkArea, GetCurrentPoint(), selectedElements));
        }
    }
}
