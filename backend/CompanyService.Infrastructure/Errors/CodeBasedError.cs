using Newtonsoft.Json;

namespace CompanyService.Infrastructure.Errors
{
    public abstract class CodeBasedError<TErrorCode> : IError
       where TErrorCode : System.Enum
    {
        public TErrorCode Code { get; }

        [JsonConstructor]
        protected CodeBasedError(TErrorCode code)
        {
            Code = code;
        }
    }
}
