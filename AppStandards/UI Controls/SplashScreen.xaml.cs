using AppStandards.MVVM;
using AppStandards.UI_Controls.UI_Control_View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppStandards.UIControls
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private SplashScreenViewModel _viewModel
        {
            get
            {
                return (SplashScreenViewModel)DataContext;
            }
        }

        /// <summary>
        /// Constructs a new <see cref="SplashScreen"/>.
        /// </summary>
        /// <param name="appInfo">The application information.</param>
        public SplashScreen(IAppInfo appInfo)
        {
            appInfo.Log.QueueLogMessageAsync($"Initializing splash screen.");
            InitializeComponent();
            DataContext = new SplashScreenViewModel(appInfo);
            Application.Current.MainWindow.Loaded += MainWindow_Loaded;
            appInfo.Log.QueueLogMessageAsync($"Splash screen initialized.");
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.AppInfo.Log.QueueLogMessageAsync($"Closing splash screen.");
            Close();
            _viewModel.AppInfo.Log.QueueLogMessageAsync($"Splash screen closed.");
        }
    }
}
