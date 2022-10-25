using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SecretBot.DataAccess.Models;
using SecretBot.DataAccess.Repositories;
using SecretBot.LeagueAPI.API;
using SecretBot.LeagueAPI.DTO;

namespace DiscordNetBotTemplate.Services;

public class LolService
{
    private IGenericRepository<LolUser> _usersRepository;

    private IConfiguration _configuration;

    public LolService(IGenericRepository<LolUser> usersRepository, IConfiguration configuration)
    {
        _usersRepository = usersRepository;
        _configuration = configuration;
    }

    public async Task AddUserAsync(LolUser user)
    {
        await _usersRepository.AddAsync(user);
    }

    public async Task RemoveUserByDiscordAsync(string discordName)
    {
        var lolUser = await _usersRepository.FindFirstAsync(x => x.DiscordUsername == discordName);

        if (lolUser == null)
        {
            throw new ArgumentException("User with given discord name does not exist in DB");
        }

        await _usersRepository.RemoveAsync(lolUser);
    }

    public async Task<SummonerDto> GetSummonerByName(string name)
    {
        var api = new SummonerApi(_configuration["BotSettings:LOL_API_KEY"]);

        var summoner = await api.GetSummonerByNameAsync(name);

        return summoner;
    }

    public async Task<bool> UserExistInDbAsync(string discordName)
    {
        return await _usersRepository.FindFirstAsync(x => x.DiscordUsername == discordName) != null;
    }

    public async Task<string> GetSummonerNameByDiscordAsync(string discordName)
    {
        return (await _usersRepository.FindFirstAsync(x => x.DiscordUsername == discordName)).LolUsername;
    }
}