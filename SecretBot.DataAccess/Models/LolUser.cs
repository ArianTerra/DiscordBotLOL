namespace SecretBot.DataAccess.Models;

public class LolUser : BaseModel
{
    public string DiscordUsername { get; set; }

    public string LolUsername { get; set; }

    public string LolPuuid { get; set; }
}