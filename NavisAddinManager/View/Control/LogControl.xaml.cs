using System.Windows.Controls;
using NavisAddinManager.ViewModel;
using UserControl = System.Windows.Controls.UserControl;

namespace NavisAddinManager.View.Control
{
    /// <summary>
    /// Interaction logic for LogControl.xaml
    /// </summary>
    public partial class LogControl : UserControl
    {
        public LogControl()
        {
            InitializeComponent();
            LogControlViewModel viewModel = new LogControlViewModel();
            DataContext = viewModel;
            viewModel.FrmLogControl = this;
            this.Loaded += viewModel.LogFileWatcher;
            this.Unloaded += viewModel.UserControl_Unloaded;

        }
        
    }
}
