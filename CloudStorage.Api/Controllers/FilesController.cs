using AutoMapper;
using CloudStorage.Api.Dtos.Request;
using CloudStorage.Api.Dtos.Response;
using CloudStorage.Api.Filters;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.Api.Controllers;

[ApiController]
[Route("api/files/")]
//[Authorize]
public class FilesController : ControllerBase
{
    private const long MaxFileSize = 10_000_000_000;
    
    private readonly ICloudStorageManager _cloudStorageManager;
    private readonly IMapper _mapper;

    public FilesController(ICloudStorageManager cloudStorageManager, IMapper mapper)
    {
        _cloudStorageManager = cloudStorageManager;
        _mapper = mapper;
    }

    [EnableCors("_myAllowSpecificOrigins")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FileGetResponse>>> GetAllFiles()
    {
        var fileDescriptions = await _cloudStorageManager.GetAllFilesAsync();
        var fileGetResponses = _mapper.Map<IEnumerable<FileGetResponse>>(fileDescriptions);

        SetContentUrl(fileGetResponses);

        return Ok(fileGetResponses);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<FileGetResponse>>> SearchFiles([FromQuery] FileSearchRequest fileSearchRequest)
    {
        var fileSearchData = _mapper.Map<FileSearchData>(fileSearchRequest);
        var fileDescriptions = await _cloudStorageManager.SearchFilesAsync(fileSearchData);
        var fileGetResponses = _mapper.Map<IEnumerable<FileGetResponse>>(fileDescriptions);

        SetContentUrl(fileGetResponses);

        return Ok(fileGetResponses);
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    [RequestSizeLimit(MaxFileSize)]
    [DisableFormValueModelBinding]
    public async Task<ActionResult<IEnumerable<FileGetResponse>>> UploadFiles([FromForm] IEnumerable<IFormFile> files)
    {
        var fileCreateDatas = _mapper.Map<IEnumerable<FileCreateData>>(files);
        var fileDescriptions = await _cloudStorageManager.CreateAsync(fileCreateDatas);
        var fileResponses = _mapper.Map<IEnumerable<FileGetResponse>>(fileDescriptions);

        SetContentUrl(fileResponses);

        return Ok(fileResponses);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] IEnumerable<int> ids)
    {
        await _cloudStorageManager.DeleteRangeAsync(ids);

        return Ok();
    }

    [HttpGet("archive/file-list/{fileId}")]
    public async Task<ActionResult<IEnumerable<string>>> GetArchiveFileNames(int fileId)
    {
        var names = await _cloudStorageManager.GetArchiveFileNamesAsync(fileId);

        return Ok(names);
    }

    [HttpGet("archive/unzip-file/{fileId}/{archiveFilePath}")]
    public async Task<ActionResult<Stream>> UnzipArchiveFile(int fileId, string archiveFilePath)
    {
        var decodedPath = archiveFilePath.Replace("%2F", "/");
        
        var (contentType, data) = await _cloudStorageManager.GetArchiveFileContent(fileId, decodedPath);

        return new FileStreamResult(data, contentType);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Stream>> GetFileContent([FromRoute] int id)
    {
        var (stream, contentType) = await _cloudStorageManager.GetFileStreamAndContentTypeAsync(id);
        return new FileStreamResult(stream, contentType);
    }

    private void SetContentUrl(IEnumerable<FileGetResponse> fileGetResponses)
    {
        foreach (var file in fileGetResponses)
        {
            file.FileSrc = Url.Action("GetFileContent", "Files", new { id = file.Id })!;
            file.ThumbnailSrc = Url.Action("GetThumbnail", "Thumbnails", new { id = file.ThumbnailId })!;
        }
    }
}