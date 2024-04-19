using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.VoiceNext;
using TutorialBot.Commands.Prefix;
using TutorialBot.Commands.Slash;
using TutorialBot.ConfigHandler;
using TutorialBot.DataBase;

namespace TutorialBot
{
    public sealed class Program
    {
        private static DiscordClient client { get; set; }
        private static CommandsNextExtension commands { get; set; }
        private static SlashCommandsExtension slash { get; set; }

        private static VoiceNextExtension voice { get; set; }


        public static async Task Main(string[] args)
        {
            Console.WriteLine("Setting Up The Bot!");
            var jsonhandler = new JSONDataHandler();
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
                EnableDms = true,
                EnableMentionPrefix = true,
                EnableDefaultHelp = false,
                PrefixResolver = async (msg) =>
                {
                    var guildId = msg.Channel.GuildId;
                    var jsonHandler = new JSONDataHandler();
                    var customprefixdata = await jsonHandler.GetAllGuildDataFromJSON((ulong)guildId);
                    if (customprefixdata.Prefix != null)
                    {
                        return msg.GetStringPrefixLength(customprefixdata.Prefix);
                    }
                    else
                    {
                        return msg.GetStringPrefixLength(config.DiscordBotPrefix);
                    }
                }
            };

            commands = client.UseCommandsNext(commandsConfiguration);
            commands.RegisterCommands<Fun>();

            slash = client.UseSlashCommands();

            slash.RegisterCommands<Basic.Messaging>();
            slash.RegisterCommands<Basic.Utility>();
            slash.RegisterCommands<Moderation>();
            slash.RegisterCommands<PrivateVCCommands>();
            slash.RegisterCommands<AudioCommands>();

            client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            var voiceConfiguration = new VoiceNextConfiguration
            {
                AudioFormat = AudioFormat.Default,
                EnableIncoming = false,
                PacketQueueSize = 50,
            };

            voice = client.UseVoiceNext(voiceConfiguration);

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

        public static async Task CreatePaths()
        {
            JSONDataHandler jsonHandler = new JSONDataHandler();
            var vc = jsonHandler.GetTypes(JSONDataHandler.DATA_TYPES.USERVC);
            var guild = jsonHandler.GetTypes(JSONDataHandler.DATA_TYPES.GUILD);
            var category = jsonHandler.GetTypes(JSONDataHandler.DATA_TYPES.CATEGORY);
            var channel = jsonHandler.GetTypes(JSONDataHandler.DATA_TYPES.CHANNEL);
            var role = jsonHandler.GetTypes(JSONDataHandler.DATA_TYPES.ROLE);
            var message = jsonHandler.GetTypes(JSONDataHandler.DATA_TYPES.MESSAGE);
            var vccategory = jsonHandler.GetCategories(JSONDataHandler.DATA_CATEGORIES.PrivateVCUserData);
            var guildcategory = jsonHandler.GetCategories(JSONDataHandler.DATA_CATEGORIES.GuildData);
            var categorycategory = jsonHandler.GetCategories(JSONDataHandler.DATA_CATEGORIES.CategoryData);
            var channelcategory = jsonHandler.GetCategories(JSONDataHandler.DATA_CATEGORIES.ChannelData);
            var rolecategory = jsonHandler.GetCategories(JSONDataHandler.DATA_CATEGORIES.RoleData);
            var messagecategory = jsonHandler.GetCategories(JSONDataHandler.DATA_CATEGORIES.MessageData);
            jsonHandler.CreatePathIfNotExists(vc, vccategory);
            await Task.Delay(TimeSpan.FromSeconds(3));
            jsonHandler.CreatePathIfNotExists(guild, guildcategory);
            await Task.Delay(TimeSpan.FromSeconds(3));
            jsonHandler.CreatePathIfNotExists(category, categorycategory);
            await Task.Delay(TimeSpan.FromSeconds(3));
            jsonHandler.CreatePathIfNotExists(channel, channelcategory);
            await Task.Delay(TimeSpan.FromSeconds(3));
            jsonHandler.CreatePathIfNotExists(role, rolecategory);
            await Task.Delay(TimeSpan.FromSeconds(3));
            jsonHandler.CreatePathIfNotExists(message, messagecategory);
            await Task.CompletedTask;
        }

        private static async Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            Console.WriteLine("Bot is ready!");
            await CreatePaths();
            
        }
    }
}
