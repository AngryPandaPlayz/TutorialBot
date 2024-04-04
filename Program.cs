using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using TutorialBot.Commands.Prefix;
using TutorialBot.Commands.Slash;
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
            client.ComponentInteractionCreated += Client_Buttom_Interaction_Created;
         
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

            slash.RegisterCommands<Basic.Messaging>();
            slash.RegisterCommands<Basic.Utility>();

            client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            await client.ConnectAsync();
            await Task.Delay(-1);

        }

        private static async Task Client_Buttom_Interaction_Created(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            switch(args.Interaction.Data.CustomId)
            {
                case "userinfo":
                    var user = args.Interaction.User;
                    var embed = new DiscordEmbedBuilder()
                        .WithTitle("User Info")
                        .WithDescription($"User: {user.Mention}\nID: {user.Id}\nCreated At: {user.CreationTimestamp}")
                        .WithThumbnail(user.AvatarUrl)
                        .WithColor(DiscordColor.Gold)
                        .AddField("User", user.Mention, true)
                        .AddField("ID", user.Id.ToString(), true)
                        .AddField("Created At", user.CreationTimestamp.ToString(), true);
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(embed));
                    break;
            }
        }

        private static async Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            Console.WriteLine("Bot is ready!");
        }
    }
}
