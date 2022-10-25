namespace DiscordNetBotTemplate;

public static class Program
{
    private static void Main()
        => new Startup().RunAsync().GetAwaiter().GetResult();
}