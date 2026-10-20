using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.Controllers
{
    public class OpenWeatherController : Controller
    {
        private readonly IWeatherService _service;
        public OpenWeatherController(IWeatherService service) { _service = service; }

        public async  Task<IActionResult> Index(string city = "Tallinn")
        {
            var weather = await _service.GetWeatherAsync(city);
            return View(weather);
        }
    }
}
