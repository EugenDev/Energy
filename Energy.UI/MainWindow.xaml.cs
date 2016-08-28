using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Energy.Solving;
using Energy.UI.Controls;
using Energy.UI.Model;
using Energy.UI.Serialization;
using Energy.UI.Windows;
using Microsoft.Win32;

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
            //TODO: Refresh columns automatically
            //TaskModel.FeaturesModel.FeaturesNames.CollectionChanged += (sender, args) => RefreshColumns();
            GraphControl.LinkAdded += GraphControl_LinkAdded;
            //GraphControl.ElementDeleted += GraphControl_ElementDeleted;
            Loaded += (sender, args) => RefreshColumns();
        }

        private void GraphControl_ElementDeleted(object sender, ObjectEventArgs<ModelBase> e)
        {
            //if(e.Item is StationModel)
            //    TaskModel.DeleteStation(e.Item as StationModel);
            //else if (e.Item is ConsumerModel)
            //    TaskModel.DeleteConsumer(e.Item as ConsumerModel);
        }

        private void GraphControl_LinkAdded(object sender, LinkAddedEventArgs e)
        {
            //TODO: Dirty hack
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
        
        private void RefreshColumns()
        {
            RefreshColumns(StationsDataGrid);
            RefreshColumns(ConsumersDataGrid);
        }

        private void RefreshColumns(DataGrid dataGrid)
        {
            dataGrid.Columns.Clear();
            var columns = TaskModel.GetColumns();
            foreach (var column in columns)
            {
                dataGrid.Columns.Add(column);
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
            if (TaskModel.FeaturesNames.Count != 0)
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

                var result = MainSolver.Solve(rMatrix, sMatrix);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при расчётах", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void TestMenuItem1_OnClickestMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
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
            RefreshColumns();
        }

        private void TestMenuItem2_OnClickestMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var w = new EditLinkWindow(null, true);
            w.ShowDialog();
        }
    }
}

//TODO: Калькулятор растояний - даём ему TaskModel, он считает все расстояния
//TODO: Корректно удалять линки
//TODO: Попробовать сделать GraphControl составным, чтобы можно было биндиться к некомпозитной коллекции
//TODO: Надписи на линках другим цветом