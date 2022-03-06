using System.Diagnostics;

namespace SWGEmuModManagerApi.Models;

public class ModDb
{
    public int? Id { get; }
    private int? Downloads { get; }
    private int? Rating1 { get; }
    private int? Rating2 { get; }
    private int? Rating3 { get; }
    private int? Rating4 { get; }
    private int? Rating5 { get; }
    private int? Ratings { get; }
    public string? IpAddress { get; }

    public static async Task AddDownload(int id)
    {
        List<ModDb>? modData = await DatabaseConnection.ExecuteModDbAsync(data: 
            $"SELECT Downloads FROM ModDb where Id = '{id}'");

        if (modData is not null && modData.Count > 0)
        {
            int? downloads = modData![0].Downloads + 1;

            if (downloads is not null)
            {
                await DatabaseConnection.ExecuteModDbAsync(data: 
                    $"UPDATE ModDb SET Downloads = '{downloads}' WHERE Id = '{id}'");
            }
        }
    }

    public static async Task AddRating(int id, int rating, string ip)
    {
        List<ModDb>? modData = await DatabaseConnection.ExecuteModDbAsync(data: 
            $"SELECT Ratings, Rating1, Rating2, Rating3, Rating4, Rating5 FROM ModDb where Id = '{id}'");

        if (modData is not null && modData.Count > 0)
        {
            int? ratings = modData![0].Ratings + 1;

            switch (rating)
            {
                case 1:
                    await DatabaseConnection.ExecuteModDbAsync(data: 
                        $"UPDATE ModDb SET Rating1 = '{modData![0].Rating1 + 1}' WHERE Id = '{id}'");
                    break;
                case 2:
                    await DatabaseConnection.ExecuteModDbAsync(data:
                        $"UPDATE ModDb SET Rating2 = '{modData![0].Rating2 + 1}' WHERE Id = '{id}'");
                    break;
                case 3:
                    await DatabaseConnection.ExecuteModDbAsync(data:
                        $"UPDATE ModDb SET Rating3 = '{modData![0].Rating3 + 1}' WHERE Id = '{id}'");
                    break;
                case 4:
                    await DatabaseConnection.ExecuteModDbAsync(data:
                        $"UPDATE ModDb SET Rating4 = '{modData![0].Rating4 + 1}' WHERE Id = '{id}'");
                    break;
                case 5:
                    await DatabaseConnection.ExecuteModDbAsync(data:
                        $"UPDATE ModDb SET Rating5 = '{modData![0].Rating5 + 1}' WHERE Id = '{id}'");
                    break;
            }

            if (ratings is not null)
            {
                await DatabaseConnection.ExecuteModDbAsync(data: 
                    $"UPDATE ModDb SET Ratings = '{ratings}', IpAddress = '{ip}' WHERE Id = '{id}'");
            }
        }
    }

    public static async Task<bool> CheckExistingIp(int modId, string ip)
    {
        List<ModDb>? modData = await DatabaseConnection.ExecuteModDbAsync(data: 
            $"SELECT IpAddress FROM ModDb where Id = '{modId}' AND IpAddress = '{ip}'");

        if (modData is null || modData.Count == 0) return false;

        return true;
    }
}
