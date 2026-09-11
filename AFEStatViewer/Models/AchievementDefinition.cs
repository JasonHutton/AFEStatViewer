using System;
using System.Text.Json;

namespace AFEStatViewer.Models
{
    public record AchievementDefinition(
        string Id,
        string Name,
        string Description,
        int Target,
        Func<JsonElement, int> ValueResolver);
}