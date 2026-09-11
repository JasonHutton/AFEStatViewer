using System;
using System.Text.Json;

namespace AFEStatViewer.Models
{
    public sealed record AFE1HardcoreAchievementDefinition(
        string Id,
        string Name,
        string Description,
        int Target,
        Func<JsonElement, int> ValueResolver)
        : AchievementDefinition(
            Id,
            Name,
            Description,
            Target,
            ValueResolver);
}