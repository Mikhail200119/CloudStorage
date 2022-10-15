using System.Security.Cryptography;
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
        CreateMap<FileCreateData, FileDescriptionDbModel>()
            .ForMember(dest => dest.ProvidedName,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.UniqueName,
                expression => expression.MapFrom(src => Guid.NewGuid().ToString()))
            .ForPath(dest => dest.FileContent.Content,
                opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.ContentHash,
                opt => 
                    opt.MapFrom(src => GetContentHashFromByteArray(src.Content)));

        CreateMap<FileDescriptionDbModel, File>()
            .ForMember(dest => dest.Name, expression =>
                expression.MapFrom(source => source.ProvidedName));

        CreateMap<FileUpdateData, FileDescriptionDbModel>();
    }

    private static string GetContentHashFromByteArray(byte[] data) => 
        string.Concat(SHA1.HashData(data).Select(byteElement => byteElement.ToString("X2")));
}