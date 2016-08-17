using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

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
            _controlsFactory = new ControlsFactory();
            _controlsFactory.WantDeleteLink += ControlsFactoryOnWantDeleteLink;
            _controlsFactory.WantDeleteControl += ControlsFactoryOnWantDeleteControl;

            Canvas.MouseLeftButtonDown += CanvasOnMouseLeftButtonDown;
            Canvas.MouseLeftButtonUp += CanvasOnMouseLeftButtonUp;
        }

        private void ControlsFactoryOnWantDeleteControl(object sender, WantDeleteControlEventArgs wantDeleteControlEventArgs)
        {
            DeleteControl(wantDeleteControlEventArgs.Control);
        }

        private void DeleteControl(ControlBase control)
        {
            if (Links.ContainsKey(control))
            {
                var links = Links[control].ToList();
                foreach (var link in links)
                {
                    Links[link.From].Remove(link);
                    Links[link.To].Remove(link);
                    Canvas.Children.Remove(link);
                }
                Links.Remove(control);
            }

            Canvas.Children.Remove(control);
        }

        private void ControlsFactoryOnWantDeleteLink(object sender, WantDeleteLinkEventArgs e)
        {
            if (Links.ContainsKey(e.Link.From))
                Links[e.Link.From].Remove(e.Link);

            if (Links.ContainsKey(e.Link.To))
                Links[e.Link.To].Remove(e.Link);

            Canvas.Children.Remove(e.Link);
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
            var link = _controlsFactory.CreateLink(from, to, 10.0);
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
                DeleteControl(element);
            }
            ClearSelection();
            SetStartMode();
        }
    }
}