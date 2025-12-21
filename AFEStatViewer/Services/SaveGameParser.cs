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

            // Materialize once (avoid re-enumerating IEnumerable multiple times)
            var missionList = missions as IList<MissionDefinition> ?? missions.ToList();

            // Split missions by type (Campaign vs Challenge)
            var campaignMissions = missionList.Where(m => Difficulties.IsCampaignKey(m.SaveKey)).ToList();
            var challengeMissions = missionList.Where(m => Difficulties.IsChallengeKey(m.SaveKey)).ToList();

            // Ensure dictionary exists and set value
            static void SetCount(ModeProgress p, string saveKey, Difficulty d, int value)
            {
                if (!p.Counts.TryGetValue(saveKey, out var byDifficulty))
                {
                    byDifficulty = new Dictionary<Difficulty, int>();
                    p.Counts[saveKey] = byDifficulty;
                }

                byDifficulty[d] = value;
            }

            // Campaign missions: read from *|Campaign sets
            foreach (var difficulty in Difficulties.All)
            {
                var difficultySetKey = Difficulties.ToCampaignSaveKey[difficulty];

                if (!sets.TryGetProperty(difficultySetKey, out var difficultySet))
                    continue;

                if (!difficultySet.TryGetProperty("Vars", out var vars))
                    continue;

                foreach (var mission in campaignMissions)
                {
                    int value = vars.TryGetProperty(mission.SaveKey, out var valueElement)
                        ? valueElement.GetInt32()
                        : 0;

                    SetCount(progress, mission.SaveKey, difficulty, value);
                }
            }

            // Horde missions: read from *|Challenge sets
            foreach (var difficulty in Difficulties.All)
            {
                var difficultySetKey = Difficulties.ToChallengeSaveKey[difficulty];

                if (!sets.TryGetProperty(difficultySetKey, out var difficultySet))
                    continue;

                if (!difficultySet.TryGetProperty("Vars", out var vars))
                    continue;

                foreach (var mission in challengeMissions)
                {
                    int value = vars.TryGetProperty(mission.SaveKey, out var valueElement)
                        ? valueElement.GetInt32()
                        : 0;

                    SetCount(progress, mission.SaveKey, difficulty, value);
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
