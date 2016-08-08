using System.Windows.Controls;

namespace Energy.UI.Controls
{
    /// <summary>
    /// Interaction logic for Station.xaml
    /// </summary>
    public partial class Station : UserControl
    {
        public Station(string name)
        {
            InitializeComponent();
            NameTextBlock.Text = name;
        }
    }
}
