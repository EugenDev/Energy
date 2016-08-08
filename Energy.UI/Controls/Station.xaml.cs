using System.Windows;
using System.Windows.Media;

namespace Energy.UI.Controls
{
    /// <summary>
    /// Interaction logic for Station.xaml
    /// </summary>
    public partial class Station : ControlBase, ISelectable
    {
        public Station(string name)
        {
            InitializeComponent();
            NameTextBlock.Text = name;
        }
        
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(Station), new PropertyMetadata(false));


    }
}
