using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Models
{
    public sealed class AchievementProgress
    {
        // key -> current value
        public Dictionary<string, int> Counters { get; } = new();

        public int GetValue(string key) => Counters.TryGetValue(key, out var value) ? value : 0;

        public bool IsComplete(AchievementDefinition def) => def.Target > 0 && GetValue(def.Key) >= def.Target;

        /// <summary>
        /// Completion percentage in the range [0, 100].
        /// </summary>
        public double GetCompletionPercent(AchievementDefinition def)
        {
            if (def.Target <= 0) return 0;

            double percent = (double)GetValue(def.Key) / def.Target * 100.0;
            if (percent < 0) return 0;
            if (percent > 100) return 100;
            return percent;
        }

    }
}

