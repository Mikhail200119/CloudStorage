using AutoMapper;
using CloudStorage.DAL.Entities;
using File = CloudStorage.BLL.Models.File;
using FileCreateData = CloudStorage.BLL.Models.FileCreateData;
using FileUpdateData = CloudStorage.BLL.Models.FileUpdateData;

namespace CloudStorage.BLL.MappingProfiles;

public class FilesMappingProfile : Profile
{
    public FilesMappingProfile()
    {
        CreateMap<FileCreateData, FileDbModel>()
            .ForMember(dest => dest.ProvidedName,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.UniqueName, 
                expression => expression.MapFrom(src => Guid.NewGuid().ToString()));

        CreateMap<FileDbModel, File>()
            .ForMember(dest => dest.Name, expression =>
                expression.MapFrom(source => source.ProvidedName));

        CreateMap<FileUpdateData, FileDbModel>();
    }
}