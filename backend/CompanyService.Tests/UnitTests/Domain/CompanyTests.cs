using System;
using System.Collections.Generic;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;
using FluentAssertions;

namespace CompanyService.Tests.UnitTests.Domain
{
    public class CompanyTests
    {
        private readonly IReadOnlyDictionary<string, string> _exchangeLookup = new Dictionary<string, string>()
        {
            { "NASDAQ", "XNAS" },
            { "NEW YORK STOCK EXCHANGE", "XNYS" },
            { "EURONEXT AMSTERDAM", "XAMS" }
        };

        [Fact]
        public void Create_ValidCompany_ShouldSucceed()
        {
            // Arrange
            var request = new CreateCompanyRequest("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com");

            // Act
            var result = Company.Create(request, _exchangeLookup);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Name.Should().Be("Apple Inc.");
            result.Value.Exchange.Value.MicCode.Should().Be("XNAS");
            result.Value.Ticker.Should().Be("AAPL");
            result.Value.Isin.Value.Should().Be("US0378331005");
            result.Value.Website.Should().Be(new Uri("http://www.apple.com"));
        }

        [Theory]
        [InlineData("", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com", CompanyErrorCode.InvalidName)] // Name cannot be empty
        [InlineData("Apple Inc.", "INVALID_EXCHANGE", "AAPL", "US0378331005", "http://www.apple.com", CompanyErrorCode.UnknownExchange)]
        [InlineData("Apple Inc.", "NASDAQ", "", "US0378331005", "http://www.apple.com", CompanyErrorCode.InvalidTicker)] // Ticker cannot be empty
        [InlineData("Apple Inc.", "NASDAQ", "AAPL", "INVALIDISIN", "http://www.apple.com", CompanyErrorCode.InvalidIsinLength)]
        [InlineData("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "invalid-url", CompanyErrorCode.InvalidWebsite)]
        public void Create_InvalidCompany_ShouldFail(string name, string exchange, string ticker, string isin, string website, CompanyErrorCode expectedErrorCode)
        {
            // Arrange
            var request = new CreateCompanyRequest(name, exchange, ticker, isin, website);

            // Act
            var result = Company.Create(request, _exchangeLookup);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be(expectedErrorCode);
        }

        [Fact]
        public void UpdateCompany_ShouldModifyMutableFieldsOnly()
        {
            // Arrange
            var request = new CreateCompanyRequest("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com");
            var company = Company.Create(request, _exchangeLookup).Value;
            var updateRequest = new UpdateCompanyRequest("Apple Corp.", "NEW YORK STOCK EXCHANGE", "AAPN", "https://www.apple.org");

            // Act
            var result = company.Update(updateRequest, _exchangeLookup);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Id.Should().Be(company.Id); // Immutable
            result.Value.Isin.Should().Be(company.Isin); // Immutable
            result.Value.Name.Should().Be("Apple Corp.");
            result.Value.Exchange.Value.MicCode.Should().Be("XNYS");
            result.Value.Ticker.Should().Be("AAPN");
            result.Value.Website.Should().Be(new Uri("https://www.apple.org"));
        }

        [Theory]
        [InlineData("Apple Corp.", "INVALID_EXCHANGE", "AAPN", "https://www.apple.org", CompanyErrorCode.UnknownExchange)] // Invalid exchange
        [InlineData("Apple Corp.", "NASDAQ", "AAPN", "invalid-url", CompanyErrorCode.InvalidWebsite)] // Invalid website
        public void Update_InvalidCompany_ShouldFail(string name, string exchange, string ticker, string website, CompanyErrorCode expectedErrorCode)
        {
            // Arrange
            var request = new CreateCompanyRequest("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com");
            var company = Company.Create(request, _exchangeLookup).Value;
            var updateRequest = new UpdateCompanyRequest(name, exchange, ticker, website);

            // Act
            var result = company.Update(updateRequest, _exchangeLookup);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be(expectedErrorCode);
        }

        [Fact]
        public void UpdateCompany_RemoveWebsite_ShouldSetToNull()
        {
            // Arrange
            var request = new CreateCompanyRequest("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com");
            var company = Company.Create(request, _exchangeLookup).Value;
            var updateRequest = new UpdateCompanyRequest("Apple Corp.", "NASDAQ", "AAPN", string.Empty);

            // Act
            var result = company.Update(updateRequest, _exchangeLookup);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Website.Should().BeNull();
        }
    }
}
