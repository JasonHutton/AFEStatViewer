using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFEStatViewer.Services;

namespace AFEStatViewer.Models
{
    public static class AFE1GameDefinitions
    {
        public static readonly ClassKitVisualStyle ClassKitStyle = new(
            CompleteColorHex: "#1F6F3D",
            IncompleteColorHex: "#16351F",
            IconBackgroundColorHex: "#00000000",
            CompletionSectionBackgroundColorHex: "#00000000",
            CompletionTextColorHex: "#000000",
            CompleteOpacity: 1.0,
            IncompleteOpacity: 0.18,
            IconBackgroundOpacity: 0.0,
            IconSize: 18.0
        );

        public static readonly IReadOnlyList<ClassKitDefinition> ClassKits = new[]
        {
            new ClassKitDefinition("Gunner",      "Player_Gunner",      "AFE1_Class_Gunner.png"),
            new ClassKitDefinition("Demolisher",  "Player_Demolisher",  "AFE1_Class_Demolisher.png"),
            new ClassKitDefinition("Technician",  "Player_Technician",  "AFE1_Class_Technician.png"),
            new ClassKitDefinition("Doc",         "Player_Doc",         "AFE1_Class_Doc.png"),
            new ClassKitDefinition("Recon",       "Player_Recon",       "AFE1_Class_Recon.png"),
            new ClassKitDefinition("Phalanx",     "Player_Phalanx",     "AFE1_Class_Phalanx.png"),
            new ClassKitDefinition("Lancer",      "Player_Lancer",      "AFE1_Class_Lancer.png"),
        };

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
            new AchievementDefinition("AFE1_High_Voltage", "High Voltage", "Kill 1000 enemies with electricity.", 1000, AchievementValueResolvers.CounterTracker("Any", "ElectricKills")),
            new AchievementDefinition("AFE1_I_Think_They_Like_Me", "I Think They Like Me", "Get grappled five times in the same mission.", 5, AchievementValueResolvers.CounterTracker("Any", "MostGrapplesPerMission")),
            new AchievementDefinition("AFE1_Improvised_Explosives", "Improvised Explosives", "Kill 50 enemies with explosive barrels or pods.", 50, AchievementValueResolvers.CounterTracker("Any", "ExplosiveBarrelsKills")),
            new AchievementDefinition("AFE1_Suturing_Expert", "Suturing Expert", "Heal your fireteam with an aid kit 100 times.", 100, AchievementValueResolvers.CounterTracker("Any", "MedkitsUsedOnAllies")),
            new AchievementDefinition("AFE1_Supportive_Squad", "Supportive Squad", "Complete 50 missions without anyone being downed.", 50, AchievementValueResolvers.CounterTracker("Any", "MissionsCompletedWithoutDownsOrDeaths")),
            new AchievementDefinition("AFE1_Tower_Defense", "Tower Defense", "Use 500 consumables.", 500, AchievementValueResolvers.CounterTracker("Any", "ConsumablesUsed")),
            new AchievementDefinition("AFE1_Burn_Em_Out", "Burn 'Em Out", "Kill 1000 enemies with fire.", 1000, AchievementValueResolvers.CounterTracker("Any", "ThermalKills")),
            new AchievementDefinition("AFE1_Its_A_Bug_Hunt", "It's A Bug Hunt", "Kill 10000 Xenomorphs.", 10000, AchievementValueResolvers.CounterTracker("Any", "Kills|Xenos")),
            new AchievementDefinition("AFE1_Anti_Mutation_Station", "Anti-Mutation Station", "Eliminate 300 Pathogen.", 300, AchievementValueResolvers.CounterTracker("Any", "Kills|Pathogen")),
            new AchievementDefinition("AFE1_Glorified_Toasters", "Glorified Toasters", "Eliminate 1000 Synthetics.", 1000, AchievementValueResolvers.CounterTracker("Any", "Kills|Synths")),
            new AchievementDefinition("AFE1_Keen_Eye", "Keen Eye", "Open 50 hidden caches.", 50, AchievementValueResolvers.CounterTracker("Any", "HiddenCachesFound")),

            /*
            new AchievementDefinition("I Can't Lie About Your Chances", "Finish a mission on Hardcore 10 Difficulty.",                  "Any",          "XXXX",                             1),
            new AchievementDefinition("CMISRS Resource",                "Return 48 pieces of intel on a single character.",             "Any",          "XXXX",                             48),
            new AchievementDefinition("Promise of a Flower",            "Finish the \"Promise of a Flower\" Campaign.",                 "Any",          "XXXX",                             1),
            new AchievementDefinition("Pod Popper",                     "Destroy 50 explosive spore pods with Weapons or Abilities.",   "Any",          "XXXX",                             1),
            new AchievementDefinition("CMISRS Asset",                   "Return 39 pieces of intel on a single character.",             "Any",          "XXXX",                             39),
            new AchievementDefinition("All My Personal Friends",        "Level 30 weapons to four stars on a single character.",        "Any",          "XXXX",                             39),
            new AchievementDefinition("Reticulum Theatre Medal",        "Finish all Campaigns on Insane Difficulty.",                   "Any",          "XXXX",                             1), // This doesn't include Promise of a Flower
            new AchievementDefinition("Trigger Discipline",             "Complete a mission on Intense Difficulty or above without anyone taking friendly fire damage.",    "Any",          "XXXX",                             1),
            new AchievementDefinition("LV-895 Campaign Medal",          "Finish all Campaigns on Extreme Difficulty.",                  "Any",          "XXXX",                             1), // This doesn't include Promise of a Flower
            new AchievementDefinition("Got All I Need",                 "Complete a mission wtihout using aid kits, ammo crates, or consumables.",                          "Any",          "XXXX",                             1),
            new AchievementDefinition("Expeditionary Service Ribbon",   "Finish all Campaigns on Standard Difficulty.",                 "Any",          "XXXX",                             1), // This doesn't include Promise of a Flower
            new AchievementDefinition("Those Things Were Huge",         "Eliminate 2000 elite enemies.",                                "Any",          "XXXX",                             2000),
            new AchievementDefinition("My Kind of Crazy",               "Finish a mission on Insane Difficulty.",                       "Any",          "XXXX",                             1),
            new AchievementDefinition("A Stand Up Fight",               "Finish a mission on Extreme Difficulty.",                      "Any",          "XXXX",                             1),
            new AchievementDefinition("It's a Cover, not a Hat",        "Collect 20 hats on a single character.",                       "Any",          "XXXX",                             20),
            new AchievementDefinition("Art Gallery",                    "Collect 40 decals on a single character.",                     "Any",          "XXXX",                             40),
            new AchievementDefinition("Overwhelming Confidence",        "Complete 25 successful Challenge runs.",                       "Any",          "XXXX",                             25),
            new AchievementDefinition("The Only Way to be Sure",        "Finish the \"The Only Way to be Sure\" Campaign.",             "Any",          "XXXX",                             1),
            new AchievementDefinition("The Gift of Fire",               "Finish the \"Gift of Fire\" Campaign.",                        "Any",          "XXXX",                             1),
            new AchievementDefinition("Specialist",                     "Level a Kit to Rank 8.",                                       "Any",          "XXXX",                             1),
            new AchievementDefinition("Express Yourself",               "Collect 20 emotes on a single character.",                     "Any",          "XXXX",                             20),
            new AchievementDefinition("Fully Rigged",                   "Fill every slot in a perk board.",                             "Any",          "XXXX",                             1),
            new AchievementDefinition("Ready for Anything",             "Collect 25 attachments on a single character.",                "Any",          "XXXX",                             25),
            new AchievementDefinition("State of the Art Firepower",     "Collect one attachment of every type on a single character.",  "Any",          "XXXX",                             1),
            new AchievementDefinition("Giants in the Earth",            "Finish the \"Giants in the Earth\" Campaign.",                 "Any",          "XXXX",                             1),
            new AchievementDefinition("A Personal Friend of Mine",      "Level a weapon to four stars.",                                "Any",          "XXXX",                             1),
            new AchievementDefinition("Priority One",                   "Finish the \"Priority One\" Campaign.",                        "Any",          "XXXX",                             1),
            new AchievementDefinition("Confidence Course",              "Do a successful Challenge run.",                               "Any",          "XXXX",                             1),
            new AchievementDefinition("Didn't Break a Sweat",           "Finish a mission on Intense Difficulty.",                      "Any",          "XXXX",                             1),
            new AchievementDefinition("Nukes, Knives, AND Sharp Sticks","Equip three attachments on a single weapon.",                  "Any",          "XXXX",                             1),
            new AchievementDefinition("Fashion Team",                   "Change your hat and outfit.",                                  "Any",          "XXXX",                             1),
            new AchievementDefinition("Red Makes It Shoot Faster",      "Equip a colorway and decal onto a gun.",                       "Any",          "XXXX",                             1),
            */
        };

        public static IEnumerable<MissionDefinition> CampaignOnlyMissions => Campaigns.SelectMany(c => c.Missions);
        public static IEnumerable<MissionDefinition> HordeOnlyMissions => GameModes.SelectMany(c => c.Missions);
        public static IEnumerable<MissionDefinition> AllMissions => CampaignOnlyMissions.Concat(HordeOnlyMissions);
    }

}
