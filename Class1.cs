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
    
    public class Commands
    {
        static public string user_money;

        [Command("join"), Description("Joins a voice channel.")]
        public async Task Join(CommandContext ctx, DiscordChannel chn = null)
        {
            var vnext = ctx.Client.GetVoiceNextClient();
            if (vnext == null)
            {
                await ctx.RespondAsync("VNext is not enabled or configured.");
                return;
            }

            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc != null)
            {
                vnc = await vnext.ConnectAsync(chn);
                await ctx.RespondAsync($"Already connected in this guild: `{chn.Name}`");
                return;
            }

            var vstat = ctx.Member?.VoiceState;
            if (vstat?.Channel == null && chn == null)
            {
                await ctx.RespondAsync("You are not in a voice channel.");
                return;
            }

            if (chn == null)
                chn = vstat.Channel;

            vnc = await vnext.ConnectAsync(chn);
            await ctx.RespondAsync($"Connected to `{chn.Name}`");
        }

        [Command("leave")]
        public async Task Leave(CommandContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNextClient();

            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc == null)
                throw new InvalidOperationException("Not connected in this guild.");

            vnc.Disconnect();
            await ctx.RespondAsync("👌");
        }

        [Command("kanał_nazwa")]
        public async Task Name_channel(CommandContext ctx, string n1, string n2 = null, string n3 = null, string n4 = null)
        {
            var chn = ctx.Member?.VoiceState;
            if(chn?.Channel == null)
            {
                ctx.RespondAsync("Nie znajdujesz się na kanale głosowym (ctx.Member.Voicestate.Channel != null)");
            }
            string name = ($"{n1} {n2} {n3} {n4}");
            chn.Channel.ModifyAsync(name);
            ctx.RespondAsync($"Nazwa kanału '{ctx.Member.VoiceState.Channel.Name}' została pomyślnie zmieniona na '{name}'");


        }

        [Command("zbanuj")]
        public async Task BanAsync(CommandContext ctx, DiscordMember member, int delete_message_days = 0, string reason = null)
        {
        //    ctx.Guild.BanMemberAsync(member, delete_message_days, reason);
        //    await ctx.RespondAsync($"{member} pomyślnie zbanowany");
        }

        [Command("postac")]
        public async Task Postac(CommandContext ctx, int row)
        {
            var rnd = new Random();
            var rnd1 = new Random();
            await ctx.RespondAsync($"🎲 Rząd: {rnd.Next(0, row)}");
            await ctx.RespondAsync($"🎲 Postać: {rnd1.Next(0, 7)}");
        }

        [Command("miotaj")]
        public async Task move(CommandContext ctx, DiscordMember member, DiscordChannel chn, DiscordChannel chn1, int a)
        {
            for (int i = 0; i < a; i++)
            {
                member.PlaceInAsync(chn);
                member.PlaceInAsync(chn1);
            }
            ctx.RespondAsync($"{ctx.Member.Mention}, Miotanie zakończone");
        }

        [Command("role")]
        public async Task rola(CommandContext ctx, string name, Permissions? permissions,
            DiscordColor? color, bool? hoist = null, bool? mentionable = null,
            string reason = null)
        {
            ctx.Guild.CreateRoleAsync(name, permissions, color, hoist, mentionable, reason);
        }

        [Command("wakeup!")]
        public async Task wakeup(CommandContext ctx, DiscordMember member, int times = 1)
        {
            for (int i = 0; i < times; i++)
                ctx.RespondAsync($"{member.Mention}");
        }

        [Command("ban")]
        public async Task Ban(CommandContext ctx)
        {
            Random rng = new Random();
            int random = rng.Next(0, 2);
            if (random == 0)
                await ctx.RespondAsync($"{ctx.User.Mention} czego");
            else if (random == 1)
                await ctx.RespondAsync($"{ctx.User.Mention} dostajesz bana, ty kurwo jebana!");
        }

        [Command("hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"Witaj Towarzyszu {ctx.User.Mention}");
        }

        [Command("random")]
        public async Task Random(CommandContext ctx, int min, int max)
        {
            var rnd = new Random();
            await ctx.RespondAsync($"🎲 Wylosowana liczba to: {rnd.Next(min, max)}");
        }

        [Command("rola")]
        public async Task Role(CommandContext ctx, DiscordRole role = null)
        {
            string name = "Prawa ręka boga";
            DiscordMember member = ctx.Member;
            await ctx.Guild.GrantRoleAsync(member, role, reason: null);
        }

        [Command("hajs")]
        public async Task Money(CommandContext ctx)
        {
            user_money = ctx.Member.Username;
            Talony.StreamReader();
            await ctx.RespondAsync($"Stan konta {user_money} wynosi: {Talony.i_money}");
        }

        [Command("dodaj_hajs")]
        public async Task AddMoney(CommandContext ctx, int money)
        {
            user_money = ctx.Member.Username;
            Talony.StreamReader();
            Talony.i_money = Talony.i_money + money;
            Talony.StreamWriter();
            await ctx.RespondAsync($"Dodano {money} dla użytkownika {user_money}, {Talony.i_money} ");
        }
    }
}
