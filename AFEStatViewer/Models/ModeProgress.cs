using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Models
{
    public sealed class ModeProgress
    {
        public Dictionary<string, Dictionary<Difficulty, int>> Counts { get; } = new();

        public int GetCount(string saveKey, Difficulty d) => Counts.TryGetValue(saveKey, out var byDiff) && byDiff.TryGetValue(d, out var value) ? value : 0;
    }

}
