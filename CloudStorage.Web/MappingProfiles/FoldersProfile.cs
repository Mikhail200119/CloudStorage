using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.Web.Models;

namespace CloudStorage.Web.MappingProfiles;

public class FoldersProfile : Profile
{
    public FoldersProfile()
    {
        CreateMap<FileFolderCreateModel, FileFolderCreateData>();
        CreateMap<FileFolderUpdateModel, FileFolderUpdateData>();

        CreateMap<FileFolder, FileFolderViewModel>();
    }
}