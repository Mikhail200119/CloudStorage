using AutoMapper;
using CloudStorage.BLL.Exceptions;
using CloudStorage.BLL.Helpers;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.Common.Extensions;
using CloudStorage.DAL;
using CloudStorage.DAL.Entities;

namespace CloudStorage.BLL.Services;

public class CloudStorageManager : ICloudStorageManager
{
    private readonly ICloudStorageUnitOfWork _cloudStorageUnitOfWork;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDataHasher _dataHasher;

    public CloudStorageManager(ICloudStorageUnitOfWork cloudStorageUnitOfWork, IMapper mapper, IUserService userService, IFileStorageService fileStorageService, IDataHasher dataHasher)
    {
        _cloudStorageUnitOfWork = cloudStorageUnitOfWork;
        _mapper = mapper;
        _userService = userService;
        _fileStorageService = fileStorageService;
        _dataHasher = dataHasher;
    }

    public async Task<FileDescription> CreateAsync(FileCreateData newFile)
    {
        var fileDbModel = _mapper.Map<FileCreateData, FileDescriptionDbModel>(newFile);
        fileDbModel.UploadedBy = _userService.Current.Email;
        var contentAsArray = newFile.Content.ToArray();

        fileDbModel.ContentHash = _dataHasher.HashData(contentAsArray);

        await ValidateFCreatedFile(fileDbModel);

        await _fileStorageService.UploadAsync(fileDbModel.UniqueName, contentAsArray);

        if (ContentTypeDeterminant.IsImage(fileDbModel.ContentType))
        {
            fileDbModel.Preview = _fileStorageService.CompressImage(contentAsArray);
        }
        else if (ContentTypeDeterminant.IsVideo(fileDbModel.ContentType))
        {
            var thumbnail = await _fileStorageService.GetVideoThumbnailAsync(fileDbModel.UniqueName);
            fileDbModel.Preview = _fileStorageService.CompressImage(thumbnail);
        }

        try
        {
            await _cloudStorageUnitOfWork.FileDescription.CreateAsync(fileDbModel);
            await _cloudStorageUnitOfWork.SaveChangesAsync();
        }
        catch
        {
            _fileStorageService.Delete(fileDbModel.UniqueName);
            throw;
        }

        var file = _mapper.Map<FileDescriptionDbModel, FileDescription>(fileDbModel);
        
        return file;
    }

    public async Task<FileDescription> GetByIdAsync(int id)
    {
        var fileDbModel = await _cloudStorageUnitOfWork.FileDescription.GetByIdAsync(id);

        var file = _mapper.Map<FileDescriptionDbModel, FileDescription>(fileDbModel);

        return file;
    }

    public async Task<byte[]> GetFileContentAsync(int id)
    {
        var item = await _cloudStorageUnitOfWork.FileDescription.GetByIdAsync(id);

        var content = await _fileStorageService.GetAsync(item.UniqueName);

        return content;
    }

    public async Task<Stream> GetFileStreamAsync(int fileId)
    {
        var item = await _cloudStorageUnitOfWork.FileDescription.GetByIdAsync(fileId);

        var content = await _fileStorageService.GetStreamAsync(item.UniqueName);

        return content;
    }

    public async Task<FileDescription> UpdateAsync(FileUpdateData existingFile)
    {
        var fileDbModel = _mapper.Map<FileUpdateData, FileDescriptionDbModel>(existingFile);

        _cloudStorageUnitOfWork.FileDescription.Update(fileDbModel);
        await _cloudStorageUnitOfWork.SaveChangesAsync();

        var file = _mapper.Map<FileDescriptionDbModel, FileDescription>(fileDbModel);

        return file;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _cloudStorageUnitOfWork.FileDescription.GetByIdAsync(id);

        _cloudStorageUnitOfWork.FileDescription.Delete(item.Id);

        await _cloudStorageUnitOfWork.SaveChangesAsync();

        _fileStorageService.Delete(item.UniqueName);
    }

    public async Task<IEnumerable<FileDescription>> GetAllFilesAsync()
    {
        var filesDbModel = await _cloudStorageUnitOfWork.FileDescription.GetAllFilesAsync(_userService.Current.Email);

        var files = _mapper.Map<IEnumerable<FileDescriptionDbModel>, IEnumerable<FileDescription>>(filesDbModel);

        return files;
    }

    private async Task ValidateFCreatedFile(FileDescriptionDbModel fileDbModel)
    {
        var contentHashExist = await _cloudStorageUnitOfWork.FileDescription.ContentHashExist(fileDbModel.ContentHash, _userService.Current.Email);

        if (contentHashExist)
        {
            throw new FileContentDuplicationException("A file with such content is already exist.");
        }

        var fileNameExist = await _cloudStorageUnitOfWork.FileDescription.FileNameExist(fileDbModel.ProvidedName, _userService.Current.Email);

        if (fileNameExist)
        {
            throw new FileNameDuplicationException("A file with such name is already exist.");
        }
    }
}