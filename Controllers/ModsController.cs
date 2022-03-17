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

        switch (inputPageSize)
        {
            case 0: inputPageSize = 10; break;
            case 1: inputPageSize = 20; break;
            case 2: inputPageSize = 30; break;
            case 3: inputPageSize = 40; break;
            case 4: inputPageSize = 50; break;
            default: inputPageSize = 10; break;
        }

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
}
