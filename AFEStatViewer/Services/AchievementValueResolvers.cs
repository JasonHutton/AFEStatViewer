using AFEStatViewer.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace AFEStatViewer.Services
{
    public static class AchievementValueResolvers
    {
        public static Func<JsonElement, int> CounterTracker(string set, string key)
        {
            return root =>
            {
                if (!root.TryGetProperty("CounterTracker", out var counterTracker) ||
                    !counterTracker.TryGetProperty("Sets", out var sets) ||
                    !sets.TryGetProperty(set, out var counterSet) ||
                    !counterSet.TryGetProperty("Vars", out var vars) ||
                    !vars.TryGetProperty(key, out var valueElement))
                {
                    return 0;
                }

                return valueElement.GetInt32();
            };
        }

        public static int HighestKitLevel(JsonElement root)
        {
            if (!root.TryGetProperty("CharacterInventory", out var characterInventory) || !characterInventory.TryGetProperty("CharacterKits", out var characterKits))
            {
                return 0;
            }

            int highestLevel = 0;

            foreach (var kit in characterKits.EnumerateArray())
            {
                if (!kit.TryGetProperty("LevelData", out var levelData) ||
                    !levelData.TryGetProperty("Level", out var levelElement))
                {
                    continue;
                }

                int level = levelElement.GetInt32();

                if (level > highestLevel)
                {
                    highestLevel = level;
                }
            }

            return highestLevel;
        }

        public static int WeaponAttachmentCollectionCount(JsonElement root)
        {
            if (!root.TryGetProperty("ModInventory", out var modInventory) || !modInventory.TryGetProperty("UnlimitedModStorage", out var unlimitedModStorage))
            {
                return 0;
            }

            HashSet<string> collectedAttachments = new(StringComparer.Ordinal);

            foreach (var slot in unlimitedModStorage.EnumerateArray())
            {
                if (!slot.TryGetProperty("ModDef", out var modDefElement))
                {
                    continue;
                }

                string? modDef = modDefElement.GetString();

                if (string.IsNullOrEmpty(modDef))
                {
                    continue;
                }

                bool isAttachment = modDef.StartsWith("/Game/Blueprints/Venus_Weapons/Attachments/", StringComparison.Ordinal);

                bool isWeaponTrait = modDef.StartsWith("/Game/Blueprints/Venus_Weapons/Perks/Mastery/", StringComparison.Ordinal);

                bool isInternalAttachmentVariant = modDef.Contains("/Avo_Overclock_Explosive_Cryo.", StringComparison.Ordinal); // Unsure, I think this may be bugged though?

                if ((isAttachment || isWeaponTrait) && !isInternalAttachmentVariant)
                {
                    collectedAttachments.Add(modDef);
                }
            }

            return collectedAttachments.Count;
        }
    }
}