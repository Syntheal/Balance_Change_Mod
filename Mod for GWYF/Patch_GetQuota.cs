using Extensions;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(GameSettings), "GetQuota")]
public class Patch_GetQuota
{
    static bool Prefix(
        int index,
        long previousQuota,
        long currentMoney,
        ref long __result)
    {
        if (!FixingPlguin.quotaSetting.Value)
            return true;

        if (index == 0)
        {
            __result = 1200;
            return false;
        }

        if (previousQuota <= 0)
        {
            __result = 1200;
            return false;
        }

        var gm = GameManager.Instance;

        int daysPassed = gm != null ? gm.daysPassed : 0;

        float multiplier = Mathf.Clamp(2.5f + (0.5f * daysPassed),2.0f,7.5f);

        __result = (long)(previousQuota * multiplier * FixingPlguin.scaler.Value);

        return false;
    }
}

[HarmonyPatch(typeof(GameManager), "ProgressNextQuota")]
class Patch_ProgressNextQuota
{
    static long cachedQuota;

    static void Prefix(GameManager __instance)
    {
        cachedQuota = __instance.currentQuota;
    }

    static void Postfix()
    {
        var money = NetworkSingleton<MoneyManager>.Instance;

        money.TryChangeBalance(-cachedQuota, null, ChangeType.Misc);
    }
}