using System.ComponentModel.Design;
using EmptyServerShutdown.Common.Services;
using Terraria.ModLoader;

namespace EmptyServerShutdown.Common.Systems;

public class EmptyServerShutdownSystem : ModSystem
{
    private bool firstTick = true;

    public override void PostUpdateTime()
    {
        // schedule shutdown if no players are online after the server started
        if (firstTick && EmptyServerShutDownService.Config.ShutdownAfterStartup)
        {
            firstTick = false;
            EmptyServerShutDownService.ScheduleShutdown();
        }
    }
}
