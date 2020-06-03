using AppStandards.Logging;
using AppStandards.UIControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AppStandards
{
    /// <summary>
    /// Contains standard application routines.
    /// </summary>
    public static class Routines
    {
        /// <summary>
        /// The application shutdown log message.
        /// </summary>
        private const string _shutdownString = "Shutting down.";

        #region Startup overloads
        /// <summary>
        /// Startup routine that logs an application startup message.
        /// <para>Note: The <see cref="Startup(IAppInfo)"/> overload is preferred becuase it includes the application version number in the startup log message.</para>
        /// </summary>
        /// <param name="log">The application <see cref="Log"/>.</param>
        public static void Startup(Log log)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            log.QueueLogMessageAsync($"Starting up.");
        }

        /// <summary>
        /// Startup routine that logs an application startup message.
        /// </summary>
        /// <param name="appInfo">The application's information.</param>
        public static void Startup(IAppInfo appInfo)
        {
            if (appInfo == null)
            {
                throw new ArgumentNullException(nameof(appInfo));
            }

            appInfo.Log.QueueLogMessageAsync($"Starting up. | Version: {appInfo.VersionNumber}");
        }
        #endregion

        #region Shutdown overloads
        /// <summary>
        /// Shutdown routine that logs program shutdown and disposes the application's <see cref="Log"/>.
        /// <para>Note: This method may take a few minutes to finish executing. If it's being called when a window closes, call it from the 'Closed' event rather than the 'Closing' event so that the window doesn't remain open after the user closes it.</para>
        /// </summary>
        /// <param name="log">The application's log file.</param>
        public static void Shutdown(Log log)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            log.QueueLogMessageAsync(_shutdownString);
            Task.Delay(10).Wait();
            log.Dispose();
        }

        /// <summary>
        /// Shutdown routine that logs program shutdown and disposes the application's <see cref="Log"/>.
        /// <para>Note: This method may take a few minutes to finish executing. If it's being called when a window closes, call it from the 'Closed' event rather than the 'Closing' event so that the window doesn't remain open after the user closes it.</para>
        /// </summary>
        /// <param name="appInfo">The application's information.</param>
        public static void Shutdown(IAppInfo appInfo)
        {
            if (appInfo == null)
            {
                throw new ArgumentNullException(nameof(appInfo));
            }

            appInfo.Log.QueueLogMessageAsync(_shutdownString);
            Task.Delay(100).Wait();
            appInfo.Log.Dispose();
        }
        #endregion
    }
}
