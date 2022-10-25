using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordNetBotTemplate.Services;
using SecretBot.DataAccess.Models;
using SecretBot.LeagueAPI.DTO;

namespace DiscordNetBotTemplate.Modules;

public class LolModule : ModuleBase<SocketCommandContext>
{
    private readonly LolService _lolService;

    public LolModule(LolService lolService)
    {
        _lolService = lolService;
    }

    [Command("subscribe")]
    [Alias("sub")]
    [Summary("Subscribe your discord account to LOL bot")]
    public async Task SubscribeAsync([Remainder]string summonerName)
    {
        if (string.IsNullOrWhiteSpace(summonerName))
        {
            await ReplyAsync("Username is empty");

            return;
        }

        if (await _lolService.UserExistInDbAsync(Context.User.Username))
        {
            await ReplyAsync("You are already subscribed");

            return;
        }

        SummonerDto summoner = null;
        try
        {
            summoner = await _lolService.GetSummonerByName(summonerName);
        }
        catch (Exception e)
        {
            await ReplyAsync($"Summoner with name {summonerName} does not exist");
            return;
        }

        await _lolService.AddUserAsync(new LolUser()
        {
            DiscordUsername = this.Context.User.Username,
            LolUsername = summonerName,
            LolPuuid = summoner.Puuid
        });

        await ReplyAsync("Subscribed!");
    }

    [Command("unsubscribe")]
    [Alias("unsub")]
    [Summary("Unsubscribe your discord account from LOL bot")]
    public async Task UnsubscribeAsync()
    {
        if (!await _lolService.UserExistInDbAsync(Context.User.Username))
        {
            await ReplyAsync("You are not subscribed");

            return;
        }

        await _lolService.RemoveUserByDiscordAsync(Context.User.Username);

        await ReplyAsync("Unsubscribed!");
    }

    [Command("info")]
    [Summary("Get info about your LOL summoner account")]
    public async Task InfoAsync()
    {
        await InfoAsync(Context.User.Username);

        // await ReplyAsync($"```\n" +
        //                  $"Name: {summoner.Name}\n" +
        //                  $"Level: {summoner.SummonerLevel}\n" +
        //                  $"Icon: #{summoner.ProfileIconId}\n" +
        //                  $"Last changed: {summoner.RevisionDateParsed}\n" +
        //                  $"ID: {summoner.Id}\n" +
        //                  $"Account ID: {summoner.AccountId}\n" +
        //                  $"PUUID: {summoner.Puuid}\n" +
        //                  "```");
    }

    [Command("info")]
    [Summary("Get info about your LOL summoner account")]
    public async Task InfoAsync(string summonerName)
    {
        if (string.IsNullOrWhiteSpace(summonerName))
        {
            await ReplyAsync("Username is empty");
            return;
        }

        SummonerDto summoner = null;
        try
        {
            summoner = await _lolService.GetSummonerByName(summonerName);
        }
        catch
        {
            await ReplyAsync("Summoner with this name does not exist");
            return;
        }

        var fields = new List<EmbedFieldBuilder>()
        {
            new EmbedFieldBuilder().WithName("Level").WithValue(summoner.SummonerLevel),
            new EmbedFieldBuilder().WithName("Profile changed").WithValue(summoner.RevisionDateParsed)
        };

        var embed = new EmbedBuilder()
            .WithTitle($"{summoner.Name}")
            .WithFields(fields)
            .WithThumbnailUrl($"https://static.u.gg/assets/lol/riot_static/12.20.1/img/profileicon/{summoner.ProfileIconId}.png")
            .WithColor(Color.Blue)
            .Build();

        await ReplyAsync(embed: embed);
    }
}