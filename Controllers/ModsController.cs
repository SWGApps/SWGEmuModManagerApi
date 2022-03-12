using Microsoft.AspNetCore.Mvc;
using SWGEmuModManagerApi.Models;
using X.PagedList;

namespace SWGEmuModManagerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ModsController : ControllerBase
{
    [HttpGet("/Mods/{inputPageNumber}/{inputPageSize}")]
    public async Task<IActionResult> Get(int inputPageNumber, int inputPageSize)
    {
        IEnumerable<Mod> mods = await Mod.GetMods();
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
