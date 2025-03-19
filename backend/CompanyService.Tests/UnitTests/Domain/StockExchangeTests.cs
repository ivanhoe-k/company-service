using System;
using System.Collections.Generic;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;
using FluentAssertions;

namespace CompanyService.Tests.UnitTests.Domain
{
    public class StockExchangeTests
    {
        private readonly IReadOnlyDictionary<string, string> _exchangeLookup = new Dictionary<string, string>()
        {
            { "NASDAQ", "XNAS" },
            { "NEW YORK STOCK EXCHANGE", "XNYS" },
            { "EURONEXT AMSTERDAM", "XAMS" }
        };

        [Theory]
        [InlineData("NASDAQ", "XNAS")]
        [InlineData("nasdaq", "XNAS")] // Case insensitive lookup
        [InlineData("  NASDAQ  ", "XNAS")] // Trimmed input
        public void CreateFromName_ValidExchange_ShouldSucceed(string exchangeName, string expectedMicCode)
        {
            // Act
            var result = StockExchange.CreateFromName(exchangeName, _exchangeLookup);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Value.MicCode.Should().Be(expectedMicCode);
            result.Value.Value.ExchangeName.Should().Be("NASDAQ");
        }

        [Theory]
        [InlineData("LONDON STOCK EXCHANGE", CompanyErrorCode.UnknownExchange)] // Unknown exchange
        [InlineData(" ", CompanyErrorCode.InvalidExchange)] // Empty exchange name
        public void CreateFromName_InvalidExchange_ShouldFail(string exchangeName, CompanyErrorCode expectedErrorCode)
        {
            // Act
            var result = StockExchange.CreateFromName(exchangeName, _exchangeLookup);
        
            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be(expectedErrorCode);
        }

        [Fact]
        public void CreateFromName_NullExchangeLookup_ShouldThrow()
        {
            // Arrange
            Dictionary<string, string>? nullLookup = null;

            // Act
            var act = () => StockExchange.CreateFromName("NASDAQ", nullLookup!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
