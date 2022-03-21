using Microsoft.AspNetCore.Mvc;
using SWGEmuModManagerApi.Models;
using X.PagedList;

namespace SWGEmuModManagerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ModsController : ControllerBase
{
    [HttpGet("/Mods/{inputPageNumber}/{inputPageSize}/{sortType}/{sortOrder}/{filterValue}")]
    public async Task<IActionResult> Get(int inputPageNumber, int inputPageSize, int sortType, int sortOrder, string filterValue)
    {
        IEnumerable<Mod> mods = await Mod.GetMods(sortType, sortOrder, filterValue);

        inputPageSize = inputPageSize switch
        {
            0 => 10,
            1 => 20,
            2 => 30,
            3 => 40,
            4 => 50,
            5 => 999999,
            _ => 10,
        };

        IPagedList<Mod> modList = mods.ToPagedList(inputPageNumber, inputPageSize);

        return Ok(new PaginatedResponse<IPagedList<Mod>>(
            modList, 
            modList.TotalItemCount, 
            modList.PageSize, 
            modList.PageNumber, 
            modList.PageCount, 
            modList.IsLastPage, 
            modList.IsFirstPage, 
            modList.LastItemOnPage, 
            modList.FirstItemOnPage, 
            modList.HasPreviousPage, 
            modList.HasNextPage));
    }

    [HttpGet("/Mods/FileList/{ids}")]
    public async Task<IActionResult> FileList(string ids)
    {
        List<int> modIDs = new();

        List<string> tempIDs = ids.Split(separator: '-').ToList();

        tempIDs.ForEach(id =>
        {
            modIDs.Add(item: Convert.ToInt32(id));
        });

        List<Mod> mods = await Mod.GetMods
            (sortType: 0, sortOrder: 0, filterValue: "null");

        Dictionary<int, List<string>> retData = new();

        mods.ToList().ForEach(mod =>
        {
            modIDs.ForEach(modId =>
            {
                if (mod.Id == modId)
                {
                    retData.Add(modId, mod.FileList!);
                }
            });
        });

        return Ok(new Response<Dictionary<int, List<string>>>(retData));
    }
}
