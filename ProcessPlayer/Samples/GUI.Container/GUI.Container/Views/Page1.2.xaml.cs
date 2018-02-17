using GUIContainer.ViewModels;
using System.Windows.Controls;

namespace GUIContainer.Views
{
    /// <summary>
    /// Interaction logic for Process.xaml
    /// </summary>
    public partial class Page1_2 : UserControl
    {
        public Page1_2()
        {
            InitializeComponent();
            DataContext = new Page1_2ViewModel(this);
        }
    }
}
