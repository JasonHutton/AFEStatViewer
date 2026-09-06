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

        public static readonly IReadOnlyList<AchievementDefinition> Achievements = new[]
        {
            new AchievementDefinition("Spilled Milk", "Eliminate 1000 Synthetics.", "BasicKills|Synths", 1000),
            //new AchievementDefinition("Well Stocked Arsenal", "Level 10 weapons to at least two stars.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Reckoning", "Complete the \"Final Endeavor\" Campaign.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Legionnaire's Service Medal", "Complete all Campaigns on Casual or higher difficulty.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("The Story of the Quarry", "Complete the \"Ancient Anomaly\" Campaign.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("", "", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Voided Warranty", "Collect 5 Augments.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Canary", "Complete the \"Rock Bottom\" campaign.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Next Top Marine", "Equip cosmetics on your head, torso and legs.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Worst Party Ever", "Complete the \"Boarding Party\" campaign.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Hell and Back", "Level a weapon to four stars.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Worthiness Tested", "Attempt a Challenge run.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("How Do You See Out Of These Things?", "Collect 10 hats.", "[PLACEHOLDER]", 10),

            //new AchievementDefinition("Not Just For Decoration", "Equip at least one attachment on a sidearm.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("It's a rescue mission, you'll love it!", "Complete the \"Rescue and Recovery\" campaign.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Welcoming Committee", "Complete the \"Prologue\" campaign.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Hardened Trooper", "Complete a mission on Intense difficulty.", "[PLACEHOLDER]", XX),

            //new AchievementDefinition("Yellow Makes It Shoot Better", "Equip a colorway and three decals onto a gun.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Fully Loaded", "Equip a weapon with all attachments, including an Augment.", "[PLACEHOLDER]", XX),

            new AchievementDefinition("Seasoned Veteran", "Level a Kit to Rank 10.", "HighestKitLevelReached", 10),


            //new AchievementDefinition("Maxed Out", "Fill every slot in a perk board.", "[PLACEHOLDER]", XX),

            new AchievementDefinition("Purge the Unclean", "Eliminate 1000 Pathogen.", "Kills|Pathogen", 1000),
            new AchievementDefinition("Head Hunter", "Get 1000 Headshot kills.", "HeadshotKills", 1000),
            
            //new AchievementDefinition("Wave Goodbye", "Survive 5 waves in Horde Mode.", "[PLACEHOLDER]", XX),

            //new AchievementDefinition("Overly Attached", "Collect 50 weapon attachments.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Custom Made Marine", "Complete 5 missions with the Specialist kit.", "[PLACEHOLDER]", XX),
            new AchievementDefinition("Another Bug Hunt", "Eliminate 10000 Xenomorphs.", "BasicKills|Xenos", 10000),

            //new AchievementDefinition("Noble Soldier", "Complete a mission on Extreme difficulty.", "[PLACEHOLDER]", XX),
            new AchievementDefinition("Decommission Mission", "Eliminate 1000 Combat Automatons.", "BasicKills|Hyperdyne", 1000),

            //new AchievementDefinition("Fervent Warrior", "Complete a mission on Insane Difficulty.", "[PLACEHOLDER]", XX),
            //new AchievementDefinition("Pure Professionals", "Complete a mission on Intense Difficulty or above without anyone taking friendly fire damage.", "[PLACEHOLDER]", XX),
        };

        public static IEnumerable<MissionDefinition> CampaignOnlyMissions => Campaigns.SelectMany(c => c.Missions);

        public static IEnumerable<MissionDefinition> GameModeMissions => GameModes.SelectMany(c => c.Missions);

        public static IEnumerable<MissionDefinition> AllMissions => CampaignOnlyMissions.Concat(GameModeMissions);
    }
}