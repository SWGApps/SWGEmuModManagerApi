using System.Diagnostics;

namespace SWGEmuModManagerApi.Models;

public class ModDb
{
    public int? Id { get; set; }
    public int? Downloads { get; set; }

    public static async Task AddDownload(int id)
    {
        DatabaseConnection conn = new();
        List<ModDb>? modData = await conn.ExecuteModDbAsync(data: 
            $"SELECT Downloads FROM ModDb where Id = '{id}'");

        if (modData is not null && modData.Count > 0)
        {
            int? downloads = modData![0].Downloads + 1;

            if (downloads is not null)
            {
                await conn.ExecuteModDbAsync(data: 
                    $"UPDATE ModDb SET Downloads = '{downloads}' WHERE Id = '{id}'");
            }
        }

        conn.Dispose();
    }
}
