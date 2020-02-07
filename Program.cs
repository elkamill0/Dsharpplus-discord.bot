using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus.Net.Abstractions;
using DSharpPlus.Net.WebSocket;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;

namespace bot_discord
{
    public class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;
        static VoiceNextClient voice;
        static DiscordApplication app;
        static DiscordUser usr;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();

        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "private",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "@Kirito Senpai#3481 "
            });

            discord.MessageCreated += async e =>
            {

                if (e.Message.Content.ToLower().Contains("kurwa") ||
                    e.Message.Content.ToLower().Contains("jebać") ||
                    e.Message.Content.ToLower().Contains("huj"))
                {
                    await e.Message.RespondAsync($"{e.Author.Mention}, nie przeklinaj!");
                //    await e.Message.RespondAsync($"Abyś nauczył się dobrych manier (zanim dostaniesz bana) zostanie odebrane Ci 200 szt. Kiri-talonów");
                    
                }
            };

            discord.MessageCreated += async e =>
            {
                string reason = null;
                if (e.Message.Content.ToLower().StartsWith("!") && e.Message.Channel.Name != "kanał-do-bota-muzycznego-d")
                {
                    await e.Message.RespondAsync($"{e.Author.Mention}, to nie jest kanał do bota muzycznego!");
                    e.Message.DeleteAsync(reason);
                }
            };




            discord.MessageCreated += async e =>
            {
                string reason = null;
                if (!e.Message.Content.Contains("!") && e.Message.Channel.Name == ("kanał-do-bota-muzycznego-d"))
                {
                    CommandContext ctx = null;
                    string content = ($"Warn{e.Message.Author.Mention}, pisanie na kanale do bota muzycznego");

                    if (!e.Message.Author.IsBot)
                    {
                        e.Message.DeleteAsync(reason);
                        e.Message.RespondAsync($"Warn {e.Message.Author.Mention}, pisanie na kanale do bota muzycznego!");
                    }
                    var chn = e.Channel;
                }

            };

            discord.GuildMemberAdded += async e =>
            {
                CommandContext ctx = null;
                var vnext = ctx.Client.GetVoiceNextClient();
                var chn_name = ctx.Channel.Name;

                chn_name = "Gry #1";
                string name = "Gry #2";
                var user = ctx.Client.GetVoiceNextClient();
                //  var vstat = ctx.Member?.VoiceState;

                if ((e.Member.VoiceState.Guild.Name == chn_name) != null)
                {
                    e.Guild.CreateChannelAsync(name, type: ChannelType.Voice, parent: null, bitrate: null, user_limit: null, overwrites: null, reason: null);
                }
            };

            discord.GuildMemberAdded += async e =>
            {

                CommandContext ctx = null;
                DiscordEmbed embed;
                bool is_tts;
                string content = $"Witaj w Rosyjskim Towarzystwie.";
                await e.Member.SendMessageAsync(content, is_tts = false, embed = null);

                //  string name = e.Member.name;

                DiscordMember member = e.Member;
                DiscordRole role;
                await e.Guild.GrantRoleAsync(member, role = null, reason: null);


            };

            voice = discord.UseVoiceNext();
            commands.RegisterCommands<Commands>();
            await discord.ConnectAsync();
            await Task.Delay(-1);

        }
    }

}