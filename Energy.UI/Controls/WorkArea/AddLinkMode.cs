﻿using Energy.UI.Model;

namespace Energy.UI.Controls.WorkArea
{
    public class AddLinkMode : WorkAreaModeBase
    {
        public AddLinkMode(GraphControl graphControl) : base(graphControl)
        {
        }

        private ISelectable _fromElement;

        public override void MouseLeftButtonUp()
        {
            HitTest();
        }

        protected override void ProcessHitTest(ModelBase element)
        {
            if(!(element is ISelectable))
                return;

            var selectedElement = (ISelectable) element;

            if (_fromElement != null && _fromElement != selectedElement)
            {
                GraphControl.ClearSelection();
                GraphControl.LinkElements(_fromElement, selectedElement);
                GraphControl.SetStartMode();
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

            //GraphControl.ToggleElementSelection(selectedElement);
        }
    }
}
