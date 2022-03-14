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
