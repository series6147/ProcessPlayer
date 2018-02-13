using Calculator.ViewModels;
using ProcessPlayer.Content.Common;
using ProcessPlayer.Engine;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Calculator.Views
{
    /// <summary>
    /// Interaction logic for MainFrame.xaml
    /// </summary>
    public partial class MainFrame : UserControl
    {
        #region private variables

        private readonly DispatcherTimer _clockTimer;

        #endregion

        #region dependency properties

        public static readonly DependencyProperty CurrentTimeProperty = DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainFrame)
            , new PropertyMetadata(DateTime.Now));

        #endregion

        #region properties

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        #endregion

        #region constructors

        public MainFrame()
        {
            DataContext = new MainFrameViewModel();
            InitializeComponent();

            _clockTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            _clockTimer.Tick += ClockTimer_Tick;

            Task.Delay(1000 - DateTime.Now.Millisecond)
                .ContinueWith(t =>
                {
                    _clockTimer.Start();
                });
        }

        #endregion

        #region events

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
        }

        private void StopMachine_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var cnt = ScriptPlayer.Default.Root.getContentByID("stopToggle") as CommandButton;

            if (cnt != null)
                cnt.Command.Execute(null);
        }

        #endregion
    }
}
