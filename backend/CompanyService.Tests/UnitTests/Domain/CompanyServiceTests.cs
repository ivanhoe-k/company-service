using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Core.Models;
using CompanyService.Domain.Contracts;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;
using CompanyService.Domain.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CompanyService.Tests.UnitTests.Domain
{
    public class CompanyServiceTests
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<IStockExchangeProvider> _stockExchangeProviderMock;
        private readonly Mock<ILogger<CompanyManagerService>> _loggerMock;
        private readonly ICompanyManagerService _companyManagerService;

        private readonly IReadOnlyCollection<StockExchangeInfo> _mockStockExchanges = new List<StockExchangeInfo>
        {
            new ("XNAS", "NASDAQ"),
            new ("XNYS", "New York Stock Exchange"),
            new ("XLON", "London Stock Exchange")
        };

        public CompanyServiceTests()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _stockExchangeProviderMock = new Mock<IStockExchangeProvider>();
            _loggerMock = new Mock<ILogger<CompanyManagerService>>();

            _stockExchangeProviderMock
                .Setup(provider => provider.GetStockExchangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(_mockStockExchanges));

            _companyRepositoryMock
                .Setup(repo => repo.ExistsByIsinAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(false));

            _companyManagerService = new CompanyManagerService(
                _loggerMock.Object,
                _companyRepositoryMock.Object,
                _stockExchangeProviderMock.Object);
        }

        [Fact]
        public async Task CreateCompanyAsync_Should_Create_Company_When_Valid()
        {
            // Arrange
            var createRequest = new CreateCompanyRequest("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com");
            var expectedMicCode = "XNAS";

            _companyRepositoryMock
                .Setup(repo => repo.ExistsByIsinAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(false)); // ISIN does not exist

            _companyRepositoryMock
                .Setup(repo => repo.SaveAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok());

            // Act
            var result = await _companyManagerService.CreateCompanyAsync(createRequest, CancellationToken.None);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Name.Should().Be(createRequest.Name);
            result.Value.Exchange.Value.MicCode.Should().Be(expectedMicCode);

            _companyRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateCompanyAsync_Should_Fail_When_ISIN_Already_Exists()
        {
            // Arrange
            var createRequest = new CreateCompanyRequest("Microsoft Corporation", "NASDAQ", "MSFT", "US5949181045", "https://www.microsoft.com");

            _companyRepositoryMock
                .Setup(repo => repo.ExistsByIsinAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(true)); // ISIN already exists

            // Act
            var result = await _companyManagerService.CreateCompanyAsync(createRequest, CancellationToken.None);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().Be(CompanyError.DuplicateIsin);

            _companyRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateCompanyAsync_Should_Fail_When_StockExchangeProvider_Fails()
        {
            // Arrange
            var createRequest = new CreateCompanyRequest("Amazon.com, Inc.", "NASDAQ", "AMZN", "US0231351067", "https://www.amazon.com");

            _stockExchangeProviderMock
                .Setup(provider => provider.GetStockExchangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Fail<IReadOnlyCollection<StockExchangeInfo>>(CompanyError.ExchangeLookupFailed)); // Simulate failure

            // Act
            var result = await _companyManagerService.CreateCompanyAsync(createRequest, CancellationToken.None);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().Be(CompanyError.ExchangeLookupFailed);

            _companyRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateCompanyAsync_Should_Fail_When_Company_Creation_Fails()
        {
            // Arrange
            var createRequest = new CreateCompanyRequest(string.Empty, "NASDAQ", "TSLA", "US88160R1014", "https://www.tesla.com"); // Empty name (invalid)

            _companyRepositoryMock
                .Setup(repo => repo.ExistsByIsinAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(false)); // ISIN does not exist

            // Act
            var result = await _companyManagerService.CreateCompanyAsync(createRequest, CancellationToken.None);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().Be(CompanyError.InvalidName);

            _companyRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateCompanyAsync_Should_Fail_When_Save_Fails()
        {
            // Arrange
            var createRequest = new CreateCompanyRequest("Tesla Inc.", "NASDAQ", "TSLA", "US88160R1014", "https://www.tesla.com");

            _companyRepositoryMock
                .Setup(repo => repo.ExistsByIsinAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(false)); // ISIN does not exist

            _companyRepositoryMock
                .Setup(repo => repo.SaveAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Fail(CompanyError.SomethingWentWrong)); // Simulate save failure

            // Act
            var result = await _companyManagerService.CreateCompanyAsync(createRequest, CancellationToken.None);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().Be(CompanyError.SomethingWentWrong);

            _companyRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCompanyAsync_Should_Update_Company_When_Valid()
        {
            // Arrange
            var existingCompany = CreateCompanyInstance("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com");
            var companyId = existingCompany.Id;
            var updateRequest = new UpdateCompanyRequest("Apple Corp", "NASDAQ", "APPL", "http://www.applecorp.com");

            _companyRepositoryMock
                .Setup(repo => repo.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(existingCompany));

            _companyRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok());

            // Act
            var result = await _companyManagerService.UpdateCompanyAsync(companyId, updateRequest, CancellationToken.None);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Name.Should().Be(updateRequest.Name);

            _companyRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCompanyAsync_Should_Fail_When_Company_Not_Found()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var updateRequest = new UpdateCompanyRequest("Apple Corp", "NASDAQ", "AAPL", "http://www.applecorp.com");

            _companyRepositoryMock
                .Setup(repo => repo.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Fail<CompanyDto>(CompanyError.NotFound));

            // Act
            var result = await _companyManagerService.UpdateCompanyAsync(companyId, updateRequest, CancellationToken.None);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().Be(CompanyError.NotFound);

            _companyRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCompanyAsync_Should_Fail_When_Exchange_Lookup_Fails()
        {
            // Arrange
            var existingCompany = CreateCompanyInstance("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com");
            var companyId = existingCompany.Id;
            var updateRequest = new UpdateCompanyRequest("Apple Corp", "NASDAQ", "AAPL", "http://www.applecorp.com");

            _companyRepositoryMock
                .Setup(repo => repo.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(existingCompany));

            _stockExchangeProviderMock
                .Setup(provider => provider.GetStockExchangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Fail<IReadOnlyCollection<StockExchangeInfo>>(CompanyError.ExchangeLookupFailed));

            // Act
            var result = await _companyManagerService.UpdateCompanyAsync(companyId, updateRequest, CancellationToken.None);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().Be(CompanyError.ExchangeLookupFailed);

            _companyRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCompanyAsync_Should_Fail_When_Update_Fails()
        {
            // Arrange
            var existingCompany = CreateCompanyInstance("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com");
            var companyId = existingCompany.Id;
            var updateRequest = new UpdateCompanyRequest("Apple Corp", "NASDAQ", "AAPL", "http://www.applecorp.com");

            _companyRepositoryMock
                .Setup(repo => repo.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(existingCompany));

            _companyRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Fail(CompanyError.SomethingWentWrong));

            // Act
            var result = await _companyManagerService.UpdateCompanyAsync(companyId, updateRequest, CancellationToken.None);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().Be(CompanyError.SomethingWentWrong);

            _companyRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_Should_Return_Company_When_Exists()
        {
            // Arrange
            var existingCompany = CreateCompanyInstance("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com");
            var companyId = existingCompany.Id;

            _companyRepositoryMock
                .Setup(repo => repo.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(existingCompany));

            // Act
            var result = await _companyManagerService.GetCompanyByIdAsync(companyId, CancellationToken.None);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(companyId);

            _companyRepositoryMock.Verify(repo => repo.GetByIdAsync(companyId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_Should_Fail_When_Company_Not_Found()
        {
            // Arrange
            var companyId = Guid.NewGuid();

            _companyRepositoryMock
                .Setup(repo => repo.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Fail<CompanyDto>(CompanyError.NotFound));

            // Act
            var result = await _companyManagerService.GetCompanyByIdAsync(companyId, CancellationToken.None);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().Be(CompanyError.NotFound);

            _companyRepositoryMock.Verify(repo => repo.GetByIdAsync(companyId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCompanyByIsinAsync_Should_Return_Company_When_Exists()
        {
            // Arrange
            var existingCompany = CreateCompanyInstance("Microsoft Corporation", "NASDAQ", "MSFT", "US5949181045", "https://www.microsoft.com");
            var isin = Isin.Create("US5949181045").Value;

            _companyRepositoryMock
                .Setup(repo => repo.GetByIsinAsync(isin, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(existingCompany));

            // Act
            var result = await _companyManagerService.GetCompanyByIsinAsync("US5949181045", CancellationToken.None);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Isin.Should().Be(isin);

            _companyRepositoryMock.Verify(repo => repo.GetByIsinAsync(isin, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCompanyByIsinAsync_Should_Fail_When_Company_Not_Found()
        {
            // Arrange
            _companyRepositoryMock
                .Setup(repo => repo.GetByIsinAsync(It.IsAny<Isin>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Fail<CompanyDto>(CompanyError.NotFound));

            // Act
            var result = await _companyManagerService.GetCompanyByIsinAsync("US5949181045", CancellationToken.None);

            // Assert
            result.Failed.Should().BeTrue();
            result.Error.Should().Be(CompanyError.NotFound);

            _companyRepositoryMock.Verify(repo => repo.GetByIsinAsync(It.IsAny<Isin>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCompaniesAsync_Should_Return_Companies_When_Exists()
        {
            // Arrange
            var request = GetCompaniesRequest.Create().Value;
            var companies = new List<CompanyDto>
            {
                CreateCompanyInstance("Apple Inc.", "NASDAQ", "AAPL", "US0378331005", "http://www.apple.com"),
                CreateCompanyInstance("Microsoft Corporation", "NASDAQ", "MSFT", "US5949181045", "https://www.microsoft.com"),
            };

            var pageResult = CreatePageInstance(companies, companies.Count);

            _companyRepositoryMock
                .Setup(repo => repo.GetCompaniesAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(pageResult));

            // Act
            var result = await _companyManagerService.GetCompaniesAsync(request, CancellationToken.None);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Items.Should().HaveCount(2);
            result.Value.TotalCount.Should().Be(companies.Count);

            _companyRepositoryMock.Verify(repo => repo.GetCompaniesAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCompaniesAsync_Should_Return_Empty_When_No_Companies()
        {
            // Arrange
            var request = GetCompaniesRequest.Create().Value;
            var emptyPageResult = CreatePageInstance(Array.Empty<CompanyDto>(), 0);

            _companyRepositoryMock
                .Setup(repo => repo.GetCompaniesAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyError>.Ok(emptyPageResult));

            // Act
            var result = await _companyManagerService.GetCompaniesAsync(request, CancellationToken.None);

            // Assert
            result.Failed.Should().BeFalse();
            result.Value.Items.Should().BeEmpty();
            result.Value.TotalCount.Should().Be(0);
        }

        private CompanyDto CreateCompanyInstance(string name, string exchangeName, string ticker, string isin, string? website)
        {
            var exchangeLookup = _mockStockExchanges.ToDictionary(e => e.ExchangeName, e => e.MicCode, StringComparer.OrdinalIgnoreCase);

            return new CompanyDto(
                id: Guid.NewGuid(),
                name: name,
                exchangeMicCode: exchangeLookup[exchangeName], 
                ticker: ticker, 
                isin: isin, 
                website != null ? new Uri(website) : null);
        }

        private Page<CompanyDto> CreatePageInstance(IReadOnlyCollection<CompanyDto> companies, int totalCount)
        {
            var pageInfo = new PageInfo(
                currentPage: 1,
                pageSize: 2,
                totalPages: 1,
                hasNextPage: false,
                hasPreviousPage: false);

            return new Page<CompanyDto>(companies, totalCount, pageInfo);
        }
    }
}
