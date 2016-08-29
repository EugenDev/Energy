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
            InitializeComponent();
            _link = link;
            Title = isNewLink ? "Новая связь" : "Параметры связи";
            DistanceTextBox.Text = _link.Distance.ToString();
            ConductionComboBox.SelectedIndex = isNewLink ? 0 : link.Conduction - 1;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var distance = 0.0;
            if(double.TryParse(DistanceTextBox.Text, out distance))
                _link.Distance = distance;
            _link.Conduction = ConductionComboBox.SelectedIndex + 1;
            DialogResult = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        
        public static bool ShowEditDialog(LinkModel link, bool isNewLink, Window owner)
        {
            var window = new EditLinkWindow(link, isNewLink) {Owner = owner};
            var dialogResult = window.ShowDialog();
            return dialogResult.HasValue && dialogResult.Value;
        }
    }
}
