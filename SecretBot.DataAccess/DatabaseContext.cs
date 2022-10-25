using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SecretBot.DataAccess.Models;

namespace SecretBot.DataAccess;

public class DatabaseContext : DbContext
{
    public DbSet<LolUser> LolUsers { get; set; }

    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:/Users/Arian/OneDrive/Документи/Study/DiscordBot/DiscordNetBotTemplate/SecretBot.DataAccess/sql.db");
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    // }
}