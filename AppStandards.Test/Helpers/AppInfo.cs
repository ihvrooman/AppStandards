using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AppStandards.Logging;
using AppStandards.Test.Properties;

namespace AppStandards.Test.Helpers
{
    /// <summary>
    /// A static class containing app-specific information.
    /// </summary>
    public static class AppInfo
    {
        /// <summary>
        /// An instance of the <see cref="BaseAppInfo"/> class which contains app-specific information. This object is used for many <see cref="AppStandards"/> operations.
        /// </summary>
        public static BaseAppInfo BaseAppInfo { get; private set; } = new BaseAppInfo();
    }

    /// <summary>
    /// An instance class containing app-specific information and components that inherits from <see cref="IAppInfo"/>.
    /// </summary>
    public class BaseAppInfo : IAppInfo
    {
        /// <summary>
        /// The application <see cref="Logging.Log"/>.
        /// </summary>
        private Log _log;
        /// <summary>
        /// The name of the application obtained from the assembly.
        /// </summary>
        public string AppName { get; } = Assembly.GetExecutingAssembly().GetName().Name;
        /// <summary>
        /// The application version number obtained from the assembly.
        /// </summary>
        public string VersionNumber { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// The company which developed the application obtained from the assembly.
        /// </summary>
        public string Company { get; } = "Weyerhaeuser";
        /// <summary>
        /// The application <see cref="Logging.Log"/>.
        /// </summary>
        public Log Log
        {
            get
            {
                if (_log == null)
                {
                    _log = new Log(Settings.Default.OnlineLogFolderPath, Settings.Default.OfflineLogFolderPath, AppName);
                }
                return _log;
            }
        }
    }
}
