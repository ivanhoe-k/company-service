using System;

namespace CompanyService.Domain.Models
{
    public sealed record CompanyDto
    {
        public Guid Id { get; }

        public string Name { get; }

        public string ExchangeMicCode { get; }

        public string Ticker { get; }

        public string Isin { get; }

        public Uri? Website { get; init; }

        public CompanyDto(Guid id, string name, string exchangeMicCode, string ticker, string isin, Uri? website)
        {
            Id = id;
            Name = name;
            ExchangeMicCode = exchangeMicCode;
            Ticker = ticker;
            Isin = isin;
            Website = website;
        }
    }
}
