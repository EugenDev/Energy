using System.Windows.Input;

namespace Energy.UI.Controls.WorkArea
{
    public class MultiselectMode : WorkAreaModeBase
    {
        public MultiselectMode(GraphControl graphControl) : base(graphControl)
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
            GraphControl.ToggleElementSelection(selectable);
        }

        public override void ProcessKeyUp(KeyEventArgs args)
        {
            if(args.Key == Key.LeftCtrl || args.Key == Key.RightCtrl)
                GraphControl.SetStartMode();
        }
    }
}