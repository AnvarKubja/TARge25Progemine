
using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.Controllers
{
    public class CocktailController : Controller
    {
        private readonly ICocktailServices _cocktailServices;

        public CocktailController(ICocktailServices cocktailServices)
        {
            _cocktailServices = cocktailServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new List<TARge25Shop.Core.Dto.CocktailRootDto>());
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            string name,
            CancellationToken cancellationToken)
        {
            ViewBag.SearchName = name;

            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Message = "Sisesta kokteili nimi.";
                return View("Index",
                    new List<TARge25Shop.Core.Dto.CocktailRootDto>());
            }

            try
            {
                var cocktails = await _cocktailServices
                    .SearchCocktailsAsync(name, cancellationToken);

                if (cocktails.Count == 0)
                {
                    ViewBag.Message = "Kokteili ei leitud.";
                }

                return View("Index", cocktails);
            }
            catch (HttpRequestException)
            {
                ViewBag.Message = "Kokteilide API pole praegu kättesaadav.";

                return View("Index",
                    new List<TARge25Shop.Core.Dto.CocktailRootDto>());
            }
        }
    }
}
