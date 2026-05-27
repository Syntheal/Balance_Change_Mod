using HarmonyLib;

[HarmonyPatch(typeof(GameSettings), "GetQuotaExcessReward")]
class Patch_QuotaReward
{
    static bool Prefix(int floor, long quota, long money, ref int __result)
    {
        if (!FixingPlguin.ticketSetting.Value)
            return true;

        if (quota <= 0 || money <= quota)
        {
            __result = 0;
            return false;
        }

        long percent = (money * 100) / quota;

        int tickets = 0;

        if (percent <= 200)
            tickets = (int)((percent - 100) / 20);

        else if (percent <= 300)
            tickets = 5 + (int)((percent - 200) / 20);

        else if (percent <= 450)
            tickets = 10 + (int)((percent - 300) / 30);

        else if (percent <= 600)
            tickets = 15 + (int)((percent - 450) / 30);

        else if (percent <= 750)
            tickets = 20 + (int)((percent - 600) / 30);

        else if (percent <= 850)
            tickets = 25 + (int)((percent - 750) / 30);

        else if (percent <= 1000)
            tickets = 30 + (int)((percent - 850) / 30);

        else if (percent <= 1250)
            tickets = 35 + (int)((percent - 1000) / 50);

        else if (percent <= 1500)
            tickets = 40 + (int)((percent - 1250) / 50);

        else
            tickets = 45 + (int)((percent - 1500) / 100);

        if (tickets > 50)
            tickets = 50;

        __result = tickets;
        return false;
    }
}