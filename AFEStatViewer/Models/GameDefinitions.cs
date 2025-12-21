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
            /*new CampaignDefinition(5, "Promise of a Flower", new[]
            {
                new MissionDefinition(1,    "Scout",        "Campaign|SC-C2|SC-C2M1"),
                new MissionDefinition(2,    "Approach",     "Campaign|SC-C2|SC-C2M2"),
                new MissionDefinition(3,    "Strike",       "Campaign|SC-C2|SC-C2M3"),
            }),
            new CampaignDefinition(0, "Game Modes", new[]
            {
                new MissionDefinition(1,    "Horde Mode: Likasi Tower",         "Campaign|SC-C2|SC-C2M1"),
                new MissionDefinition(2,    "Horde Mode: Ruptured Cistern",     "Campaign|SC-C2|SC-C2M2"),
                new MissionDefinition(3,    "Horde Mode: Terminal Containment", "Campaign|SC-C2|SC-C2M3"),
                new MissionDefinition(4,    "Point Defense: Quake",             "Campaign|SC-C2|SC-C2M3"),
                new MissionDefinition(5,    "Restock Turrets: Tower",           "Campaign|SC-C2|SC-C2M3"),
            }),*/
        };
    
        public static IEnumerable<MissionDefinition> AllMissions => Campaigns.SelectMany(c => c.Missions);
    }

}
