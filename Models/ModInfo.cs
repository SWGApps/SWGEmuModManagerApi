using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace SWGEmuModManagerApi.Models;

public class ModInfoContext : DbContext
{
    public DbSet<ModInfo>? ModInfo { get; set; }

    public string DbPath { get; }

    public ModInfoContext()
    {
        DbPath = Path.Join("ModInfo.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite($"Data Source={DbPath}");
}

public class ModInfo
{
    public int? Id { get; set; }
    public int? Downloads { get; set; }

    public static async Task InitializeDatabase()
    {
        List<Mod>? mods = new();
        await Task.Run(() => { mods = JsonSerializer.Deserialize<List<Mod>>(File.ReadAllText("mods.json")); });

        if (!File.Exists(path: "ModInfo.db")) return;
        using ModInfoContext db = new();

        foreach (Mod mod in mods)
        {
            ModInfo? modInfo = await db.ModInfo!
                .SingleOrDefaultAsync(x => x.Id == mod.Id);

            if (modInfo is null)
            {
                db.Add(new ModInfo()
                {
                    Id = mod.Id,
                    Downloads = 0
                });
            }

            await db.SaveChangesAsync();
        }

        Log.Logger.Information("Database has been initialized.");
        Log.Logger.Information("API ready for requests.");
    }

    public static async Task<bool> AddDownload(int id)
    {
        if (!File.Exists(path: "ModInfo.db")) return false;

        using ModInfoContext db = new();

        ModInfo? mod = await db.ModInfo!
            .SingleOrDefaultAsync(x => x.Id == id);

        if (mod is null) return false;

        mod.Downloads += 1;

        await db.SaveChangesAsync();

        return true;
    }

    public static async Task<int?> GetDownloads(int? id)
    {
        if (!File.Exists(path: "ModInfo.db")) return 0;

        using var db = new ModInfoContext();

        return await db.ModInfo!
            .Where(x => x.Id == id)
            .Select(x => x.Downloads)
            .SingleOrDefaultAsync();
    }
}
