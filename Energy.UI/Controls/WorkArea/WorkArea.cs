using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

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

        private static int _counter;

        public void AddElement(ElementType elementType)
        {
            var control = new Station("Station " + _counter++);
            Canvas.Children.Add(control);
        }

        public void ClearSelection()
        {
            foreach (var control in SelectedControls)
            {
                control.IsSelected = false;
            }
            SelectedControls.Clear();
        }

        public void SelectElement(ISelectable element)
        {
            SelectedControls.Add(element);
        }
    }
}
