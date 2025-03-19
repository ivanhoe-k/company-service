using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;

namespace CompanyService.Domain.Contracts
{
    public interface IStockExchangeProvider
    {
        Task<Result<CompanyError, IReadOnlyCollection<StockExchangeInfo>>> GetStockExchangesAsync(CancellationToken cancellationToken);
    }
}
