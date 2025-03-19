namespace CompanyService.Domain.Models
{
    /// <summary>
    /// Represents a stock exchange with its MIC code and display name.
    /// </summary>
    public sealed record StockExchangeInfo
    {
        public string MicCode { get; }

        public string ExchangeName { get; }

        public StockExchangeInfo(string micCode, string exchangeName)
        {
            MicCode = micCode;
            ExchangeName = exchangeName;
        }
    }
}
