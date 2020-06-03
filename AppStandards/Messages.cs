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
    /// Contains methods for showing dialog messages to the user.
    /// </summary>
    public static class Messages
    {
        #region Error message overloads
        /// <summary>
        /// Shows an error message to the user with the error image.
        /// </summary>
        /// <param name="userMessage">The error message to display to the user.</param>
        /// <param name="appInfo">The application's information.</param>
        /// <param name="writeToLog">Indicates whether or not a message should be written to the application's log file before the error message is shown to the user.</param>
        /// <param name="logMessage">The message to log. If null, with writeToLog set to true, the userMessage will be logged.</param>
        /// <param name="buttons">The buttons to display on the <see cref="MessageBox"/>.</param>
        /// <param name="defaultResult">The defualt result for the <see cref="MessageBox"/>.</param>
        /// <param name="closeApp">Indicates whether or not the application should be closed after displaying the error message to the user. Performs an Application.Current.Shutdown() call.</param>
        /// <param name="forceQuit">Indicates whether or not the application should be terminated after displaying the error message to the user. Performs an Environment.Exit() call.</param>
        /// <returns>The <see cref="MessageBoxResult"/> of the <see cref="MessageBox"/>.</returns>
        public static MessageBoxResult ErrorMessage(string userMessage, IAppInfo appInfo, bool writeToLog = true, string logMessage = null, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxResult defaultResult = MessageBoxResult.OK, bool closeApp = false, bool forceQuit = false)
        {
            #region Null checks
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new ArgumentException("userMessage", nameof(userMessage));
            }

            if (appInfo == null)
            {
                throw new ArgumentNullException(nameof(appInfo));
            }
            #endregion

            if (writeToLog)
            {
                string _logMessage = string.IsNullOrWhiteSpace(logMessage) ? userMessage : logMessage;
                appInfo.Log.QueueLogMessageAsync(_logMessage, LogMessageType.Error);
            }

            var result = MessageBox.Show(userMessage, $"Error - {appInfo.AppName}", buttons, MessageBoxImage.Error, defaultResult);

            if (forceQuit)
            {
                Environment.Exit(0);
            }

            if (closeApp)
            {
                Application.Current.Shutdown();
            }

            return result;
        }

        /// <summary>
        /// Shows an error message to the user with the error image.
        /// </summary>
        /// <param name="userMessage">The error message to display to the user.</param>
        /// <param name="appName">The application's name.</param>
        /// <param name="buttons">The buttons to display on the <see cref="MessageBox"/>.</param>
        /// <param name="defaultResult">The defualt result for the <see cref="MessageBox"/>.</param>
        /// <param name="closeApp">Indicates whether or not the application should be closed after displaying the error message to the user. Performs an Application.Current.Shutdown() call.</param>
        /// <param name="forceQuit">Indicates whether or not the application should be terminated after displaying the error message to the user. Performs an Environment.Exit() call.</param>
        /// <returns>The <see cref="MessageBoxResult"/> of the <see cref="MessageBox"/>.</returns>
        public static MessageBoxResult ErrorMessage(string userMessage, string appName, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxResult defaultResult = MessageBoxResult.OK, bool closeApp = false, bool forceQuit = false)
        {
            #region Null checks
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new ArgumentException("userMessage", nameof(userMessage));
            }

            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentNullException(nameof(appName));
            }
            #endregion

            var result = MessageBox.Show(userMessage, $"Error - {appName}", buttons, MessageBoxImage.Error, defaultResult);

            if (forceQuit)
            {
                Environment.Exit(0);
            }

            if (closeApp)
            {
                Application.Current.Shutdown();
            }

            return result;
        }
        #endregion

        #region Warning message overloads
        /// <summary>
        /// Shows a warning message to the user with the warning image.
        /// </summary>
        /// <param name="userMessage">The warning message to display to the user.</param>
        /// <param name="appInfo">The application's information.</param>
        /// <param name="writeToLog">Indicates whether or not a message should be written to the application's log file before the warning message is shown to the user.</param>
        /// <param name="logMessage">The message to log. If null, with writeToLog set to true, the userMessage will be logged.</param>
        /// <param name="buttons">The buttons to display on the <see cref="MessageBox"/>.</param>
        /// <param name="defaultResult">The defualt result for the <see cref="MessageBox"/>.</param>
        /// <returns>The <see cref="MessageBoxResult"/> of the <see cref="MessageBox"/>.</returns>
        public static MessageBoxResult WarningMessage(string userMessage, IAppInfo appInfo, bool writeToLog = false, string logMessage = null, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxResult defaultResult = MessageBoxResult.OK)
        {
            #region Null checks
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new ArgumentException("userMessage", nameof(userMessage));
            }

            if (appInfo == null)
            {
                throw new ArgumentNullException(nameof(appInfo));
            }
            #endregion

            if (writeToLog)
            {
                string _logMessage = string.IsNullOrWhiteSpace(logMessage) ? userMessage : logMessage;
                appInfo.Log.QueueLogMessageAsync(_logMessage, LogMessageType.Warning);
            }

            return MessageBox.Show(userMessage, $"Warning - {appInfo.AppName}", buttons, MessageBoxImage.Warning, defaultResult);
        }

        /// <summary>
        /// Shows a warning message to the user with the warning image.
        /// </summary>
        /// <param name="userMessage">The warning message to display to the user.</param>
        /// <param name="appName">The application's name.</param>
        /// <param name="buttons">The buttons to display on the <see cref="MessageBox"/>.</param>
        /// <param name="defaultResult">The defualt result for the <see cref="MessageBox"/>.</param>
        /// <returns>The <see cref="MessageBoxResult"/> of the <see cref="MessageBox"/>.</returns>
        public static MessageBoxResult WarningMessage(string userMessage, string appName, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxResult defaultResult = MessageBoxResult.OK)
        {
            #region Null checks
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new ArgumentException("userMessage", nameof(userMessage));
            }

            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentNullException(nameof(appName));
            }
            #endregion

            return MessageBox.Show(userMessage, $"Warning - {appName}", buttons, MessageBoxImage.Warning, defaultResult);
        }
        #endregion

        #region Info message overloads
        /// <summary>
        /// Shows an information message to the user with the information image.
        /// </summary>
        /// <param name="userMessage">The information message to display to the user.</param>
        /// <param name="appInfo">The application's information.</param>
        /// <param name="writeToLog">Indicates whether or not a message should be written to the application's log file before the information message is shown to the user.</param>
        /// <param name="logMessage">The message to log. If null, with writeToLog set to true, the userMessage will be logged.</param>
        /// <param name="buttons">The buttons to display on the <see cref="MessageBox"/>.</param>
        /// <param name="defaultResult">The defualt result for the <see cref="MessageBox"/>.</param>
        /// <returns>The <see cref="MessageBoxResult"/> of the <see cref="MessageBox"/>.</returns>
        public static MessageBoxResult InfoMessage(string userMessage, IAppInfo appInfo, bool writeToLog = false, string logMessage = null, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxResult defaultResult = MessageBoxResult.OK)
        {
            #region Null checks
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new ArgumentException("message", nameof(userMessage));
            }

            if (appInfo == null)
            {
                throw new ArgumentNullException(nameof(appInfo));
            }
            #endregion

            if (writeToLog)
            {
                string _logMessage = string.IsNullOrWhiteSpace(logMessage) ? userMessage : logMessage;
                appInfo.Log.QueueLogMessageAsync(_logMessage, LogMessageType.Information);
            }

            return MessageBox.Show(userMessage, $"Info - {appInfo.AppName}", buttons, MessageBoxImage.Information, defaultResult);
        }

        /// <summary>
        /// Shows an information message to the user with the information image.
        /// </summary>
        /// <param name="userMessage">The information message to display to the user.</param>
        /// <param name="appName">The application's name.</param>
        /// <param name="buttons">The buttons to display on the <see cref="MessageBox"/>.</param>
        /// <param name="defaultResult">The defualt result for the <see cref="MessageBox"/>.</param>
        /// <returns>The <see cref="MessageBoxResult"/> of the <see cref="MessageBox"/>.</returns>
        public static MessageBoxResult InfoMessage(string userMessage, string appName, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxResult defaultResult = MessageBoxResult.OK)
        {
            #region Null checks
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new ArgumentException("message", nameof(userMessage));
            }

            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentNullException(nameof(appName));
            }
            #endregion

            return MessageBox.Show(userMessage, $"Info - {appName}", buttons, MessageBoxImage.Information, defaultResult);
        }
        #endregion

        #region Confirm action overloads
        /// <summary>
        /// Shows a message to the user with the question image.
        /// <para>Used to confirm a user action.</para>
        /// </summary>
        /// <param name="userMessage">The message to display to the user.</param>
        /// <param name="appInfo">The application's information.</param>
        /// <param name="buttons">The buttons to display on the <see cref="MessageBox"/>.</param>
        /// <param name="defaultResult">The defualt result for the <see cref="MessageBox"/>.</param>
        /// <returns>The <see cref="MessageBoxResult"/> of the <see cref="MessageBox"/>.</returns>
        public static MessageBoxResult ConfirmAction(string userMessage, IAppInfo appInfo, MessageBoxButton buttons = MessageBoxButton.YesNo, MessageBoxResult defaultResult = MessageBoxResult.No)
        {
            #region Null checks
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new ArgumentException("message", nameof(userMessage));
            }

            if (appInfo == null)
            {
                throw new ArgumentNullException(nameof(appInfo));
            }
            #endregion

            return MessageBox.Show(userMessage, $"Confirm Action - {appInfo.AppName}", buttons, MessageBoxImage.Question, defaultResult);
        }

        /// <summary>
        /// Shows a message to the user with the question image.
        /// <para>Used to confirm a user action.</para>
        /// </summary>
        /// <param name="userMessage">The message to display to the user.</param>
        /// <param name="appName">The application's name.</param>
        /// <param name="buttons">The buttons to display on the <see cref="MessageBox"/>.</param>
        /// <param name="defaultResult">The defualt result for the <see cref="MessageBox"/>.</param>
        /// <returns>The <see cref="MessageBoxResult"/> of the <see cref="MessageBox"/>.</returns>
        public static MessageBoxResult ConfirmAction(string userMessage, string appName, MessageBoxButton buttons = MessageBoxButton.YesNo, MessageBoxResult defaultResult = MessageBoxResult.No)
        {
            #region Null checks
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new ArgumentException("message", nameof(userMessage));
            }

            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentNullException(nameof(appName));
            }
            #endregion

            return MessageBox.Show(userMessage, $"Confirm Action - {appName}", buttons, MessageBoxImage.Question, defaultResult);
        }
        #endregion
    }
}
