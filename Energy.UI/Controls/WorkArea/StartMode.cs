using System.Windows.Input;
using Energy.UI.Model;

namespace Energy.UI.Controls.WorkArea
{
    public class StartMode : WorkAreaModeBase
    {
        public StartMode(GraphControl graphControl) : base(graphControl)
        {
        }
        
        public override void MouseLeftButtonDown()
        {
            HitTest();
        }

        public override void ProcessKeyDown(KeyEventArgs args)
        {
            if (args.Key == Key.LeftShift)
                GraphControl.SetMode(new AddLinkMode(GraphControl));
        }

        protected override void ProcessHitTest(ModelBase element)
        {
            GraphControl.ToggleElementSelection(element);
            GraphControl.SetMode(new DragMode(GraphControl));
        }
    }
}
