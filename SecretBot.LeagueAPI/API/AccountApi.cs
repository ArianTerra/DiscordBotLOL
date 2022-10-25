using System.Net.Http.Json;
using SecretBot.LeagueAPI.DTO;

namespace SecretBot.LeagueAPI.API;

public class AccountApi
{
    private readonly string _apiKey;

    public AccountApi(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<AccountDto> GetAccountByUsernameAsync(string username, string tagline)
    {
        var uri =
            $"https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{username}/{tagline}?api_key={_apiKey}";

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

        var client = new HttpClient();
        var response = await client.SendAsync(httpRequestMessage);

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<AccountDto>();

        return dto;
    }
}