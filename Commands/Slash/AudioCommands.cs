using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.VoiceNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialBot.Commands.Slash
{
    public sealed class AudioCommands : ApplicationCommandModule
    {
        // Slash command to join a voice channel
        [SlashCommand("join", "Makes the bot join your voice channel.")]
        public async Task Join(InteractionContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc != null)
                throw new InvalidOperationException("Already connected in this guild.");

            var chn = ctx.Member?.VoiceState?.Channel;
            if (chn == null)
                throw new InvalidOperationException("You need to be in a voice channel.");

            vnc = await vnext.ConnectAsync(chn);
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("👌"));
        }

        // Slash command to leave a voice channel
        [SlashCommand("leave", "Makes the bot leave the voice channel.")]
        public async Task Leave(InteractionContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc == null)
                throw new InvalidOperationException("Not connected in this guild.");

            vnc.Disconnect();
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("👌"));
        }

        // Slash command to play a sound
        [SlashCommand("play", "Plays a sound.")]
        public async Task Play(InteractionContext ctx, [Choice("Smack", "Smack")][Option("sound", "The sound to play.")] string file)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Playing sound."));
            var bot = ctx.Guild.Members[ctx.Client.CurrentUser.Id];
            if(bot.VoiceState != null)
            {
                if (file == "Smack")
                {
                    var vnext = ctx.Client.GetVoiceNext();
                    var connection = vnext.GetConnection(ctx.Guild);
                    var transmit = connection.GetTransmitSink();
                    //Get the file from the path as a pulse code modulation (PCM) stream
                    var pcm = new FileStream($"{AppDomain.CurrentDomain.BaseDirectory}\\Sounds\\Smack-No!.wav", FileMode.Open, FileAccess.Read);
                    await pcm.CopyToAsync(transmit);
                    await pcm.DisposeAsync();
                }
            }
            else
            {
                await ctx.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().WithContent("I'm not in a voice channel."));
            }
        }
    }
}
