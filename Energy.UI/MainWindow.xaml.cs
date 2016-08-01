using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Energy.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TaskModel _taskModel;

        private int _stationNumber;
        private int _consumerNumber;
        private int _featureNumber;

        public MainWindow()
        {
            _stationNumber = 0;
            _consumerNumber = 0;
            _featureNumber = 0;
            InitializeComponent();
            DataContext = _taskModel = new TaskModel();
        }

        private void AddStation_OnClick(object sender, RoutedEventArgs e)
        {
            _taskModel.AddStation("Station_" + _stationNumber++);
        }

        private void AddConsumer_OnClick(object sender, RoutedEventArgs e)
        {
            _taskModel.AddConsumer("Consumer_" + _consumerNumber++);
        }

        private void AddFeature_OnClick(object sender, RoutedEventArgs e)
        {
            var featureName = "Feature_" + _featureNumber++;
            _taskModel.AddFeature(featureName);
            ConsumersDataGrid.Columns.Add(CreateFeatureColumn(featureName));
            StationsDataGrid.Columns.Add(CreateFeatureColumn(featureName));
        }

        private DataGridTextColumn CreateFeatureColumn(string featureName)
        {
            return new DataGridTextColumn
            {
                Header = featureName,
                Binding = new Binding { Path = new PropertyPath(featureName) }
            };
        }
    }
}
