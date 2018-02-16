using GUIContainer.ViewModels;
using System.Windows.Controls;

namespace GUIContainer.Views
{
    /// <summary>
    /// Interaction logic for Process.xaml
    /// </summary>
    public partial class MainFrame : UserControl
    {
        public MainFrame()
        {
            DataContext = new MainFrameViewModel();
            InitializeComponent();
        }
    }
}
