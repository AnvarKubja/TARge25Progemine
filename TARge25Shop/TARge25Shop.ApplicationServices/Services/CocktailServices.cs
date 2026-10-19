
using System.Text.Json;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services
{
    public class CocktailServices : ICocktailServices
    {
        private readonly HttpClient _httpClient;

        public CocktailServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CocktailRootDto>> SearchCocktailsAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<CocktailRootDto>();
            }

            var url = $"search.php?s={Uri.EscapeDataString(name.Trim())}";

            using var response = await _httpClient.GetAsync(
                url, cancellationToken);

            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(
                cancellationToken);

            var result = await JsonSerializer.DeserializeAsync<CocktailRootDto>(
                stream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                },
                cancellationToken);

            return result?.Drinks ?? new List<CocktailRootDto>();
        }
    }
}
