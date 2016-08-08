using System.Windows;

namespace Energy.UI.Windows
{
    /// <summary>
    /// Interaction logic for NameRequestWindow.xaml
    /// </summary>
    public partial class NameRequestWindow : Window
    {
        public string EnteredName { get; set; }

        public NameRequestWindow()
        {
            InitializeComponent();
            NameTextBox.Focus();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            EnteredName = NameTextBox.Text;
            DialogResult = true;
        }
    }
}
