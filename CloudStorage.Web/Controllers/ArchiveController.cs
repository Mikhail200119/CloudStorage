using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.Web.Extensions;
using CloudStorage.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.Web.Controllers;

[Authorize]
public class ArchiveController : Controller
{
    private readonly ICloudStorageManager _cloudStorageManager;
    private readonly IMapper _mapper;

    public ArchiveController(ICloudStorageManager cloudStorageManager, IMapper mapper)
    {
        _cloudStorageManager = cloudStorageManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> ViewArchiveEntries([FromRoute] int fileId)
    {
        var filesInArchive = await _cloudStorageManager.GetFilesFromArchive(fileId);

        var fileViewModels = _mapper.Map<IEnumerable<FileDescription>, IEnumerable<FileViewModel>>(filesInArchive);

        return View((fileId, fileViewModels));
    }

    public async Task<IActionResult> GetEntryContent([FromQuery] string name, [FromRoute] int archiveId)
    {
        var stream = await _cloudStorageManager.GetArchiveEntryContent(archiveId, name);

        return File(stream!, "");
    }
}