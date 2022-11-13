using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.DAL.Entities;

namespace CloudStorage.BLL.MappingProfiles;

public class FolderMappingProfile : Profile
{
    public FolderMappingProfile()
    {
        CreateMap<FileFolderCreateData, FileFolderDbModel>();
        CreateMap<FileFolderUpdateData, FileFolderDbModel>();

        CreateMap<FileFolderDbModel, FileFolder>();
    }
}