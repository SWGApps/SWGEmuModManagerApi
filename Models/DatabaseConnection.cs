﻿using Serilog;
using SQLite;

namespace SWGEmuModManagerApi.Models;

public class DatabaseConnection
{
    private SQLiteAsyncConnection? _connection;

    public async Task Initialize()
    {
        _connection = new(databasePath: "ModDb.db");

        await CreateTables();
        await ImportData();

        Log.Logger.Information(messageTemplate: "SQLite Database Initialized.");
        Log.Logger.Information(messageTemplate: "API is ready for requests.");
    }

    public async Task<List<ModDb>?> ExecuteModDbAsync(string data)
    {
        try
        {
            if (_connection is null) return null;

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

    private async Task CreateTables()
    {
        if (_connection is null || File.Exists("ModDb.db")) return;

        await _connection.CreateTableAsync<ModDb>();
    }

    private async Task ImportData()
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

    public async Task<int?> GetDownloads(int? id)
    {
        if (_connection is null || !File.Exists(path: "ModDb.db")) return 0;

        List<ModDb> modDbData = await ExecuteModDbAsync(
            data: $"SELECT Downloads FROM ModDb WHERE Id = '{id}'") ?? new List<ModDb>();

        if (modDbData.Count > 0) return modDbData[0].Downloads;
        
        return 0;
    }

    public async void Dispose()
    {
        if (_connection == null) return;

        await Task.Factory.StartNew(() =>
        {
            _connection.GetConnection().Close();
            _connection.GetConnection().Dispose();
            _connection = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        });
    }
}
