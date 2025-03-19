using System;
using System.Runtime.CompilerServices;
using Serilog;

namespace CompanyService.Core.Common
{
    public static class ThrowExtensions
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the provided <paramref name="argument"/> is null.
        /// </summary>
        /// <param name="argument">The object to check for null.</param>
        /// <param name="paramName">The name of the parameter (automatically generated if not provided).</param>
        /// <param name="memberName">The name of the calling member (automatically generated).</param>
        /// <param name="sourceFilePath">The path to the source file (automatically generated).</param>
        /// <param name="sourceLineNumber">The line number in the source file (automatically generated).</param>
        public static void ThrowIfNull(
            this object? argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (argument != null)
            {
                return;
            }

            var exception = new ArgumentNullException(paramName);
            Log.Error(exception, "Exception in { memberName}, { sourceFilePath}, { sourceLineNumber}", memberName, sourceFilePath, sourceLineNumber);
            throw exception;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the provided <paramref name="argument"/> is null or empty.
        /// </summary>
        /// <param name="argument">The string to check for null or empty.</param>
        /// <param name="paramName">The name of the parameter (automatically generated if not provided).</param>
        /// <param name="memberName">The name of the calling member (automatically generated).</param>
        /// <param name="sourceFilePath">The path to the source file (automatically generated).</param>
        /// <param name="sourceLineNumber">The line number in the source file (automatically generated).</param>
        public static void ThrowIfNullOrWhiteSpace(
            this string argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!string.IsNullOrWhiteSpace(argument))
            {
                return;
            }

            var exception = new ArgumentNullException(paramName);
            Log.Error(exception, "Exception in { memberName}, { sourceFilePath}, { sourceLineNumber}", memberName, sourceFilePath, sourceLineNumber);
            throw exception;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the provided <paramref name="argument"/> is <see cref="Guid.Empty"/>.
        /// </summary>
        /// <param name="argument">The Guid to check for empty.</param>
        /// <param name="paramName">The name of the parameter (automatically generated if not provided).</param>
        /// <param name="memberName">The name of the calling member (automatically generated).</param>
        /// <param name="sourceFilePath">The path to the source file (automatically generated).</param>
        /// <param name="sourceLineNumber">The line number in the source file (automatically generated).</param>
        public static void ThrowIfEmpty(
            this Guid argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (argument != Guid.Empty)
            {
                return;
            }

            var exception = new ArgumentNullException(paramName);
            Log.Error(exception, "Exception in { memberName}, { sourceFilePath}, { sourceLineNumber}", memberName, sourceFilePath, sourceLineNumber);
            throw exception;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if a condition is true.
        /// </summary>
        /// <param name="argument">The object to which the condition applies.</param>
        /// <param name="condition">The condition that, if true, triggers the exception.</param>
        /// <param name="errorMessage">The error message to include in the exception.</param>
        /// <param name="paramName">The name of the parameter being checked (automatically inferred).</param>
        /// <param name="memberName">The name of the calling member (automatically inferred).</param>
        /// <param name="sourceFilePath">The path to the source file where the exception occurred (automatically inferred).</param>
        /// <param name="sourceLineNumber">The line number in the source file where the exception occurred (automatically inferred).</param>
        public static void ThrowIf(
            this object argument,
            bool condition,
            string errorMessage,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!condition)
            {
                return;
            }

            var exception = new ArgumentException(errorMessage, paramName);
            Log.Error(exception, "Exception in { memberName}, { sourceFilePath}, { sourceLineNumber}", memberName, sourceFilePath, sourceLineNumber);
            throw exception;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if url is invalid.
        /// </summary>
        /// <param name="url">The url to which the condition applies.</param>
        /// <param name="paramName">The name of the parameter being checked (automatically inferred).</param>
        /// <param name="memberName">The name of the calling member (automatically inferred).</param>
        /// <param name="sourceFilePath">The path to the source file where the exception occurred (automatically inferred).</param>
        /// <param name="sourceLineNumber">The line number in the source file where the exception occurred (automatically inferred).</param>
        public static void ThrowIfInvalidUrl(
            this string url,
            [CallerArgumentExpression(nameof(url))] string? paramName = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                return;
            }

            var exception = new ArgumentException("Invalid URL", nameof(url));
            Log.Error(exception, "Exception in { memberName}, { sourceFilePath}, { sourceLineNumber}", memberName, sourceFilePath, sourceLineNumber);
            throw exception;
        }
    }
}
