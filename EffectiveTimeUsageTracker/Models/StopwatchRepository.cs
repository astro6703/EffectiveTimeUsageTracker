using System;
using System.Collections.Generic;

namespace EffectiveTimeUsageTracker.Models
{
    public class StopwatchRepository
    {
        private IDictionary<string, ObjectiveStopwatch> _stopwatchesMap;

        public StopwatchRepository()
        {
            _stopwatchesMap = new Dictionary<string, ObjectiveStopwatch>();
        }

        public ObjectiveStopwatch GetUserStopwatch(string username)
        {
            if (username == null) throw new ArgumentNullException("Username was null");

            if (_stopwatchesMap.TryGetValue(username, out var userStopwatch))
                return userStopwatch;

            var stopwatch = new ObjectiveStopwatch();
            _stopwatchesMap.Add(username, stopwatch);

            return stopwatch;
        }
    }
}