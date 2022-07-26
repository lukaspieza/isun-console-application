﻿namespace isun.Domain.Models;

public class CityWeatherForecast
{
    public string City { get; set; } = null!;
    public string Temperature { get; set; } = null!;
    public string Precipitation { get; set; } = null!;
    public string WindSpeed { get; set; } = null!;
    public string Summary { get; set; } = null!;

    public override string ToString()
    {
        return $"{Temperature}°C, {Precipitation}mm, {WindSpeed}m/s, {Summary}, {City}";
    }
}
