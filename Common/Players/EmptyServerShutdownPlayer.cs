using EmptyServerShutdown.Common.Services;
using Terraria.ModLoader;
using static EmptyServerShutdown.Common.Configs.EmptyServerShutdownConfig;

namespace EmptyServerShutdown.Common.Players;

public class EmptyServerShutdownPlayerService : ModPlayer
{
    public override void PlayerDisconnect()
    {
        // current player disconnecting is still marked as active therefore -1
        if ((EmptyServerShutDownService.ActivePlayerCount() - 1) == 0)
        {
            EmptyServerShutDownService.ScheduleShutdown();
        }
    }

    public override void PlayerConnect()
    {
        EmptyServerShutDownService.BroadcastServer(
            "Player connected. Aborting shutdown.",
            LogLevel.Info
        );
        EmptyServerShutDownService.CancelShutdown();
    }
}
