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
        public HashSet<ISelectable> SelectedControls { get; private set; }

        public WorkArea(Canvas canvas)
        {
            Canvas = canvas;
            SelectedControls = new HashSet<ISelectable>();
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

            var line = new Line
            {
                X1 = from.X,
                Y1 = from.Y,
                X2 = to.X,
                Y2 = to.Y,
                Stroke = Brushes.DarkRed,
                StrokeThickness = 2
            };

            Canvas.Children.Add(line);
        }
    }
}