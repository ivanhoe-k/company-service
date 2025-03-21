using System.Collections.Generic;
using CompanyService.Core.Common;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;

namespace CompanyService.Api.Auth
{
    /// <summary>
    /// Dummy implementation of machine-to-machine authentication store.
    /// In a real-world scenario, client credentials would be stored securely in a database or an identity provider.
    /// </summary>
    public static class MachineClients
    {
        private static readonly Dictionary<string, string> _clients = new ()
        {
            { "client-id-123", "client-secret-abc" },
            { "another-client", "another-secret" } 
        };

        public static Result<CompanyError> ValidateClient(string clientId, string clientSecret)
        {
            clientId.ThrowIfNullOrWhiteSpace(nameof(clientId));
            clientSecret.ThrowIfNullOrWhiteSpace(nameof(clientSecret));

            if (_clients.TryGetValue(clientId, out var storedSecret) && storedSecret == clientSecret)
            {
                return Result<CompanyError>.Ok();
            }

            return Result<CompanyError>.Fail(CompanyError.InvalidClientCredentials);
        }
    }
}
