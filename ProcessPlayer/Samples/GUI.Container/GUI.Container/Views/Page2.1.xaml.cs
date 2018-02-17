using GUIContainer.ViewModels;
using System.Windows.Controls;

namespace GUIContainer.Views
{
    /// <summary>
    /// Interaction logic for Process.xaml
    /// </summary>
    public partial class Page2_1 : UserControl
    {
        public Page2_1()
        {
            InitializeComponent();
            DataContext = new Page2_1ViewModel(this);
        }
    }
}
