using ID_Mark120.ViewModels;
using System.Windows.Controls;

namespace ID_Mark120.Views
{
    /// <summary>
    /// Interaction logic for Process.xaml
    /// </summary>
    public partial class ProcessEngrDisplay : UserControl
    {
        public ProcessEngrDisplay()
        {
            InitializeComponent();
            DataContext = new ProcessEngrDisplayViewModel(this);
        }
    }
}
