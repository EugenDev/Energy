using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Energy.UI.Controls
{
    public class ControlBase : UserControl, IControl, ISelectable
    {
        public ControlBase(string name)
        {
            X = Y = 0;
            IsSelected = false;
            Links = new List<Link>();
            ContextMenu = CreateContextMenu();
            DataContext = this;
            Caption = name;
        }

        public string Caption { get; set; }

        public List<Link> Links { get; private set; }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }

        public event EventHandler PositionChanged;

        public void OnPositionChanged()
        {
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        public double X
        {
            get { return Canvas.GetLeft(this); }
            set
            {
                Canvas.SetLeft(this, value);
                OnPositionChanged();
            }
        }

        public double Y
        {
            get { return Canvas.GetTop(this); }
            set
            {
                Canvas.SetTop(this, value);
                OnPositionChanged();
            }
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

        public event EventHandler WantDelete;

        private void OnWantDelete()
        {
            WantDelete?.Invoke(this, EventArgs.Empty);
        }

        private ContextMenu CreateContextMenu()
        {
            var result = new ContextMenu();
            var deleteItem = new MenuItem { Header = "Удалить" };
            deleteItem.Click += (sender, args) => OnWantDelete();
            result.Items.Add(deleteItem);
            return result;
        }
    }
}
