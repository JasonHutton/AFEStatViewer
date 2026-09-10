using System;
using System.Text.Json;

namespace AFEStatViewer.Services
{
    public abstract class AchievementValueResolvers
    {
        public static Func<JsonElement, int> CounterTracker(string set, string key)
        {
            return root => GetCounterValue(root, set, key);
        }

        public static Func<JsonElement, int> CounterTrackerThreshold(string set, string key, int threshold)
        {
            return root => GetCounterValue(root, set, key) >= threshold ? 1 : 0;
        }

        public static Func<JsonElement, int> CompletedCounterKeys(string set, params string[] keys)
        {
            return root =>
            {
                int completed = 0;

                foreach (string key in keys)
                {
                    if (GetCounterValue(root, set, key) > 0)
                    {
                        completed++;
                    }
                }

                return completed;
            };
        }

        protected static int GetCounterValue(JsonElement root, string set, string key)
        {
            if (!root.TryGetProperty("CounterTracker", out var counterTracker) ||
                !counterTracker.TryGetProperty("Sets", out var sets) ||
                !sets.TryGetProperty(set, out var counterSet) ||
                !counterSet.TryGetProperty("Vars", out var vars) ||
                !vars.TryGetProperty(key, out var valueElement))
            {
                return 0;
            }

            if (valueElement.ValueKind != JsonValueKind.Number)
            {
                return 0;
            }

            if (valueElement.TryGetInt32(out int intValue))
            {
                return intValue;
            }

            if (valueElement.TryGetDouble(out double doubleValue))
            {
                return (int)Math.Floor(doubleValue);
            }

            return 0;
        }
    }
}