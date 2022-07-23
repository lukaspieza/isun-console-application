using AutoMapper;
using isun.Domain.Models;
using isun.Infrastructure.Models.WeatherApi;

namespace isun.Infrastructure.Mappers;

public class CityWeatherForecastProfile : Profile
{
    public CityWeatherForecastProfile()
    {
        CreateMap<CityWeatherForecastResponse, CityWeatherForecast>()
            .ForMember(dest =>
                    dest.City,
                opt => opt.MapFrom(src => src.City))
            .ForMember(dest =>
                    dest.Precipitation,
                opt => opt.MapFrom(src => src.Precipitation))
            .ForMember(dest =>
                    dest.Summary,
                opt => opt.MapFrom(src => src.Summary))
            .ForMember(dest =>
                    dest.Temperature,
                opt => opt.MapFrom(src => src.Temperature))
            .ForMember(dest =>
                    dest.WindSpeed,
                opt => opt.MapFrom(src => src.WindSpeed));
    }
}
