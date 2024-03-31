using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialBot.Commands.Prefix
{
    public sealed class Fun : BaseCommandModule
    {
        [Command("ping")]
        public async Task Ping(CommandContext ctx, string? type = null)
        {
            //Set type to null if the type is empty.
            if(type == "")
            {
                type = null;
            }
            //If the type is null, then respond with error embed.
            if(type == null)
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = "Invalid"
                };
                await ctx.RespondAsync(embed: embed);
            }
            
            else if(type != null)
            {
                if (type == "Text")
                {
                    // Responds with "Pong!" when the command is called and shows the bot's latency.
                    await ctx.RespondAsync($"Pong! {ctx.Client.Ping}ms");
                }
                else if (type == "Embed")
                {
                    // Responds with an embed when the command is called and shows the bot's latency.
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = "Pong!",
                        Description = $"{ctx.Client.Ping}ms",
                        Color = DiscordColor.Green
                    };
                    await ctx.RespondAsync(embed: embed);
                }
                else
                {
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = "Error",
                        Description = "Invalid"
                    };
                    await ctx.RespondAsync(embed: embed);
                }
            }
        }

        [Command("say")]
        public async Task Say(CommandContext ctx, string message)
        {
            // Responds with the message that the user provided.
            await ctx.RespondAsync(message);
        }

        [Command("randomnumber")]
        public async Task RandomNumber(CommandContext ctx, int min, int max)
        {
            // Responds with a random number between the min and max values that the user provided.
            Random random = new Random();
            int randomNumber = random.Next(min, max);
            await ctx.RespondAsync($"Random Number: {randomNumber}");
        }

        [Command("rps")]
        public async Task RPS(CommandContext ctx, string choice)
        {
            // Responds with the result of a Rock, Paper, Scissors game.
            string[] choices = new string[] { "Rock", "Paper", "Scissors" };
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            string botChoice = choices[randomNumber];
            string result = "";
            //Make the rps results more accurate and correct.
            if (choice == "Rock" && botChoice == "Rock")
            {
                result = "It's a tie!";
            }
            else if (choice == "Rock" && botChoice == "Paper")
            {
                result = "You lose!";
            }
            else if (choice == "Rock" && botChoice == "Scissors")
            {
                result = "You win!";
            }
            else if (choice == "Paper" && botChoice == "Rock")
            {
                result = "You win!";
            }
            else if (choice == "Paper" && botChoice == "Paper")
            {
                result = "It's a tie!";
            }
            else if (choice == "Paper" && botChoice == "Scissors")
            {
                result = "You lose!";
            }
            else if (choice == "Scissors" && botChoice == "Rock")
            {
                result = "You lose!";
            }
            else if (choice == "Scissors" && botChoice == "Paper")
            {
                result = "You win!";
            }
            else if (choice == "Scissors" && botChoice == "Scissors")
            {
                result = "It's a tie!";
            }
            await ctx.RespondAsync($"You chose: {choice}\nBot chose: {botChoice}\nResult: {result}");
        }

        [Command("coinflip")]
        public async Task CoinFlip(CommandContext ctx)
        {
            // Responds with the result of a coin flip.
            Random random = new Random();
            int randomNumber = random.Next(0, 2);
            string result = "";
            if(randomNumber == 0)
            {
                result = "Heads";
            }else
            {
                result = "Tails";
            }
            await ctx.RespondAsync($"Coin Flip Result: {result}");
        }

        [Command("8ball")]
        public async Task EightBall(CommandContext ctx, [RemainingText]string question)
        {
            // Responds with a random answer to the question that the user provided.
            string[] answers = new string[] { "Yes", "No", "Maybe", "Ask again later", "Most likely", "I don't know", "I'm not sure", "It's uncertain", "It's unlikely", "It's likely" };
            Random random = new Random();
            int randomNumber = random.Next(0, answers.Length);
            string answer = answers[randomNumber];
            await ctx.RespondAsync($"Question: {question}\nAnswer: {answer}");
        }

        [Command("diceroll")]
        public async Task DiceRoll(CommandContext ctx, int sides)
        {
            // Responds with the result of a dice roll with the number of sides that the user provided.
            Random random = new Random();
            int diceRoll = random.Next(1, sides + 1);
            await ctx.RespondAsync($"Dice Roll Result: {diceRoll}");
        }

        //Create a command that gets a dad joke from a website api and sends it as an embed.
        [Command("dadjoke")]
        public async Task DadJoke(CommandContext ctx)
        {
            // Responds with a dad joke from a website api.
            //Create a new http client called client
            var client = new HttpClient();
            //Create a new string called response and get the response from the official-joke-api.appspot.com API
            string response = await client.GetStringAsync("https://official-joke-api.appspot.com/jokes/programming/random");
            //Create a new json object called joke and parse the response from the official-joke-api.appspot.com API
            dynamic joke = JsonConvert.DeserializeObject(response);
            //Create a new embed builder called embed and add a title, description, and color to the embed and add a field to the embed with the name "Joke" and the value of the random joke that was received from the official-joke-api.appspot.com API and send the embed to the channel
            var embed = new DiscordEmbedBuilder
            {
                Title = "Joke",
                Description = "Get a random joke.",
                Color = new DiscordColor(0x00ff00)
            };
            embed.AddField("Joke", $"{joke[0].setup}\n{joke[0].punchline}");
            await ctx.RespondAsync(embed: embed);
        }
    }
}
