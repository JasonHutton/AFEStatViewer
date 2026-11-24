using System;
using System.Collections.Generic;
using System.Text.Json;
using AFEStatViewer.Models;

namespace AFEStatViewer.Services
{
    public abstract class AchievementValueResolvers
    {
        public static Func<JsonElement, int> CounterTracker(string set, params string[] keys)
        {
            return root => GetCounterValue(root, set, keys);
        }

        public static Func<JsonElement, int> CounterTrackerThreshold(string set, string key, int threshold)
        {
            return root => GetCounterValue(root, set, key) >= threshold ? 1 : 0;
        }

        public static Func<JsonElement, int> CounterTrackerThreshold(string set, int threshold, params string[] keys)
        {
            return root => GetCounterValue(root, set, keys) >= threshold ? 1 : 0;
        }

        public static Func<JsonElement, int> CompletedCounterKeys(string set, params string[] keys)
        {
            return root =>
            {
                int completed = 0;

                foreach (string key in keys)
                {
                    if (GetCounterValue(root, set, key) > 0)
                    {
                        completed++;
                    }
                }

                return completed;
            };
        }

        protected static int GetCounterValue(JsonElement root, string set, params string[] keys)
        {
            int maximum = 0;

            if (!root.TryGetProperty("CounterTracker", out var counterTracker) ||
                !counterTracker.TryGetProperty("Sets", out var sets) ||
                !sets.TryGetProperty(set, out var counterSet) ||
                !counterSet.TryGetProperty("Vars", out var vars))
            {
                return maximum;
            }

            foreach (string key in keys)
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

            return maximum;
        }

        public static Func<JsonElement, int> CampaignCompletion(CampaignDefinition campaign, string set = "Campaign")
        {
            return root =>
            {
                int completed = 0;

                foreach (MissionDefinition mission in campaign.Missions)
                {
                    if (GetCounterValue(root, set, mission.SaveKeys) > 0)
                    {
                        completed++;
                    }
                }

                return completed;
            };
        }

        public static Func<JsonElement, int> CompletedCampaignCount(IEnumerable<CampaignDefinition> campaigns, string set = "Campaign")
        {
            return root =>
            {
                int completedCampaigns = 0;

                foreach (CampaignDefinition campaign in campaigns)
                {
                    bool complete = true;

                    foreach (MissionDefinition mission in campaign.Missions)
                    {
                        if (GetCounterValue(root, set, mission.SaveKeys) <= 0)
                        {
                            complete = false;
                            break;
                        }
                    }

                    if (complete)
                    {
                        completedCampaigns++;
                    }
                }

                return completedCampaigns;
            };
        }

        public static Func<JsonElement, int> SumCounters(string key, params string[] sets)
        {
            return root =>
            {
                int total = 0;

                foreach (string set in sets)
                {
                    total += GetCounterValue(root, set, key);
                }

                return total;
            };
        }
    }
}