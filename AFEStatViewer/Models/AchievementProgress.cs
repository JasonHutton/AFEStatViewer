using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Models
{
    public sealed class AchievementProgress
    {
        // set -> key -> current value
        public Dictionary<string, Dictionary<string, int>> Counters { get; } = new();

        public void SetValue(string set, string key, int value)
        {
            if (!Counters.TryGetValue(set, out var setCounters))
            {
                setCounters = new Dictionary<string, int>();
                Counters[set] = setCounters;
            }

            setCounters[key] = value;
        }

        public int GetValue(string set, string key)
        {
            if (!Counters.TryGetValue(set, out var setCounters))
            {
                return 0;
            }

            return setCounters.TryGetValue(key, out var value) ? value : 0;
        }

        public int GetValue(AchievementDefinition def)
        {
            return GetValue(def.Set, def.Key);
        }

        public bool IsComplete(AchievementDefinition def) => def.Target > 0 && GetValue(def) >= def.Target;

        /// <summary>
        /// Completion percentage in the range [0, 100].
        /// </summary>
        public double GetCompletionPercent(AchievementDefinition def)
        {
            if (def.Target <= 0) return 0;

            double percent = (double)GetValue(def) / def.Target * 100.0;

            if (percent < 0) return 0;
            if (percent > 100) return 100;

            return percent;
        }
    }
}