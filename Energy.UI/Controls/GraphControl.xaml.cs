using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Energy.UI.Controls.WorkArea;
using Energy.UI.Helpers;
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

        public event EventHandler<LinkAddedEventArgs> WantAddLink;
        internal void OnWantAddLink(ModelBase from, ModelBase to)
        {
            WantAddLink?.Invoke(this, new LinkAddedEventArgs(from, to));
        }

        public event EventHandler<ObjectEventArgs<LinkModel>> WantDeleteLink;
        internal void OnWantDeleteLink(LinkModel element)
        {
            WantDeleteLink?.Invoke(this, new ObjectEventArgs<LinkModel>(element));
        }

        public event EventHandler<ObjectEventArgs<LinkModel>> WantEditLink;
        internal void OnWantEditLink(LinkModel element)
        {
            WantEditLink?.Invoke(this, new ObjectEventArgs<LinkModel>(element));
        }
        
        public event EventHandler<ObjectEventArgs<ModelBase>> WantEditElement;
        internal void OnWantEditElement(ModelBase element)
        {
            WantEditElement?.Invoke(this, new ObjectEventArgs<ModelBase>(element));
        }

        public event EventHandler<ObjectEventArgs<ModelBase>> WantDeleteElement;
        internal void OnWantDeleteElement(ModelBase element)
        {
            WantDeleteElement?.Invoke(this, new ObjectEventArgs<ModelBase>(element));
        }

        private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var model = ModelBindingHelper.TryGetBoundItem(sender as DependencyObject);
            if (model == null)
            {
                var linkModel = ModelBindingHelper.TryGetLinkItem(sender as DependencyObject);
                if (linkModel == null)
                    return;
                OnWantDeleteLink(linkModel);
            }
            else
                OnWantDeleteElement(model);
        }

        private void EditMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var model = ModelBindingHelper.TryGetBoundItem(sender as DependencyObject);
            if (model == null)
            {
                var linkModel = ModelBindingHelper.TryGetLinkItem(sender as DependencyObject);
                if (linkModel == null)
                    return;
                OnWantEditLink(linkModel);
            }
            else
                OnWantEditElement(model);
        }
    }
}
