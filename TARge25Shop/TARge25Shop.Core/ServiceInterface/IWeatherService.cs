using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Domain;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IWeatherService
    {
        Task<CurrentWeather> GetWeatherAsync(string city);
    }
}
