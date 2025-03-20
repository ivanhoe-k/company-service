using Newtonsoft.Json;

namespace CompanyService.Core.Errors
{
    public abstract record CodeBasedError<TErrorCode> : IError
       where TErrorCode : System.Enum
    {
        public TErrorCode Code { get; }

        [JsonConstructor]
        protected CodeBasedError(TErrorCode code)
        {
            Code = code;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {Code}";
        }
    }
}
