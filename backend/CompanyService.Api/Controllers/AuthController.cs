using CompanyService.Api.Auth;
using CompanyService.Api.Models;
using CompanyService.Api.Utils;
using CompanyService.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyService.Api.Controllers
{
    [Route(ApiConstants.AuthEndpoint)]
    public sealed class AuthController : CompanyControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenService _tokenService;

        public AuthController(ILogger<AuthController> logger, ITokenService tokenService)
        {
            logger.ThrowIfNull();
            tokenService.ThrowIfNull();

            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpPost(ApiConstants.TokenRoute)]
        public ActionResult<TokenResponse> GetToken([FromBody] TokenRequest request)
        {
            request.ThrowIfNull();

            _logger.LogInformation("Generating token for client ID: {ClientId}", request.ClientId);

            return HandleResult(_tokenService.GetToken(request), (resp) => resp);
        }
    }
}
