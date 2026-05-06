using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(GoOnlineClient), "CreateLobby")]
class Patch_CreateLobby
{
    static void Prefix(GoOnlineClient __instance)
    {
        var field = AccessTools.Field(typeof(GoOnlineClient), "lobbySettings");
        var settings = field.GetValue(__instance) as LobbySettings;

        if (settings != null)
        {
            settings.maxPlayers = UnityEngine.Random.Range(6, 100000);
        }
    }
}