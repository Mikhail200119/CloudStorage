using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Services;
using CloudStorage.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using File = CloudStorage.BLL.Models.File;

namespace CloudStorage.Web.Controllers;

[Authorize]
public class FilesController : Controller
{
    private readonly ICloudStorageManager _cloudStorageManager;
    private readonly IMapper _mapper;

    public FilesController(ICloudStorageManager cloudStorageManager, IMapper mapper)
    {
        _cloudStorageManager = cloudStorageManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> ViewAllFiles()
    {
        var files = await _cloudStorageManager.GetAllFilesAsync();

        var filesToView = _mapper.Map<IEnumerable<File>, IEnumerable<FileViewModel>>(files);

        return View(filesToView);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new FileCreateModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(FileCreateModel file)
    {
        var fileCreateData = _mapper.Map<FileCreateModel, FileCreateData>(file);

        await _cloudStorageManager.CreateAsync(fileCreateData);

        return RedirectToAction(nameof(ViewAllFiles));
    }

    [HttpPost]
    public async Task<IActionResult> Update(FileUpdateModel file, int id)
    {
        var fileUpdateModel = _mapper.Map<FileUpdateModel, FileUpdateData>(file);

        await _cloudStorageManager.UpdateAsync(fileUpdateModel);

        return RedirectToAction(nameof(ViewAllFiles));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _cloudStorageManager.DeleteAsync(id);

        return RedirectToAction(nameof(ViewAllFiles));
    }

    [HttpGet]
    public async Task<IActionResult> GetFile(int id, string contentType)
    {
        var content = await _cloudStorageManager.GetContentByFileDescriptionIdAsync(id);

        return File(content, contentType);
    }
}