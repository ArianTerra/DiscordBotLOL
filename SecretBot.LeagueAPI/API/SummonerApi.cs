using System.Net.Http.Json;
using SecretBot.LeagueAPI.DTO;

namespace SecretBot.LeagueAPI.API;

public class SummonerApi
{
    private readonly string _apiKey;

    public SummonerApi(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<SummonerDto> GetSummonerByNameAsync(string name)
    {
        var uri =
            $"https://ru.api.riotgames.com/lol/summoner/v4/summoners/by-name/{name}?api_key={_apiKey}";

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

        var client = new HttpClient();
        var response = await client.SendAsync(httpRequestMessage);

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<SummonerDto>();

        return dto;
    }
}