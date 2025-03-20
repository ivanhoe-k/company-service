using System;
using CompanyService.Core.Common;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CompanyService.Api.Controllers
{
    [ApiController]
    public abstract class CompanyControllerBase : ControllerBase
    {
        protected ActionResult<TApiResult> HandleResult<TApiResult, TResult>(
            Result<CompanyError, TResult> result, Func<TResult, TApiResult> mapper)
        {
            result.ThrowIfNull();

            if (!result.Failed)
            {
                return Ok(mapper(result.Value));
            }

            return HandleResult(result);
        }

        protected ActionResult HandleResult(Result<CompanyError> result)
        {
            result.ThrowIfNull();
            
            if (!result.Failed)
            {
                return Ok();
            }

            return result.Error!.Code switch
            {
                CompanyErrorCode.NotFound => NotFound("Company not found."),
                CompanyErrorCode.InvalidName => BadRequest("Invalid company name."),
                CompanyErrorCode.InvalidTicker => BadRequest("Invalid company ticker."),
                CompanyErrorCode.InvalidWebsite => BadRequest("Invalid company website."),
                CompanyErrorCode.DuplicateIsin => BadRequest("Duplicate ISIN."),
                CompanyErrorCode.ExchangeLookupFailed => BadRequest("Failed to lookup exchange."),
                CompanyErrorCode.InvalidIsinLength => BadRequest("Invalid ISIN length."),
                CompanyErrorCode.InvalidIsinCountryCode => BadRequest("Invalid ISIN country code."),
                CompanyErrorCode.InvalidIsinAlphanumeric => BadRequest("Invalid ISIN alphanumeric."),
                CompanyErrorCode.InvalidIsinCheckDigit => BadRequest("Invalid ISIN check digit."),
                CompanyErrorCode.InvalidExchange => BadRequest("Invalid exchange."),
                CompanyErrorCode.UnknownExchange => BadRequest("Unknown exchange."),
                _ => StatusCode(500, "An error occurred while processing the request.")
            };
        }
    }
}
