using HarmonyLib;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

[HarmonyPatch(typeof(CosmeticWardrobe), "TryEquipCosmetic")]
class Patch_EquipLockedCosmetics
{
    static bool Prefix(CosmeticWardrobe __instance, int spawnIndex)
    {
        if (NetworkClient.localPlayer == null)
            return false;

        var inst = Traverse.Create(__instance);

        var spawnTransforms = inst.Field("spawnTransforms")
            .GetValue<Transform[]>();

        var currentPage = inst.Field("currentPage")
            .GetValue<int>();

        var cosmetics = inst.Field("currentCategoryCosmetics")
            .GetValue<List<CosmeticData>>();

        if (spawnTransforms == null || spawnIndex < 0 || spawnIndex >= spawnTransforms.Length)
            return false;

        if (spawnIndex == 0)
        {
            var custom = NetworkClient.localPlayer.GetComponent<PlayerCustomization>();
            custom?.ClearCategory(inst.Field("currentCategory").GetValue<CosmeticType>());
            return false;
        }

        int usableSlotsPerPage = inst.Method("GetUsableSlotsPerPage").GetValue<int>();

        int num = currentPage * usableSlotsPerPage + (spawnIndex - 1);

        if (cosmetics == null || num < 0 || num >= cosmetics.Count)
            return false;

        var cosmetic = cosmetics[num];
        if (cosmetic == null)
            return false;

        var playerCustomization = NetworkClient.localPlayer.GetComponent<PlayerCustomization>();
        if (playerCustomization == null)
            return false;

        playerCustomization.CmdChangeCustomization(cosmetic.cosmeticId, true);

        FixingPlguin.Log.LogInfo(
            $"Forced equip: {cosmetic.cosmeticName} ({cosmetic.cosmeticId})");

        return false;
    }
}