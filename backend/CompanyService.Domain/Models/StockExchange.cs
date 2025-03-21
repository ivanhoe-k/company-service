using System.Collections.Generic;
using CompanyService.Core.Common;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;

namespace CompanyService.Domain.Models
{
    /// <summary>
    /// Represents a Stock Exchange, ensuring validity using ISO 10383 MIC codes.
    /// </summary>
    public sealed record StockExchange
    {
        public StockExchangeInfo Value { get; }

        private StockExchange(StockExchangeInfo value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a <see cref="StockExchange"/> instance from name.
        /// </summary>
        /// <param name="name">The exchange name.</param>
        /// <param name="exchangeLookupByName">A dictionary of valid exchange names and MIC codes.</param>
        /// <returns>A valid StockExchange instance or an error.</returns>
        public static Result<CompanyError, StockExchange> CreateFromName(string name, IReadOnlyDictionary<string, string> exchangeLookupByName)
        {
            exchangeLookupByName.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<CompanyError>.Fail<StockExchange>(CompanyError.InvalidExchange);
            }

            var normalizedName = name.Trim().ToUpperInvariant();

            if (!exchangeLookupByName.TryGetValue(normalizedName, out string? micCode))
            {
                return Result<CompanyError>.Fail<StockExchange>(CompanyError.UnknownExchange);
            }

            return Result<CompanyError, StockExchange>.Ok(new StockExchange(
                new StockExchangeInfo(micCode, normalizedName)));
        }

        /// <summary>
        /// Creates a <see cref="StockExchange"/> instance from name.
        /// </summary>
        /// <param name="micCode">The exchange MIC.</param>
        /// <param name="exchangeLookupByMicCode">A dictionary of valid exchange names and MIC codes.</param>
        /// <returns>A valid StockExchange instance or an error.</returns>
        public static Result<CompanyError, StockExchange> CreateFromMicCode(string micCode, IReadOnlyDictionary<string, string> exchangeLookupByMicCode)
        {
            exchangeLookupByMicCode.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(micCode))
            {
                return Result<CompanyError>.Fail<StockExchange>(CompanyError.InvalidExchange);
            }

            var normalizedMicCode = micCode.Trim().ToUpperInvariant();

            if (!exchangeLookupByMicCode.TryGetValue(normalizedMicCode, out string? name))
            {
                return Result<CompanyError>.Fail<StockExchange>(CompanyError.UnknownExchange);
            }

            return Result<CompanyError, StockExchange>.Ok(new StockExchange(
                new StockExchangeInfo(micCode, name)));
        }
    }
}
