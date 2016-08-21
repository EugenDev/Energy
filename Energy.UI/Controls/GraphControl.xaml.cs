using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Energy.UI.Controls.WorkArea;
using Energy.UI.Model;

namespace Energy.UI.Controls
{
    /// <summary>
    /// Interaction logic for GraphControl.xaml
    /// </summary>
    public partial class GraphControl : ItemsControl
    {
        //public Canvas Canvas => new Canvas();

        public WorkAreaModeBase Mode { get; private set; }
        public HashSet<ModelBase> SelectedElements { get; }
        //private Dictionary<ControlBase, List<Link>> Links { get; }

        public GraphControl()
        {
            InitializeComponent();

            SetStartMode();
            SelectedElements = new HashSet<ModelBase>();
            Loaded += GraphControl_Loaded;
            //Links = new Dictionary<ControlBase, List<Link>>();
        }

        private void GraphControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if(window == null)
                throw new InvalidOperationException("No window found for GraphicControl");

            window.KeyDown += OnKeyDown;
            window.KeyUp += OnKeyUp;
        }
        
        //private void DeleteControl(ControlBase control)
        //{
        //    if (Links.ContainsKey(control))
        //    {
        //        var links = Links[control].ToList();
        //        foreach (var link in links)
        //        {
        //            Links[link.From].Remove(link);
        //            Links[link.To].Remove(link);
        //            Canvas.Children.Remove(link);
        //        }
        //        Links.Remove(control);
        //    }

        //    Canvas.Children.Remove(control);

        //}

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

        public void StartAddLink()
        {
            ClearSelection();
            SetMode(new AddLinkMode(this));
        }

        public void LinkElements(ISelectable fromElement, ISelectable selectedElement)
        {
            //var from = (ControlBase)fromElement;
            //var to = (ControlBase)selectedElement;
            //var link = _controlsFactory.CreateLink(from, to, 10.0);
            //Canvas.Children.Add(link);
            //CollectLink(from, link);
            //CollectLink(to, link);
        }

        //private void CollectLink(ControlBase element, Link link)
        //{
        //    if (!Links.ContainsKey(element))
        //    {
        //        Links[element] = new List<Link>();
        //    }
        //    Links[element].Add(link);
        //}

        //public void DeleteSelected()
        //{
        //    foreach (var control in SelectedElements)
        //    {
        //        var element = control as ControlBase;
        //        DeleteControl(element);
        //    }
        //    ClearSelection();
        //    SetStartMode();
        //}
    }
}
