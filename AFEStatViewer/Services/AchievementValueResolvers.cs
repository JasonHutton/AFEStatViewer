using System;
using System.Collections.Generic;
using System.Text.Json;

namespace AFEStatViewer.Services
{
    public static class AchievementValueResolvers
    {
        public static Func<JsonElement, int> CounterTracker(string set, string key)
        {
            return root => GetCounterValue(root, set, key);
        }

        public static Func<JsonElement, int> CounterTrackerThreshold(string set, string key, int threshold)
        {
            return root => GetCounterValue(root, set, key) >= threshold ? 1 : 0;
        }

        public static Func<JsonElement, int> CompletedCounterKeys(string set, params string[] keys)
        {
            return root =>
            {
                int completed = 0;

                foreach (string key in keys)
                {
                    if (GetCounterValue(root, set, key) > 0)
                    {
                        completed++;
                    }
                }

                return completed;
            };
        }

        public static Func<JsonElement, int> AchievementCompleted(string achievementId, int completedValue = 1)
        {
            return root =>
            {
                string key = $"Achievements_MM_{achievementId}";

                return GetCounterValue(root, "Achievements", key) > 0
                    ? completedValue
                    : 0;
            };
        }

        public static int HighestKitLevel(JsonElement root)
        {
            if (!root.TryGetProperty("CharacterInventory", out var characterInventory) ||
                !characterInventory.TryGetProperty("CharacterKits", out var characterKits))
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

        public static Func<JsonElement, int> WeaponsAtLeastLevel(int minimumLevel)
        {
            return root =>
            {
                if (!root.TryGetProperty("GunInventory", out var gunInventory) ||
                    !gunInventory.TryGetProperty("GunFrames", out var gunFrames))
                {
                    return 0;
                }

                HashSet<string> qualifyingWeapons = new(StringComparer.Ordinal);

                foreach (var gunFrame in gunFrames.EnumerateArray())
                {
                    if (!gunFrame.TryGetProperty("GunClass", out var gunClassElement) ||
                        !gunFrame.TryGetProperty("LevelData", out var levelData) ||
                        !levelData.TryGetProperty("Level", out var levelElement))
                    {
                        continue;
                    }

                    string? gunClass = gunClassElement.GetString();

                    if (string.IsNullOrEmpty(gunClass))
                    {
                        continue;
                    }

                    if (levelElement.GetInt32() >= minimumLevel)
                    {
                        qualifyingWeapons.Add(gunClass);
                    }
                }

                return qualifyingWeapons.Count;
            };
        }

        public static int BodyCosmeticsEquipped(JsonElement root)
        {
            int equipped = 0;

            if (GetCounterValue(root, "Any", "HatEquipped") > 0)
            {
                equipped++;
            }

            if (GetCounterValue(root, "Any", "KitSkinTorsoOverrideEquipped") > 0)
            {
                equipped++;
            }

            if (GetCounterValue(root, "Any", "KitSkinLegsOverrideEquipped") > 0)
            {
                equipped++;
            }

            return equipped;
        }

        public static int HatCollectionCount(JsonElement root)
        {
            if (!root.TryGetProperty("GeneralInventory", out var generalInventory) ||
                !generalInventory.TryGetProperty("Items", out var items))
            {
                return 0;
            }

            HashSet<string> collectedHats = new(StringComparer.Ordinal);

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

                if (itemClass.StartsWith(
                    "/Game/GeneratedCustomizationData/HeadGearDefinitions/",
                    StringComparison.Ordinal))
                {
                    collectedHats.Add(itemClass);
                }
            }

            return collectedHats.Count;
        }

        public static int WeaponAttachmentCollectionCount(JsonElement root)
        {
            if (!root.TryGetProperty("ModInventory", out var modInventory) ||
                !modInventory.TryGetProperty("UnlimitedModStorage", out var unlimitedModStorage))
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

                bool isAttachment = modDef.StartsWith(
                    "/Game/Blueprints/Venus_Weapons/Attachments/",
                    StringComparison.Ordinal);

                bool isWeaponTrait = modDef.StartsWith(
                    "/Game/Blueprints/Venus_Weapons/Perks/Mastery/",
                    StringComparison.Ordinal);

                bool isInternalAttachmentVariant = modDef.Contains(
                    "/Avo_Overclock_Explosive_Cryo.", // This is bugged I think. I don't believe this is actually ingame, but it's all over the save. Maybe future content?
                    StringComparison.Ordinal);

                if ((isAttachment || isWeaponTrait) && !isInternalAttachmentVariant)
                {
                    collectedAttachments.Add(modDef);
                }
            }

            return collectedAttachments.Count;
        }

        public static int AugmentCollectionCount(JsonElement root)
        {
            if (!root.TryGetProperty("ModInventory", out var modInventory) ||
                !modInventory.TryGetProperty("UnlimitedModStorage", out var unlimitedModStorage))
            {
                return 0;
            }

            HashSet<string> collectedAugments = new(StringComparer.Ordinal);

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

                bool isAugment = modDef.StartsWith(
                    "/Game/Blueprints/Venus_Weapons/Attachments/Overclocks/",
                    StringComparison.Ordinal);

                bool isInternalAttachmentVariant = modDef.Contains(
                    "/Avo_Overclock_Explosive_Cryo.", // This is bugged I think. I don't believe this is actually ingame, but it's all over the save. Maybe future content?
                    StringComparison.Ordinal);

                if (isAugment && !isInternalAttachmentVariant)
                {
                    collectedAugments.Add(modDef);
                }
            }

            return collectedAugments.Count;
        }

        private static int GetCounterValue(JsonElement root, string set, string key)
        {
            if (!root.TryGetProperty("CounterTracker", out var counterTracker) ||
                !counterTracker.TryGetProperty("Sets", out var sets) ||
                !sets.TryGetProperty(set, out var counterSet) ||
                !counterSet.TryGetProperty("Vars", out var vars) ||
                !vars.TryGetProperty(key, out var valueElement))
            {
                return 0;
            }

            if (valueElement.ValueKind != JsonValueKind.Number)
            {
                return 0;
            }

            if (valueElement.TryGetInt32(out int intValue))
            {
                return intValue;
            }

            if (valueElement.TryGetDouble(out double doubleValue))
            {
                return (int)Math.Floor(doubleValue);
            }

            return 0;
        }

        public static int MaximumGunCosmeticsEquipped(JsonElement root)
        {
            // Once the achievement has been completed, preserve that state even if the
            // player later removes cosmetics from the weapon that originally earned it.
            if (GetCounterValue(
                root,
                "Achievements",
                "Achievements_MM_A_Avo_Cosmetic_EquipGun") > 0)
            {
                return 4;
            }

            if (!root.TryGetProperty("GunInventory", out var gunInventory) ||
                !gunInventory.TryGetProperty("GunFrames", out var gunFrames))
            {
                return 0;
            }

            int maximumEquipped = 0;

            foreach (var gunFrame in gunFrames.EnumerateArray())
            {
                if (!gunFrame.TryGetProperty("GunInstances", out var gunInstances))
                {
                    continue;
                }

                foreach (var gunInstance in gunInstances.EnumerateArray())
                {
                    int equipped = 0;

                    if (gunInstance.TryGetProperty("AssignedColorway", out var assignedColorway))
                    {
                        string? colorway = assignedColorway.GetString();

                        if (!string.IsNullOrEmpty(colorway))
                        {
                            equipped++;
                        }
                    }

                    if (gunInstance.TryGetProperty("AssignedDecals", out var assignedDecals))
                    {
                        foreach (var decal in assignedDecals.EnumerateArray())
                        {
                            if (!decal.TryGetProperty("Class", out var classElement))
                            {
                                continue;
                            }

                            string? decalClass = classElement.GetString();

                            if (string.IsNullOrEmpty(decalClass))
                            {
                                continue;
                            }

                            bool isEmptyDecal = decalClass.Contains(
                                "/GunDecal_None.",
                                StringComparison.Ordinal);

                            if (!isEmptyDecal)
                            {
                                equipped++;
                            }
                        }
                    }

                    if (equipped > maximumEquipped)
                    {
                        maximumEquipped = equipped;
                    }
                }
            }

            return Math.Min(maximumEquipped, 4);
        }
    }
}