using System;
using System.Collections.Generic;
using AFEStatViewer.Models;

namespace AFEStatViewer
{
    /// <summary>
    /// Temporary adapter that maps model-based progress into the legacy CampaignCompletionFrontend shape.
    ///
    /// - Mission completion: ModeProgress -> Frontend.Set(mapKey, difficultyKey, bool)
    /// - Achievements: AchievementProgress + definitions -> Frontend.Ach* (AchievementCount properties)
    ///
    /// Delete this once the UI binds to real MVVM ViewModels.
    /// </summary>
    public static class FrontendAdapter
    {
        public static void ApplyToFrontend(
            CampaignCompletionFrontend frontend,
            ModeProgress modeProgress,
            AchievementProgress? achievementProgress = null,
            IEnumerable<AchievementDefinition>? achievementDefinitions = null)
        {
            if (frontend == null) throw new ArgumentNullException(nameof(frontend));
            if (modeProgress == null) throw new ArgumentNullException(nameof(modeProgress));

            ApplyMissions(frontend, modeProgress);

            if (achievementProgress != null)
            {
                var defs = achievementDefinitions ?? GameDefinitions.Achievements;
                ApplyAchievements(frontend, achievementProgress, defs);
            }
        }

        private static void ApplyMissions(CampaignCompletionFrontend frontend, ModeProgress progress)
        {
            foreach (var mission in GameDefinitions.AllMissions)
            {
                foreach (var difficulty in Difficulties.All)
                {
                    var difficultyKey = Difficulties.ToSaveKey[difficulty];
                    bool completed = progress.GetCount(mission.SaveKey, difficulty) > 0;

                    frontend.Set(mission.SaveKey, difficultyKey, completed);
                }
            }
        }

        private static void ApplyAchievements(
            CampaignCompletionFrontend frontend,
            AchievementProgress progress,
            IEnumerable<AchievementDefinition> definitions)
        {
            foreach (var def in definitions)
            {
                // Build an AchievementCount using definition metadata + parsed value
                var count = new AchievementCount(def.Name, def.Key, def.Target)
                {
                    Value = progress.GetValue(def.Key)
                };

                AssignAchievement(frontend, def.Key, count);
            }
        }

        /// <summary>
        /// Maps an achievement key to the corresponding legacy Frontend property.
        /// Keep this centralized so you can delete it later when achievements become a collection in the UI.
        /// </summary>
        private static void AssignAchievement(CampaignCompletionFrontend frontend, string key, AchievementCount value)
        {
            switch (key)
            {
                case "ElectricKills":
                    frontend.AchHighVoltage = value;
                    break;

                case "MostGrapplesPerMission":
                    frontend.AchIThinkTheyLikeMe = value;
                    break;

                case "ExplosiveBarrelsKills":
                    frontend.AchImprovisedExplosives = value;
                    break;

                case "MedkitsUsedOnAllies":
                    frontend.AchSuturingExpert = value;
                    break;

                case "MissionsCompletedWithoutDownsOrDeaths":
                    frontend.AchSupportiveSquad = value;
                    break;

                case "ConsumablesUsed":
                    frontend.AchTowerDefense = value;
                    break;

                case "ThermalKills":
                    frontend.AchBurnEmOut = value;
                    break;

                case "BasicKills|Xenos":
                    frontend.AchItsABugHunt = value;
                    break;

                case "Kills|Pathogen":
                    frontend.AchAntiMutationStation = value;
                    break;

                case "BasicKills|Synths":
                    frontend.AchGlorifiedToasters = value;
                    break;

                case "HiddenCachesFound":
                    frontend.AchHiddenCachesFound = value;
                    break;

                default:
                    // Unknown key: ignore (or log)
                    break;
            }
        }
    }
}
