using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Energy.UI.Controls;
using Energy.UI.Controls.WorkArea;
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
            PreviewKeyUp += GraphControl.ProcessKeyUp;
            PreviewKeyDown += GraphControl.ProcessKeyDown;
            TaskModel = new TaskModel();
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
        
        private void AddFeature_OnClick(object sender, RoutedEventArgs e)
        {
            var requestedName = RequestName();
            if (!string.IsNullOrWhiteSpace(requestedName))
            {
                TaskModel.FeaturesModel.AddFeature(requestedName);
                RefreshColumns();
            }
        }

        private void RemoveFeature_Click(object sender, RoutedEventArgs e)
        {
            var selectedColumn = ConsumersDataGrid.SelectedCells[0].Column;
            var featureName = ((selectedColumn as DataGridTextColumn).Binding as Binding).Path.Path;
            TaskModel.FeaturesModel.RemoveFeature(featureName);
            RefreshColumns();
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
                //Представить расчёты
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при расчётах", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void TestMenuItem_OnClickestMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
        }
        
        private static int _controlsCounter;

        private void AddStation_OnClick(object sender, RoutedEventArgs e)
        {
            var name = "Station_" + _controlsCounter++;
            GraphControl.AddElement(name, ControlType.Station);
            TaskModel.FeaturesModel.AddStation(name);
        }

        private void AddConsumer_OnClick(object sender, RoutedEventArgs e)
        {
            var name = "Consumer_" + _controlsCounter++;
            GraphControl.AddElement(name, ControlType.Consumer);
            TaskModel.FeaturesModel.AddConsumer(name);
        }

        private void AddLink_OnClick(object sender, RoutedEventArgs e)
        {
            GraphControl.StartAddLink();
        }
    }
}

//Калькулятор растояний - даём ему граф, он считает все расстояния и мы потом только спрашиваем
//Экспортировать и импортировать график и таблицы
//Менять курср если находимся в режиме добавления связи.
//Убрать инфу о ссылках из контролов. Сделать слежение на уровне WorkArea