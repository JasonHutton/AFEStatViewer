using System.Collections.Generic;

namespace AFEStatViewer.Models
{
    public sealed class ModeProgress
    {
        public Dictionary<string, Dictionary<Difficulty, Dictionary<string, int>>> Counts { get; } = new();

        public int GetCount(string saveKey, Difficulty difficulty, string classKitSaveKey)
        {
            return Counts.TryGetValue(saveKey, out var byDifficulty) &&
                   byDifficulty.TryGetValue(difficulty, out var byClassKit) &&
                   byClassKit.TryGetValue(classKitSaveKey, out var value)
                ? value
                : 0;
        }

        public void SetCount(string saveKey, Difficulty difficulty, string classKitSaveKey, int value)
        {
            if (!Counts.TryGetValue(saveKey, out var byDifficulty))
            {
                byDifficulty = new Dictionary<Difficulty, Dictionary<string, int>>();
                Counts[saveKey] = byDifficulty;
            }

            if (!byDifficulty.TryGetValue(difficulty, out var byClassKit))
            {
                byClassKit = new Dictionary<string, int>();
                byDifficulty[difficulty] = byClassKit;
            }

            byClassKit[classKitSaveKey] = value;
        }
    }
}