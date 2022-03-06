using System.Text.Json;
using System.Text.Json.Serialization;

namespace SWGEmuModManagerApi.Models;

public class Mod
{
    private static List<string>? _downloadMirrors;

    // Start at -1 so the first mirror is 0
    private static int _lastMirror = -1;
    private const int MirrorType = 0;
    private const int RoundRobin = 0;
    private const int Random = 1;

    public Mod()
    {
        _downloadMirrors = new()
        {
            "https://mods.darknaught.com/",
            "https://mods.swgemu.com/",
            "https://mods.swglegacy.com/"
        };
    }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("author")]
    public string? Author { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("size")]
    public ulong? Size { get; set; }

    [JsonPropertyName("downloads")]
    public ulong? Downloads { get; set; }

    [JsonPropertyName("released")]
    public DateTime? Released { get; set; }

    [JsonPropertyName("rating1")]
    public int? Rating1 { get; set; }

    [JsonPropertyName("rating2")]
    public int? Rating2 { get; set; }

    [JsonPropertyName("rating3")]
    public int? Rating3 { get; set; }

    [JsonPropertyName("rating4")]
    public int? Rating4 { get; set; }

    [JsonPropertyName("rating5")]
    public int? Rating5 { get; set; }

    [JsonPropertyName("ratings")]
    public int? Ratings { get; set; }

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

        if (mods is null)  return new List<Mod>();
            
        return mods;
    }

    public static string GetAvailableMirror()
    {
        if (MirrorType == RoundRobin)
        {
            if (_lastMirror >= _downloadMirrors!.Count - 1) _lastMirror = -1;
            int nextMirror = _lastMirror += 1;
            return _downloadMirrors[nextMirror];
        }

#pragma warning disable CS0162 // Unreachable code detected
        if (MirrorType == Random)
        {
            return _downloadMirrors![new Random().Next(_downloadMirrors.Count)];
        }
#pragma warning restore CS0162 // Unreachable code detected
    }
}

public class RatingRequestResponse
{
    [JsonPropertyName("result")]
    public string? Result { get; set; }
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
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
