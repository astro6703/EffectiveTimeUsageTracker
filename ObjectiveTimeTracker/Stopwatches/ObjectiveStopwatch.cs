using System;
using System.Diagnostics;

namespace ObjectiveTimeTracker.Stopwatches
{
    public class ObjectiveStopwatch
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public string ObjectiveName { get; private set; }

        public TimeSpan Elapsed => _stopwatch.Elapsed;

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public void SetObjective(string objectiveName)
        {
            ObjectiveName = objectiveName ?? throw new ArgumentNullException("Objective name was null");

            _stopwatch.Reset();
        }

        public void ResetObjective()
        {
            ObjectiveName = null;
        }
    }
}