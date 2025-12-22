using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Models
{
    public static class GameDefinitions
    {
        public static readonly IReadOnlyList<CampaignDefinition> Campaigns = new[]
        {
            new CampaignDefinition(1, "Priority One", new[]
            {
                new MissionDefinition(1,    "Ingress",      "Campaign|SC-C1|SC-C1M1"),
                //new MissionDefinition(2,  "Rescue",       "Campaign|SC-C1|SC-C1M2"),
                new MissionDefinition(2,    "Rescue",       "Campaign|SC-C1|SC_C1M2"), // This appears to be a typo in the original game data.
                new MissionDefinition(3,    "Extract",      "Campaign|SC-C1|SC-C1M3"),
            }),
            new CampaignDefinition(2, "Giants in the Earth", new[]
            {
                new MissionDefinition(1,    "Insertion",    "Campaign|SC-C2|SC-C2M1"),
                new MissionDefinition(2,    "Contact",      "Campaign|SC-C2|SC-C2M2"),
                new MissionDefinition(3,    "Evacuate",     "Campaign|SC-C2|SC-C2M3"),
            }),
            new CampaignDefinition(3, "The Gift of Fire", new[]
            {
                new MissionDefinition(1,    "Recon",        "Campaign|SC-C3|SC-C3M1"),
                new MissionDefinition(2,    "Advance",      "Campaign|SC-C3|SC-C3M2"),
                new MissionDefinition(3,    "Boarding",     "Campaign|SC-C3|SC-C3M3"),
            }),
            new CampaignDefinition(4, "The Only Way to be Sure", new[]
            {
                new MissionDefinition(1,    "Breach",       "Campaign|SC-C4|SC-C4M1"),
                new MissionDefinition(2,    "Search",       "Campaign|SC-C4|SC-C4M2"),
                new MissionDefinition(3,    "Regicide",     "Campaign|SC-C4|SC-C4M3"),
            }),
            new CampaignDefinition(5, "Promise of a Flower", new[]
            {
                new MissionDefinition(1,    "Scout",        "Campaign|SC-C5|SC-C5M1"),
                new MissionDefinition(2,    "Approach",     "Campaign|SC-C5|SC-C5M2"),
                new MissionDefinition(3,    "Strike",       "Campaign|SC-C5|SC-C5M3"),
            }),
        };

        public static readonly IReadOnlyList<CampaignDefinition> GameModes = new[]
        {
            new CampaignDefinition(0, "Game Modes", new[]
            {
                new MissionDefinition(1,    "Horde Mode: Likasi Tower",         "Challenge|Challenge_Horde|Challenge_Horde_Xeno1"),
                new MissionDefinition(2,    "Horde Mode: Ruptured Cistern",     "Challenge|Challenge_Horde_Cistern|Challenge_Horde_Cistern1"),
                new MissionDefinition(3,    "Horde Mode: Terminal Containment", "Challenge|Challenge_Horde_Reactor|Challenge_Horde_Reactor1"),
                new MissionDefinition(4,    "Point Defense: Quake",             "Challenge|Challenge_PD|GM_PD_Quake"),
                new MissionDefinition(5,    "Restock Turrets: Tower",           "Challenge|Challenge_HM_RT|GM_HM_RT_Tower"),
            }),
        };

        public static readonly IReadOnlyList<AchievementDefinition> Achievements = new[]
        {
            new AchievementDefinition("High Voltage",           "ElectricKills",                            1000),
            new AchievementDefinition("I Think They Like Me",   "MostGrapplesPerMission",                   5),
            new AchievementDefinition("Improvised Explosives",  "ExplosiveBarrelsKills",                    50),
            new AchievementDefinition("Suturing Expert",        "MedkitsUsedOnAllies",                      100),
            new AchievementDefinition("Supportive Squad",       "MissionsCompletedWithoutDownsOrDeaths",    50),
            new AchievementDefinition("Tower Defense",          "ConsumablesUsed",                          500),
            new AchievementDefinition("Burn 'Em Out",           "ThermalKills",                             1000),
            new AchievementDefinition("It's A Bug Hunt",        "BasicKills|Xenos",                         10000),
            new AchievementDefinition("Anti-Mutation Station",  "Kills|Pathogen",                           300),
            new AchievementDefinition("Glorified Toasters",     "BasicKills|Synths",                        1000),
            new AchievementDefinition("Hidden Caches Found",    "HiddenCachesFound",                        50),
        };

        public static IEnumerable<MissionDefinition> CampaignOnlyMissions => Campaigns.SelectMany(c => c.Missions);
        public static IEnumerable<MissionDefinition> HordeOnlyMissions => GameModes.SelectMany(c => c.Missions);
        public static IEnumerable<MissionDefinition> AllMissions => CampaignOnlyMissions.Concat(HordeOnlyMissions);
    }

}
