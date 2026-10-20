using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25Shop.Core.Domain
{
    public class CurrentWeather
    {
        public string City { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
