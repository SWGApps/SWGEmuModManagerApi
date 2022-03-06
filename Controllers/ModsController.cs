using Microsoft.AspNetCore.Mvc;
using SWGEmuModManagerApi.Models;

namespace SWGEmuModManagerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ModsController : ControllerBase
{
    [HttpGet(Name = "GetMods")]
    public async Task<List<Mod>> Get()
    {
        return await Mod.GetMods();
    }
}
