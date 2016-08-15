using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Energy.UI.Controls.WorkArea
{
    public class WorkArea
    {
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

            Canvas.MouseLeftButtonDown += CanvasOnMouseLeftButtonDown;
            Canvas.MouseLeftButtonUp += CanvasOnMouseLeftButtonUp;
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

        private static int _staionsCounter;
        private static int _consumersCounter;

        public void AddElement(ElementType elementType)
        {
            switch (elementType)
            {
                case ElementType.Station:
                    Canvas.Children.Add(new Station("Station " + _staionsCounter++));
                    break;

                case ElementType.Consumer:
                    Canvas.Children.Add(new Consumer("Consumer " + _consumersCounter++));
                    break;

                default:
                    throw new InvalidOperationException();
            }
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
        }
    }
}