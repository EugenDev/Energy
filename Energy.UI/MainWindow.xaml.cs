using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Energy.UI.Controls;
using Energy.UI.Model;
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
            GraphControl.LinkAdded += GraphControl_LinkAdded;
        }

        private void GraphControl_LinkAdded(object sender, LinkAddedEventArgs e)
        {
            TaskModel.AddLink(e.From, e.To);
        }

        private string RequestName()
        {
            var nameRequestWindow = new NameRequestWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var dialogResult = nameRequestWindow.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                return nameRequestWindow.EnteredName;
            }
            return null;
        }

        private void RemoveFeature_Click(object sender, RoutedEventArgs e)
        {
            //var selectedColumn = ConsumersDataGrid.SelectedCells[0].Column;
            //var featureName = ((selectedColumn as DataGridTextColumn).Binding as Binding).Path.Path;
            //TaskModel.FeaturesModel.RemoveFeature(featureName);
            //RefreshColumns();
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
            foreach (var featureName in TaskModel.FeaturesModel.FeaturesNames)
            {
                StationsDataGrid.Columns.Add(CreateFeatureColumn(featureName));
                ConsumersDataGrid.Columns.Add(CreateFeatureColumn(featureName));
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
            if (TaskModel.FeaturesModel.FeaturesNames.Count != 0)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при расчётах", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void TestMenuItem1_OnClickestMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            //var collection = new List<ModelBase>
            //{
            //    new StationModel("Станция 1") {IsSelected = true},
            //    new StationModel("Станция 2") {IsSelected = false, X = 200},
            //    new ConsumerModel("Клиент 1") {IsSelected = true, Y = 200},
            //    new ConsumerModel("Клиент 1") {IsSelected = false, X = 200, Y = 200}
            //};

            //GraphControl.ItemsSource = collection;
        }

        private static int _stationsCounter;
        private void AddStation_OnClick(object sender, RoutedEventArgs e)
        {
            var name = "Station_" + _stationsCounter++;
            TaskModel.AddParticipant(name, ParticipantType.Station);
        }

        private static int _consumersCounter;
        private void AddConsumer_OnClick(object sender, RoutedEventArgs e)
        {
            var name = "Consumer_" + _consumersCounter++;
            TaskModel.AddParticipant(name, ParticipantType.Consumer);
        }

        private static int _featuresCounter;
        private void AddFeature_OnClick(object sender, RoutedEventArgs e)
        {
            var name = "Feature_" + _featuresCounter++;
            TaskModel.AddFeature(name);
            //TODO: Перестраивать колонки
        }

        private void TestMenuItem2_OnClickestMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}

//TODO: Калькулятор растояний - даём ему граф, он считает все расстояния и мы потом только спрашиваем
//TODO:Экспортировать и импортировать график и таблицы
//TODO:Убрать инфу о ссылках из контролов. Сделать слежение на уровне WorkArea