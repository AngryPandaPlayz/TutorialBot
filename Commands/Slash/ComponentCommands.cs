using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace TutorialBot.Commands.Slash
{
    public sealed class ComponentCommands : ApplicationCommandModule
    {
        // Create a new slash command with the name "button" and description "Button command" with a button component with the label "Click me!" and another button component with the label "Click me too!"
        [SlashCommand("button", "Button command")]
        [SlashRequireGuild]
        [SlashCooldown(5, 100, SlashCooldownBucketType.User)]
        public async Task ButtonCommand(InteractionContext ctx)
        {
            await ctx.Interaction.DeferAsync();
            var builder = new DiscordButtonComponent(ButtonStyle.Primary, "button1", "Click me!");
            var builder2 = new DiscordButtonComponent(ButtonStyle.Secondary, "button2", "Click me too!");
                                                                     

            await ctx.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().WithContent("Click a button!").AddComponents(builder, builder2));
        }

        // Create a new slash command with the name "select" and description "Select command" with a select menu component with the options "Option 1" and "Option 2"
        [SlashCommand("select", "Select command")]
        [SlashRequireGuild]
        [SlashCooldown(5, 100, SlashCooldownBucketType.User)]
        public async Task SelectCommand(InteractionContext ctx)
        {
            await ctx.Interaction.DeferAsync();
            var builder = new DiscordSelectComponent("select1", "Select an option", new[]
                                                  {
                                       new DiscordSelectComponentOption("Option 1", "option1"),
                                       new DiscordSelectComponentOption("Option 2", "option2")
                                   });
            await ctx.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().WithContent("Select an option!").AddComponents(builder));
        }

        //create a new slash command with the name "userdropdown" and description "User dropdown command" with a user dropdown component
        [SlashCommand("userdropdown", "User dropdown command")]
        [SlashRequireGuild]
        [SlashCooldown(5, 100, SlashCooldownBucketType.User)]
        public async Task UserDropdownCommand(InteractionContext ctx)
        {
            await ctx.Interaction.DeferAsync();
            var builder = new DiscordUserSelectComponent("userdropdown1", "Select a user");
            await ctx.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().WithContent("Select a User!").AddComponents(builder));
        }
    }
}
