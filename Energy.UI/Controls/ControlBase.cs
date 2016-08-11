using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Energy.UI.Controls
{
    public class ControlBase : UserControl, IControl, ISelectable
    {
        public ControlBase()
        {
            X = Y = 0;
            IsSelected = false;
        }

        public string ControlName { get; set; }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }

        public double X
        {
            get { return Canvas.GetLeft(this); }
            set { Canvas.SetLeft(this, value); }
        }

        public double Y
        {
            get { return Canvas.GetTop(this); }
            set { Canvas.SetTop(this, value); }
        }
        
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(ControlBase), new PropertyMetadata(false));

        public void Select()
        {
            IsSelected = true;
        }

        public void Unselect()
        {
            IsSelected = false;
        }

        public void ToggleSelection()
        {
            IsSelected = !IsSelected;
        }
    }
}
