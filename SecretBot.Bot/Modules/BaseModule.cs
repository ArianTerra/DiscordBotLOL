using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordNetBotTemplate.Modules;

public class BaseModule : ModuleBase<SocketCommandContext>
{
    [Command("amogus")]
    public Task AbobusAsync()
    {
        return ReplyAsync("<:amogus:847136233817899018>");
    }

    [Command("miha")]
    public Task MihaAsync()
    {
        return ReplyAsync("<:miha:697801695018877038>");
    }
}