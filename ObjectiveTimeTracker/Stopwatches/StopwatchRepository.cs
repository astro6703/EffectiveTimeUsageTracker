using System;
using System.Collections.Generic;

namespace ObjectiveTimeTracker.Stopwatches
{
    public class StopwatchRepository : IStopwatchRepository
    {
        private IDictionary<string, ObjectiveStopwatch> _stopwatchesMap;

        public StopwatchRepository()
        {
            _stopwatchesMap = new Dictionary<string, ObjectiveStopwatch>();
        }

        public ObjectiveStopwatch GetUserStopwatch(string userId)
        {
            if (userId == null) throw new ArgumentNullException($"{nameof(userId)} was null");
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException($"{nameof(userId)} was empty or whitespace");

            if (_stopwatchesMap.TryGetValue(userId, out var userStopwatch))
                return userStopwatch;

            var stopwatch = new ObjectiveStopwatch();
            _stopwatchesMap.Add(userId, stopwatch);

            return stopwatch;
        }
    }
}