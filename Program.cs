using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using TutorialBot.Commands.Prefix;
using TutorialBot.ConfigHandler;

namespace TutorialBot
{
    public sealed class Program
    {
        private static DiscordClient client { get; set; }
        private static CommandsNextExtension commands { get; set; }
        private static SlashCommandsExtension slash { get; set; }

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Setting Up The Bot!");

            var config = new JsonBotConfig();
            await config.ReadJSON();
            var clientConfiguration = new DiscordConfiguration
            {
                Intents = DiscordIntents.All,
                ReconnectIndefinitely = true,
                ShardId = 0,
                ShardCount = 1,
                AutoReconnect = true,
                TokenType = TokenType.Bot,
                Token = $"{config.DiscordBotToken}"
            };

            client = new DiscordClient(clientConfiguration);

            client.Ready += Client_Ready;
         
            var commandsConfiguration = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { $"{config.DiscordBotPrefix}" },
                EnableDms = false,
                EnableMentionPrefix = true,
                EnableDefaultHelp = false
            };

            commands = client.UseCommandsNext(commandsConfiguration);

            commands.RegisterCommands<Fun>();

            slash = client.UseSlashCommands();

            await client.ConnectAsync();
            await Task.Delay(-1);

        }

        private static async Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            Console.WriteLine("Bot is ready!");
        }
    }
}
