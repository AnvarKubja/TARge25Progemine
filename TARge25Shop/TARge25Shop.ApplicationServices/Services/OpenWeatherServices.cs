using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public WeatherService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["OpenWeather:ApiKey"];
        }

        public async Task<CurrentWeather> GetWeatherAsync(string city)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric&lang=et";
            var dto = await _http.GetFromJsonAsync<WeatherDto>(url);

            // DTO -> Domain
            return new CurrentWeather
            {
                City = dto.Name,
                Temperature = dto.Main.Temp,
                Humidity = dto.Main.Humidity,
                WindSpeed = dto.Wind.Speed,
                Description = dto.Weather[0].Description,
                Icon = dto.Weather[0].Icon
            };
        }
    }
}

