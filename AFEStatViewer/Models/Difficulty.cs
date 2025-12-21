using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Models
{
    public enum Difficulty
    {
        Casual,
        Standard,
        Intense,
        Extreme,
        Insane
    }

    public static class Difficulties
    {
        // Campaign
        public static readonly IReadOnlyDictionary<Difficulty, string> ToCampaignSaveKey =
            new Dictionary<Difficulty, string>
            {
                [Difficulty.Casual] = "Easy|Campaign",
                [Difficulty.Standard] = "Normal|Campaign",
                [Difficulty.Intense] = "Hard|Campaign",
                [Difficulty.Extreme] = "Extreme|Campaign",
                [Difficulty.Insane] = "Insane|Campaign",
            };

        // Horde modes
        public static readonly IReadOnlyDictionary<Difficulty, string> ToChallengeSaveKey =
            new Dictionary<Difficulty, string>
            {
                [Difficulty.Casual] = "Easy|Challenge",
                [Difficulty.Standard] = "Normal|Challenge",
                [Difficulty.Intense] = "Hard|Challenge",
                [Difficulty.Extreme] = "Extreme|Challenge",
                [Difficulty.Insane] = "Insane|Challenge",
            };

        public static bool IsCampaignKey(string missionSaveKey) =>
            missionSaveKey != null &&
            missionSaveKey.StartsWith("Campaign|", StringComparison.OrdinalIgnoreCase);
        public static bool IsChallengeKey(string missionSaveKey) =>
            missionSaveKey != null &&
            missionSaveKey.StartsWith("Challenge|", StringComparison.OrdinalIgnoreCase);

        public static readonly IReadOnlyList<Difficulty> All = new[]
{
            Difficulty.Casual,
            Difficulty.Standard,
            Difficulty.Intense,
            Difficulty.Extreme,
            Difficulty.Insane
        };

    }
}
