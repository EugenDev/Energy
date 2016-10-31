using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Energy.Solving;
using Energy.UI.Controls;
using Energy.UI.Model;
using Energy.UI.Serialization;
using Energy.UI.Windows;
using Microsoft.Win32;
using DataGrid = System.Windows.Controls.DataGrid;
using Task = System.Threading.Tasks.Task;

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

        public double CanvasScale
        {
            get { return (double)GetValue(CanvasScaleProperty); }
            set { SetValue(CanvasScaleProperty, value); }
        }
        public static readonly DependencyProperty CanvasScaleProperty =
            DependencyProperty.Register("CanvasScale", typeof(double), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            TaskModel = new TaskModel();
            Loaded += (sender, args) => RefreshColumns();
            TaskModel.FeaturesNames.CollectionChanged += (sender, args) => RefreshColumns();

            GraphControl.WantAddLink += GraphControlWantAddLink;
            GraphControl.WantDeleteElement += GraphControlWantDeleteElement;
            GraphControl.WantDeleteLink += GraphControlOnWantDeleteLink;
            GraphControl.WantEditLink += GraphControl_WantEditLink;
            GraphControl.WantEditElement += GraphControl_WantEditElement;

            CanvasScale = 100;

            if (Properties.Settings.Default.DebugMode)
                CreateTestMenu();
        }

        private void GraphControl_WantEditElement(object sender, ObjectEventArgs<ModelBase> e)
        {
            var newName = NameRequestWindow.RequestName(this, e.Item.Name);
            if (string.IsNullOrWhiteSpace(newName))
                return;

            e.Item.Name = newName;
        }

        private void GraphControl_WantEditLink(object sender, ObjectEventArgs<LinkModel> e)
        {
            var editLink = new LinkModel(e.Item.From, e.Item.To)
            {
                Conduction = e.Item.Conduction,
                Distance = e.Item.Distance
            };
            if (!EditLinkWindow.ShowEditDialog(editLink, false, this))
                return;
            e.Item.Distance = editLink.Distance;
            e.Item.Conduction = editLink.Conduction;
        }

        private void GraphControlOnWantDeleteLink(object sender, ObjectEventArgs<LinkModel> e)
        {
            TaskModel.DeletLink(e.Item);
        }

        private void GraphControlWantDeleteElement(object sender, ObjectEventArgs<ModelBase> e)
        {
            TaskModel.DeleteParticipant(e.Item);
        }

        private void GraphControlWantAddLink(object sender, LinkAddedEventArgs e)
        {
            var link = new LinkModel(e.From, e.To);
            if (Properties.Settings.Default.DebugMode)
            {
                var r = new Random(DateTime.Now.Millisecond);
                link.Distance = r.Next(5, 20);
                link.Conduction = r.Next(1, 4);
            }
            else
            {
                if (!EditLinkWindow.ShowEditDialog(link, true, this))
                    return;
            }
            TaskModel.AddLink(link);
        }
        
        private void RefreshColumns()
        {
            RefreshColumns(StationsDataGrid, forStation: true);
            RefreshColumns(ConsumersDataGrid);
        }

        private void RefreshColumns(DataGrid dataGrid, bool forStation = false)
        {
            dataGrid.Columns.Clear();
            var columns = TaskModel.GetColumns(forStation);
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

        private void SolveTaskMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var combineType = (CombineType)Enum.Parse(typeof(CombineType), Properties.Settings.Default.CombinationType);

                if (!TaskModel.IsValid)
                    throw new InvalidOperationException("Отсутствуют станции или потребители");
                
                var task = TaskModelExporter.GetTask(TaskModel);
                var result = MainSolver.Solve(task);
                
                ResultTextBox.Text = SolveResultPrinter.PrintTaskSolveResult(
                    TaskModel.Stations.Select(s => s.Name).ToList(),
                    TaskModel.Consumers.Select(c => c.Name).ToList(),
                    result, combineType, fullPrint: false);

                File.WriteAllText("result.txt", SolveResultPrinter.PrintTaskSolveResult(
                    TaskModel.Stations.Select(s => s.Name).ToList(),
                    TaskModel.Consumers.Select(c => c.Name).ToList(),
                    result, combineType));

                TaskModel.SetResult(result.GetCombinedResult(combineType));
                MessageBox.Show("Задача решена", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при расчётах", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static int _stationsCounter;
        private void AddStation_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string name;
                if (Properties.Settings.Default.AutoGenerateParticipantNames)
                {
                    name = "Station_" + _stationsCounter++;
                }
                else
                {
                    name = NameRequestWindow.RequestName(this);
                    if(name == null)
                        return;
                }
                TaskModel.AddParticipant(name, ParticipantType.Station);
            }
            catch (DuplicateNameException)
            {
                MessageBox.Show("Участник с таким именем уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static int _consumersCounter;
        private void AddConsumer_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string name;
                if (Properties.Settings.Default.AutoGenerateParticipantNames)
                {
                    name = "Consumer_" + _consumersCounter++;
                }
                else
                {
                    name = NameRequestWindow.RequestName(this);
                    if (name == null)
                        return;
                }
                TaskModel.AddParticipant(name, ParticipantType.Consumer);
            }
            catch (DuplicateNameException)
            {
                MessageBox.Show("Участник с таким именем уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static int _featuresCounter;
        private void AddFeature_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string name;
                if (Properties.Settings.Default.AutoGenerateParticipantNames)
                {
                    name = "Feature_" + _featuresCounter++;
                }
                else
                {
                    name = NameRequestWindow.RequestName(this);
                    if (name == null)
                        return;
                }
                TaskModel.AddFeature(name);
                RefreshColumns();
            }
            catch (DuplicateNameException)
            {
                MessageBox.Show("Свойство с таким именем уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateTestMenu()
        {
            var menu = new MenuItem { Header = "Тест" };
            var menuItem = new MenuItem { Header = "Тест 1" };
            menuItem.Click += TestMenuItem1_OnClick;
            menu.Items.Add(menuItem);
            menuItem = new MenuItem { Header = "Тест 2" };
            menuItem.Click += TestMenuItem2_OnClick;
            menu.Items.Add(menuItem);
            MainMenu.Items.Add(menu);
        }

        private void TestMenuItem1_OnClick(object sender, RoutedEventArgs e)
        {
            var a = TaskModelExporter.GetTask(TaskModel);
        }

        private void TestMenuItem2_OnClick(object sender, RoutedEventArgs e)
        {
            var res = Validation.GetHasError(StationsDataGrid);
        }
        
        private void RemoveStation_OnClick(object sender, RoutedEventArgs e)
        {
            var choosedItem = ChooseItemWindow.ShowChooseDialog(TaskModel.Stations.Select(c => c.Name).ToList(), this);
            if (choosedItem == null)
                return;

            TaskModel.DeleteParticipant(TaskModel.Stations.First(x => x.Name.Equals(choosedItem)));
        }

        private void RemoveConsumer_OnClick(object sender, RoutedEventArgs e)
        {
            var choosedItem = ChooseItemWindow.ShowChooseDialog(TaskModel.Consumers.Select(c => c.Name).ToList(), this);
            if (choosedItem == null)
                return;

            TaskModel.DeleteParticipant(TaskModel.Consumers.First(x => x.Name.Equals(choosedItem)));
        }

        private void RemoveFeature_OnClick(object sender, RoutedEventArgs e)
        {
            var featuresNames = TaskModel
                .FeaturesNames
                .Where(f => !Constants.ConstantFeatures.Contains(f))
                .ToList();
            var choosedItem = ChooseItemWindow.ShowChooseDialog(featuresNames, this);
            if (choosedItem == null)
                return;

            TaskModel.RemoveFeature(choosedItem);
            RefreshColumns();
        }
    }
}

//TODO: Попробовать сделать GraphControl составным, чтобы можно было биндиться к некомпозитной коллекции
//TODO: Кеширование контролов для исключения перерисовки