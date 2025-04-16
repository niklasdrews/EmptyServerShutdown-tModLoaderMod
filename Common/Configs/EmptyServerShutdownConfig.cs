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

    [DefaultValue(true)]
    public bool ShutdownAfterStartup;

    [DefaultValue(300)]
    [Range(1, 18000)]
    /// <summary>
    /// Specifies the number of seconds to wait before shutting down the server
    /// when it is empty.
    /// </summary>
    public int SecondsUntilShutdown;

    [DefaultValue(LogLevel.Info)]
    public LogLevel LoggingLevel;

    [DefaultValue(LoggingChannel.Console)]
    public LoggingChannel LogChannel;

    public enum LogLevel
    {
        /// <summary>
        /// No logging.
        /// </summary>
        None = 0,

        /// <summary>
        /// Log only info.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Log debug messages and info.
        /// </summary>
        Debug = 2,
    }

    public enum LoggingChannel
    {
        /// <summary>
        /// Log to the all channels.
        /// </summary>
        All = 0,

        /// <summary>
        /// Log to the console.
        /// </summary>
        Console = 1,

        /// <summary>
        /// Log to the chat.
        /// </summary>
        Chat = 2,
    }
}
