using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

static class StampRuntimeState
{
    public static bool IsRerolling;
}

[HarmonyPatch(typeof(ItemStampManager), "RerollAllItemStamps")]
class Patch_RerollAllItemStamps
{
    static void Prefix(ItemStampManager __instance)
    {
        if (!FixingPlguin.rerollSetting.Value)
            return;

        StampRuntimeState.IsRerolling = true;

        var preAssigned = AccessTools
            .Field(typeof(ItemStampManager), "_preAssignedItems")
            .GetValue(__instance) as Dictionary<string, GameObject>;

        preAssigned?.Clear();
    }

    static void Postfix()
    {
        StampRuntimeState.IsRerolling = false;
    }
}


[HarmonyPatch(typeof(ItemStampManager), "DestroyAllSpawnedInstances")]
class Patch_DestroyAllSpawnedInstances
{
    static void Postfix(ItemStampManager __instance)
    {
        if (!FixingPlguin.rerollSetting.Value)
            return;

        if (!StampRuntimeState.IsRerolling)
            return;

        var purchased = AccessTools
            .Field(typeof(ItemStampManager), "_purchasedStamps")
            .GetValue(__instance) as HashSet<ItemStamp>;

        purchased?.Clear();
    }
}

[HarmonyPatch(typeof(ItemStampManager), "RetrieveAndRespawnAllItemStamps")]
class Patch_RetrieveAndRespawnAllItemStamps
{
    static void Prefix(ItemStampManager __instance)
    {
        if (!FixingPlguin.rerollSetting.Value)
            return;

        var preAssigned = AccessTools
            .Field(typeof(ItemStampManager), "_preAssignedItems")
            .GetValue(__instance) as Dictionary<string, GameObject>;

        preAssigned?.Clear();
    }
}