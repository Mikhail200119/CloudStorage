using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.Web.Controllers
{
    public class FoldersController : Controller
    {
        private readonly ICloudStorageManager _cloudStorageManager;

        public FoldersController(ICloudStorageManager cloudStorageManager)
        {
            _cloudStorageManager = cloudStorageManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder(FileFolderCreateModel folder)
        {

        }
    }
}
