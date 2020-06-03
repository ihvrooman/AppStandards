using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStandards.Logging
{
    /// <summary>
    /// The type of log message.
    /// </summary>
    public enum LogMessageType
    {
        Error,
        Warning,
        Information,
        Verbose,
        Debug,
        DebugError,
        DebugWarning,
        DebugInformation,
        DebugVerbose,
        Unknown,
    }
}
