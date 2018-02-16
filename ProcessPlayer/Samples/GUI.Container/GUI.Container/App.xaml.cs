using GUIContainer.ViewModels;
using System.Windows;

namespace GUIContainer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected async override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (MainWindowViewModel.Current != null
                && MainWindowViewModel.Current.ScriptPlayer != null
                && MainWindowViewModel.Current.ScriptPlayer.Root != null)
                await MainWindowViewModel.Current.ScriptPlayer.Root.Dispose();
        }
    }
}
