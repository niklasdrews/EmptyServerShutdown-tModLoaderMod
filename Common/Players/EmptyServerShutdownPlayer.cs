using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EmptyServerShutdown.Common.Players;

public class EmptyServerShutdownPlayer : ModPlayer
{
    public override void PlayerDisconnect()
    {
        // current player disconnecting is still marked as active therefore -1
        if ((ShutDownService.ActivePlayerCount() - 1) == 0)
        {
            ShutDownService.ScheduleShutdown();
        }
    }

    public override void PlayerConnect()
    {
        BroadcastServer("Player connected. Aborting shutdown.");
        ShutDownService.CancelShutdown();
    }

    private static void BroadcastServer(string message)
    {
        message = $"[EmptyServerShutdown] {message}";
        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), Color.Orange);
        Console.WriteLine(message);
    }
}
