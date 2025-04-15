using System;
using System.Threading.Tasks;
using EmptyServerShutdown.Common.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using static EmptyServerShutdown.Common.Configs.EmptyServerShutdownConfig;

namespace EmptyServerShutdown.Common
{
    public class ShutDownService
    {
        // if shutdown is scheduled
        private static bool shutDownScheduled;

        private static EmptyServerShutdownConfig Config =>
            ModContent.GetInstance<EmptyServerShutdownConfig>();

        /// <summary>
        /// Schedules a server shutdown if the server is empty.
        /// This method starts a background task that periodically checks the player count
        /// and shuts down the server after a specified countdown if no players reconnect.
        /// </summary>
        /// <remarks>
        /// The shutdown process involves the following steps:
        /// 1. Marks the shutdown as scheduled.
        /// 2. Broadcasts a message to the server indicating the shutdown is scheduled.
        /// 3. Starts a background task to monitor the player count and countdown timer.
        /// 4. If the countdown reaches the specified time without any players reconnecting,
        ///    the server is saved and shut down.
        /// 5. If a player reconnects during the countdown, the shutdown is canceled.
        /// </remarks>
        public static void ScheduleShutdown()
        {
            if (Main.dedServ)
            {
                shutDownScheduled = true;
                BroadcastServer("Server is empty. Scheduling shut down.", CustomLogLevel.Info);

                // Start shutdown timer on a background task
                Task.Run(async () =>
                {
                    int waitedSeconds = 0;

                    // while shutdown is scheduled and countdown is running, wait for player to (re)connect
                    while (shutDownScheduled && waitedSeconds < Config.SecondsUntilShutdown)
                    {
                        // delay next check
                        await Task.Delay(Config.CheckPlayerCountIntervall * 1000);
                        waitedSeconds += Config.CheckPlayerCountIntervall;

                        BroadcastServer($"Waited {waitedSeconds} seconds.", CustomLogLevel.Debug);

                        // when the shutdown time is reached, save and shut down
                        if (waitedSeconds >= Config.SecondsUntilShutdown)
                        {
                            SaveAndShutdown();
                        }
                        // if a player connected, cancel shutdown
                        else if (!shutDownScheduled)
                        {
                            return;
                        }
                    }
                });
            }
        }

        public static void CancelShutdown()
        {
            BroadcastServer($"Player (re)connected. Canceling shutdown.", CustomLogLevel.Info);
            shutDownScheduled = false;
        }

        /// <summary>
        /// Calculates the number of active players currently in the game.
        /// </summary>
        /// <returns>
        /// The total count of active players.
        /// </returns>
        public static int ActivePlayerCount()
        {
            int activePlayers = 0;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active)
                {
                    activePlayers++;
                }
            }

            return activePlayers;
        }

        /// <summary>
        /// Saves the current game world and initiates a server shutdown process.
        /// </summary>
        /// <remarks>
        /// This method broadcasts a message to the server indicating that the world is being saved,
        /// saves the world, and then broadcasts another message indicating that the server is shutting down.
        /// It sets <see cref="Main.autoShutdown"/> to true to trigger the shutdown process.
        ///
        /// Additionally, a fallback mechanism is implemented using a delayed task to forcefully exit
        /// the application if the server does not shut down within a short period.
        /// </remarks>
        private static void SaveAndShutdown()
        {
            BroadcastServer("Saving world...", CustomLogLevel.Info);
            WorldFile.SaveWorld(true);
            BroadcastServer("World saved. Shutting down.", CustomLogLevel.Info);

            Main.autoShutdown = true;

            // Fallback force shutdown after short delay
            Task.Run(async () =>
            {
                await Task.Delay(3000); // Give it a moment to process shutdown
                BroadcastServer("Server did not shut down. Forcing exit.", CustomLogLevel.Debug);
                Environment.Exit(0); // Force exit if nothing happens
            });
        }

        /// <summary>
        /// Broadcasts a message to all players on the server and logs it to the console.
        /// </summary>
        /// <param name="message">The message to broadcast. It will be prefixed with "[EmptyServerShutdown]".</param>
        public static void BroadcastServer(
            string message,
            CustomLogLevel logLevel = CustomLogLevel.None
        )
        {
            if (Config.LoggingLevel >= logLevel)
            {
                message = $"[EmptyServerShutdown] {message}";
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), Color.Orange);
                Console.WriteLine(message);
            }
        }
    }
}
