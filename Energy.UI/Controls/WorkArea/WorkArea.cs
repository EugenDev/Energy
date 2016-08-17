using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Energy.UI.Controls.WorkArea
{
    public class WorkArea
    {
        private readonly ControlsFactory _controlsFactory;
        public Canvas Canvas { get; }
        public WorkAreaModeBase Mode { get; private set; }
        public HashSet<ISelectable> SelectedControls { get; }
        private Dictionary<ControlBase, List<Link>> Links { get; }

        public WorkArea(Canvas canvas)
        {
            Canvas = canvas;
            SelectedControls = new HashSet<ISelectable>();
            Links = new Dictionary<ControlBase, List<Link>>();
            Mode = new StartMode(this);
            _controlsFactory = new ControlsFactory(HandleWantDelete);

            Canvas.MouseLeftButtonDown += CanvasOnMouseLeftButtonDown;
            Canvas.MouseLeftButtonUp += CanvasOnMouseLeftButtonUp;
        }

        private void HandleWantDelete(object sender, RoutedEventArgs args)
        {
            MessageBox.Show("Delete!!!");
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

        private void CanvasOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Mode.MouseLeftButtonUp();
        }

        private void CanvasOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Mode.MouseLeftButtonDown();
        }

        public void ProcessKeyUp(object sender, KeyEventArgs e)
        {
            Mode.ProcessKeyUp(e);
        }

        public void ProcessKeyDown(object sender, KeyEventArgs e)
        {
            Mode.ProcessKeyDown(e);
        }

        private static int _controlsCounter;

        public void AddElement(ElementType elementType)
        {
            var name = "Control_" + _controlsCounter++;
            Canvas.Children.Add(_controlsFactory.CreateControl(elementType, name));
        }
        
        public void ClearSelection()
        {
            foreach (var control in SelectedControls)
            {
                control.Unselect();
            }
            SelectedControls.Clear();
        }

        public void ToggleElementSelection(ISelectable element)
        {
            if (SelectedControls.Contains(element))
            {
                SelectedControls.Remove(element);
                element.Unselect();
            }
            else
            {
                SelectedControls.Add(element);
                element.Select();
            }
        }

        public void StartAddLink()
        {
            ClearSelection();
            SetMode(new AddLinkMode(this));
        }

        public void LinkElements(ISelectable fromElement, ISelectable selectedElement)
        {
            var from = (ControlBase) fromElement;
            var to = (ControlBase) selectedElement;

            var link = new Link(from, to, 10)
            {
                Stroke = Brushes.DarkRed,
                StrokeThickness = 2
            };

            Canvas.Children.Add(link);

            CollectLink(from, link);
            CollectLink(to, link);
        }

        private void CollectLink(ControlBase element, Link link)
        {
            if (!Links.ContainsKey(element))
            {
                Links[element] = new List<Link>();
            }
            Links[element].Add(link);
        }

        public void DeleteSelected()
        {
            foreach (var control in SelectedControls)
            {
                var element = control as ControlBase;
                if (element == null)
                    continue;

                Canvas.Children.Remove(element);

                if(!Links.ContainsKey(element))
                    continue;

                foreach (var link in Links[element])
                {
                    Canvas.Children.Remove(link);
                }
            }
            ClearSelection();
            SetStartMode();
        }
    }
}