using AppStandards.Logging;
using AppStandards.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppStandards.Test.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        Timer _timer = new Timer(1000);
        Stopwatch _stopWatch = new Stopwatch();
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            Routines.Startup(AppInfo.BaseAppInfo);
            Closing += MainWindow_Closing;
            _timer.Elapsed += Timer_Elapsed;
            _stopWatch.Start();
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            AppInfo.BaseAppInfo.Log.QueueLogMessageAsync($"Timer fired. Stopwatch elapsed total milliseconds: {_stopWatch.Elapsed.TotalMilliseconds}", LogMessageType.Debug);
        }
        #endregion

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Routines.Shutdown(AppInfo.BaseAppInfo);
            _timer.Stop();
            _stopWatch.Stop();
        }
    }
}
