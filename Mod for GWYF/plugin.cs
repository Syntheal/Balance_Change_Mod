using BepInEx;
using BepInEx.Logging;
using FMOD.Studio;
using FMODUnity;
using HarmonyLib;
using UnityEngine;

[BepInPlugin("fixes.gamble", "Gamble Fixer", "0.1.0")]
public class FixingPlguin    : BaseUnityPlugin
{
    public static ManualLogSource Log;

    private void Awake()
    {
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