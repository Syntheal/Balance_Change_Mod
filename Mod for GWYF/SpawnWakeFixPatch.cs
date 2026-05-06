using HarmonyLib;
using System.Collections;
using System.Reflection;
using UnityEngine;
using Mirror;

[HarmonyPatch]
public static class SpawnWakeFixPatch
{
    [HarmonyPatch(typeof(SpawnBoxPlayerRagdollTrigger), "AssignPlayer")]
    [HarmonyPostfix]
    private static void AssignPlayerPatch(PlayerController player)
    {
        if (player == null)
            return;

        if (!NetworkServer.active)
            return;

        player.StartCoroutine(FinalWakeRoutine(player));
    }
    private static IEnumerator FinalWakeRoutine(PlayerController pc)
    {
        yield return new WaitForSeconds(0.5f);

        if (pc == null)
            yield break;

        SafeInvoke(pc, "ServerLock", false);
        pc.LocalLock(false);

        SafeInvoke(pc, "ServerWakeUp");

        yield return new WaitForSeconds(0.5f);

        SafeInvoke(pc, "ServerLock", false);
        pc.LocalLock(false);

        Debug.Log("[WakeFix] Wake applied");
    }

    private static void SafeInvoke(object obj, string methodName, params object[] args)
    {
        MethodInfo method = obj.GetType().GetMethod(
            methodName,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        method?.Invoke(obj, args);
    }
}