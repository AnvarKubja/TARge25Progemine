using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface ICocktailServices
    {
        Task<List<CocktailRootDto>> SearchCocktailsAsync(
        string name,
        CancellationToken cancellationToken = default);
    }
}
