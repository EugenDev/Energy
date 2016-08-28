using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Energy.Solving;
using Energy.UI.Calculations;
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
            GraphControl.LinkAdded += GraphControl_LinkAdded;
            GraphControl.ElementDeleted += GraphControl_ElementDeleted;
            GraphControl.LinkDeleted += GraphControlOnLinkDeleted;
            Loaded += (sender, args) => RefreshColumns();
            //TODO: Refresh columns automatically< through binding
            TaskModel.FeaturesNames.CollectionChanged += (sender, args) => RefreshColumns();
        }

        private void GraphControlOnLinkDeleted(object sender, ObjectEventArgs<LinkModel> e)
        {
            //TODO: Dirty hack 3
            TaskModel.DeletLink(e.Item);
        }

        private void GraphControl_ElementDeleted(object sender, ObjectEventArgs<ModelBase> e)
        {
            //TODO: Dirty hack 2
            TaskModel.DeleteParticipant(e.Item);
        }

        private void GraphControl_LinkAdded(object sender, LinkAddedEventArgs e)
        {
            //TODO: Dirty hack 1
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
            var selectedColumn = ConsumersDataGrid.SelectedCells[0].Column;
            var featureName = ((selectedColumn as DataGridTextColumn).Binding as Binding).Path.Path;
            TaskModel.RemoveFeature(featureName);
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
            var calc = new Calculator();
            calc.ReCalculateDistances(TaskModel);
            RefreshColumns();
        }

        private static int _stationsCounter;
        private void AddStation_OnClick(object sender, RoutedEventArgs e)
        {
            string name;
            if (App.IsDebug)
            {
                name = "Station_" + _stationsCounter++;
            }
            else
            {
                name = RequestName();
                if(name == null)
                    return;
            }
            TaskModel.AddParticipant(name, ParticipantType.Station);
        }

        private static int _consumersCounter;
        private void AddConsumer_OnClick(object sender, RoutedEventArgs e)
        {
            string name;
            if (App.IsDebug)
            {
                name = "Consumer_" + _consumersCounter++;
            }
            else
            {
                name = RequestName();
                if (name == null)
                    return;
            }
            TaskModel.AddParticipant(name, ParticipantType.Consumer);
        }

        private static int _featuresCounter;
        private void AddFeature_OnClick(object sender, RoutedEventArgs e)
        {
            string name;
            if (App.IsDebug)
            {
                name = "Feature_" + _featuresCounter++;
            }
            else
            {
                name = RequestName();
                if (name == null)
                    return;
            }
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

//TODO: Попробовать сделать GraphControl составным, чтобы можно было биндиться к некомпозитной коллекции
//TODO: Надписи на линках другим цветом
//TODO: Несвязный граф!!!!
//TODO: Окна появляются непонятно где