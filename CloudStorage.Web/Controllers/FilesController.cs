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
        var claims = HttpContext.User.Claims;

        var files = await _cloudStorageManager.GetAllFiles();

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
        file.UserName = "mikhail";

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

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _cloudStorageManager.Delete(id);

        return RedirectToAction(nameof(ViewAllFiles));
    }
}