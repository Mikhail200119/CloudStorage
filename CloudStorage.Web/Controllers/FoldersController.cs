using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.Web.Controllers
{
    public class FoldersController : Controller
    {
        private readonly ICloudStorageManager _cloudStorageManager;
        private readonly IMapper _mapper;

        public FoldersController(ICloudStorageManager cloudStorageManager, IMapper mapper)
        {
            _cloudStorageManager = cloudStorageManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CreateFolder(int? parentFolderId) => View(new FileFolderCreateModel { ParentFolderId = parentFolderId });

        [HttpPost]
        public async Task<IActionResult> CreateFolderAsync(FileFolderCreateModel folder)
        {
            var folderCreateData = _mapper.Map<FileFolderCreateModel, FileFolderCreateData>(folder);

            await _cloudStorageManager.CreateFolderAsync(folderCreateData);

            return RedirectToAction("ViewAllFiles", "Files");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFolderAsync(FileFolderUpdateModel folder)
        {
            var folderUpdateData = _mapper.Map<FileFolderUpdateModel, FileFolderUpdateData>(folder);

            await _cloudStorageManager.UpdateFolderAsync(folderUpdateData);

            return RedirectToAction(controllerName: "Files", actionName: "ViewAllFiles");
        }
    }
}
