using Microsoft.EntityFrameworkCore;
using TreeViewCrud.Models;

public class AppUserDbContext : DbContext
{
    public DbSet<AppUser> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Используем строку подключения из вашего ConfigManager
        optionsBuilder.UseSqlServer(ConfigManager.ConnectionString);
    }
}