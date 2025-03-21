using CompanyService.Api.Models;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;

namespace CompanyService.Api.Auth
{
    public interface ITokenService
    {
        Result<CompanyError, TokenResponse> GetToken(TokenRequest request);
    }
}
