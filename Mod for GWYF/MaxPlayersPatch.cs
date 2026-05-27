using HarmonyLib;
using Mirror;

namespace GambleMaxPlayers
{
    [HarmonyPatch(typeof(GoOnlineClient), "CreateLobby")]
    internal static class Patch_CreateLobby
    {
        static void Prefix(GoOnlineClient __instance)
        {
            var field = AccessTools.Field(
                typeof(GoOnlineClient),
                "lobbySettings"
            );

            var settings = field.GetValue(__instance) as LobbySettings;

            if (settings != null && FixingPlguin.maxPlayers.Value)
            {
                settings.maxPlayers = 40;
            }
        }
    }

    [HarmonyPatch(typeof(NetworkServer), nameof(NetworkServer.Listen))]
    internal static class NetworkServerListenPatch
    {
        static void Prefix(ref int maxConns)
        {
            maxConns = 40;
        }
    }
}
