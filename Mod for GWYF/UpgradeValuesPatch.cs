using HarmonyLib;
using TMPro;
using UnityEngine;
using System;

[HarmonyPatch(typeof(Upgrade), "Start")]
class Patch_UpgradeValues
{
    static void Prefix(Upgrade __instance)
    {
        var typeField =
            AccessTools.Field(typeof(Upgrade), "upgradeType");

        var valueField =
            AccessTools.Field(typeof(Upgrade), "value");

        var valueTextField =
            AccessTools.Field(typeof(Upgrade), "valueText");

        var upgradeType =
            (PlayerUpgradeType)typeField.GetValue(__instance);

        float baseValue = 0.1f;
        float minimumValue = 0.05f;

        switch (upgradeType)
        {
            case PlayerUpgradeType.GamblersConfidence:
                baseValue = 0.50f;
                minimumValue = 0.125f;
                break;

            case PlayerUpgradeType.Insurance:
                baseValue = 0.25f;
                minimumValue = 0.10f;
                break;

            case PlayerUpgradeType.BonusDraw:
                baseValue = 0.25f;
                minimumValue = 0.075f;
                break;

            case PlayerUpgradeType.Stakeholder:
                baseValue = 0.50f;
                minimumValue = 0.50f;
                break;
        }

        var players =
            UnityEngine.Object.FindObjectsByType<PlayerProfile>(
                FindObjectsSortMode.None
            );

        int playerCount = Mathf.Max(players.Length, 1);

        float splitValue = baseValue / playerCount;

        splitValue =
            Mathf.Clamp(
                splitValue,
                minimumValue,
                baseValue
            );

        splitValue =
            (float)Math.Round(splitValue, 3);

        valueField.SetValue(__instance, splitValue);

        var valueText =
            (TextMeshPro)valueTextField.GetValue(__instance);

        valueText.text =
            (splitValue * 100f).ToString("0.#") + "%";
    }
}