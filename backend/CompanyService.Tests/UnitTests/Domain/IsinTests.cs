using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;
using FluentAssertions;

namespace CompanyService.Tests.UnitTests.Domain
{
    public sealed class IsinTests
    {
        [Fact]
        public void Create_ValidIsin_ShouldSucceed()
        {
            // Arrange
            var validIsin = "US0378331005"; // Apple Inc.

            // Act
            var result = Isin.Create(validIsin);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Value.Should().Be(validIsin);
        }

        [Theory]
        [InlineData("US03783310", CompanyErrorCode.InvalidIsinLength)] // Too short
        [InlineData("330378331005", CompanyErrorCode.InvalidIsinCountryCode)] // Invalid country code
        [InlineData("US0378331!05", CompanyErrorCode.InvalidIsinAlphanumeric)] // Contains invalid character
        [InlineData("US0378331006", CompanyErrorCode.InvalidIsinCheckDigit)] // Invalid check digit
        public void Create_InvalidIsin_ShouldFail(string invalidIsin, CompanyErrorCode expectedErrorCode)
        {
            // Act
            var result = Isin.Create(invalidIsin);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be(expectedErrorCode);
        }
    }
}
