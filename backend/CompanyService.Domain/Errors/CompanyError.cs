﻿using CompanyService.Core.Errors;

namespace CompanyService.Domain.Errors
{
    public record CompanyError : CodeBasedError<CompanyErrorCode>
    {
        public CompanyError(CompanyErrorCode code)
            : base(code)
        {
        }

        public static CompanyError InvalidName => new (CompanyErrorCode.InvalidName);

        public static CompanyError InvalidTicker => new (CompanyErrorCode.InvalidTicker);

        public static CompanyError InvalidWebsite => new (CompanyErrorCode.InvalidWebsite);

        public static CompanyError InvalidIsinLength => new (CompanyErrorCode.InvalidIsinLength);

        public static CompanyError InvalidIsinCountryCode => new (CompanyErrorCode.InvalidIsinCountryCode);

        public static CompanyError InvalidIsinAlphanumeric => new (CompanyErrorCode.InvalidIsinAlphanumeric);

        public static CompanyError InvalidIsinCheckDigit => new (CompanyErrorCode.InvalidIsinCheckDigit);

        public static CompanyError InvalidExchange => new (CompanyErrorCode.InvalidExchange);

        public static CompanyError UnknownExchange => new (CompanyErrorCode.UnknownExchange);
    }
}
