using System.Windows.Input;
using Energy.UI.Model;

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

        protected override void ProcessHitTest(ModelBase element)
        {
           GraphControl.ToggleElementSelection(element);
        }

        public override void ProcessKeyUp(KeyEventArgs args)
        {
            if(args.Key == Key.LeftCtrl || args.Key == Key.RightCtrl)
                GraphControl.SetStartMode();
        }
    }
}