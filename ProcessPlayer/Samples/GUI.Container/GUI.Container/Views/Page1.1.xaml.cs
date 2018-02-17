using GUIContainer.ViewModels;
using System.Windows.Controls;

namespace GUIContainer.Views
{
    /// <summary>
    /// Interaction logic for Process.xaml
    /// </summary>
    public partial class Page1_1 : UserControl
    {
        public Page1_1()
        {
            InitializeComponent();
            DataContext = new Page1_1ViewModel(this);
        }
    }
}
