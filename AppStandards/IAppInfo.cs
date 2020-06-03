using AppStandards.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppStandards
{
    /// <summary>
    /// An application information interface.
    /// </summary>
    public interface IAppInfo
    {
        /// <summary>
        /// The name of the application.
        /// </summary>
        string AppName { get; }
        /// <summary>
        /// The application's version number.
        /// </summary>
        string VersionNumber { get; }
        /// <summary>
        /// The company which produced the application.
        /// </summary>
        string Company { get; }
        /// <summary>
        /// The application's log file.
        /// </summary>
        Log Log { get; }
    }
}
