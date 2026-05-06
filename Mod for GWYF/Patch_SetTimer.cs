using Extensions;
using HarmonyLib;

[HarmonyPatch(typeof(GameManager), "OnAwake")]
public class Patch_GameManager_OnAwake
{
    static void Postfix(GameManager __instance)
    {
        var gsField = AccessTools.Field(typeof(GameManager), "_gs");

        if (gsField.GetValue(__instance) is GameSettings gs)
        {
            gs.dayDuration = 450;
        }
    }
}

[HarmonyPatch(typeof(GameManager), "OnTimerChanged")]
public class Patch_OnTimerChanged
{
    static bool Prefix(float oldValue, float newValue)
    {
        NetworkSingleton<GameUI>.Instance.SetTimerText(450f - newValue);
        return false;
    }
}