using System;
using System.Collections.Generic;
using CompanyService.Core.Common;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;

namespace CompanyService.Domain.Models
{
    /// <summary>
    /// Represents a company entity with core business attributes.
    /// This follows the Always Valid Model principle, using a factory method that returns a result.
    /// </summary>
    public sealed record Company
    {
        public Guid Id { get; }

        public string Name { get; }

        public StockExchange Exchange { get; }

        public string Ticker { get; }

        public Isin Isin { get; }

        public Uri? Website { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Company"/> class.
        /// The private constructor enforces controlled creation through the factory method.
        /// </summary>
        private Company(Guid id, string name, StockExchange exchange, string ticker, Isin isin, Uri? website)
        {
            Id = id;
            Name = name;
            Exchange = exchange;
            Ticker = ticker;
            Isin = isin;
            Website = website;
        }

        /// <summary>
        /// Creates a new <see cref="Company"/> entity, ensuring all validation rules are met.
        /// </summary>
        /// <param name="createRequest">The request containing the details of the new company.</param>
        /// <param name="exchangeLookupByName">
        /// A dictionary mapping exchange names to MIC codes, ensuring the provided exchange is valid.
        /// </param>
        /// <returns>
        /// A <see cref="Result{TError, TValue}"/> containing the created <see cref="Company"/> entity 
        /// if successful, or an error result if validation fails.
        /// </returns>
        public static Result<CompanyError, Company> Create(
            CreateCompanyRequest createRequest,
            IReadOnlyDictionary<string, string> exchangeLookupByName)
        {
            createRequest.ThrowIfNull();
            exchangeLookupByName.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(createRequest.Name))
            {
                return Result<CompanyError>.Fail<Company>(CompanyError.InvalidName);
            }

            if (string.IsNullOrWhiteSpace(createRequest.Ticker))
            {
                return Result<CompanyError>.Fail<Company>(CompanyError.InvalidTicker);
            }

            var exchangeResult = StockExchange.CreateFromName(createRequest.ExchangeName, exchangeLookupByName);

            if (exchangeResult.Failed)
            {
                return Result<CompanyError>.Fail<Company>(exchangeResult.Error!);
            }

            var isinResult = Isin.Create(createRequest.Isin);

            if (isinResult.Failed)
            {
                return Result<CompanyError>.Fail<Company>(isinResult.Error!);
            }

            var validWebsiteResult = ValidateAndGetWebsite(createRequest.Website);

            if (validWebsiteResult.Failed)
            {
                return Result<CompanyError>.Fail<Company>(validWebsiteResult.Error!);
            }

            return Result<CompanyError>.Ok(new Company(
                id: Guid.NewGuid(),
                name: createRequest.Name.Trim(),
                exchange: exchangeResult.Value,
                ticker: createRequest.Ticker.Trim(),
                isin: isinResult.Value,
                website: validWebsiteResult.Value));
        }

        public static Result<CompanyError, Company> Create(
            CompanyDto companyDto,
            IReadOnlyDictionary<string, string> exchangeLookupByMicCode)
        {
            companyDto.ThrowIfNull();
            exchangeLookupByMicCode.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(companyDto.Name))
            {
                return Result<CompanyError>.Fail<Company>(CompanyError.InvalidName);
            }

            if (string.IsNullOrWhiteSpace(companyDto.Ticker))
            {
                return Result<CompanyError>.Fail<Company>(CompanyError.InvalidTicker);
            }

            var exchangeResult = StockExchange.CreateFromMicCode(companyDto.ExchangeMicCode, exchangeLookupByMicCode);

            if (exchangeResult.Failed)
            {
                return Result<CompanyError>.Fail<Company>(exchangeResult.Error!);
            }

            var isinResult = Isin.Create(companyDto.Isin);

            if (isinResult.Failed)
            {
                return Result<CompanyError>.Fail<Company>(isinResult.Error!);
            }

            return Result<CompanyError>.Ok(new Company(
                id: companyDto.Id,
                name: companyDto.Name.Trim(),
                exchange: exchangeResult.Value,
                ticker: companyDto.Ticker.Trim(),
                isin: isinResult.Value,
                website: companyDto.Website));
        }

        /// <summary>
        /// Updates an existing <see cref="Company"/> entity with new values.
        /// </summary>
        /// <param name="updateRequest">The request containing updated company information.</param>
        /// <param name="exchangeLookupByName">
        /// A dictionary mapping exchange names to MIC codes, ensuring the provided exchange is valid.
        /// </param>
        /// <returns>
        /// A <see cref="Result{TError, TValue}"/> containing the updated <see cref="Company"/> entity 
        /// if the update is successful, or an error result if validation fails.
        /// </returns>
        /// <remarks>
        /// - Immutable Fields: `Id` and `Isin` remain unchanged after creation.
        /// </remarks>
        public Result<CompanyError, Company> Update(
            UpdateCompanyRequest updateRequest,
            IReadOnlyDictionary<string, string> exchangeLookupByName)
        {
            updateRequest.ThrowIfNull();
            exchangeLookupByName.ThrowIfNull();

            var exchangeResult = UpdateExchange(updateRequest.ExchangeName, exchangeLookupByName);

            if (exchangeResult.Failed)
            {
                return Result<CompanyError>.Fail<Company>(exchangeResult.Error!);
            }

            var websiteResult = UpdateWebsite(updateRequest.Website);

            if (websiteResult.Failed)
            {
                return Result<CompanyError>.Fail<Company>(websiteResult.Error!);
            }

            return Result<CompanyError>.Ok(new Company(
                id: Id,
                name: UpdateName(updateRequest.Name),
                exchange: exchangeResult.Value,
                ticker: UpdateTicker(updateRequest.Ticker),
                isin: Isin,
                website: websiteResult.Value));
        }

        private static Result<CompanyError, Uri?> ValidateAndGetWebsite(string? website)
        {
            var validWebsite = default(Uri);

            if (!string.IsNullOrWhiteSpace(website) && !Uri.TryCreate(website.Trim(), UriKind.Absolute, out validWebsite))
            {
                return Result<CompanyError>.Fail<Uri?>(CompanyError.InvalidWebsite);
            }

            return Result<CompanyError, Uri?>.Ok(validWebsite);
        }

        private Result<CompanyError, StockExchange> UpdateExchange(
            string? exchangeName,
            IReadOnlyDictionary<string, string> exchangeLookupByName)
        {
            if (string.IsNullOrWhiteSpace(exchangeName))
            {
                return Result<CompanyError>.Ok(Exchange);
            }

            return StockExchange.CreateFromName(exchangeName, exchangeLookupByName);
        }

        private string UpdateName(string? name)
        {
            return string.IsNullOrWhiteSpace(name) ? Name : name.Trim();
        }

        private string UpdateTicker(string? ticker)
        {
            return string.IsNullOrWhiteSpace(ticker) ? Ticker : ticker.Trim();
        }

        private Result<CompanyError, Uri?> UpdateWebsite(string? website)
        {
            if (string.IsNullOrWhiteSpace(website))
            {
                return Result<CompanyError>.Ok(default(Uri));
            }

            return ValidateAndGetWebsite(website);
        }
    }
}
