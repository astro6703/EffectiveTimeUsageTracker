using EffectiveTimeUsageTracker.Models.Objectives;
using System.Collections.Generic;

namespace EffectiveTimeUsageTracker.MyExtensionMethods
{
    public static class MyExtensionMethods
    {
        public static IEnumerable<Objective> UpdateObjective(this IEnumerable<Objective> enumerable, Objective objective)
        {
            foreach (var item in enumerable)
            {
                if (item.Name != objective.Name)
                    yield return item;

                else yield return objective;
            }
        }

        public static IEnumerable<Objective> AddObjective(this IEnumerable<Objective> enumerable, Objective objective)
        {
            foreach (var item in enumerable)
                yield return item;

            yield return objective;
        }

        public static IEnumerable<Objective> RemoveObjective(this IEnumerable<Objective> enumerable, Objective objective)
        {
            foreach (var item in enumerable)
                if (item.Name != objective.Name)
                    yield return item;
        }
    }
}