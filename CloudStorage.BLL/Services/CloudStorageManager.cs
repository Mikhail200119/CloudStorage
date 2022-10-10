using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.DAL;
using CloudStorage.DAL.Entities;
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
        var fileDbModel = _mapper.Map<FileCreateData, FileDbModel>(newFile);

        fileDbModel.UserName = _userService.Current.Email;

        await _cloudStorageUnitOfWork.Files.CreateAsync(fileDbModel);

        await _cloudStorageUnitOfWork.SaveChangesAsync();

        var file = _mapper.Map<FileDbModel, File>(fileDbModel);

        return file;
    }

    public async Task<File> UpdateAsync(FileUpdateData existingFile)
    {
        var fileDbModel = _mapper.Map<FileUpdateData, FileDbModel>(existingFile);

        await Task.Run(() => _cloudStorageUnitOfWork.Files.Update(fileDbModel));

        var file = _mapper.Map<FileDbModel, File>(fileDbModel);

        return file;
    }

    public void Delete(int id)
    {
        _cloudStorageUnitOfWork.Files.Delete(id);
    }

    public async Task<IEnumerable<File>> GetAllFiles()
    {
        var filesDbModel = await _cloudStorageUnitOfWork.Files.GetAllFilesAsync(_userService.Current.Email);

        var files = _mapper.Map<IEnumerable<FileDbModel>, IEnumerable<File>>(filesDbModel);

        return files;
    }
}