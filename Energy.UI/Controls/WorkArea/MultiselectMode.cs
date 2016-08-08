using System.Windows.Input;

namespace Energy.UI.Controls.WorkArea
{
    public class MultiselectMode : WorkAreaModeBase
    {
        public MultiselectMode(WorkArea workArea) : base(workArea)
        {
        }

        public override void MouseLeftButtonUp()
        {
            HitTest();
        }

        protected override void ProcessHitTest(ControlBase hitElement)
        {
            if(!(hitElement is ISelectable))
                return;

            var selectable = hitElement as ISelectable;
            selectable.IsSelected = true;
            WorkArea.SelectElement(selectable);
        }

        public override void ProcessKeyUp(KeyEventArgs args)
        {
            if(args.Key == Key.LeftCtrl || args.Key == Key.RightCtrl)
                WorkArea.SetMode(new StartMode(WorkArea));
        }
    }
}