using AutoMapper;
using CloudStorage.BLL.Exceptions;
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

    public async Task<FileDescription> CreateAsync(FileCreateData newFile)
    {
        var fileDbModel = _mapper.Map<FileCreateData, FileDescriptionDbModel>(newFile);
        fileDbModel.UploadedBy = _userService.Current.Email;

        fileDbModel.ContentHash = _dataHasher.HashStreamData(newFile.Content);

        await ValidateCreatedFile(fileDbModel);

        await _fileStorageService.UploadStreamAsync(fileDbModel.UniqueName, newFile.Content);

        if (ContentTypeDeterminant.IsImage(fileDbModel.ContentType))
        {
            fileDbModel.Preview = _fileStorageService.CompressImage(newFile.Content);
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

    public async Task<IEnumerable<FileDescription>> CreateAsync(IEnumerable<FileCreateData> files)
    {
        var filesArray = files.ToArray();
        var filesDbModels = _mapper.Map<IEnumerable<FileCreateData>, IEnumerable<FileDescriptionDbModel>>(filesArray);
        var f = filesArray.DistinctBy(data => data.Name);

        if (f.Count() < filesArray.Length)
        {
            throw new FileNameDuplicationException("Some files have the same name.");
        }

        var dbModelsToUpload = filesDbModels.Select(model =>
        {
            var content = filesArray.Single(file => file.Name == model.ProvidedName).Content;
            model.ContentHash = _dataHasher.HashStreamData(content);
            model.UploadedBy = _userService.Current.Email;

            return model;
        }).ToArray();

        var namesWithContents = new List<(string FileName, Stream Content)>();

        foreach (var fileCreateData in filesArray)
        {
            var uniqueName = dbModelsToUpload.Single(file => file.ProvidedName == fileCreateData.Name).UniqueName;

            namesWithContents.Add((uniqueName, fileCreateData.Content));
        }

        await _fileStorageService.UploadRangeAsync(namesWithContents);
        
        var dbModelsWithContent = dbModelsToUpload.Select(model =>
        {
            var content = filesArray.Single(file => file.Name == model.ProvidedName).Content;

            return (model, content);
        }).ToArray();

        await ValidateCreatedFile(dbModelsToUpload);
        await SetFilesThumbnail(dbModelsWithContent);

        try
        {
            await _cloudStorageUnitOfWork.FileDescription.CreateRangeAsync(dbModelsToUpload);
            await _cloudStorageUnitOfWork.SaveChangesAsync();
        }
        catch
        {
            var uniqueNames = dbModelsToUpload.Select(file => file.UniqueName);

            foreach (var name in uniqueNames)
            {
                _fileStorageService.Delete(name);
            }

            throw;
        }

        var fileDescriptions = _mapper.Map<IEnumerable<FileDescriptionDbModel>, IEnumerable<FileDescription>>(dbModelsToUpload);

        return fileDescriptions;
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

    public async Task<string> GetUniqueNameAsync(int fileId)
    {
        var file = await _cloudStorageUnitOfWork.FileDescription.GetByIdAsync(fileId);

        return file.UniqueName;
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

    private async Task ValidateCreatedFile(params FileDescriptionDbModel[] fileDbModel)
    {
        var hashes = fileDbModel.Select(model => model.ContentHash);

        var contentHashesExist = await _cloudStorageUnitOfWork.FileDescription.ContentHashesExistAsync(_userService.Current.Email, hashes.ToArray());

        if (contentHashesExist)
        {
            throw new FileContentDuplicationException("A file with such content is already exist.");
        }

        var fileNames = fileDbModel.Select(file => file.ProvidedName);

        var fileNamesExist = await _cloudStorageUnitOfWork.FileDescription.FileNamesExist(_userService.Current.Email, fileNames.ToArray());

        if (fileNamesExist)
        {
            throw new FileNameDuplicationException("A file with such name is already exist.");
        }
    }

    private async Task SetFilesThumbnail(params (FileDescriptionDbModel DbModel, Stream Content)[] filesInfo)
    {
        foreach (var fileDbModel in filesInfo)
        {
            fileDbModel.Content.Seek(0, SeekOrigin.Begin);

            if (ContentTypeDeterminant.IsImage(fileDbModel.DbModel.ContentType))
            {
                fileDbModel.DbModel.Preview = _fileStorageService.CompressImage(fileDbModel.Content);
            }
            else if (ContentTypeDeterminant.IsVideo(fileDbModel.DbModel.ContentType))
            {
                var thumbnail = await _fileStorageService.GetVideoThumbnailAsync(fileDbModel.DbModel.UniqueName);
                fileDbModel.DbModel.Preview = _fileStorageService.CompressImage(thumbnail);
            }
        }
    }
}