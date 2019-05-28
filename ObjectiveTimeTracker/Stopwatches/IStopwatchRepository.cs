using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectiveTimeTracker.Stopwatches
{
    public interface IStopwatchRepository
    {
        ObjectiveStopwatch GetUserStopwatch(string userId);
    }
}
