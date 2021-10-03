using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;

using NAryDictionary;

namespace AFEStatViewer
{
    public class CampaignCompletion
    {
        public NAryDictionary<string, string, int> data { get; set; }
        private string[] difficultyKeys = { "Easy|Campaign", "Normal|Campaign", "Hard|Campaign", "Extreme|Campaign", "Insane|Campaign" };
        public CampaignCompletionFrontend Frontend { get; set; }
        public AchievementCount AchHighVoltage;
        public AchievementCount AchIThinkTheyLikeMe;
        public AchievementCount AchImprovisedExplosives;
        public AchievementCount AchSuturingExpert;
        public AchievementCount AchSupportiveSquad;
        public AchievementCount AchTowerDefense;
        public AchievementCount AchBurnEmOut;
        public AchievementCount AchItsABugHunt;
        public AchievementCount AchAntiMutationStation;
        public AchievementCount AchGlorifiedToasters;
        public AchievementCount AchHiddenCachesFound;

        public CampaignCompletion()
        {
            data = new NAryDictionary<string, string, int>();
            Frontend = new CampaignCompletionFrontend();

            Init();
        }

        public void Init()
        {
            // Add all the maps
            AddMap("Campaign|SC-C1|SC-C1M1");
            //AddMap("Campaign|SC-C1|SC-C1M2");
            AddMap("Campaign|SC-C1|SC_C1M2"); // This appears to be a typo in the original game data.
            AddMap("Campaign|SC-C1|SC-C1M3");

            AddMap("Campaign|SC-C2|SC-C2M1");
            AddMap("Campaign|SC-C2|SC-C2M2");
            AddMap("Campaign|SC-C2|SC-C2M3");

            AddMap("Campaign|SC-C3|SC-C3M1");
            AddMap("Campaign|SC-C3|SC-C3M2");
            AddMap("Campaign|SC-C3|SC-C3M3");

            AddMap("Campaign|SC-C4|SC-C4M1");
            AddMap("Campaign|SC-C4|SC-C4M2");
            AddMap("Campaign|SC-C4|SC-C4M3");

            // Default some properties
            AchHighVoltage = new AchievementCount("High Voltage", "ElectricKills", 1000);
            AchIThinkTheyLikeMe = new AchievementCount("I Think They Like Me", "MostGrapplesPerMission", 5);
            AchImprovisedExplosives = new AchievementCount("Improvised Explosives", "ExplosiveBarrelsKills", 50);
            AchSuturingExpert = new AchievementCount("Suturing Expert", "MedkitsUsedOnAllies", 100);
            AchSupportiveSquad = new AchievementCount("Supportive Squad", "MissionsCompletedWithoutDownsOrDeaths", 50);
            AchTowerDefense = new AchievementCount("Tower Defense", "ConsumablesUsed", 500);
            AchBurnEmOut = new AchievementCount("Burn 'Em Out", "ThermalKills", 1000);
            AchItsABugHunt = new AchievementCount("It's A Bug Hunt", "BasicKills|Xenos", 10000);
            AchAntiMutationStation = new AchievementCount("Anti-Mutation Station", "Kills|Pathogen", 300);
            AchGlorifiedToasters = new AchievementCount("Glorified Toasters", "BasicKills|Synths", 1000);
            AchHiddenCachesFound = new AchievementCount("Hidden Caches Found", "HiddenCachesFound", 50);
        }

        /// <summary>
        /// Add all the difficulties for this map
        /// </summary>
        /// <param name="map"></param>
        public void AddMap(string map)
        {
            data[map] = data.New();

            foreach (string difficulty in difficultyKeys)
            {
                data[map][difficulty] = 0;
            }
        }

        /// <summary>
        /// Parse map and difficulty completion for all maps and difficulties
        /// </summary>
        /// <param name="jsonString"></param>
        public void LoadCampaignMapData(string jsonString)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;
                JsonElement counterTrackerElement = root.GetProperty("CounterTracker");
                JsonElement sets = counterTrackerElement.GetProperty("Sets");

                foreach (string difficulty in difficultyKeys)
                {
                    // Parse the difficulty element
                    if (sets.TryGetProperty(difficulty, out JsonElement campaignDifficulty))
                    {
                        JsonElement vars = campaignDifficulty.GetProperty("Vars");

                        foreach (string mapKey in data.Keys)
                        {
                            // Parse the map element
                            if (vars.TryGetProperty(mapKey, out JsonElement mapElement))
                            {
                                data[mapKey][difficulty] = mapElement.GetInt32(); ;
                            }
                        }
                    }
                }
            }

            foreach (string mapKey in data.Keys)
            {
                foreach (string difficulty in difficultyKeys)
                {
                    if (data[mapKey][difficulty] > 0)
                        Frontend.Set(mapKey, difficulty, true);
                    else
                        Frontend.Set(mapKey, difficulty, false);
                }
            }
        }

        /// <summary>
        /// Parse player statistics
        /// </summary>
        /// <param name="jsonString"></param>
        public void LoadPlayerData(string jsonString)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;
                JsonElement counterTrackerElement = root.GetProperty("CounterTracker");
                JsonElement sets = counterTrackerElement.GetProperty("Sets");
                JsonElement playerClass = sets.GetProperty("Any");
                JsonElement vars = playerClass.GetProperty("Vars");
                if (vars.TryGetProperty(AchHighVoltage.Key, out JsonElement electricKills))
                {
                    AchHighVoltage.Value = electricKills.GetInt32();
                }
                if (vars.TryGetProperty(AchIThinkTheyLikeMe.Key, out JsonElement iThinkTheyLikeMe))
                {
                    AchIThinkTheyLikeMe.Value = iThinkTheyLikeMe.GetInt32();
                }
                if (vars.TryGetProperty(AchImprovisedExplosives.Key, out JsonElement improvisedExplosives))
                {
                    AchImprovisedExplosives.Value = improvisedExplosives.GetInt32();
                }
                if (vars.TryGetProperty(AchSuturingExpert.Key, out JsonElement suturingExpert))
                {
                    AchSuturingExpert.Value = suturingExpert.GetInt32();
                }
                if (vars.TryGetProperty(AchSupportiveSquad.Key, out JsonElement supportiveSquad))
                {
                    AchSupportiveSquad.Value = supportiveSquad.GetInt32();
                }
                if (vars.TryGetProperty(AchTowerDefense.Key, out JsonElement towerDefense))
                {
                    AchTowerDefense.Value = towerDefense.GetInt32();
                }
                if (vars.TryGetProperty(AchBurnEmOut.Key, out JsonElement burnEmOut))
                {
                    AchBurnEmOut.Value = burnEmOut.GetInt32();
                }
                if (vars.TryGetProperty(AchItsABugHunt.Key, out JsonElement itsABugHunt))
                {
                    AchItsABugHunt.Value = itsABugHunt.GetInt32();
                }
                if (vars.TryGetProperty(AchAntiMutationStation.Key, out JsonElement antiMutationStation))
                {
                    AchAntiMutationStation.Value = antiMutationStation.GetInt32();
                }
                if (vars.TryGetProperty(AchGlorifiedToasters.Key, out JsonElement glorifiedToasters))
                {
                    AchGlorifiedToasters.Value = glorifiedToasters.GetInt32();
                }
                if (vars.TryGetProperty(AchHiddenCachesFound.Key, out JsonElement hiddenCachesFound))
                {
                    AchHiddenCachesFound.Value = hiddenCachesFound.GetInt32();
                }

                Frontend.AchHighVoltage = AchHighVoltage;
                Frontend.AchIThinkTheyLikeMe = AchIThinkTheyLikeMe;
                Frontend.AchImprovisedExplosives = AchImprovisedExplosives;
                Frontend.AchSuturingExpert = AchSuturingExpert;
                Frontend.AchSupportiveSquad = AchSupportiveSquad;
                Frontend.AchTowerDefense = AchTowerDefense;
                Frontend.AchBurnEmOut = AchBurnEmOut;
                Frontend.AchItsABugHunt = AchItsABugHunt;
                Frontend.AchAntiMutationStation = AchAntiMutationStation;
                Frontend.AchGlorifiedToasters = AchGlorifiedToasters;
                Frontend.AchHiddenCachesFound = AchHiddenCachesFound;
            }
        }
    }
}
