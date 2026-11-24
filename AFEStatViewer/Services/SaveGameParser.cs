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
        public ModeProgress ParseModeProgress(
    string jsonString,
    IEnumerable<MissionDefinition> missions,
    IEnumerable<ClassKitDefinition> classKits)
        {
            if (jsonString == null)
                throw new ArgumentNullException(nameof(jsonString));

            if (missions == null)
                throw new ArgumentNullException(nameof(missions));

            if (classKits == null)
                throw new ArgumentNullException(nameof(classKits));

            var progress = new ModeProgress();

            using var document = JsonDocument.Parse(jsonString);
            var root = document.RootElement;

            if (!root.TryGetProperty("CounterTracker", out var counterTracker) ||
                !counterTracker.TryGetProperty("Sets", out var sets))
            {
                return progress;
            }

            var missionList = missions as IList<MissionDefinition> ?? missions.ToList();
            var classKitList = classKits as IList<ClassKitDefinition> ?? classKits.ToList();

            foreach (var difficulty in Difficulties.All)
            {
                foreach (var classKit in classKitList)
                {
                    string setKey = $"{Difficulties.ToSaveKey[difficulty]}|{classKit.SaveKey}";

                    if (!sets.TryGetProperty(setKey, out var difficultyClassSet))
                        continue;

                    if (!difficultyClassSet.TryGetProperty("Vars", out var vars))
                        continue;

                    foreach (var mission in missionList)
                    {
                        int maximum = 0;

                        foreach (string key in mission.SaveKeys)
                        {
                            if (!vars.TryGetProperty(key, out var valueElement) || valueElement.ValueKind != JsonValueKind.Number)
                            {
                                continue;
                            }

                            if (valueElement.TryGetInt32(out int intValue))
                            {
                                maximum = Math.Max(maximum, intValue);
                            }
                            else if (valueElement.TryGetDouble(out double doubleValue))
                            {
                                maximum = Math.Max(maximum, (int)Math.Floor(doubleValue));
                            }
                        }

                        foreach (string key in mission.SaveKeys)
                        {
                            progress.SetCount(
                                key,
                                difficulty,
                                classKit.SaveKey,
                                maximum
                            );
                        }
                    }
                }
            }

            return progress;
        }

        public AchievementProgress ParseAchievementProgress(string jsonString, IEnumerable<AchievementDefinition> definitions)
        {
            if (jsonString == null)
                throw new ArgumentNullException(nameof(jsonString));

            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));

            var progress = new AchievementProgress();

            using var document = JsonDocument.Parse(jsonString);
            var root = document.RootElement;

            foreach (var def in definitions)
            {
                int value = def.ValueResolver(root);

                progress.SetValue(def.Id, value);
            }

            return progress;
        }
    }
}
