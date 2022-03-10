using System.Text.Json;
using System.Text.Json.Serialization;

namespace SWGEmuModManagerApi.Models;

public class Mod
{
    private static List<string>? _downloadMirrors;

    // Start at -1 so the first mirror is 0
    private static int _lastMirror = -1;
    private static int MirrorType { get; set; }
    private const int RoundRobin = 0;
    private const int Random = 1;

    public Mod()
    {
        // Round Robin
        MirrorType = 0;

        _downloadMirrors = new()
        {
            "http://login.darknaught.com:8080/mods/",
            //"https://mods.swgemu.com/",
            //"https://mods.swglegacy.com/"
        };
    }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("bannerUrl")]
    public string? BannerUrl { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("author")]
    public string? Author { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("size")]
    public ulong? Size { get; set; }

    [JsonPropertyName("downloads")]
    public int? Downloads { get; set; }

    [JsonPropertyName("released")]
    public DateTime? Released { get; set; }

    [JsonPropertyName("archive")]
    public string? Archive { get; set; }

    [JsonPropertyName("fileList")]
    public List<string>? FileList { get; set; }

    [JsonPropertyName("conflictList")]
    public List<int>? ConflictList { get; set; }

    public static async Task<Mod> GetMod(int id)
    {
        List<Mod>? mods = new();

        await Task.Run(() => { mods = JsonSerializer.Deserialize<List<Mod>>(File.ReadAllText("mods.json")); });

        if (mods is null) return new Mod();

        return mods.FirstOrDefault(mod => mod.Id == id)!;
    }

    public static async Task<List<Mod>> GetMods()
    {
        List<Mod>? mods = new();

        await Task.Run(() => { mods = JsonSerializer.Deserialize<List<Mod>>(File.ReadAllText("mods.json")); });

        foreach (Mod mod in mods)
        {
            mod.Downloads = await DatabaseConnection.GetDownloads(mod.Id);
        }

        if (mods is null)  return new List<Mod>();
            
        return mods;
    }

    public static string GetAvailableMirror()
    {
        switch (MirrorType)
        {
            case RoundRobin:
                if (_lastMirror >= _downloadMirrors!.Count - 1) _lastMirror = -1;
                int nextMirror = _lastMirror += 1;
                return _downloadMirrors[nextMirror];
            case Random:
                return _downloadMirrors![new Random().Next(_downloadMirrors.Count)];
        }

        return _downloadMirrors![0];
    }
}

public class InstallRequestResponse
{
    [JsonPropertyName("result")]
    public string? Result { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("downloadUrl")]
    public string? DownloadUrl { get; set; }

    [JsonPropertyName("fileList")]
    public List<string>? FileList { get; set; }

    [JsonPropertyName("archive")]
    public string? Archive { get; set; }

    [JsonPropertyName("conflictList")]
    public List<int>? ConflictList { get; set; }
}

public class UninstallRequestResponse
{
    [JsonPropertyName("result")]
    public string? Result { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("fileList")]
    public List<string>? FileList { get; set; }
}
