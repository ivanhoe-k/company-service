using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;

namespace CompanyService.Domain.Contracts
{
    /// <summary>
    /// Port interface for retrieving stock exchange information.
    /// Defines a contract for infrastructure adapters to provide exchange data.
    /// </summary>
    public interface IStockExchangeProvider
    {
        Task<Result<CompanyError, IReadOnlyCollection<StockExchangeInfo>>> GetStockExchangesAsync(CancellationToken cancellationToken);
    }
}
