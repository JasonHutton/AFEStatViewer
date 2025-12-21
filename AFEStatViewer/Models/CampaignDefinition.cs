using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Models
{
    public sealed record CampaignDefinition(int Number, string Name, IReadOnlyList<MissionDefinition> Missions);
}
