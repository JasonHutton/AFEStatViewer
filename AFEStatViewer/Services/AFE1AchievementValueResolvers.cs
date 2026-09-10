using System;
using System.Collections.Generic;
using System.Text.Json;

namespace AFEStatViewer.Services
{
    public sealed class AFE1AchievementValueResolvers : AchievementValueResolvers
    {
        private AFE1AchievementValueResolvers()
        {
        }

        public static Func<JsonElement, int> AnyCounterThreshold(
            string key,
            int threshold,
            params string[] sets)
        {
            return root =>
            {
                int total = 0;

                foreach (string set in sets)
                {
                    total += GetCounterValue(root, set, key);
                }

                return total >= threshold ? 1 : 0;
            };
        }

        public static Func<JsonElement, int> GeneralInventoryUniqueClassCount(
            string requiredPath,
            string? excludedPath = null)
        {
            return root =>
            {
                if (!root.TryGetProperty("GeneralInventory", out var generalInventory) ||
                    !generalInventory.TryGetProperty("Items", out var items))
                {
                    return 0;
                }

                HashSet<string> classes = new(StringComparer.Ordinal);

                foreach (var item in items.EnumerateArray())
                {
                    if (!item.TryGetProperty("Class", out var classElement))
                    {
                        continue;
                    }

                    string? itemClass = classElement.GetString();

                    if (string.IsNullOrEmpty(itemClass))
                    {
                        continue;
                    }

                    if (!itemClass.Contains(requiredPath, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (excludedPath != null &&
                        itemClass.Contains(excludedPath, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    classes.Add(itemClass);
                }

                return classes.Count;
            };
        }

        public static int AttachmentCollectionCount(JsonElement root)
        {
            if (!root.TryGetProperty("ModInventory", out var modInventory) ||
                !modInventory.TryGetProperty("UnlimitedModStorage", out var unlimitedModStorage))
            {
                return 0;
            }

            HashSet<string> attachments = new(StringComparer.Ordinal);

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

                if (modDef.Contains(
                    "/Game/Blueprints/Weapons/Attachments/",
                    StringComparison.Ordinal))
                {
                    attachments.Add(modDef);
                }
            }

            return attachments.Count;
        }

        public static int AttachmentTypeCount(JsonElement root)
        {
            if (!root.TryGetProperty("ModInventory", out var modInventory) ||
                !modInventory.TryGetProperty("UnlimitedModStorage", out var unlimitedModStorage))
            {
                return 0;
            }

            HashSet<string> attachmentTypes = new(StringComparer.Ordinal);

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

                const string attachmentRoot = "/Game/Blueprints/Weapons/Attachments/";

                int rootIndex = modDef.IndexOf(
                    attachmentRoot,
                    StringComparison.Ordinal);

                if (rootIndex < 0)
                {
                    continue;
                }

                string remainder = modDef[(rootIndex + attachmentRoot.Length)..];

                int slashIndex = remainder.IndexOf('/');

                string type = slashIndex >= 0
                    ? remainder[..slashIndex]
                    : remainder;

                if (!string.IsNullOrEmpty(type))
                {
                    attachmentTypes.Add(type);
                }
            }

            return attachmentTypes.Count;
        }

        public static int EmoteCollectionCount(JsonElement root)
        {
            if (!root.TryGetProperty("ModInventory", out var modInventory) ||
                !modInventory.TryGetProperty("UnlimitedModStorage", out var unlimitedModStorage))
            {
                return 0;
            }

            HashSet<string> emotes = new(StringComparer.Ordinal);

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

                bool isStandardEmote = modDef.Contains(
                    "/Emotes/EmoteMods/",
                    StringComparison.Ordinal);

                bool isSeasonalEmote = modDef.Contains(
                    "/_Emotes/",
                    StringComparison.Ordinal);

                if (isStandardEmote || isSeasonalEmote)
                {
                    emotes.Add(modDef);
                }
            }

            return emotes.Count;
        }
    }
}