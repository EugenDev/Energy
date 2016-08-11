namespace Energy.UI.Controls
{
    /// <summary>
    /// Interaction logic for Station.xaml
    /// </summary>
    public partial class Station : ControlBase
    {
        public Station(string name)
        {
            InitializeComponent();
            ControlName = NameTextBlock.Text = name;
            DataContext = this;
        }
    }
}
