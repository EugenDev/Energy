using System.Windows;

namespace Energy.UI.Windows
{
    /// <summary>
    /// Interaction logic for NameRequestWindow.xaml
    /// </summary>
    public partial class NameRequestWindow : Window
    {
        public string EnteredName
        {
            get { return (string)GetValue(EnteredNameProperty); }
            set { SetValue(EnteredNameProperty, value); }
        }
        public static readonly DependencyProperty EnteredNameProperty =
            DependencyProperty.Register("EnteredName", typeof(string), typeof(NameRequestWindow), new PropertyMetadata(null));
        
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
            DialogResult = true;
        }

        public static string RequestName(Window owner, string oldName = null)
        {
            var window = new NameRequestWindow {Owner = owner};
            if (oldName != null)
                window.EnteredName = oldName;
            var result = window.ShowDialog();
            return result.HasValue && result.Value ? window.EnteredName : null;
        }
    }
}
