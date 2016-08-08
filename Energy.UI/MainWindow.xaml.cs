using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Energy.UI.Controls;
using Energy.UI.Serialization;
using Energy.UI.Windows;
using Microsoft.Win32;
using Start;

namespace Energy.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskModel TaskModel
        {
            get { return (TaskModel) DataContext; }
            set { DataContext = value; }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            TaskModel = new TaskModel();
        }

        private string RequestName()
        {
            var nameRequestWindow = new NameRequestWindow();
            nameRequestWindow.Owner = this;
            nameRequestWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var dialogResult = nameRequestWindow.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                return nameRequestWindow.EnteredName;
            }
            return null;
        }

        private void AddStation_OnClick(object sender, RoutedEventArgs e)
        {
            var requestedName = RequestName();
            if (!string.IsNullOrWhiteSpace(requestedName))
            {
                TaskModel.AddStation(requestedName);
                RefreshColumns();
            }
        }

        private void AddConsumer_OnClick(object sender, RoutedEventArgs e)
        {
            var requestedName = RequestName();
            if (!string.IsNullOrWhiteSpace(requestedName))
            {
                TaskModel.AddConsumer(requestedName);
                RefreshColumns();
            }
        }

        private void AddFeature_OnClick(object sender, RoutedEventArgs e)
        {
            var requestedName = RequestName();
            if (!string.IsNullOrWhiteSpace(requestedName))
            {
                TaskModel.AddCommonFeature(requestedName);
                RefreshColumns();
            }
        }

        private DataGridTextColumn CreateFeatureColumn(string featureName)
        {
            return new DataGridTextColumn
            {
                Header = featureName,
                Binding = new Binding { Path = new PropertyPath(featureName) }
            };
        }

        private void RefreshColumns()
        {
            StationsDataGrid.Columns.Clear();
            ConsumersDataGrid.Columns.Clear();
            foreach (var featureName in TaskModel.StationsFeaturesNames)
            {
                StationsDataGrid.Columns.Add(CreateFeatureColumn(featureName));   
            }
            foreach (var featureName in TaskModel.ConsumersFeaturesNames)
            {
                ConsumersDataGrid.Columns.Add(CreateFeatureColumn(featureName));
            }
            foreach (var featureName in TaskModel.CommonFeatureNames)
            {
                ConsumersDataGrid.Columns.Add(CreateFeatureColumn(featureName));
                StationsDataGrid.Columns.Add(CreateFeatureColumn(featureName));
            }
        }
        
        private void SaveTaskMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Задача|*.task"
            };
            var dialogResult = sfd.ShowDialog(this);
            if (dialogResult.Value)
            {
                File.WriteAllText(sfd.FileName, TaskModelExporter.ToText(TaskModel));   
            }
        }

        private void NewTaskMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (TaskModel.CommonFeatureNames.Count != 0
                || TaskModel.ConsumersFeaturesNames.Count != 0
                || TaskModel.StationsFeaturesNames.Count != 0)
            {
                var mbResult = MessageBox.Show("Создать новую модель, удалив старую?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(mbResult == MessageBoxResult.No)
                    return;
            }
            TaskModel = new TaskModel();
            ConsumersDataGrid.Columns.Clear();
            StationsDataGrid.Columns.Clear();
        }

        private void LoadTaskMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog
                {
                    Multiselect = false,
                    Filter = "Задача|*.task"
                };
                var dialogResult = ofd.ShowDialog(this);
                if (dialogResult.Value)
                {
                    TaskModel = TaskModelImporter.CreateFromText(File.ReadAllLines(ofd.FileName));
                }
                RefreshColumns();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при открытии файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateDataMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            TaskModel = TaskModelGenerator.GenerateRandom();
            RefreshColumns();
        }

        private void SolveTaskMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var rMatrix = TaskModelExporter.GetRMatrix(TaskModel);
                var sMatrix = TaskModelExporter.GetSMatrix(TaskModel);

                var result = Solver.Solve(rMatrix, sMatrix);
                //Представить расчёты
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при расчётах", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void RemoveFeatureOnConsumer_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedColumn = ConsumersDataGrid.SelectedCells[0].Column;
            var featureName = ((selectedColumn as DataGridTextColumn).Binding as Binding).Path.Path;
            if (TaskModel.ConsumersFeaturesNames.Contains(featureName))
            {
                MessageBox.Show("Невозможно удалить обязательную фичу", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            TaskModel.RemoveConsumerFeature(featureName);
            RefreshColumns();
        }

        private void RemoveFeatureOnStation_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedColumn = StationsDataGrid.SelectedCells[0].Column;
            var featureName = ((selectedColumn as DataGridTextColumn).Binding as Binding).Path.Path;
            if (TaskModel.StationsFeaturesNames.Contains(featureName))
            {
                MessageBox.Show("Невозможно удалить обязательную фичу", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            TaskModel.RemoveStationFeature(featureName);
            RefreshColumns();
        }
        
        private void RemoveStation_OnClick(object sender, RoutedEventArgs e)
        {
            if(StationsDataGrid.SelectedItem != null)
                TaskModel.RemoveStation(StationsDataGrid.SelectedItem as FeaturedItem);
        }

        private void RemoveConsumer_OnClick(object sender, RoutedEventArgs e)
        {
            if (StationsDataGrid.SelectedItem != null)
                TaskModel.RemoveConsumer(ConsumersDataGrid.SelectedItem as FeaturedItem);
        }

        private void AddStationOnCanvas_OnClick(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Add(new Station("Станция 1"));
        }
    }
}

//Калькулятор растояний - даём ему граф, он считает вс расстояния и мы потом только спрашиваем
//Внести в контролы координаты и сериализовать всё как турникеты, только связи отдельным списком
