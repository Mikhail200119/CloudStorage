using AutoMapper;
using CloudStorage.BLL.Exceptions;
using CloudStorage.BLL.Helpers;
using CloudStorage.BLL.Models;
using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.DAL;
using CloudStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;

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
            var uniqueNames = dbModelsToUpload
                .Select(file => file.UniqueName)
                .ToArray();

            await _fileStorageService.DeleteRangeAsync(uniqueNames);

            throw;
        }

        var fileDescriptions = _mapper.Map<IEnumerable<FileDescriptionDbModel>, IEnumerable<FileDescription>>(dbModelsToUpload);

        return fileDescriptions;
    }

    public async Task<Stream> GetFileStreamAsync(int fileId)
    {
        var item = await _cloudStorageUnitOfWork.FileDescription.GetByIdAsync(fileId);
        ValidateUserPermissions(item);

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
        ValidateUserPermissions(item);

        if (item is null)
        {
            return;
        }

        _cloudStorageUnitOfWork.FileDescription.Delete(item.Id);

        await _cloudStorageUnitOfWork.SaveChangesAsync();

        _fileStorageService.Delete(item.UniqueName);

        if (item.ThumbnailInfo is not null)
        {
            _fileStorageService.Delete(item.ThumbnailInfo.UniqueName);
        }
    }

    public async Task DeleteRangeAsync(IEnumerable<int> ids)
    {
        var fileIds = ids.ToArray();
        _cloudStorageUnitOfWork.FileDescription.DeleteRange(fileIds);
        
        var filesToDelete = _cloudStorageUnitOfWork.FileDescription
            .GetAllFilesAsQueryable(_userService.Current.Email)
            .AsNoTracking()
            .Where(file => ids.Contains(file.Id))
            .ToArray();

        ValidateUserPermissions(filesToDelete);

        await _fileStorageService.DeleteRangeAsync(filesToDelete.Select(file => file.UniqueName).ToArray());

        var thumbs = filesToDelete
            .Where(file => file.ThumbnailInfo is not null)
            .Select(file => file.ThumbnailInfo!.UniqueName)
            .ToArray();

        await _fileStorageService.DeleteRangeAsync(thumbs);
        await _cloudStorageUnitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<FileDescription>> GetAllFilesAsync()
    {
        var filesDbModel = await _cloudStorageUnitOfWork.FileDescription.GetAllFilesAsync(_userService.Current.Email);
        ValidateUserPermissions(filesDbModel.ToArray());

        var files = _mapper.Map<IEnumerable<FileDescriptionDbModel>, IEnumerable<FileDescription>>(filesDbModel);

        var allFilesAsync = files as FileDescription[] ?? files.ToArray();

        var getThumbTasks = allFilesAsync.Select(file =>
        {
            var thumbnailName = filesDbModel.Single(dbModel => dbModel.Id == file.Id).ThumbnailInfo?.UniqueName;

            if (thumbnailName is null)
            {
                return Task.CompletedTask;
            }

            return Task.Run(async () =>
                file.Thumbnail = await _fileStorageService.GetStreamAsync(thumbnailName));
        });

        await Task.WhenAll(getThumbTasks);
        
        /*foreach (var file in allFilesAsync)
        {
            var thumbnailName = filesDbModel.Single(dbModel => dbModel.Id == file.Id).ThumbnailInfo?.UniqueName;

            if (thumbnailName is null)
            {
                continue;
            }

            file.Thumbnail = await _fileStorageService.GetStreamAsync(thumbnailName);
        }*/

        return allFilesAsync;
    }

    public async Task RenameFileAsync(int id, string newName)
    {
        await _cloudStorageUnitOfWork.FileDescription.RenameFileAsync(id, newName);
        await _cloudStorageUnitOfWork.SaveChangesAsync();
    }

    private async Task ValidateCreatedFile(params FileDescriptionDbModel[] fileDbModel)
    {
        var hashes = fileDbModel.Select(model => model.ContentHash);

        var contentHashesExist = await _cloudStorageUnitOfWork.FileDescription.ContentHashesExistAsync(_userService.Current.Email, hashes.ToArray());

        if (contentHashesExist)
        {
            throw new FileContentDuplicationException("A file with such content already exists.");
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
        foreach (var (dbModel, content) in filesInfo)
        {
            content.Seek(0, SeekOrigin.Begin);

            if (ContentTypeDeterminant.IsVideo(dbModel.ContentType) ||
                ContentTypeDeterminant.IsImage(dbModel.ContentType))
            {
                var thumbName = Guid.NewGuid().ToString();
                dbModel.ThumbnailInfo = new ThumbnailInfoDbModel
                {
                    UniqueName = thumbName
                };

                await _fileStorageService.CreateVideoThumbnailAsync(dbModel.UniqueName, thumbName);
            }
        }
    }

    private void ValidateUserPermissions(params FileDescriptionDbModel[]? files)
    {
        var currentUser = _userService.Current.Email;

        if (files?.Any(file => file.UploadedBy != currentUser) == true)
        {
            throw new UnauthorizedUserException("Current user does not have access to some files.");
        }
    }
}