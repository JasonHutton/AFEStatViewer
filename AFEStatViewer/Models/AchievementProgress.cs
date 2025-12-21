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
    }
}

