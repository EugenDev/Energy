namespace Energy.UI.Controls.WorkArea
{
    public class AddLinkMode : WorkAreaModeBase
    {
        public AddLinkMode(WorkArea workArea) : base(workArea)
        {
        }

        private ISelectable _fromElement;

        public override void MouseLeftButtonUp()
        {
            HitTest();
        }

        protected override void ProcessHitTest(ControlBase hitElement)
        {
            if(!(hitElement is ISelectable))
                return;

            var selectedElement = (ISelectable) hitElement;

            if (_fromElement != null && _fromElement != selectedElement)
            {
                WorkArea.ClearSelection();
                WorkArea.LinkElements(_fromElement, selectedElement);
                WorkArea.SetStartMode();
                return;
            }

            if (_fromElement == selectedElement)
            {
                _fromElement = selectedElement;
            }

            if (_fromElement == null)
            {
                _fromElement = selectedElement;
            }

            WorkArea.ToggleElementSelection(selectedElement);
        }
    }
}
