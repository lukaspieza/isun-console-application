using AutoMapper;
using isun.Domain.Models.Options;
using isun.Infrastructure.Models.WeatherApi;

namespace isun.Infrastructure.Mappers;

public class AuthorizationRequestProfile : Profile
{
    public AuthorizationRequestProfile()
    {
        CreateMap<ExternalWeatherApiOptions, AuthorizationRequest>()
            .ForMember(dest =>
                    dest.UserName,
                opt => opt.MapFrom(src => src.AuthorizationUserName))
            .ForMember(dest =>
                    dest.Password,
                opt => opt.MapFrom(src => src.AuthorizationPassword));
    }
}
