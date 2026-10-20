using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25Shop.Core.Dto
{
    public class WeatherDto
    {
        public string Name { get; set; }
        public MainDto Main { get; set; }
        public List<WeatherItemDto> Weather { get; set; }
        public WindDto Wind { get; set; }
    }
    public class MainDto 
    {
        public double Temp { get; set; }
        public int Humidity { get; set; }
    }
    public class WeatherItemDto 
    {
        public string Description { get; set; }
        public string Icon { get; set; }
    }
    public class WindDto
    {
        public double Speed { get; set; }
    }
}
