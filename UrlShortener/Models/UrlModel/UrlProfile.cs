using AutoMapper;
using UrlShortener.DataAccess.Entities;

namespace UrlShortener.Models.UrlModel;

public class UrlProfile : Profile
{
    public UrlProfile()
    {
        CreateMap<UrlEntity, UrlGetDto>()
            .ForMember(x => x.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                x => x.Url,
                opt => opt.MapFrom(src => src.Url))
            .ForMember(
                x => x.ShortUrl,
                opt => opt.MapFrom(src => src.ShortUrl));

        CreateMap<UrlUpdateDto, UrlEntity>()
            .ForMember(
                x => x.Url,
                opt => opt.MapFrom(src => src.Url))
            .ForMember(
                x => x.ShortUrl,
                opt => opt.MapFrom(src => src.ShortUrl))
            .IgnoreAllPropertiesWithAnInaccessibleSetter();

        CreateMap<UrlAddDto, UrlEntity>()
            .ForMember(
                x => x.Url,
                opt => opt.MapFrom(src => src.Url))
            .AfterMap((_, dest) =>
            {
                dest.Id = Guid.NewGuid().ToString();
                dest.InsertDate = DateTime.UtcNow;
            })
            .IgnoreAllPropertiesWithAnInaccessibleSetter();
    }
}