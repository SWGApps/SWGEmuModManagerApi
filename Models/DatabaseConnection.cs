using System.Diagnostics;
using Serilog;
using SQLite;

namespace SWGEmuModManagerApi.Models;

public static class DatabaseConnection
{
    private static SQLiteAsyncConnection? _connection;

    public static async Task Initialize()
    {
        _connection = new(databasePath: "ModDb.db");

        await CreateTables();
        await ImportData();

        Log.Logger.Information(messageTemplate: "SQLite Database Initialized.");
    }

    public static async Task<List<ModDb>?> ExecuteModDbAsync(string data)
    {
        try
        {
            if (_connection is null) return null;

            Trace.WriteLine(data);
            
            List<ModDb> results = await _connection.QueryAsync<ModDb>(data);

            await _connection.CloseAsync();

            return results;
        }
        catch (Exception e)
        {
            Log.Logger.Error(messageTemplate: $"Error executing query: {Environment.NewLine}{data}{Environment.NewLine}{Environment.NewLine}{e.Message}");
        }

        return new List<ModDb>();
    }

    private static async Task CreateTables()
    {
        if (_connection is null || File.Exists("ModDb.db")) return;

        await _connection.CreateTableAsync<ModDb>();
    }

    private static async Task ImportData()
    {
        if (_connection is null || !File.Exists(path: "ModDb.db")) return;

        List<Mod> mods = await Mod.GetMods();

        foreach (Mod mod in mods)
        {
            List<ModDb>? modDb = await ExecuteModDbAsync(data: $"SELECT id FROM ModDb where id = '{mod.Id}'");

            if (modDb is not null && modDb.Count < 1)
            {
                await ExecuteModDbAsync(data: "INSERT INTO ModDb (Id, Downloads) " +
                                              $"VALUES ('{mod.Id}', '{mod.Downloads}')");
            }
        }
    }
}
