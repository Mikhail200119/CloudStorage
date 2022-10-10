using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.Common.Extensions;
using CloudStorage.Web.Models;
using File = CloudStorage.BLL.Models.File;

namespace CloudStorage.Web.MappingProfiles;

public class FileProfile : Profile
{
    public FileProfile()
    {
        CreateMap<FileCreateModel, FileCreateData>()
            .ForMember(dest => dest.Name, expression => expression.MapFrom(src => src.FormFile.FileName))
            .ForMember(dest => dest.Content, opt => 
                opt.MapFrom(src => src.FormFile.ToByteArray()));

        CreateMap<FileUpdateModel, FileUpdateData>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FormFile.Name))
            .ForMember(dest => dest.Content, opt =>
                opt.MapFrom(src => src.FormFile.ToByteArray()));
            
        CreateMap<File, FileViewModel>();
    }
}