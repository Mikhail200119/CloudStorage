using AutoMapper;
using CloudStorage.BLL.Helpers;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Services.Interfaces;
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

    public async Task<FileDescription> CreateFileAsync(FileCreateData newFile)
    {
        var fileDbModel = _mapper.Map<FileCreateData, FileDescriptionDbModel>(newFile);
        fileDbModel.UploadedBy = _userService.Current.Email;
        fileDbModel.ContentHash = _dataHasher.HashData(newFile.Content);

        await _fileStorageService.UploadAsync(fileDbModel.UniqueName, newFile.Content);

        if (ContentTypeDeterminant.IsImage(newFile.ContentType))
        {
            fileDbModel.Preview = _fileStorageService.CompressImage(newFile.Content);
        }
        else if (ContentTypeDeterminant.IsVideo(newFile.ContentType))
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

    public async Task<byte[]> GetFileContentAsync(int fileId)
    {
        var item = await _cloudStorageUnitOfWork.FileDescription.GetByIdAsync(fileId);

        var content = await _fileStorageService.GetAsync(item.UniqueName);

        return content;
    }

    public async Task<FileDescription> UpdateFileAsync(FileUpdateData existingFile)
    {
        var fileDbModel = _mapper.Map<FileUpdateData, FileDescriptionDbModel>(existingFile);

        _cloudStorageUnitOfWork.FileDescription.Update(fileDbModel);
        await _cloudStorageUnitOfWork.SaveChangesAsync();

        var file = _mapper.Map<FileDescriptionDbModel, FileDescription>(fileDbModel);

        return file;
    }

    public async Task DeleteFileAsync(int id)
    {
        var item = await _cloudStorageUnitOfWork.FileDescription.GetByIdAsync(id);

        _cloudStorageUnitOfWork.FileDescription.Delete(item.Id);

        await _cloudStorageUnitOfWork.SaveChangesAsync();

        _fileStorageService.Delete(item.UniqueName);
    }

    public async Task<IEnumerable<FileDescription>> GetAllFilesAsync()
    {
        var allFiles = await _cloudStorageUnitOfWork.FileDescription.GetAllFilesAsync(_userService.Current.Email);
        var allFolders = await _cloudStorageUnitOfWork.FileFolder.GetAllFoldersByIdsAsync(allFiles.Select(file => file.FolderId));

        var folders = await _cloudStorageUnitOfWork.FileFolder.GetAllFoldersAsync(_userService.Current.Email);

        var fileDescriptions = _mapper.Map<IEnumerable<FileDescriptionDbModel>, IEnumerable<FileDescription>>(allFiles);
        var fileFolders = _mapper.Map<IEnumerable<FileFolderDbModel>, IEnumerable<FileFolder>>(allFolders);

        foreach (var file in fileDescriptions)
        {
            file.Folder = fileFolders.Single(folder => folder.Id == file.Folder.Id);
        }

        return fileDescriptions;
    }

    public async Task CreateFolderAsync(FileFolderCreateData folder)
    {
        var folderDbModel = _mapper.Map<FileFolderCreateData, FileFolderDbModel>(folder);

        await _cloudStorageUnitOfWork.FileFolder.CreateAsync(folderDbModel);

        await _cloudStorageUnitOfWork.SaveChangesAsync();
    }

    public async Task UpdateFolderAsync(FileFolderUpdateData folder)
    {
        var folderDbModel = _mapper.Map<FileFolderUpdateData, FileFolderDbModel>(folder);

        _cloudStorageUnitOfWork.FileFolder.Update(folderDbModel);

        await _cloudStorageUnitOfWork.SaveChangesAsync();
    }

    public async Task DeleteFolderAsync(int id)
    {
        _cloudStorageUnitOfWork.FileFolder.Delete(id);

        await _cloudStorageUnitOfWork.SaveChangesAsync();
    }

    public async Task<int> GetRootFolderIdAsync() => await _cloudStorageUnitOfWork.FileFolder.GetRootFolderIdAsync();
    public async Task<IEnumerable<FileFolder>> GetAllFoldersAsync()
    {
        var foldersDbModel = await _cloudStorageUnitOfWork.FileFolder.GetAllFoldersAsync(_userService.Current.Email);

        var folders = _mapper.Map<IEnumerable<FileFolderDbModel>, IEnumerable<FileFolder>>(foldersDbModel);

        return folders;
    }
}