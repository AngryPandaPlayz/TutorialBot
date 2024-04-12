using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using TutorialBot.DataBase;

namespace TutorialBot.Commands.Slash
{
    public sealed class PrivateVCCommands : ApplicationCommandModule
    {
        // Command to create a private category and voice channel and custom role with random DiscordColor and for all of them the name as $"{ctx.Member.Username}'s Private VC".
        [SlashCommand("createprivatevc", "Creates a private voice channel for the user.")]
        [SlashRequireGuild]
        [SlashRequireUserPermissions(Permissions.UseApplicationCommands)]
        [SlashRequireBotPermissions(Permissions.Administrator)]
        [SlashCooldown(1, 5, SlashCooldownBucketType.User)]
        public async Task CreatePrivateVC(InteractionContext ctx)
        {
            // Respond with a message that the private voice channel is being created.
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Creating Private Voice Channel!"));
            // Check if the user is in a voice channel.
            if (ctx.Member.VoiceState == null)
            {
                // Respond with a message that the user is not in a voice channel.
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You are not in a voice channel!"));
                return;
            }
            else
            {
               //If category or channel or role name matching the vc name exists send a embed with color red saying that the category, channel, and role for your private vc already exists.
               if(ctx.Guild.Channels.Values.Any(x => x.Name == $"{ctx.Member.Username}'s Private VC") || ctx.Guild.Roles.Values.Any(x => x.Name == $"{ctx.Member.Username}'s Private VC"))
                {
                   var embed = new DiscordEmbedBuilder
                   {
                       Title = "Error",
                       Description = "Category, channel, and role for your private vc already exists!",
                       Color = DiscordColor.Red
                   };
                   await ctx.Channel.SendMessageAsync(embed);
                   return;
               }
                //If category or channel or role name matching the vc name does not exist create a category, channel, and role for your private vc.
                else
                {
                    // Create a new JSONDataHandler.
                    JSONDataHandler jsonHandler = new JSONDataHandler();


                    // Create a new role with the name as the user's username and a color.
                    var role = await ctx.Guild.CreateRoleAsync($"{ctx.Member.Username}'s Private VC", null, DiscordColor.Green, false, false, "Private VC Role", null, null);

                    //Create new channel overwrites for role.
                    var overwrites = new List<DiscordOverwriteBuilder>
                        {
                            new DiscordOverwriteBuilder().For(ctx.Guild.EveryoneRole).Allow(Permissions.AccessChannels),
                            new DiscordOverwriteBuilder().For(role).Allow(Permissions.All).Deny(Permissions.None)
                        };

                    // Create a new category with the name as the user's username and the role above.
                    var category = await ctx.Guild.CreateChannelCategoryAsync($"{ctx.Member.Username}'s Private VC", overwrites, null, "Private VC Category");

                    // Create a new voice channel with the name as the user's username and the category above.
                    var voiceChannel = await ctx.Guild.CreateVoiceChannelAsync($"{ctx.Member.Username}'s Private VC", null, null, 4, overwrites, null, null, "Private VC Voice Channel");

                    //Modify the channel to be under the category.
                    await voiceChannel.ModifyAsync(x => x.Parent = category);

                    // Create a new PrivateVCUserData with the user's ID, the guild's ID, the category's ID, the voice channel's ID, and the role's ID.
                    var privateVCUserData = new JSONDataHandler.PrivateVCUserData(ctx.Member.Id, ctx.Guild.Id, category.Id, voiceChannel.Id, role.Id);

                    // Save the PrivateVCUserData to a JSON file.
                    await jsonHandler.SavePrivateVCUserDataToJSON(privateVCUserData, ctx.Member.Id);

                    // Create a new CategoryData with the category's ID and the guild's ID.
                    var categoryData = new JSONDataHandler.CategoryData(category.Id, ctx.Guild.Id);

                    // Save the CategoryData to a JSON file.
                    await jsonHandler.SaveCategoryDataToJSON(categoryData, category.Id);

                    // Create a new ChannelData with the voice channel's ID and the guild's ID.
                    var channelData = new JSONDataHandler.ChannelData(voiceChannel.Id, ctx.Guild.Id);

                    // Save the ChannelData to a JSON file.
                    await jsonHandler.SaveChannelDataToJSON(channelData, voiceChannel.Id);

                    // Create a new RoleData with the role's ID and the guild's ID.
                    var roleData = new JSONDataHandler.RoleData(role.Id, ctx.Guild.Id);

                    // Save the RoleData to a JSON file.
                    await jsonHandler.SaveRoleDataToJSON(roleData, role.Id);

                    // Add the user to the role.
                    await ctx.Member.GrantRoleAsync(role);

                    // Respond with a message that the private voice channel has been created.
                    await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Private Voice Channel Created!"));

                    // Respond with a message that the user has been added to the role.
                    await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("You have been added to the role!"));

                    // Respond with a message that the user has been added to the voice channel.
                    await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("You have been added to the voice channel!"));

                    if (ctx.Member.VoiceState.Channel != null)
                    {
                        // Move the user to the voice channel.
                        await ctx.Member.PlaceInAsync(voiceChannel);
                    }
                    else
                    {
                        // Respond with a message that the user is not in a voice channel.
                        await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("You are not in a voice channel! I couldn't bring you to your private voice channel!"));
                    }
                }
            }

        }

        // Command to add a user to a private voice channel.
        [SlashCommand("addusertoprivatevc", "Adds a user to a private voice channel.")]
        [SlashRequireGuild]
        [SlashRequireUserPermissions(Permissions.UseApplicationCommands)]
        [SlashRequireBotPermissions(Permissions.Administrator)]
        [SlashCooldown(1, 5, SlashCooldownBucketType.User)]
        public async Task AddUserToPrivateVC(InteractionContext ctx, [Option("user", "The user to add to the private voice channel.")] DiscordUser user)
        {
            // Respond with a message that the user is being added to the private voice channel.
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Adding User to Private Voice Channel!"));
            // Create a new JSONDataHandler.
            JSONDataHandler jsonHandler = new JSONDataHandler();

            // Get the private VC user data.
            var privateVCUserData = await jsonHandler.GetAllPrivateVcUserDataFromJSON(ctx.Member.Id);

            if (privateVCUserData != null)
            {
                // Get the category, voice channel, and role.
                var category = ctx.Guild.GetChannel(privateVCUserData.CategoryID);
                var voiceChannel = ctx.Guild.GetChannel(privateVCUserData.ChannelID);
                var role = ctx.Guild.GetRole(privateVCUserData.RoleID);

                var member = await ctx.Guild.GetMemberAsync(user.Id);

                // Add the user to the role.
                await member.GrantRoleAsync(role);

                // Respond with a message that the user has been added to the role.
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("User has been added to the role!"));

                // Respond with a message that the user has been added to the voice channel.
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("User has been added to the voice channel!"));

                if(member.VoiceState.Channel != null)
                {
                    // Move the user to the voice channel.
                    await member.PlaceInAsync(voiceChannel);
                }
                else
                {
                    // Respond with a message that the user is not in a voice channel.
                    await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("User is not in a voice channel! I couldn't bring the user to the private voice channel!"));
                }
            }
            else
            {
                // Respond with a message that the user does not have a private voice channel.
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = "You do not have a private voice channel!",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed);
            }
        }

        // Command to transfer the ownership of a private voice channel to another user.
        [SlashCommand("transferownership", "Transfers the ownership of a private voice channel to another user.")]
        [SlashRequireGuild]
        [SlashRequireUserPermissions(Permissions.UseApplicationCommands)]
        [SlashRequireBotPermissions(Permissions.Administrator)]
        [SlashCooldown(1, 5, SlashCooldownBucketType.User)]
        public async Task TransferOwnership(InteractionContext ctx, [Option("user", "The user to transfer the ownership to.")] DiscordUser user)
        {
            // Respond with a message that the ownership is being transferred.
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Transferring Ownership!"));
            // Create a new JSONDataHandler.
            JSONDataHandler jsonHandler = new JSONDataHandler();

            // Get the private VC user data.
            var privateVCUserData = await jsonHandler.GetAllPrivateVcUserDataFromJSON(ctx.Member.Id);

            if (privateVCUserData != null)
            {
                // Get the category, voice channel, and role.
                var category = ctx.Guild.GetChannel(privateVCUserData.CategoryID);
                var voiceChannel = ctx.Guild.GetChannel(privateVCUserData.ChannelID);
                var role = ctx.Guild.GetRole(privateVCUserData.RoleID);

                var member = await ctx.Guild.GetMemberAsync(privateVCUserData.ID);

                // Remove the user from the role.
                await ctx.Member.RevokeRoleAsync(role);

                // Respond with a message that the user has been removed from the role.
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"{member.Mention} You have been removed from the role!"));

                // Respond with a message that the user has been removed from the voice channel.
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"{member.Mention} You have been removed from the voice channel!"));

                // Let the new owner know that they have been added as the new owner of the private voice channel and it's category and role.
                var newOwner = await ctx.Guild.GetMemberAsync(user.Id);
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"{user.Mention} You have been added as the new owner of {ctx.Member.Username}'s Private VC!"));

                // Modify the role, category, and voice channel to have the new owner's username.
                await role.ModifyAsync(x => x.Name = $"{user.Username}'s Private VC");
                await category.ModifyAsync(x => x.Name = $"{user.Username}'s Private VC");
                await voiceChannel.ModifyAsync(x => x.Name = $"{user.Username}'s Private VC");
                // Create a new PrivateVCUserData with the new owner's ID, the guild's ID, the category's ID, the voice channel's ID, and the role's ID.
                var newPrivateVCUserData = new JSONDataHandler.PrivateVCUserData(user.Id, ctx.Guild.Id, category.Id, voiceChannel.Id, role.Id);
                // Save the PrivateVCUserData to a JSON file.
                await jsonHandler.SavePrivateVCUserDataToJSON(newPrivateVCUserData, user.Id);
                // Delete the old PrivateVCUserData of the previous owner.
                await jsonHandler.DeletePrivateVCUserData(member.Id);
            }
        }

        // Command to delete a private voice channel.
        [SlashCommand("deleteprivatevc", "Deletes a private voice channel.")]
        [SlashRequireGuild]
        [SlashRequireUserPermissions(Permissions.UseApplicationCommands)]
        [SlashRequireBotPermissions(Permissions.Administrator)]
        [SlashCooldown(1, 5, SlashCooldownBucketType.User)]
        public async Task DeletePrivateVC(InteractionContext ctx)
        {
            // Respond with a message that the private voice channel is being deleted.
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Deleting Private Voice Channel!"));
            // Create a new JSONDataHandler.
            JSONDataHandler jsonHandler = new JSONDataHandler();

            // Get the private VC user data.
            var privateVCUserData = await jsonHandler.GetAllPrivateVcUserDataFromJSON(ctx.Member.Id);

            if (privateVCUserData != null)
            {
                // Get the category, voice channel, and role.
                var category = ctx.Guild.GetChannel(privateVCUserData.CategoryID);
                var voiceChannel = ctx.Guild.GetChannel(privateVCUserData.ChannelID);
                var role = ctx.Guild.GetRole(privateVCUserData.RoleID);

                // Delete the category, voice channel, and role.
                await category.DeleteAsync();
                await voiceChannel.DeleteAsync();
                await role.DeleteAsync();

                // Respond with a message that the private voice channel has been deleted.
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Private Voice Channel Deleted!"));
                // Delete the CategoryData of the category.
                await jsonHandler.DeleteCategoryData(privateVCUserData.CategoryID);
                // Delete the ChannelData of the voice channel.
                await jsonHandler.DeleteChannelData(privateVCUserData.ChannelID);
                // Delete the RoleData of the role.
                await jsonHandler.DeleteRoleData(privateVCUserData.RoleID);
                // Delete the PrivateVCUserData of the user.
                await jsonHandler.DeletePrivateVCUserData(ctx.Member.Id);
            }
            else
            {
                // Respond with a message that the user does not have a private voice channel.
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = "You do not have a private voice channel!",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed);
            }
        }
    }
}
