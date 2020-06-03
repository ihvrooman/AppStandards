using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppStandards.Helpers
{
    /// <summary>
    /// Contains computer-related methods and properties.
    /// </summary>
    public class ComputerHelper
    {
        #region Public methods
        /// <summary>
        /// Gets the timespan representing the difference between the remote computer's local time and the current computer's local time.
        /// </summary>
        /// <param name="remoteComputerName">The name of the remote computer.</param>
        public static TimeSpan GetRemoteComputerOffsetFromLocalTime(string remoteComputerName)
        {
            #region Validate parameters
            var cDrive = new DirectoryInfo($"\\\\{remoteComputerName}\\c$");
            if (!cDrive.Exists)
            {
                throw new ArgumentException($"The specified remote computer could not be found.");
            }
            #endregion

            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "net";
            process.StartInfo.Arguments = @"time \\" + remoteComputerName;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();

            while (!process.StandardOutput.EndOfStream)
            {
                string currentline = process.StandardOutput.ReadLine();
                if (!string.IsNullOrEmpty(currentline) && currentline.Contains("(GMT"))
                {
                    var remoteComputerGMTOffsetString = string.Empty;
                    var remoteComputerGMTOffsetHours = 0;
                    var remoteComputerGMTOffsetMins = 0;
                    if (currentline.Contains("(GMT)"))
                    {
                        /* If the result has the string '(GMT)', that means that the remote computer's 
                         * GMT offset is zero (a.k.a that the server is set to UTC time).
                         */                        
                        remoteComputerGMTOffsetHours = 0;
                        remoteComputerGMTOffsetMins = 0;
                    }
                    else
                    {
                        try
                        {
                            remoteComputerGMTOffsetString = Regex.Match(currentline, @"(\(GMT[\+-]?\d{2}:\d{2}\))").Value;
                            remoteComputerGMTOffsetHours = Convert.ToInt32(remoteComputerGMTOffsetString.Substring(4, 3));
                            remoteComputerGMTOffsetMins = Convert.ToInt32(remoteComputerGMTOffsetString.Substring(8, 2));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Could not get remote computer GMT offset. RemoteComputerGMTOffsetString: \"{remoteComputerGMTOffsetString}\". Error message: {ex.Message}");
                        }
                    }
                    var remoteComputerUTCOffset = new TimeSpan(remoteComputerGMTOffsetHours, remoteComputerGMTOffsetMins, 0);
                    var localComputerUTCOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
                    return remoteComputerUTCOffset - localComputerUTCOffset;
                }
            }

            return new TimeSpan(0, 0, 0);
        }
        #endregion
    }
}
