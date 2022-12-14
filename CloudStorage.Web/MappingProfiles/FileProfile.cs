using AutoMapper;
using CloudStorage.BLL.Models;
using CloudStorage.Web.Models;

namespace CloudStorage.Web.MappingProfiles;

public class FileProfile : Profile
{
    public FileProfile()
    {
        /*CreateMap<FileCreateModel, FileCreateData>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.FormFiles.FileName))
            .ForMember(dest => dest.Content, opt =>
                opt.MapFrom(src => src.FormFiles.OpenReadStream()))
            .ForMember(dest => dest.ContentType,
                opt => opt.MapFrom(src => src.FormFiles.ContentType));
                */

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

        CreateMap<FileDescription, FileViewModel>();
    }
}