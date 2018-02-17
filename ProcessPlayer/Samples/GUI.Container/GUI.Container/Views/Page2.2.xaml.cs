using GUIContainer.ViewModels;
using System.Windows.Controls;

namespace GUIContainer.Views
{
    /// <summary>
    /// Interaction logic for Process.xaml
    /// </summary>
    public partial class Page2_2 : UserControl
    {
        public Page2_2()
        {
            InitializeComponent();
            DataContext = new Page2_2ViewModel(this);
        }
    }
}
