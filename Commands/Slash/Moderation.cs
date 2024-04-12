using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorialBot.DataBase;

namespace TutorialBot.Commands.Slash
{
    public sealed class Moderation : ApplicationCommandModule
    {
        // Create a new slash command called "prefix" that will change the bot's prefix.
        [SlashCommand("prefix", "Changes the bot's prefix.")]
        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashRequireBotPermissions(Permissions.Administrator)]
        [SlashRequireGuild]
        [SlashCooldown(1, 5, SlashCooldownBucketType.User)]
        public async Task Prefix(InteractionContext ctx, [Option("prefix", "The new prefix for the bot.")] string prefix)
        {
            // Responds with a message that the bot's prefix has been changed.
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Prefix has been changed to {prefix}"));

            // Creates a new JSONDataHandler object.
            JSONDataHandler jsonHandler = new JSONDataHandler();

            // Gets the guild data from the JSON file.
            var data = await jsonHandler.GetAllGuildDataFromJSON(ctx.Guild.Id);

            // Checks if the guild data is null.
            if(data == null)
            {
                // Creates a new guild data object with the guild's ID and the new prefix.
                var guilddata = new JSONDataHandler.GuildData(ctx.Guild.Id, prefix);

                // Saves the guild data to a JSON file.
                await jsonHandler.SaveGuildDataToJSON(guilddata, ctx.Guild.Id);

                // Responds with an embed in the channel that the bot's prefix has been changed.
                var embed = new DiscordEmbedBuilder()
                    .WithTitle("Prefix Changed")
                    .WithDescription($"Prefix has been changed to {prefix}")
                    .WithColor(DiscordColor.Green)
                    .AddField("Guild", ctx.Guild.Name, true)
                    .AddField("Prefix", prefix, true);
                embed.Build();
                await ctx.Channel.SendMessageAsync(embed);
            }
            // If the guild data is not null, then set the custom prefix.
            else
            {
                // Sets the custom prefix for the guild.
                await jsonHandler.SetCustomPrefix(prefix, ctx.Guild.Id);

                // Responds with an embed in the channel that the bot's prefix has been changed.
                var embed = new DiscordEmbedBuilder()
                    .WithTitle("Prefix Changed")
                    .WithDescription($"Prefix has been changed to {prefix}")
                    .WithColor(DiscordColor.Green)
                    .AddField("Guild", ctx.Guild.Name, true)
                    .AddField("Prefix", prefix, true);
                embed.Build();
                await ctx.Channel.SendMessageAsync(embed);
            }
        }

        // Create a new slash command called "clear" that will clear a specified amount of messages.
        [SlashCommand("clear", "Clears a specified amount of messages.")]
        [SlashRequireUserPermissions(Permissions.ManageMessages)]
        [SlashRequireBotPermissions(Permissions.ManageMessages)]
        [SlashRequireGuild]
        [SlashCooldown(1, 5, SlashCooldownBucketType.User)]
        public async Task Clear(InteractionContext ctx, [Option("amount", "The amount of messages to clear.")] long amount)
        {
            // Responds with a message that the specified amount of messages have been cleared.
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Cleared {amount} messages"));

            // Deletes the specified amount of messages.
            if (ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.ManageMessages))
            {
                // Check if the channel is a text channel
                if (ctx.Channel.Type is ChannelType.Text)
                {
                    if(amount != 0 || amount !< 0 || amount !> 100)
                    {
                        //Get the messages in the channel.
                        var messages = await ctx.Channel.GetMessagesAsync((int)amount);
                        //Get pinned messages in the channel used in the command.
                        var pinnedMessages = await ctx.Channel.GetPinnedMessagesAsync();
                        //Delete messages older than 14 days and Skip the pinned messages.
                        var messagesToDelete = messages.Where(x => !pinnedMessages.Contains(x)).Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays < 14);
                        //Delete the messages.
                        await ctx.Channel.DeleteMessagesAsync(messagesToDelete, $"Cleared all channel messages in {ctx.Channel.Name}!");
                        //Create a new embed with the color Blue and with a title of Cleared and an embed field saying Cleared! Add an embed author with the bot's username and iconurl. And send the embed.
                        var embed = new DiscordEmbedBuilder
                        {
                            Title = "Cleared",
                            Color = DiscordColor.Blue,
                            Description = "Cleared!"
                        };
                        embed.WithAuthor(ctx.Client.CurrentUser.Username, null, ctx.Client.CurrentUser.AvatarUrl);
                        embed.Build();
                        await ctx.Channel.SendMessageAsync(embed: embed);
                    }
                    else
                    {
                        var embed = new DiscordEmbedBuilder
                        {
                            Title = "Error",
                            Color = DiscordColor.Red,
                            Description = "The amount of messages to clear must be between 1 and 100."
                        };
                        embed.Build();
                        await ctx.Channel.SendMessageAsync(embed: embed);
                    }
                }
                else
                {
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = "Error",
                        Color = DiscordColor.Red,
                        Description = "This command can only be used in text channels."
                    };
                    embed.Build();
                    await ctx.Channel.SendMessageAsync(embed: embed);
                }
            }
            else
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Color = DiscordColor.Red,
                    Description = "You do not have the required permissions to use this command."
                };
                embed.Build();
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
        }
    }
}
