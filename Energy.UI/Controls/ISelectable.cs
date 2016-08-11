namespace Energy.UI.Controls
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }

        void Select();

        void Unselect();

        void ToggleSelection();
    }
}