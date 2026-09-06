using System.Collections.Generic;
using System.Linq;

namespace AFEStatViewer.Models
{
    public static class AFE2GameDefinitions
    {
        public static readonly IReadOnlyList<CampaignDefinition> Campaigns = new[]
        {
            new CampaignDefinition(1, "Prologue", new[]
            {
                new MissionDefinition(1, "Banshee Down", "Campaign|AvoPrologue|AvoPrologue01"),
            }),

            new CampaignDefinition(2, "Rescue and Recovery", new[]
            {
                new MissionDefinition(1, "Piping Hot", "Campaign|AVOColony|AVOColony1"),
                new MissionDefinition(2, "Breaking and Entering", "Campaign|AVOColony|AVOColony2"),
                new MissionDefinition(3, "Synth Headache", "Campaign|AVOColony|AVOColony3"),
            }),

            new CampaignDefinition(3, "Boarding Party", new[]
            {
                new MissionDefinition(1, "Bomb Squad", "Campaign|AVOSpaceCruiser|AVOSpaceCruiser1"),
                new MissionDefinition(2, "Dumpster Diving", "Campaign|AVOSpaceCruiser|AVOSpaceCruiser2"),
                new MissionDefinition(3, "Sleep Study", "Campaign|AVOSpaceCruiser|AVOSpaceCruiser3"),
            }),

            new CampaignDefinition(4, "Rock Bottom", new[]
            {
                new MissionDefinition(1, "Power Problems", "Campaign|AVOUnderbelly|AVOUnderbelly1"),
                new MissionDefinition(2, "Running on Fumes", "Campaign|AVOUnderbelly|AVOUnderbelly2"),
                new MissionDefinition(3, "Hive Mind", "Campaign|AVOUnderbelly|AVOUnderbelly3"),
            }),

            new CampaignDefinition(5, "Ancient Anomaly", new[]
            {
                new MissionDefinition(1, "Canyon Crawlers", "Campaign|AVOEngineerShip|AVOEngineerShip1"),
                new MissionDefinition(2, "Muthur May I", "Campaign|AVOEngineerShip|AVOEngineerShip3"),
                new MissionDefinition(3, "Deadly Descendants", "Campaign|AVOEngineerShip|AVOEngineerShip5"),
            }),

            new CampaignDefinition(6, "Final Endeavor", new[]
            {
                new MissionDefinition(1, "Reckoning", "Campaign|AVOQueenFight|AVOUnderbellyFinal"),
                new MissionDefinition(2, "Queen Fight", "Campaign|AVOQueenFight|AVOSpaceCruiserFinal"),
            }),
        };

        public static readonly IReadOnlyList<CampaignDefinition> GameModes = new[]
        {
            new CampaignDefinition(0, "Game Modes", new[]
            {
                new MissionDefinition(1, "Horde Mode: Crash Landed", "[PLACEHOLDER]"),
                new MissionDefinition(2, "Horde Mode: Blacksite",     "[PLACEHOLDER]"),
                new MissionDefinition(3, "Horde Mode: Crusher",       "[PLACEHOLDER]"),
            }),
        };

        public static IEnumerable<MissionDefinition> CampaignOnlyMissions => Campaigns.SelectMany(c => c.Missions);

        public static IEnumerable<MissionDefinition> GameModeMissions => GameModes.SelectMany(c => c.Missions);

        public static IEnumerable<MissionDefinition> AllMissions => CampaignOnlyMissions.Concat(GameModeMissions);
    }
}