using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SWGEmuModManagerApi.Models;

namespace SWGEmuModManagerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ModController : ControllerBase
{
    [HttpGet(template: "{id}")]
    public async Task<Mod> Get(int id)
    {
        return await Mod.GetMod(id);
    }

    [HttpGet(template: "Install/{id}")]
    public async Task<InstallRequestResponse> InstallMod(int id)
    {
        Mod mod = await Mod.GetMod(id);

        if (mod.FileList is null || mod.FileList.Count < 1)
        {
            return new InstallRequestResponse()
            {
                Result = "Fail",
                Reason = "Mod ID is invalid or File List is empty"
            };
        }

        await ModDb.AddDownload(id);

        return new InstallRequestResponse()
        {
            Result = "Success",
            DownloadUrl = Mod.GetAvailableMirror(),
            FileList = mod.FileList,
            Archive = mod.Archive,
            ConflictList = mod.ConflictList
        };
    }

    [HttpGet(template: "Uninstall/{id}")]
    public async Task<UninstallRequestResponse> UninstallMod(int id)
    {
        Mod mod = await Mod.GetMod(id);

        if (mod.FileList is null || mod.FileList.Count < 1)
        {
            return new UninstallRequestResponse()
            {
                Result = "Fail",
                Reason = "Mod ID is invalid or File List is empty"
            };
        }

        return new UninstallRequestResponse()
        {
            Result = "Success",
            FileList = mod.FileList
        };
    }
}
