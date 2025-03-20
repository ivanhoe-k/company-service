using System;
using CompanyService.Persistence.Models;

namespace CompanyService.Persistence
{
    public static class SeedData
    {
        public static CompanyEntity[] GetCompanies()
        {
            return new[]
            {
                new CompanyEntity
                {
                    Id = new Guid("3E5B91E5-6A2F-4F77-8B25-4A6BCA7263B1"),
                    Name = "Apple Inc.",
                    ExchangeMicCode = "XNAS",
                    Ticker = "AAPL",
                    Isin = "US0378331005",
                    Website = new Uri("https://www.apple.com")
                },
                new CompanyEntity
                {
                    Id = new Guid("66E0B1F9-13B3-4B63-B9E6-77DAA21E2F7F"),
                    Name = "Microsoft Corporation",
                    ExchangeMicCode = "XNAS",
                    Ticker = "MSFT",
                    Isin = "US5949181045",
                    Website = new Uri("https://www.microsoft.com")
                },
                new CompanyEntity
                {
                    Id = new Guid("A2828BFC-254B-4DF1-9A8A-95F8F0DCCF47"),
                    Name = "Amazon.com Inc.",
                    ExchangeMicCode = "XNAS",
                    Ticker = "AMZN",
                    Isin = "US0231351067",
                    Website = new Uri("https://www.amazon.com")
                },
                new CompanyEntity
                {
                    Id = new Guid("847E915F-56A7-4C8D-B7D1-ABF2E1D496AD"),
                    Name = "Alphabet Inc. (Google)",
                    ExchangeMicCode = "XNAS",
                    Ticker = "GOOGL",
                    Isin = "US02079K3059",
                    Website = new Uri("https://www.abc.xyz")
                },
                new CompanyEntity
                {
                    Id = new Guid("A56F7C6C-88F8-4E77-96D8-58F82C4D3B94"),
                    Name = "Meta Platforms Inc. (Facebook)",
                    ExchangeMicCode = "XNAS",
                    Ticker = "META",
                    Isin = "US30303M1027",
                    Website = null
                },
                new CompanyEntity
                {
                    Id = new Guid("D8A2FE5F-9D3C-4B17-AE9C-46C6B8B3B6A2"),
                    Name = "Tesla Inc.",
                    ExchangeMicCode = "XNAS",
                    Ticker = "TSLA",
                    Isin = "US88160R1014",
                    Website = new Uri("https://www.tesla.com")
                },
                new CompanyEntity
                {
                    Id = new Guid("3F1E3C45-44BB-40B9-BE5E-0E54D36CFB49"),
                    Name = "NVIDIA Corporation",
                    ExchangeMicCode = "XNAS",
                    Ticker = "NVDA",
                    Isin = "US67066G1040",
                    Website = null
                },
                new CompanyEntity
                {
                    Id = new Guid("56D9F87D-EC24-4F88-8339-7E2A9D9D50F2"),
                    Name = "Visa Inc.",
                    ExchangeMicCode = "XNYS",
                    Ticker = "V",
                    Isin = "US92826C8394",
                    Website = new Uri("https://www.visa.com")
                },
                new CompanyEntity
                {
                    Id = new Guid("7CB6E2C8-8F9D-40E8-BBD6-54730B2C2C0A"),
                    Name = "Johnson & Johnson",
                    ExchangeMicCode = "XNYS",
                    Ticker = "JNJ",
                    Isin = "US4781601046",
                    Website = null
                },
                new CompanyEntity
                {
                    Id = new Guid("9FBAA3C6-CE8E-48B6-908E-6BAF6A0A3DB7"),
                    Name = "The Coca-Cola Company",
                    ExchangeMicCode = "XNYS",
                    Ticker = "KO",
                    Isin = "US1912161007",
                    Website = new Uri("https://www.coca-colacompany.com")
                }
            };
        }
    }
}
