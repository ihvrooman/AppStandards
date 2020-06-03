using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AppStandards.Logging
{
    /// <summary>
    /// Contains methods for writing to a log file.
    /// </summary>
    public class Log
    {
        #region Fields
        /// <summary>
        /// The online (server) log folder.
        /// </summary>
        private DirectoryInfo _onlineLogFolder;
        /// <summary>
        /// The offline (local) log folder.
        /// </summary>
        private DirectoryInfo _offlineLogFolder;
        /// <summary>
        /// The application name.
        /// </summary>
        private readonly string _appName;
        /// <summary>
        /// The online (server) log file.
        /// </summary>
        private FileInfo _onlineLogFile
        {
            get
            {
                return new FileInfo(_onlineLogFolder.FullName + "\\" + DateTime.UtcNow.ToString("yyyyMMdd") + "_" + _appName + ".log");
            }
        }
        /// <summary>
        /// The offline (local) log file.
        /// </summary>
        private FileInfo _offlineLogFile
        {
            get
            {
                return new FileInfo(_offlineLogFolder.FullName + "\\" + DateTime.UtcNow.ToString("yyyyMMdd") + "_" + _appName + ".log");
            }
        }
        /// <summary>
        /// The timestamp used for each log message.
        /// </summary>
        private string _logMessageTimestamp
        {
            get
            {
                return DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss:ffff");
            }
        }
        /// <summary>
        /// Indicates whether or not the application is online (able to reach the online log folder).
        /// </summary>
        private volatile bool _online;
        /// <summary>
        /// Indicates whether or not the async tasks should run.
        /// </summary>
        private volatile bool _runAsyncTasks = true;
        /// <summary>
        /// A string containing the <see cref="Environment.UserName"/> and <see cref="Environment.MachineName"/> of the calling application that can be appended to a log message which helps to identify the application that logged the message.
        /// </summary>
        private string _identifyingInfoSuffix
        {
            get
            {
                return $" | Username: {Environment.UserName} | Computer name: {Environment.MachineName}";
            }
        }
        /// <summary>
        /// A queue that holds log messages that are waiting to be written to the log file.
        /// </summary>
        private volatile List<string> _logMessageQueue = new List<string>();
        /// <summary>
        /// Indicates whether or not the <see cref="_logMessageQueue"/> is being flushed.
        /// </summary>
        private volatile bool _flushingLogMessageQueue;
        /// <summary>
        /// Indicates whether or not logging is disabled.
        /// </summary>
        private volatile bool _loggingDisabled;
        /// <summary>
        /// Indicates whether or not the log has been initialized.
        /// </summary>
        private volatile bool _initialized;
        private readonly object _logMessageQueueLock = new object();
        private readonly object _queueLogMessageLock = new object();
        private volatile int _numberOfLogMessagesWaitingToBeQueued;
        private readonly object _logMessageQueueCountLock = new object();
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new <see cref="Log"/>.
        /// </summary>
        /// <param name="onlineLogFolderPath">The log folder path used when the client running the application is connected to the intranet. Usually a dev server.</param>
        /// <param name="offlineLogFolderPath">The local log folder path used when the client running the application is not connected to the intranet. Must exist locally on the client machine.</param>
        /// <param name="appName">The application name.</param>
        public Log(string onlineLogFolderPath, string offlineLogFolderPath, string appName)
        {
            _appName = appName;
            InitializeAsync(onlineLogFolderPath, offlineLogFolderPath);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Addes a log message to the log message queue. The log message queue is automatically flushed which writes all log messages to the log file in the order that they were received.
        /// <para>The flushing operation first tries to write to the online log. If unsuccessful, will write to the offline log.</para>
        /// </summary>
        /// <param name="logMessage">The message to log.</param>
        /// <param name="logMessageType">The log message type.</param>
        /// <param name="addIdentifyingInformation">Indicates whether or not identifying information (such as the username and machine name) should be added to the log message.</param>
        public void QueueLogMessageAsync(string logMessage, LogMessageType logMessageType = LogMessageType.Information, bool addIdentifyingInformation = true)
        {
            lock(_logMessageQueueCountLock)
            {
                _numberOfLogMessagesWaitingToBeQueued++;
            }
            var timeStamp = _logMessageTimestamp;
            Task.Run(() =>
            {
                lock (_queueLogMessageLock)
                {
                    logMessage = logMessage.Replace(Environment.NewLine, " ");
                    logMessage = logMessage.Replace("-E-", "_E_");
                    logMessage = logMessage.Replace("-W-", "_W_");
                    logMessage = logMessage.Replace("-I-", "_I_");
                    logMessage = logMessage.Replace("-V-", "_V_");
                    logMessage = logMessage.Replace("-D-", "_D_");
                    logMessage = logMessage.Replace("-D:E-", "_D:E_");
                    logMessage = logMessage.Replace("-D:W-", "_D:W_");
                    logMessage = logMessage.Replace("-D:I-", "_D:I_");
                    logMessage = logMessage.Replace("-D:V-", "_D:V_");
                    logMessage = logMessage.Replace("-U-", "_U_");
                    logMessage = $"{GetLogMessagePrefix(logMessageType)} {timeStamp} {logMessage}";

                    if (addIdentifyingInformation)
                    {
                        logMessage += _identifyingInfoSuffix;
                    }

                    try
                    {
                        if (Debugger.IsAttached)
                        {
                            //If a debugger is attached, write to the debug output.
                            Debug.WriteLine(logMessage);
                            Debug.Flush();
                        }
                    }
                    catch
                    {
                        //Fail silently
                    }

                    if (!_loggingDisabled)
                    {
                        AddToLogMessageQueue(logMessage);
                    }

                    lock (_logMessageQueueCountLock)
                    {
                        _numberOfLogMessagesWaitingToBeQueued--;
                    }
                }
            });
        }

        /// <summary>
        /// Stops async log tasks.
        /// <para>Note: This method may take a few minutes to finish executing. If it's being called when a window closes, call it from the 'Closed' event rather than the 'Closing' event so that the window doesn't remain open after the user closes it.</para>
        /// </summary>
        public void Dispose()
        {
            Task.Delay(100).Wait();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            while (!_initialized && stopWatch.Elapsed.TotalSeconds < 60)
            {
                //If the log is initializing, wait up to 60s for it to finish
                Task.Delay(100).Wait();
            }
            stopWatch.Stop();
            stopWatch.Reset();
            stopWatch.Start();
            while (_numberOfLogMessagesWaitingToBeQueued > 0 && stopWatch.Elapsed.TotalMinutes < 3)
            {
                //If log messages are waiting to be queued, wait up to 3 min
                Task.Delay(100).Wait();
            }
            stopWatch.Stop();
            stopWatch.Reset();
            stopWatch.Start();
            while (_flushingLogMessageQueue && stopWatch.Elapsed.TotalMinutes < 3)
            {
                //If the log message queue is being flushed, wait up to 3 min for it to finish
                Task.Delay(100).Wait();
            }
            stopWatch.Stop();

            //Tell async tasks to stop and wait 5s for them to gracefully exit
            _runAsyncTasks = false;
            Task.Delay(5000).Wait();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Initializes the <see cref="Logging.Log"/> asynchronously.
        /// </summary>
        /// <param name="onlineLogFolderPath">The log folder path used when the client running the application is connected to the intranet. Usually a dev server.</param>
        /// <param name="offlineLogFolderPath">The local log folder path used when the client running the application is not connected to the intranet. Must exist locally on the client machine.</param>
        private void InitializeAsync(string onlineLogFolderPath, string offlineLogFolderPath)
        {
            Task.Run(() =>
            {
                _onlineLogFolder = new DirectoryInfo(onlineLogFolderPath);
                _offlineLogFolder = new DirectoryInfo(offlineLogFolderPath);

                //Create log folders
                try
                {
                    CreateLogFolder(_onlineLogFolder, false);
                    try
                    {
                        CreateLogFolder(_offlineLogFolder, false);
                    }
                    catch (Exception ex)
                    {
                        //If online log folder was created but offline log folder couln't be created, log error message and it will be written to online log
                        QueueLogMessageAsync($"Could not create offline log folder with path \"{offlineLogFolderPath}\". Error message: {ex.Message}", LogMessageType.Warning);
                    }
                }
                catch (Exception ex)
                {
                    //If online log folder couldn't be created
                    try
                    {
                        CreateLogFolder(_offlineLogFolder, false);
                        //If offline log folder was created, log error message and it will be written to offline log
                        QueueLogMessageAsync($"Could not create online log folder with path \"{onlineLogFolderPath}\". Error message: {ex.Message}", LogMessageType.Warning);
                    }
                    catch
                    {
                        //If creation of online and offline log folders fails, notify user and disable logging
                        Messages.WarningMessage($"{_appName} could not create a folder for its logs.{Environment.NewLine}Logging will be disabled for the remainder of the session.", _appName);
                        _loggingDisabled = true;
                    }
                }

                if (!_loggingDisabled)
                {
                    CheckOnlineStatusAsync();
                    SyncLogFilesAsync();
                    FlushLogMessageQueueAsync();
                }
                _initialized = true;
            });
        }

        /// <summary>
        /// Gets the log message prefix: the letter corresponding to the provided <see cref="LogMessageType"/>.
        /// </summary>
        /// <param name="logMessageType">The type of log message.</param>
        /// <returns>A string containing the log message prefix.</returns>
        private string GetLogMessagePrefix(LogMessageType logMessageType)
        {
            string prefixLetter;
            switch (logMessageType)
            {
                case LogMessageType.Error:
                    prefixLetter = "-E-";
                    break;
                case LogMessageType.Warning:
                    prefixLetter = "-W-";
                    break;
                case LogMessageType.Information:
                    prefixLetter = "-I-";
                    break;
                case LogMessageType.Verbose:
                    prefixLetter = "-V-";
                    break;
                case LogMessageType.Debug:
                    prefixLetter = "-D-";
                    break;
                case LogMessageType.DebugError:
                    prefixLetter = "-D:E-";
                    break;
                case LogMessageType.DebugWarning:
                    prefixLetter = "-D:W-";
                    break;
                case LogMessageType.DebugInformation:
                    prefixLetter = "-D:I-";
                    break;
                case LogMessageType.DebugVerbose:
                    prefixLetter = "-D:V-";
                    break;
                default:
                    prefixLetter = "-U-";
                    break;
            }
            return prefixLetter;
        }

        /// <summary>
        /// Creates the specified log folder.
        /// </summary>
        /// <param name="logFolder">The log folder to create.</param>
        /// <param name="suppressExceptions">Indicates whether or not to suppress exceptions that are thrown when attempting to create the log folder.</param>
        /// <returns>A bool indicating whether or not the log folder was successfully created.</returns>
        private bool CreateLogFolder(DirectoryInfo logFolder, bool suppressExceptions = true)
        {
            logFolder.Refresh();
            if (logFolder.Exists)
            {
                return true;
            }

            try
            {
                logFolder.Create();
                return true;
            }
            catch (Exception ex)
            {
                if (!suppressExceptions)
                {
                    throw ex;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not the application is online (able to access the online log folder).
        /// </summary>
        private void CheckOnlineStatusAsync()
        {
            Task.Run(() =>
            {
                while (_runAsyncTasks)
                {
                    _onlineLogFolder.Refresh();
                    _online = _onlineLogFolder.Exists || CreateLogFolder(_onlineLogFolder);
                    Task.Delay(50).Wait();
                }
            });
        }

        /// <summary>
        /// Syncs the log files by transfering the offline log files to the online log file folder.
        /// </summary>
        private void SyncLogFilesAsync()
        {
            Task.Run(() =>
            {
                while (_runAsyncTasks)
                {
                    while (_runAsyncTasks && _online && GetLogFiles(_offlineLogFolder).Count > 0)
                    {
                        try
                        {
                            /* Note: The offline log file to sync and the online log file
                             * below are not neccessarily from the current day, they may be
                             * from another day and therefore different than the 
                             * _offlineLogFile and _onlineLogFile.
                             */

                            var offlineLogFileToSync = GetLogFiles(_offlineLogFolder)[0];
                            var onlineLogFile = new FileInfo(_onlineLogFolder + "//" + offlineLogFileToSync.Name);
                            var deleteOfflineLogFile = false;
                            onlineLogFile.Refresh();
                            if (!onlineLogFile.Exists)
                            {
                                onlineLogFile.Create();
                            }

                            using (StreamReader onlineSR = GetStreamReader(onlineLogFile))
                            {
                                using (StreamReader offlineSR = GetStreamReader(offlineLogFileToSync, FileShare.Write))
                                {
                                    while (_runAsyncTasks && _online && !offlineSR.EndOfStream)
                                    {
                                        var onlineLine = onlineSR.ReadLine();
                                        var offlineLine = offlineSR.ReadLine();
                                        if (onlineLine != offlineLine)
                                        {
                                            WriteLogMessageToOnlineLogFile(onlineLogFile, offlineLine, suppressExceptions: false);
                                        }
                                    }

                                    if (offlineSR.EndOfStream)
                                    {
                                        //Now that the log file has been synced to the server, delete it
                                        deleteOfflineLogFile = true;
                                    }
                                }
                            }

                            if (deleteOfflineLogFile)
                            {
                                offlineLogFileToSync.Delete();
                            }
                        }
                        catch
                        {
                            //Fail silently
                        }
                        Task.Delay(50).Wait();
                    }
                    Task.Delay(50).Wait();
                }
            });
        }

        /// <summary>
        /// Retrieves all of the log files within the specified folder.
        /// </summary>
        /// <param name="logFolder">The folder containing the log files to be retrieved.</param>
        /// <returns>A list containing all of the log files found in the specified folder.</returns>
        private List<FileInfo> GetLogFiles(DirectoryInfo logFolder)
        {
            List<FileInfo> logFiles = new List<FileInfo>();
            logFolder.Refresh();
            if (logFolder.Exists)
            {
                foreach (var file in logFolder.GetFiles())
                {
                    if (file.Extension == ".log")
                    {
                        logFiles.Add(file);
                    }
                }
            }
            return logFiles;
        }

        /// <summary>
        /// Gets a <see cref="StreamWriter"/> for the specified file.
        /// </summary>
        /// <param name="file">The file which the <see cref="StreamWriter"/> must access.</param>
        /// <returns>The requested <see cref="StreamWriter"/>.</returns>
        private StreamWriter GetStreamWriter(FileInfo file)
        {
            return new StreamWriter(new FileStream(file.FullName, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                AutoFlush = false,
            };
        }

        /// <summary>
        /// Gets a <see cref="StreamReader"/> for the specified file.
        /// </summary>
        /// <param name="file">The file which the <see cref="StreamReader"/> must access.</param>
        /// <param name="fileShare">The <see cref="FileShare"/> to use on the <see cref="StreamReader"/>.</param>
        /// <returns>The requested <see cref="StreamReader"/>.</returns>
        private StreamReader GetStreamReader(FileInfo file, FileShare fileShare = FileShare.ReadWrite)
        {
            return new StreamReader(new FileStream(file.FullName, FileMode.Open, FileAccess.Read, fileShare));
        }

        /// <summary>
        /// Adds a log message to the <see cref="_logMessageQueue"/> and calles the <see cref="FlushLogMessageQueueAsync"/> method if it is not already running.
        /// </summary>
        /// <param name="logMessage"></param>
        private void AddToLogMessageQueue(string logMessage)
        {
            //Because logMessages are written to log file from end of queue, new items should be inserted at beginning of queue so that it's first-in-first-out
            lock (_logMessageQueueLock)
            {
                _logMessageQueue.Insert(0, logMessage);
            }
        }

        /// <summary>
        /// Flushes the <see cref="_logMessageQueue"/>.
        /// </summary>
        private void FlushLogMessageQueueAsync()
        {
            Task.Run(() =>
            {
                try
                {
                    while (_runAsyncTasks)
                    {
                        while (_logMessageQueue.Count > 0)
                        {
                            _flushingLogMessageQueue = true;

                            var logMessageString = string.Empty;
                            lock (_logMessageQueueLock)
                            {
                                var logMessageQueueCount = _logMessageQueue.Count;
                                var lowerBound = logMessageQueueCount > 5 ? logMessageQueueCount - 5 : 0;
                                for (int i = logMessageQueueCount - 1; i >= lowerBound; i--)
                                {
                                    if (i < logMessageQueueCount - 1)
                                    {
                                        logMessageString += Environment.NewLine;
                                    }
                                    logMessageString += _logMessageQueue[i];
                                    _logMessageQueue.RemoveAt(i);
                                }
                            }

                            WriteLogMessageToLogFile(logMessageString);
                        }
                        _flushingLogMessageQueue = false;
                        Task.Delay(50).Wait();
                    }
                }
                catch (Exception ex)
                {
                    //If flushing log message queue fails, notify user
                    Messages.WarningMessage($"{_appName} could not successfully complete a logging operation.{Environment.NewLine}Logging will be disabled for the remainder of the session.", _appName);
                    _loggingDisabled = true;
                    try
                    {
                        //If flushing log message queue fails, try to log error message
                        var logMessage = $"{GetLogMessagePrefix(LogMessageType.Error)} {$"An unexpected error occured in AppStandards.FlushLogMessageQueue(). Logging for this application will hereby be disabled for the remainder of the session. Error message: {ex.Message + _identifyingInfoSuffix}"}".Replace(Environment.NewLine, " ");
                        WriteLogMessageToLogFile(logMessage);
                    }
                    catch
                    {
                        //If trying to log error message fails, fail silently
                    }
                }
            });
        }

        /// <summary>
        /// Writes the specified log messages to the online log file (if available and if the offline log file doesn't exist) or the offline log file in all other cases.
        /// </summary>
        /// <param name="logMessage"></param>
        private void WriteLogMessageToLogFile(string logMessage)
        {
            //Try writing to online log file as long as there's not an offline log file that hasn't been synced yet
            _offlineLogFile.Refresh();
            if (_online && !_offlineLogFile.Exists)
            {
                //Create online log folder if it doesn't exist
                if (CreateLogFolder(_onlineLogFolder))
                {
                    //Write log message
                    WriteLogMessageToOnlineLogFile(_onlineLogFile, logMessage);
                    return;
                }
            }

            //If can't write to online log or the offline log file hasn't been synced yet, write to offline log
            //Create offline log folder if it doesn't exist
            if (CreateLogFolder(_offlineLogFolder))
            {
                try
                {
                    //Write log message
                    using (var sw = GetStreamWriter(_offlineLogFile))
                    {
                        sw.WriteLine(logMessage);
                        sw.Flush();
                    }
                    return;
                }
                catch
                {
                    //If writing to offline log fails, fail silently.
                }
            }

            //If we get to this point and we haven't written to the log file, put the log entries back in the queue (which will effectively retry)
            AddToLogMessageQueue(logMessage);
        }

        /// <summary>
        /// Attempts to write the given log message to the online log file using the given stream writer. If write fails, this method will retry the number of times specified.
        /// </summary>
        /// <param name="onlineLogFile">The online log file.</param>
        /// <param name="logMessage">The log message to write.</param>
        /// <param name="retryCount">The number of times to retry. Note: This method has a maximum retry limit of 100.</param>
        /// <param name="suppressExceptions">Indicates whether or not the method should suppress exceptions.</param>
        private void WriteLogMessageToOnlineLogFile(FileInfo onlineLogFile, string logMessage, int retryCount = 50, bool suppressExceptions = true)
        {
            var tryCount = 0;
            while (tryCount < retryCount && tryCount < 100 && !_loggingDisabled && _runAsyncTasks)
            {
                try
                {
                    using (var onlineLogFileStreamWriter = GetStreamWriter(onlineLogFile))
                    {
                        onlineLogFileStreamWriter.WriteLine(logMessage);
                        onlineLogFileStreamWriter.Flush();
                    }
                    return;
                }
                catch (Exception ex)
                {
                    if (tryCount >= retryCount || tryCount >= 100)
                    {
                        //If the last try failed
                        if (!suppressExceptions)
                        {
                            //If we aren't suppressing exceptions
                            throw ex;
                        }
                        else if (!_online)
                        {
                            //If we get to the last try and we haven't written to the online log file and we are now offline, put the log entries back in the queue (which will try writing to the offline log file)
                            AddToLogMessageQueue(logMessage);
                        }
                    }
                    else
                    {
                        //Wait before trying again
                        Task.Delay(1).Wait();
                    }
                }
                tryCount++;
            }
        }
        #endregion
    }
}
