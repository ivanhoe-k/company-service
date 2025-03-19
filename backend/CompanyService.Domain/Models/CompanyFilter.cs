using CompanyService.Core.Models;
using CompanyService.Domain.Errors;

namespace CompanyService.Domain.Models
{
    public sealed record CompanyFilter
    {
        public string? Name { get; }

        public string? Exchange { get; }

        public string? Ticker { get; }

        public Isin? Isin { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyFilter"/> class.
        /// </summary>
        /// <param name="name">The company name filter (optional).</param>
        /// <param name="exchange">The exchange filter (optional).</param>
        /// <param name="ticker">The ticker filter (optional).</param>
        /// <param name="isin">The ISIN filter (optional).</param>
        private CompanyFilter(string? name = null, string? exchange = null, string? ticker = null, Isin? isin = null)
        {
            Name = name?.Trim();
            Exchange = exchange?.Trim();
            Ticker = ticker?.Trim();
            Isin = isin;
        }

        public static Result<CompanyError, CompanyFilter> Create(
            string? name, string? exchange, string? ticker, string? isin)
        {
            var isinModel = default(Isin?);

            if (!string.IsNullOrWhiteSpace(isin))
            {
                var isinResult = Isin.Create(isin);

                if (isinResult.Failed)
                {
                    return Result<CompanyError>.Fail<CompanyFilter>(isinResult.Error!);
                }

                isinModel = isinResult.Value;
            }

            return Result<CompanyError>.Ok(new CompanyFilter(
                name: name, 
                exchange: exchange, 
                ticker: ticker, 
                isin: isinModel));
        }

        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(Name) &&
            string.IsNullOrWhiteSpace(Exchange) &&
            string.IsNullOrWhiteSpace(Ticker) &&
            Isin is null;
    }
}
