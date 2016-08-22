using System.Windows.Input;
using Energy.UI.Model;

namespace Energy.UI.Controls.WorkArea
{
    public class AddLinkMode : WorkAreaModeBase
    {
        public AddLinkMode(GraphControl graphControl) : base(graphControl)
        {
            GraphControl.Cursor = Cursors.Cross;
        }
        
        public override void MouseLeftButtonUp()
        {
            HitTest();
        }

        private ModelBase _fromElement;

        protected override void ProcessHitTest(ModelBase element)
        {
            if (_fromElement == null)
            {
                _fromElement = element;
                GraphControl.ToggleElementSelection(element);
                return;
            }

            if (_fromElement == element)
                return;
            
            GraphControl.AddLink(_fromElement, element);
        }

        public override void ProcessKeyUp(KeyEventArgs args)
        {
            GraphControl.SetStartMode();
        }

        public override void Cleanup()
        {
            GraphControl.Cursor = Cursors.Arrow;
            _fromElement = null;
            GraphControl.ClearSelection();
        }
    }
}
