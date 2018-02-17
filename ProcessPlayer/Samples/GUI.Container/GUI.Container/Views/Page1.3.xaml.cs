using GUIContainer.ViewModels;
using System.Windows.Controls;

namespace GUIContainer.Views
{
    /// <summary>
    /// Interaction logic for Process.xaml
    /// </summary>
    public partial class Page1_3 : UserControl
    {
        public Page1_3()
        {
            InitializeComponent();
            DataContext = new Page1_3ViewModel(this);
        }
    }
}
