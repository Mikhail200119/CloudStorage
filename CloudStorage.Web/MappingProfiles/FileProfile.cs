using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.Common.Extensions;
using CloudStorage.Web.Models;

namespace CloudStorage.Web.MappingProfiles;

public class FileProfile : Profile
{
    public FileProfile()
    {
        CreateMap<FileUpdateModel, FileUpdateData>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FormFile.Name))
            .ForMember(dest => dest.Content, opt =>
                opt.MapFrom(src => src.FormFile.OpenReadStream()));

        CreateMap<IFormFile, FileCreateData>()
            .ForMember(dest => dest.Content,
                opt => opt.MapFrom(src => src.OpenReadStream()))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.ContentType,
                opt => opt.MapFrom(src => src.ContentType));

        CreateMap<FileDescription, FileViewModel>()
            .ForMember(dest => dest.Thumbnail,
                opt => opt.MapFrom(src => src.Thumbnail.ToArray()));
    }
}