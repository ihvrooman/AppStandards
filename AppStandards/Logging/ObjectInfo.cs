using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStandards.Logging
{
    /// <summary>
    /// Contains methods for gathering and logging information about objects that can be used for debugging purposes.
    /// </summary>
    public class ObjectInfo
    {
        #region Fields
        private static volatile Dictionary<string, ObjectCounter> _objectCounts = new Dictionary<string, ObjectCounter>();
        private readonly int _objectId;
        private readonly string _objectTypeName;
        private readonly bool _expectsMoreThanOneInstance;
        private static readonly string _processId = GetProcessId();
        private readonly IAppInfo _appInfo;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new <see cref="ObjectInfo"/> object.
        /// <para>This <see cref="ObjectInfo"/> constructor should be called from within the object's constructor.</para>
        /// </summary>
        /// <param name="objectType">The object's type.</param>
        /// <param name="appInfo">The application's <see cref="IAppInfo"/>.</param>
        /// <param name="expectsMoreThanOneInstance">Indicates whether or not there should be more than one instance of the object in existence at any given time.</param>
        public ObjectInfo(Type objectType, IAppInfo appInfo, bool expectsMoreThanOneInstance = false)
        {
            _objectTypeName = objectType.Name;
            _appInfo = appInfo;
            _expectsMoreThanOneInstance = expectsMoreThanOneInstance;      
            IncrementObjectCountByOne(_objectTypeName);
            _objectId = GetNewObjectId(_objectTypeName);

            var moreInstancesThanExpected = !_expectsMoreThanOneInstance && GetObjectCount(_objectTypeName) > 1;
            var additionalMessage = moreInstancesThanExpected ? " This object does not expect to have more than one instance." : string.Empty;
            var logMessageType = moreInstancesThanExpected ? LogMessageType.Warning : LogMessageType.Information;
            LogObjectInformation($"Constructor called. | {_objectTypeName} count: {GetObjectCount(_objectTypeName)}.{additionalMessage}", logMessageType);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Logs the given log message prefixed by the object's type name and id and with the process id appended to the end.
        /// </summary>
        /// <param name="logMessage">The message to log.</param>
        /// <param name="logMessageType">The <see cref="LogMessageType"/>.</param>
        public void LogObjectInformation(string logMessage, LogMessageType logMessageType = LogMessageType.Information)
        {
            _appInfo.Log.QueueLogMessageAsync($"{_objectTypeName}{_objectId}: {logMessage} | ProcessId: {_processId}.", logMessageType);
            Task.Delay(1).Wait();
        }

        /// <summary>
        /// Decrements the count for the object type and logs that the object has disposed of it's <see cref="ObjectInfo"/>.
        /// </summary>
        public void Dispose()
        {
            DecrementObjectCountByOne(_objectTypeName);
            LogObjectInformation($"Disposing of ObjectInfo object. New {_objectTypeName} count: {GetObjectCount(_objectTypeName)}.");
        }
        #endregion

        #region Private methods
        private static int GetObjectCount(string objectIdentifier)
        {
            if (_objectCounts.TryGetValue(objectIdentifier, out ObjectCounter objectCounter))
            {
                return objectCounter.CurrentCount;
            }
            else
            {
                _objectCounts.Add(objectIdentifier, new ObjectCounter(0));
                return 0;
            }
        }

        private static void SetObjectCount(string objectIdentifier, int objectCount)
        {
            if (_objectCounts.ContainsKey(objectIdentifier))
            {
                _objectCounts[objectIdentifier].CurrentCount = objectCount;
            }
            else
            {
                _objectCounts.Add(objectIdentifier, new ObjectCounter(objectCount));
            }
        }

        private static int GetNewObjectId(string objectIdentifier)
        {
            if (_objectCounts.TryGetValue(objectIdentifier, out ObjectCounter objectCounter))
            {
                return objectCounter.TotalCount;
            }
            else
            {
                _objectCounts.Add(objectIdentifier, new ObjectCounter(0));
                return 0;
            }
        }

        private static void IncrementObjectCountByOne(string objectIdentifier)
        {
            var existingCount = GetObjectCount(objectIdentifier);
            SetObjectCount(objectIdentifier, existingCount + 1);
        }

        private static void DecrementObjectCountByOne(string objectIdentifier)
        {
            _objectCounts[objectIdentifier].CurrentCount--;
        }

        private static string GetProcessId()
        {
            return Process.GetCurrentProcess().Id.ToString();
        }
        #endregion
    }

    internal class ObjectCounter
    {
        #region Fields
        private volatile int _currentCount;
        private volatile int _totalCount;
        #endregion

        #region Properties
        /// <summary>
        /// The current number of objects.
        /// </summary>
        internal int CurrentCount
        {
            get { return _currentCount; }
            set
            {
                IncrementTotalCount(_currentCount, value);
                _currentCount = value;
            }
        }
        /// <summary>
        /// The total number of objects that have ever existed.
        /// </summary>
        internal int TotalCount { get { return _totalCount; } private set { _totalCount = value; } }
        #endregion

        #region Constructor
        internal ObjectCounter(int currentCount)
        {
            CurrentCount = currentCount;
        }
        #endregion

        #region Private methods
        private void IncrementTotalCount(int currentCount, int newCount)
        {
            if (newCount > currentCount)
            {
                //Only increment the total count if the count is increasing
                TotalCount += newCount - currentCount;
            }
        }
        #endregion
    }
}
