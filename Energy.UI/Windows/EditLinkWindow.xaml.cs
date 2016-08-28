using System.Windows;
using Energy.UI.Model;

namespace Energy.UI.Windows
{
    /// <summary>
    /// Interaction logic for EditLinkWindow.xaml
    /// </summary>
    public partial class EditLinkWindow : Window
    {
        private readonly LinkModel _link;

        public EditLinkWindow(LinkModel link, bool isNewLink)
        {
            _link = link;
            Title = isNewLink ? "Новая связь" : "Параметр связи";
            InitializeComponent();
            ConductionComboBox.SelectedIndex = 0;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            _link.Distance = double.Parse(DistanceTextBox.Text);
            _link.Conduction = ConductionComboBox.SelectedIndex + 1;
            DialogResult = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
