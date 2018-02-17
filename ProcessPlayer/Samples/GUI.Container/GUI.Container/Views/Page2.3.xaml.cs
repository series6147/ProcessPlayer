using GUIContainer.ViewModels;
using System.Windows.Controls;

namespace GUIContainer.Views
{
    /// <summary>
    /// Interaction logic for Process.xaml
    /// </summary>
    public partial class Page2_3 : UserControl
    {
        public Page2_3()
        {
            InitializeComponent();
            DataContext = new Page2_3ViewModel(this);
        }
    }
}
