namespace CompanyService.Domain.Errors
{
    public enum CompanyErrorCode
    {
        SomethingWentWrong,
        NotFound,
        InvalidName,
        InvalidTicker,
        InvalidWebsite,
        DuplicateIsin,
        ExchangeLookupFailed,

        // ISIN ISO 6166-Specific Errors
        InvalidIsinLength,           // ISIN is not exactly 12 characters
        InvalidIsinCountryCode,      // First two characters are not letters
        InvalidIsinAlphanumeric,     // Characters 3-11 contain invalid characters
        InvalidIsinCheckDigit,       // Luhn Algorithm validation failed

        // Stock Exchange ISO 10383-Specific Errors
        InvalidExchange,             // Exchange field is empty or null
        UnknownExchange,             // Provided MIC code is not in the list

        InvalidClientCredentials,    // Client ID or secret is invalid
        AuthDisabled,                // Authentication is disabled
    }
}
