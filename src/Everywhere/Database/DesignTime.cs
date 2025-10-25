#if DEBUG

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AlfredGPT.Database;

public class ChatDbContextFactory : IDesignTimeDbContextFactory<ChatDbContext>
{
    public ChatDbContext CreateDbContext(string[] args)
    {
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AlfredGPT",
            "db");
        Directory.CreateDirectory(dbPath);

        var optionsBuilder = new DbContextOptionsBuilder<ChatDbContext>();
        optionsBuilder.UseSqlite($"Data Source=${Path.Combine(dbPath, "chat.db")}");

        return new ChatDbContext(optionsBuilder.Options);
    }
}

#endif