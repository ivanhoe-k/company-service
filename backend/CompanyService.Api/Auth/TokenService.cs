using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CompanyService.Api.Models;
using CompanyService.Core.Common;
using CompanyService.Core.Models;
using CompanyService.Core.Web.Configurations;
using CompanyService.Domain.Errors;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CompanyService.Api.Auth
{
    public sealed class TokenService : ITokenService
    {
        private const string ClientIdClaim = "client_id";

        private readonly ILogger<TokenService> _logger;
        private readonly WebAppConfiguration _webAppConfiguration;

        public TokenService(ILogger<TokenService> logger, WebAppConfiguration webAppConfiguration)
        {
            logger.ThrowIfNull();
            webAppConfiguration.ThrowIfNull();

            _logger = logger;
            _webAppConfiguration = webAppConfiguration;
        }

        /// <summary>
        /// Generates a JWT token if valid client credentials are provided.
        /// In a real-world scenario, this would involve database checks and refresh tokens.
        /// </summary>
        /// <returns>JWT token.</returns>
        public Result<CompanyError, TokenResponse> GetToken(TokenRequest request)
        {
            request.ThrowIfNull();

            _logger.LogInformation("Generating token for client ID: {ClientId}", request.ClientId);

            if (!_webAppConfiguration.AuthConfiguration.Enabled)
            {
                return Result<CompanyError>.Fail<TokenResponse>(CompanyError.AuthDisabled);
            }

            var result = MachineClients.ValidateClient(request.ClientId, request.ClientSecret);

            if (result.Failed)
            {
                return Result<CompanyError>.Fail<TokenResponse>(result.Error!);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_webAppConfiguration.AuthConfiguration.JwtSecret!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClientIdClaim, request.ClientId)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Result<CompanyError>.Ok(new TokenResponse(tokenHandler.WriteToken(token)));
        }
    }
}
