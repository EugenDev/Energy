namespace Energy.UI.Controls
{
    /// <summary>
    /// Interaction logic for Consumer.xaml
    /// </summary>
    public partial class Consumer : ControlBase
    {
        public Consumer(string name)
        {
            InitializeComponent();
            ControlName = NameTextBlock.Text = name;
            DataContext = this;
        }
    }
}
