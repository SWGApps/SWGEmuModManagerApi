using Microsoft.AspNetCore.Mvc;
using SWGEmuModManagerApi.Models;

namespace SWGEmuModManagerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ModController : ControllerBase
{
    [HttpGet(template: "{id}")]
    public async Task<Mod> GetModAsync(int id)
    {
        return await Mod.GetMod(id);
    }

    [HttpGet(template: "Install/{id}")]
    public async Task<ActionResult<Response<InstallRequestResponse>>> InstallModAsync(int id)
    {
        Mod mod = await Mod.GetMod(id);

        if (mod.FileList is null || mod.FileList.Count < 1)
        {
            List<string> errors = new() { "Mod ID is invalid or File List is empty" };

            return new Response<InstallRequestResponse>()
            {
                Succeeded = false,
                Errors = errors.ToArray()
            };
        }

        return new Response<InstallRequestResponse>()
        {
            Succeeded = true,
            Data = new InstallRequestResponse()
            {
                DownloadUrl = Mod.GetAvailableMirror(),
                FileList = mod.FileList,
                Archive = mod.Archive,
                ConflictList = mod.ConflictList
            }
        };
    }

    [HttpGet(template: "AddDownload/{id}")]
    public async Task<ActionResult<Response<object>>> AddDownloadAsync(int id)
    {
        // If download added
        if (await ModInfo.AddDownload(id))
        {
            return new Response<object>() { Succeeded = true };
        }

        // If download not added
        List<string> errors = new() { "Failed to add download" };

        return new Response<object>()
        {
            Succeeded = false,
            Errors = errors.ToArray()
        };
    }

    [HttpGet(template: "Uninstall/{id}")]
    public async Task<ActionResult<Response<UninstallRequestResponse>>> UninstallModAsync(int id)
    {
        Mod mod = await Mod.GetMod(id);

        if (mod.FileList is null || mod.FileList.Count < 1)
        {
            List<string> errors = new() { "Mod ID is invalid or File List is empty" };

            return new Response<UninstallRequestResponse>()
            {
                Succeeded = false,
                Errors = errors.ToArray()
            };
        }

        return new Response<UninstallRequestResponse>()
        {
            Succeeded = true,
            Data = new UninstallRequestResponse()
            {
                FileList = mod.FileList
            }
        };
    }
}
