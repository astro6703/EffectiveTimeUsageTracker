using EffectiveTimeUsageTracker.Models.Objectives;
using System.Collections.Generic;

namespace EffectiveTimeUsageTracker.MyExtensionMethods
{
    public static class MyExtensionMethods
    {
        public static IEnumerable<Objective> UpdateObjectiveStatus(this IEnumerable<Objective> enumerable, Objective objective)
        {
            foreach (var item in enumerable)
            {
                if (item.Name != objective.Name)
                    yield return item;

                else yield return objective;
            }
        }

        public static IEnumerable<Objective> AddOneObjective(this IEnumerable<Objective> enumerable, Objective objective)
        {
            foreach (var item in enumerable)
                yield return item;

            yield return objective;
        }
    }
}