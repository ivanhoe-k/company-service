using System;
using System.Threading;
using CompanyService.Infrastructure.Common;

namespace CompanyService.Infrastructure.Logging
{
    public static class CorrelationIdContext
    {
        private static readonly AsyncLocal<string> _correlationId = new AsyncLocal<string>();

        public static void SetCorrelationId(string correlationId)
        {
            correlationId.ThrowIfNullOrWhiteSpace();

            if (!string.IsNullOrWhiteSpace(_correlationId.Value))
            {
                throw new InvalidOperationException("Correlation Id is already set for the context");
            }

            _correlationId.Value = correlationId;
        }

        public static string GetCorrelationId()
            => _correlationId.Value ?? throw new NullReferenceException("Correlation Id was not set");

        public static bool IsInitialized => !string.IsNullOrWhiteSpace(_correlationId.Value);
    }
}
