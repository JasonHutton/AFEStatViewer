using System.Collections.Generic;
using System.Linq;
using AFEStatViewer.Services;

namespace AFEStatViewer.Models
{
    public static class AFE2GameDefinitions
    {
        public static readonly ClassKitVisualStyle ClassKitStyle = new(
            CompleteColorHex: "#1F6F3D",
            IncompleteColorHex: "#6F531C",
            IconBackgroundColorHex: "#00000000",
            CompletionSectionBackgroundColorHex: "#00000000",
            CompletionTextColorHex: "#000000",
            CompleteOpacity: 1.0,
            IncompleteOpacity: 0.22,
            IconBackgroundOpacity: 0.0,
            IconSize: 18.0
        );

        public static readonly IReadOnlyList<ClassKitDefinition> ClassKits = new[]
        {
            new ClassKitDefinition("Duelist",     "Player_Gunner_V2",       "AFE2_Class_Gunner.png"),
            new ClassKitDefinition("Machinist",   "Player_Technician_V2",   "AFE2_Class_Technician.png"),
            new ClassKitDefinition("Marauder",    "Player_Demolisher_V2",   "AFE2_Class_Demolisher.png"),
            new ClassKitDefinition("Hunter",      "Player_Lancer_V2",       "AFE2_Class_Lancer.png"),
            new ClassKitDefinition("Medic",       "Player_Medic",           "AFE2_Class_Doc.png"),
            new ClassKitDefinition("Specialist",  "Player_Custom",          "AFE2_Class_Custom.png"),
        };

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
                new MissionDefinition(1, "Horde Mode: Crash Landed",    "Challenge|HordeMode1|ColonyHordeMode"),
                new MissionDefinition(2, "Horde Mode: Blacksite",       "Challenge|HordeMode2|QuarryHordeMode"),
                new MissionDefinition(3, "Horde Mode: Crusher",         "Challenge|HordeMode3|HordeMode-1"),
            }),
        };

        public static readonly IReadOnlyList<AchievementDefinition> Achievements = new[]
{
            new AchievementDefinition(
                "AFE2_Welcoming_Committee",
                "Welcoming Committee",
                "Complete the \"Prologue\" Campaign.",
                1,
                AchievementValueResolvers.CompletedCounterKeys(
                    "Campaign",
                    "Campaign|AvoPrologue|AvoPrologue01")),

            new AchievementDefinition(
                "AFE2_Its_A_Rescue_Mission",
                "It's a rescue mission, you'll love it!",
                "Complete the \"Rescue and Recovery\" Campaign.",
                3,
                AchievementValueResolvers.CompletedCounterKeys(
                    "Campaign",
                    "Campaign|AVOColony|AVOColony1",
                    "Campaign|AVOColony|AVOColony2",
                    "Campaign|AVOColony|AVOColony3")),

            new AchievementDefinition(
                "AFE2_Worthiness_Tested",
                "Worthiness Tested",
                "Attempt a Challenge run.",
                1,
                AchievementValueResolvers.CounterTrackerThreshold(
                    "Any",
                    "MissionCardsUsed",
                    1)),

            new AchievementDefinition(
                "AFE2_Next_Top_Marine",
                "Next Top Marine",
                "Equip cosmetics on your head, torso and legs.",
                3,
                AchievementValueResolvers.BodyCosmeticsEquipped),

            new AchievementDefinition(
                "AFE2_Not_Just_For_Decoration",
                "Not Just For Decoration",
                "Equip at least one attachment on a sidearm.",
                1,
                AchievementValueResolvers.CounterTrackerThreshold(
                    "SideArm",
                    "MaxNumAttachmentsEquipped",
                    1)),

            new AchievementDefinition(
                "AFE2_Hell_And_Back",
                "Hell and Back",
                "Level a weapon to four stars.",
                1,
                AchievementValueResolvers.WeaponsAtLeastLevel(5)),

            new AchievementDefinition(
                "AFE2_Worst_Party_Ever",
                "Worst Party Ever",
                "Complete the \"Boarding Party\" Campaign.",
                3,
                AchievementValueResolvers.CompletedCounterKeys(
                    "Campaign",
                    "Campaign|AVOSpaceCruiser|AVOSpaceCruiser1",
                    "Campaign|AVOSpaceCruiser|AVOSpaceCruiser2",
                    "Campaign|AVOSpaceCruiser|AVOSpaceCruiser3")),

            new AchievementDefinition(
                "AFE2_How_Do_You_See_Out_Of_These_Things",
                "How Do You See Out Of These Things?",
                "Collect 10 hats.",
                10,
                AchievementValueResolvers.HatCollectionCount),

            new AchievementDefinition(
                "AFE2_Canary",
                "Canary",
                "Complete the \"Rock Bottom\" Campaign.",
                3,
                AchievementValueResolvers.CompletedCounterKeys(
                    "Campaign",
                    "Campaign|AVOUnderbelly|AVOUnderbelly1",
                    "Campaign|AVOUnderbelly|AVOUnderbelly2",
                    "Campaign|AVOUnderbelly|AVOUnderbelly3")),

            new AchievementDefinition(
                "AFE2_Yellow_Makes_It_Shoot_Better",
                "Yellow Makes It Shoot Better",
                "Equip a colorway and three decals onto a gun.",
                4,
                AchievementValueResolvers.MaximumGunCosmeticsEquipped),

            new AchievementDefinition(
                "AFE2_The_Story_Of_The_Quarry",
                "The Story of the Quarry",
                "Complete the \"Ancient Anomaly\" Campaign.",
                3,
                AchievementValueResolvers.CompletedCounterKeys(
                    "Campaign",
                    "Campaign|AVOEngineerShip|AVOEngineerShip1",
                    "Campaign|AVOEngineerShip|AVOEngineerShip3",
                    "Campaign|AVOEngineerShip|AVOEngineerShip5")),

            new AchievementDefinition(
                "AFE2_Fully_Loaded",
                "Fully Loaded",
                "Equip a weapon with all attachments, including an Augment.",
                5,
                AchievementValueResolvers.CounterTracker(
                    "Any",
                    "MaxNumAttachmentsEquipped")),

            new AchievementDefinition(
                "AFE2_Reckoning",
                "Reckoning",
                "Complete the \"Final Endeavor\" Campaign.",
                2,
                AchievementValueResolvers.CompletedCounterKeys(
                    "Campaign",
                    "Campaign|AVOQueenFight|AVOUnderbellyFinal",
                    "Campaign|AVOQueenFight|AVOSpaceCruiserFinal")),

            new AchievementDefinition(
                "AFE2_Legionnaires_Service_Medal",
                "Legionnaire's Service Medal",
                "Complete all Campaigns on Casual or higher difficulty.",
                15,
                AchievementValueResolvers.CompletedCounterKeys(
                    "Campaign",
                    "Campaign|AvoPrologue|AvoPrologue01",
                    "Campaign|AVOColony|AVOColony1",
                    "Campaign|AVOColony|AVOColony2",
                    "Campaign|AVOColony|AVOColony3",
                    "Campaign|AVOSpaceCruiser|AVOSpaceCruiser1",
                    "Campaign|AVOSpaceCruiser|AVOSpaceCruiser2",
                    "Campaign|AVOSpaceCruiser|AVOSpaceCruiser3",
                    "Campaign|AVOUnderbelly|AVOUnderbelly1",
                    "Campaign|AVOUnderbelly|AVOUnderbelly2",
                    "Campaign|AVOUnderbelly|AVOUnderbelly3",
                    "Campaign|AVOEngineerShip|AVOEngineerShip1",
                    "Campaign|AVOEngineerShip|AVOEngineerShip3",
                    "Campaign|AVOEngineerShip|AVOEngineerShip5",
                    "Campaign|AVOQueenFight|AVOUnderbellyFinal",
                    "Campaign|AVOQueenFight|AVOSpaceCruiserFinal")),

            new AchievementDefinition(
                "AFE2_Seasoned_Veteran",
                "Seasoned Veteran",
                "Level a Kit to Rank 10.",
                10,
                AchievementValueResolvers.HighestKitLevel),

            new AchievementDefinition(
                "AFE2_Hardened_Trooper",
                "Hardened Trooper",
                "Complete a mission on Intense Difficulty.",
                1,
                AchievementValueResolvers.CounterTrackerThreshold(
                    "Hard|Campaign",
                    "Campaign|MissionCount",
                    1)),

            new AchievementDefinition(
                "AFE2_Maxed_Out",
                "Maxed Out",
                "Fill every slot in a perk board.",
                1,
                AchievementValueResolvers.CounterTrackerThreshold(
                    "Any",
                    "PerkBoardFilled",
                    1)),

            new AchievementDefinition(
                "AFE2_Purge_The_Unclean",
                "Purge the Unclean",
                "Eliminate 1000 Pathogen.",
                1000,
                AchievementValueResolvers.CounterTracker(
                    "Any",
                    "Kills|Pathogen")),

            new AchievementDefinition(
                "AFE2_Well_Stocked_Arsenal",
                "Well Stocked Arsenal",
                "Level 10 weapons to at least two stars.",
                10,
                AchievementValueResolvers.WeaponsAtLeastLevel(3)),

            new AchievementDefinition(
                "AFE2_Head_Hunter",
                "Head Hunter",
                "Get 1000 Headshot kills.",
                1000,
                AchievementValueResolvers.CounterTracker(
                    "Any",
                    "HeadshotKills")),

            new AchievementDefinition(
                "AFE2_Wave_Goodbye",
                "Wave Goodbye",
                "Survive 5 waves in Horde Mode.",
                5,
                AchievementValueResolvers.AchievementCompleted(
                    "A_Avo_HordeMode_Survive",
                    5)),

            new AchievementDefinition(
                "AFE2_Spilled_Milk",
                "Spilled Milk",
                "Eliminate 1000 Synthetics.",
                1000,
                AchievementValueResolvers.CounterTracker(
                    "Any",
                    "Kills|Synths")),

            new AchievementDefinition(
                "AFE2_Overly_Attached",
                "Overly Attached",
                "Collect 50 weapon attachments.",
                50,
                AchievementValueResolvers.WeaponAttachmentCollectionCount),

            new AchievementDefinition(
                "AFE2_Voided_Warranty",
                "Voided Warranty",
                "Collect 5 Augments.",
                5,
                AchievementValueResolvers.AugmentCollectionCount),

            new AchievementDefinition(
                "AFE2_Another_Bug_Hunt",
                "Another Bug Hunt",
                "Eliminate 10000 Xenomorphs.",
                10000,
                AchievementValueResolvers.CounterTracker(
                    "Any",
                    "Kills|Xenos")),

            new AchievementDefinition(
                "AFE2_Custom_Made_Marine",
                "Custom Made Marine",
                "Complete 5 missions with the Specialist Kit.",
                5,
                AchievementValueResolvers.CounterTracker(
                    "Player_Custom",
                    "Campaign|MissionCount")),

            new AchievementDefinition(
                "AFE2_Noble_Soldier",
                "Noble Soldier",
                "Complete a mission on Extreme Difficulty.",
                1,
                AchievementValueResolvers.CounterTrackerThreshold(
                    "Extreme|Campaign",
                    "Campaign|MissionCount",
                    1)),

            new AchievementDefinition(
                "AFE2_Decommission_Mission",
                "Decommission Mission",
                "Eliminate 1000 Combat Automatons.",
                1000,
                AchievementValueResolvers.CounterTracker(
                    "Any",
                    "Kills|Hyperdyne")),

            new AchievementDefinition(
                "AFE2_Fervent_Warrior",
                "Fervent Warrior",
                "Complete a mission on Insane Difficulty.",
                1,
                AchievementValueResolvers.CounterTrackerThreshold(
                    "Insane|Campaign",
                    "Campaign|MissionCount",
                    1)),

            new AchievementDefinition(
                "AFE2_Pure_Professionals",
                "Pure Professionals",
                "Complete a mission on Intense Difficulty or above without anyone taking friendly fire damage.",
                1,
                AchievementValueResolvers.CounterTrackerThreshold(
                    "Any",
                    "MissionsCompletedWithoutFriendlyFire",
                    1)),
        };

        public static IEnumerable<MissionDefinition> CampaignOnlyMissions => Campaigns.SelectMany(c => c.Missions);

        public static IEnumerable<MissionDefinition> GameModeMissions => GameModes.SelectMany(c => c.Missions);

        public static IEnumerable<MissionDefinition> AllMissions => CampaignOnlyMissions.Concat(GameModeMissions);
    }
}