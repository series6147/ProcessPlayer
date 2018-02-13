using Calculator.ViewModels;
using System.Windows;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected async override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (MainWindowViewModel.Current != null)
                await MainWindowViewModel.Current.ScriptPlayer.Root.Dispose();

            if (StartupWindowViewModel.Current != null)
                await StartupWindowViewModel.Current.ScriptPlayer.Root.Dispose();
        }
    }
}
