using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace EmptyServerShutdown.Common.Configs;

public class EmptyServerShutdownConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Range(1, 100)]
    [DefaultValue(1)]
    /// <summary>
    /// Specifies the interval, in seconds, at which the player count is checked.
    /// </summary>
    public int CheckPlayerCountIntervall;

    [DefaultValue(300)]
    [Range(1, 18000)]
    /// <summary>
    /// Specifies the number of seconds to wait before shutting down the server
    /// when it is empty.
    /// </summary>
    public int SecondsUntilShutdown;
}
