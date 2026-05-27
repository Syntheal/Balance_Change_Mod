using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using FMOD.Studio;
using FMODUnity;
using HarmonyLib;
using UnityEngine;

[BepInPlugin("fixes.gamble", "Gamble Fixer", "0.1.3")]
public class FixingPlguin    : BaseUnityPlugin
{
    public static ManualLogSource Log;

    internal static ConfigEntry<float> scaler;
    internal static ConfigEntry<bool> maxPlayers;
    internal static ConfigEntry<bool> ticketSetting;
    internal static ConfigEntry<bool> shareUpgrades;
    internal static ConfigEntry<bool> empoweredUpgrades;
    internal static ConfigEntry<int> timerSetting;
    internal static ConfigEntry<bool> rerollSetting;
    internal static ConfigEntry<bool> quotaSetting;
    internal static ConfigEntry<bool> payQuotaSetting;

    private void Awake()
    {
        quotaSetting = Config.Bind<bool>(
            "Quota Settings",
            "Custom Quota",
            true,
            "Enables custom quota calculation"
            );
        scaler = Config.Bind<float>(
            "Quota Settings",
            "Quota Mult",
            1.0f,
            "Multiplies quota for each day by this amount (only custom)"
            );
        payQuotaSetting = Config.Bind<bool>(
            "Quota Settings",
            "Pay Quota",
            true,
            "Makes you pay quota at the end of each day");
        timerSetting = Config.Bind<int>(
            "Settings",
            "Game Timer",
            450,
            "Changes how long the game timer is (Seconds)"
            );
        maxPlayers = Config.Bind<bool>(
            "Settings",
            "Max Players",
            true,
            "Make max lobby size 40 instead of 8"
            );

        ticketSetting = Config.Bind<bool>(
            "Settings",
            "Custom Tickets",
            true,
            "Enable custom ticket calculation, allowing 50 tickets each day from over quota"
            );

        shareUpgrades = Config.Bind<bool>(
            "Settings",
            "Share Upgrades",
            true,
            "Enable sharing of upgrades (Divides them out)"
            );
        empoweredUpgrades = Config.Bind<bool>(
            "Settings",
            "Empowered Upgrades",
            true,
            "Enables a 1/250 chance for upgrades to give 10x value"
            );
        rerollSetting = Config.Bind<bool>(
            "Settings",
            "Reroll Logic",
            true,
            "Changes the shop to also reroll empty slots"
            );
        Log = Logger;
        var harmony = new Harmony("fixes.gamble");
        harmony.PatchAll();

        Log.LogInfo("Fixer loaded");
    }

    private void DumpAllEvents()
    {
        FMOD.Studio.Bank[] banks;
        RuntimeManager.StudioSystem.getBankList(out banks);

        foreach (var bank in banks)
        {
            bank.getEventList(out EventDescription[] events);

            foreach (var evt in events)
            {
                evt.getPath(out string path);

                FixingPlguin.Log.LogInfo($"EVENT: {path}");
            }
        }
    }
}