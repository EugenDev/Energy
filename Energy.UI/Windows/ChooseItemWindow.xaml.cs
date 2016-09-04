using System.Collections.Generic;
using System.Windows;

namespace Energy.UI.Windows
{
    /// <summary>
    /// Interaction logic for ChooseItemWindow.xaml
    /// </summary>
    public partial class ChooseItemWindow : Window
    {
        public ChooseItemWindow(List<string> strings)
        {
            InitializeComponent();
            ConductionComboBox.ItemsSource = strings;
            if (strings.Count == 0)
                ChooseButton.IsEnabled = false;
            else
                ConductionComboBox.SelectedIndex = 0;
        }

        private void ChooseButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public static string ShowChooseDialog(List<string> strings, Window owner)
        {
            var window = new ChooseItemWindow(strings) { Owner = owner };
            var dialogResult = window.ShowDialog();
            return dialogResult.HasValue && dialogResult.Value ? (string)window.ConductionComboBox.SelectedValue : null;
        }
    }
}
