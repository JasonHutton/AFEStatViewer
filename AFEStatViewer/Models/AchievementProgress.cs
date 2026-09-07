using System.Collections.Generic;

namespace AFEStatViewer.Models
{
    public sealed class AchievementProgress
    {
        // achievement id -> current value
        public Dictionary<string, int> Counters { get; } = new();

        public void SetValue(string id, int value)
        {
            Counters[id] = value;
        }

        public int GetValue(string id)
        {
            return Counters.TryGetValue(id, out var value) ? value : 0;
        }

        public int GetValue(AchievementDefinition def)
        {
            return GetValue(def.Id);
        }

        public bool IsComplete(AchievementDefinition def) =>
            def.Target > 0 && GetValue(def) >= def.Target;

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