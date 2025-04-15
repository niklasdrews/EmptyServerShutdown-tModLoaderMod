using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace EmptyServerShutdown.Common.Configs;

public class EmptyServerShutdownConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Range(1, 100)]
    [DefaultValue(1)]
    public int CheckPlayerCountIntervall;

    [DefaultValue(300)]
    [Range(1, 18000)]
    public int SecondsUntilShutdown;
}
