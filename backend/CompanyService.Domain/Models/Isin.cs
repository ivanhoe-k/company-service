using System.Linq;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;

namespace CompanyService.Domain.Models
{
    /// <summary>
    /// Represents an International Securities Identification Number (ISIN) as a value object.
    /// Ensures the ISIN is always valid according to ISO 6166.
    /// </summary>
    public sealed record Isin
    {
        public string Value { get; }

        private Isin(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates an ISIN instance, returning a result instead of throwing exceptions.
        /// </summary>
        /// <param name="isin">The ISIN string.</param>
        /// <returns>
        /// Returns a <see cref="Result{CompanyError, Isin}"/> containing either:
        /// <list type="bullet">
        ///     <item>A valid <see cref="Isin"/> instance.</item>
        ///     <item>A failure result with an error code and message if validation fails.</item>
        /// </list>
        /// </returns>
        public static Result<CompanyError, Isin> Create(string isin)
        {
            if (string.IsNullOrWhiteSpace(isin) || isin.Length != 12)
            {
                return Result<CompanyError>.Fail<Isin>(CompanyError.InvalidIsinLength);
            }

            if (!char.IsLetter(isin[0]) || !char.IsLetter(isin[1]))
            {
                return Result<CompanyError>.Fail<Isin>(CompanyError.InvalidIsinCountryCode);
            }

            for (int i = 2; i < 11; i++)
            {
                if (!char.IsLetterOrDigit(isin[i]))
                {
                    return Result<CompanyError>.Fail<Isin>(CompanyError.InvalidIsinAlphanumeric);
                }
            }

            if (!IsValidCheckDigit(isin))
            {
                return Result<CompanyError>.Fail<Isin>(CompanyError.InvalidIsinCheckDigit);
            }

            return Result<CompanyError, Isin>.Ok(new Isin(isin));
        }

        /// <summary>
        /// Validates the check digit of the ISIN using the Luhn Algorithm.
        /// </summary>
        private static bool IsValidCheckDigit(string isin)
        {
            return LuhnAlgorithmIsValid(ConvertIsinToNumeric(isin));
        }

        /// <summary>
        /// Converts the ISIN into a numeric string for Luhn validation.
        /// Letters (A-Z) are replaced with numbers (A=10, B=11, ..., Z=35).
        /// </summary>
        /// <param name="isin">The ISIN string.</param>
        /// <returns>A numeric string where letters are replaced with numbers.</returns>
        private static string ConvertIsinToNumeric(string isin)
        {
            return string.Concat(
                isin.Select(c => char.IsDigit(c) ? c.ToString() : (c - 'A' + 10).ToString()));
        }

        /// <summary>
        /// Validates a numeric string using the Luhn algorithm.
        /// </summary>
        private static bool LuhnAlgorithmIsValid(string numericIsin)
        {
            var sum = 0;
            var doubleDigit = false;

            for (int i = numericIsin.Length - 1; i >= 0; i--)
            {
                var digit = numericIsin[i] - '0';

                if (doubleDigit)
                {
                    digit *= 2;

                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
                doubleDigit = !doubleDigit;
            }

            return (sum % 10) == 0;
        }
    }
}
