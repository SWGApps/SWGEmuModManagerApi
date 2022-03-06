using System.Diagnostics;

namespace SWGEmuModManagerApi.Models;

public class ModDb
{
    public int? Id { get; set; }
    public int? Downloads { get; set; }

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
}
