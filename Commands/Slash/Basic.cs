using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialBot.Commands.Slash
{
    [SlashCommandGroup("basic", "Basic commands")]
    public sealed class Basic : ApplicationCommandModule
    {
        [SlashCommandGroup("messaging", "Messaging commands")]
        public sealed class Messaging : ApplicationCommandModule
        {
            [SlashCommand("ping", "Replies with pong!")]
            public async Task Ping(InteractionContext ctx)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Pong!"));
            }

            [SlashCommand("echo", "Echoes your input")]
            public async Task Echo(InteractionContext ctx, [Option("input", "The input to echo")] string input)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(input));
            }

            [SlashCommand("sendfile", "Sends a file.")]
            [SlashRequireUserPermissions(Permissions.AttachFiles)]
            [RequireOwner]
            public async Task SendFile(InteractionContext ctx, [Option("file", "The file to send.")] DiscordAttachment file)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Your image below!"));
                var embed = new DiscordEmbedBuilder()
                    .WithImageUrl(file.Url)
                    .WithColor(DiscordColor.Gold);
                await ctx.Channel.SendMessageAsync(embed);
            }
        }

        [SlashCommandGroup("utility", "Utility commands")]
        public sealed class Utility : ApplicationCommandModule
        {
            [SlashCommand("userinfo", "Shows information about a user.")]
            [SlashCooldown(1, 5, SlashCooldownBucketType.User)]
            public async Task UserInfo(InteractionContext ctx, [Option("user", "The user to show information about")] DiscordUser user = null)
            {
                user ??= ctx.User;
                var embed = new DiscordEmbedBuilder()
                    .WithTitle("User Info")
                    .WithDescription($"User: {user.Mention}\nID: {user.Id}\nCreated At: {user.CreationTimestamp}")
                    .WithThumbnail(user.AvatarUrl)
                    .WithColor(DiscordColor.Gold);
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
            }

            //Create command called userinfobuttons that will send an embed with buttons and sends the user info of the button clicked like username, userid, and avatar.
            [SlashCommand("userinfobuttons", "Shows information about a user with buttons.")]
            public async Task UserInfoButtons(InteractionContext ctx, [Option("user", "The user to show information about")] DiscordUser user = null)
            {
                user ??= ctx.User;
                var embed = new DiscordEmbedBuilder()
                    .WithTitle("User Info")
                    .WithDescription($"User: {user.Mention}\nID: {user.Id}\nCreated At: {user.CreationTimestamp}")
                    .WithThumbnail(user.AvatarUrl)
                    .WithColor(DiscordColor.Gold);
                var button = new DiscordButtonComponent(ButtonStyle.Primary, "userinfo", "User Info");
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed).AddComponents(button));
            }
        }
    }
}
