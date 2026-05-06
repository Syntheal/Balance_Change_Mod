using HarmonyLib;
using UnityEngine;
using System.Linq;
using FMODUnity;

[HarmonyPatch(typeof(UpgradeManager), "ChangeUpgradeData")]
class Patch_ShareUpgrades
{
    static bool isProcessing;

    static EventReference wheelWinEvent;

    static bool Prefix(
        UpgradeManager __instance,
        ulong steamId,
        PlayerUpgradeType type,
        float amount)
    {
        if (isProcessing)
            return false;

        try
        {
            isProcessing = true;

            var profiles = Object.FindObjectsByType<PlayerProfile>(
                FindObjectsSortMode.None);

            float oldValue =
                __instance.GetUpgradeData(steamId, type);

            float baseValue = oldValue + amount;

            bool jackpot = false;

            if (Mirror.NetworkServer.active)
            {
                jackpot = Random.Range(0f, 1f) < 0.004f;

                if (jackpot)
                {
                    baseValue = oldValue + amount * 10f;

                    var player = Object
                        .FindObjectsByType<PlayerController>(
                            FindObjectsSortMode.None)
                        .FirstOrDefault();

                    if (player != null)
                    {
                        if (wheelWinEvent.IsNull)
                        {
                            wheelWinEvent =
                                RuntimeManager.PathToEventReference(
                                    "event:/Games/WheelOfFortune/WheelFortuneWin");
                        }

                        SFXManager.SFXOneShot(
                            wheelWinEvent,
                            player.transform.position);

                        FixingPlguin.Log.LogInfo("Wheel jackpot played.");
                    }
                }
            }

            foreach (var profile in profiles)
            {
                __instance.SetUpgradeData(
                    profile.steamId,
                    type,
                    baseValue);
            }

            return false;
        }
        finally
        {
            isProcessing = false;
        }
    }
}