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
                        int value = vars.TryGetProperty(mission.SaveKey, out var valueElement)
                            ? valueElement.GetInt32()
                            : 0;

                        progress.SetCount(
                            mission.SaveKey,
                            difficulty,
                            classKit.SaveKey,
                            value
                        );
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
