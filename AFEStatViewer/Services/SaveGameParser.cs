using AFEStatViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AFEStatViewer.Services
{
    public sealed class SaveGameParser
    {
        public ModeProgress ParseModeProgress(string jsonString, IEnumerable<MissionDefinition> missions)
        {
            if (jsonString == null)
                throw new ArgumentNullException(nameof(jsonString));
            if (missions == null)
                throw new ArgumentNullException(nameof(missions));

            var progress = new ModeProgress();

            using var document = JsonDocument.Parse(jsonString);
            var root = document.RootElement;

            if (!root.TryGetProperty("CounterTracker", out var counterTracker) ||
                !counterTracker.TryGetProperty("Sets", out var sets))
            {
                // Save file doesn't contain campaign progress at all
                return progress;
            }

            foreach (var difficulty in Difficulties.All)
            {
                var difficultySetKey = Difficulties.ToSaveKey[difficulty];

                // Example: "Easy|Campaign", "Hard|Campaign", etc.
                if (!sets.TryGetProperty(difficultySetKey, out var difficultySet))
                    continue;

                if (!difficultySet.TryGetProperty("Vars", out var vars))
                    continue;

                foreach (var mission in missions)
                {
                    if (!progress.Counts.TryGetValue(mission.SaveKey, out var byDifficulty))
                    {
                        byDifficulty = new Dictionary<Difficulty, int>();
                        progress.Counts[mission.SaveKey] = byDifficulty;
                    }

                    if (vars.TryGetProperty(mission.SaveKey, out var valueElement))
                    {
                        // Normal case: mission present in save
                        byDifficulty[difficulty] = valueElement.GetInt32();
                    }
                    else
                    {
                        // Mission not present: treat as zero progress
                        byDifficulty[difficulty] = 0;
                    }
                }
            }

            return progress;
        }


        public AchievementProgress ParseAchievementProgress(string jsonString, IEnumerable<AchievementDefinition> definitions)
        {
            var progress = new AchievementProgress();

            using var document = JsonDocument.Parse(jsonString);
            var root = document.RootElement;
            var sets = root.GetProperty("CounterTracker").GetProperty("Sets");

            // Your current logic assumes "Any" exists.
            // If it ever doesn't, this should throw or just return empty.
            var vars = sets.GetProperty("Any").GetProperty("Vars");

            foreach (var def in definitions)
            {
                if (vars.TryGetProperty(def.Key, out var el))
                    progress.Counters[def.Key] = el.GetInt32();
                else
                    progress.Counters[def.Key] = 0;
            }

            return progress;
        }
    }

}
