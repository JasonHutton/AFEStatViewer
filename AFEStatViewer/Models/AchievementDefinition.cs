using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AFEStatViewer.Models
{
    public sealed record AchievementDefinition(string Id,
                                                string Name,
                                                string Description,
                                                int Target,
                                                Func<JsonElement, int> ValueResolver);
}

