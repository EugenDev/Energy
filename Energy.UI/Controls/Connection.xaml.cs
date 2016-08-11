using System.Windows.Controls;

namespace Energy.UI.Controls
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : UserControl
    {
        public Connection()
        {
            InitializeComponent();
            DataContext = this;
        }

        public double Distance { get; set; }
    }
}
