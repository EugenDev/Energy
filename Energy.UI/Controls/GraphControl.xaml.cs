using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Energy.UI.Controls.WorkArea;
using Energy.UI.Model;

namespace Energy.UI.Controls
{
    /// <summary>
    /// Interaction logic for GraphControl.xaml
    /// </summary>
    public partial class GraphControl : ItemsControl
    {
        public WorkAreaModeBase Mode { get; private set; }
        public HashSet<ModelBase> SelectedElements { get; }

        public GraphControl()
        {
            InitializeComponent();

            SetStartMode();
            SelectedElements = new HashSet<ModelBase>();
            Loaded += GraphControl_Loaded;
        }

        public event EventHandler<LinkAddedEventArgs> LinkAdded;

        internal void AddLink(ModelBase from, ModelBase to)
        {
            LinkAdded?.Invoke(this, new LinkAddedEventArgs(from, to));
        }

        private void GraphControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                var window = Window.GetWindow(this);
                if (window == null)
                    throw new InvalidOperationException("No window found for GraphicControl");

                window.KeyDown += OnKeyDown;
                window.KeyUp += OnKeyUp;
            }
        }

        public void SetStartMode()
        {
            SetMode(new StartMode(this));
        }

        public void SetMode(WorkAreaModeBase mode)
        {
            Mode?.Cleanup();
            Mode = mode;
        }
        
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Mode.MouseLeftButtonUp();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Mode.MouseLeftButtonDown();
        }

        protected void OnKeyUp(object sender, KeyEventArgs e)
        {
            Mode.ProcessKeyUp(e);
        }

        protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            Mode.ProcessKeyDown(e);
        }
        
        public void ToggleElementSelection(ModelBase element)
        {
            if (SelectedElements.Contains(element))
            {
                SelectedElements.Remove(element);
                element.IsSelected = false;
            }
            else
            {
                SelectedElements.Add(element);
                element.IsSelected = true;
            }
        }

        public void ClearSelection()
        {
            foreach (var control in SelectedElements)
            {
                control.IsSelected = false;
            }
            SelectedElements.Clear();
        }

        public void AddLink()
        {
            ClearSelection();
            SetMode(new AddLinkMode(this));
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }

    public class LinkAddedEventArgs : EventArgs
    {
        public ModelBase From { get; private set; }
        public ModelBase To { get; private set; }

        public LinkAddedEventArgs(ModelBase from, ModelBase to)
        {
            From = from;
            To = to;
        }
    }
}
