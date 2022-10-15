using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.DAL;
using CloudStorage.DAL.Entities;
using CloudStorage.DAL.Exceptions;
using File = CloudStorage.BLL.Models.File;

namespace CloudStorage.BLL.Services;

public class CloudStorageManager : ICloudStorageManager
{
    private readonly ICloudStorageUnitOfWork _cloudStorageUnitOfWork;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public CloudStorageManager(ICloudStorageUnitOfWork cloudStorageUnitOfWork, IMapper mapper, IUserService userService)
    {
        _cloudStorageUnitOfWork = cloudStorageUnitOfWork;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<File> CreateAsync(FileCreateData newFile)
    {
        var fileDbModel = _mapper.Map<FileCreateData, FileDescriptionDbModel>(newFile);
        fileDbModel.UserName = _userService.Current.Email;

        var contentHashes = await _cloudStorageUnitOfWork.FileDescription.GetContentHashesAsync(_userService.Current.Email);

        if (contentHashes.Contains(fileDbModel.ContentHash))
        {
            throw new FileDuplicationException("A file with such content is already exist.");
        }

        await _cloudStorageUnitOfWork.FileDescription.CreateAsync(fileDbModel);
        await _cloudStorageUnitOfWork.SaveChangesAsync();

        var file = _mapper.Map<FileDescriptionDbModel, File>(fileDbModel);

        return file;
    }

    public async Task<File> GetByIdAsync(int id)
    {
        var fileDbModel = await _cloudStorageUnitOfWork.FileDescription.GetByIdAsync(id);

        var file = _mapper.Map<FileDescriptionDbModel, File>(fileDbModel);

        return file;
    }

    public async Task<byte[]> GetContentByFileDescriptionIdAsync(int id)
    {
        var content = await _cloudStorageUnitOfWork.FileContent.GetFileContentByIdAsync(id);

        return content?.Content ?? Array.Empty<byte>();
    }

    public async Task<File> UpdateAsync(FileUpdateData existingFile)
    {
        var fileDbModel = _mapper.Map<FileUpdateData, FileDescriptionDbModel>(existingFile);

        _cloudStorageUnitOfWork.FileDescription.Update(fileDbModel);
        await _cloudStorageUnitOfWork.SaveChangesAsync();

        var file = _mapper.Map<FileDescriptionDbModel, File>(fileDbModel);

        return file;
    }

    public async Task DeleteAsync(int id)
    {
        _cloudStorageUnitOfWork.FileDescription.Delete(id);

        await _cloudStorageUnitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<File>> GetAllFilesAsync()
    {
        var filesDbModel = await _cloudStorageUnitOfWork.FileDescription.GetAllFilesAsync(_userService.Current.Email);

        var files = _mapper.Map<IEnumerable<FileDescriptionDbModel>, IEnumerable<File>>(filesDbModel);

        return files;
    }
}