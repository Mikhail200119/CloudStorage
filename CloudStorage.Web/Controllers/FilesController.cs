using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.Web.Filters;
using CloudStorage.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.Web.Controllers;

[Authorize]
public class FilesController : Controller
{
    private const long MaxFileSize = 10_000_000_000;

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

        var filesToView = _mapper.Map<IEnumerable<FileDescription>, IEnumerable<FileViewModel>>(files);

        return View(filesToView);
    }

    [HttpGet]
    public IActionResult Create() => View(new FileCreateModel());

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    [RequestSizeLimit(MaxFileSize)]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> Create(FileCreateModel fileCreateModel)
    {
        if (ModelState.IsValid)
        {
            var filesCreateData = _mapper.Map<IEnumerable<IFormFile>, IEnumerable<FileCreateData>>(fileCreateModel.FormFiles);

            await _cloudStorageManager.CreateAsync(filesCreateData);
        }

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
    public async Task<IActionResult> GetFileContent(int id, string contentType)
    {
        var content = await _cloudStorageManager.GetFileStreamAsync(id);

        return File(content, contentType, enableRangeProcessing: true);
    }

    [HttpPut]
    public async Task<IActionResult> RenameFile(int id, string newName)
    {
        await _cloudStorageManager.RenameFileAsync(id, newName);

        return RedirectToAction(nameof(ViewAllFiles));
    }
}