using ID_Mark120.ViewModels;
using System.Windows.Controls;

namespace ID_Mark120.Views
{
    /// <summary>
    /// Interaction logic for Diagnostics.xaml
    /// </summary>
    public partial class DiagnosticsLog : UserControl
    {
        public DiagnosticsLog()
        {
            InitializeComponent();
            DataContext = new DiagnosticsLogViewModel(this);
        }
    }
}
