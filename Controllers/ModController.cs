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

    [HttpGet(template: "Rate/{id}")]
    public async Task<RatingRequestResponse> RateMod(int id, int rating)
    {
        Mod mod = await Mod.GetMod(id);

        var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress!.ToString();

        if (mod is not null && mod.Id > -1)
        {
            Trace.WriteLine($"ID: {id}, Rating: {rating}");

            if (await ModDb.CheckExistingIp(id, remoteIpAddress))
            {
                return new RatingRequestResponse()
                {
                    Result = "Fail",
                    Reason = "You have already rated this mod"
                };
            }

            if (rating > -1 && rating < 6)
            {
                await ModDb.AddRating(id, rating, remoteIpAddress);
                return new RatingRequestResponse() { Result = "Success" };
            }

            return new RatingRequestResponse()
            {
                Result = "Fail",
                Reason = "Invalid rating value supplied."
            };
        }

        return new RatingRequestResponse()
        {
            Result = "Fail",
            Reason = "Mod ID does not exist."
        };
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
