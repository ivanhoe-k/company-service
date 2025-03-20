using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Core.Models;
using CompanyService.Domain.Contracts;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;

namespace CompanyService.Application
{
    /// <summary>
    /// Provides a dummy implementation of <see cref="IStockExchangeProvider"/>.
    /// 
    /// This implementation returns a static list of known stock exchanges with their MIC codes.
    /// 
    /// ⚠️ In a real-world scenario, this data should be fetched from an **external financial API** or **database** 
    /// and periodically updated to ensure accuracy. The actual implementation could:
    /// - Query an API like Refinitiv, Euronext, Bloomberg, or OpenFIGI to retrieve up-to-date stock exchange data.
    /// - Store the retrieved exchanges in a database and refresh them periodically (e.g., via a background job).
    /// - Implement caching mechanisms to reduce unnecessary API calls.
    /// 
    /// For now, this class provides a fixed list of stock exchanges for **testing purposes only**.
    /// </summary>
    public sealed class DummyStockExchangeProvider : IStockExchangeProvider
    {
        /// <summary>
        /// Simulates fetching stock exchange data.
        /// Returns a static list of stock exchanges with valid MIC codes.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to handle operation cancellation.</param>
        /// <returns>A successful result containing a list of stock exchanges.</returns>
        public Task<Result<CompanyError, IReadOnlyCollection<StockExchangeInfo>>> GetStockExchangesAsync(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<StockExchangeInfo> exchanges = new List<StockExchangeInfo>
            {
                new StockExchangeInfo("XNAS", "NASDAQ"),              // 🇺🇸 NASDAQ
                new StockExchangeInfo("XNYS", "New York Stock Exchange"), // 🇺🇸 NYSE
                new StockExchangeInfo("XLON", "London Stock Exchange"),  // 🇬🇧 LSE
                new StockExchangeInfo("XPAR", "Euronext Paris"),        // 🇫🇷 Euronext Paris
                new StockExchangeInfo("XETR", "Deutsche Börse Xetra"),  // 🇩🇪 Xetra (Germany)
                new StockExchangeInfo("XTSE", "Toronto Stock Exchange"), // 🇨🇦 TSX
                new StockExchangeInfo("XHKG", "Hong Kong Stock Exchange"), // 🇭🇰 HKEX
                new StockExchangeInfo("XTKS", "Tokyo Stock Exchange"),  // 🇯🇵 TSE (Japan)
                new StockExchangeInfo("XSHG", "Shanghai Stock Exchange"), // 🇨🇳 SSE
                new StockExchangeInfo("XSES", "Singapore Exchange"),    // 🇸🇬 SGX
                new StockExchangeInfo("XBKK", "Stock Exchange of Thailand"), // 🇹🇭 SET
                new StockExchangeInfo("XASX", "Australian Securities Exchange"), // 🇦🇺 ASX
                new StockExchangeInfo("BMFB", "B3 - Brazil Stock Exchange"), // 🇧🇷 B3
                new StockExchangeInfo("XJSE", "Johannesburg Stock Exchange"), // 🇿🇦 JSE
                new StockExchangeInfo("XKRX", "Korea Exchange"),         // 🇰🇷 KRX
                new StockExchangeInfo("XMEX", "Mexican Stock Exchange"), // 🇲🇽 BMV
                new StockExchangeInfo("XTAI", "Taiwan Stock Exchange"),  // 🇹🇼 TWSE
                new StockExchangeInfo("XBOM", "Bombay Stock Exchange"),  // 🇮🇳 BSE
                new StockExchangeInfo("XNSE", "National Stock Exchange of India"), // 🇮🇳 NSE
                new StockExchangeInfo("XWAR", "Warsaw Stock Exchange"),  // 🇵🇱 GPW
                new StockExchangeInfo("XIST", "Borsa Istanbul"),         // 🇹🇷 BIST
                new StockExchangeInfo("XSGO", "Santiago Stock Exchange"), // 🇨🇱 BCS
                new StockExchangeInfo("XKLS", "Bursa Malaysia"),         // 🇲🇾 KLSE
                new StockExchangeInfo("XDUB", "Euronext Dublin"),        // 🇮🇪 Euronext Dublin
                new StockExchangeInfo("XHEL", "Nasdaq Helsinki"),        // 🇫🇮 Nasdaq Helsinki
                new StockExchangeInfo("XOSL", "Oslo Stock Exchange"),    // 🇳🇴 OSE
                new StockExchangeInfo("XSTO", "Nasdaq Stockholm"),       // 🇸🇪 Nasdaq Stockholm
                new StockExchangeInfo("XVIE", "Vienna Stock Exchange"),  // 🇦🇹 Wiener Börse
                new StockExchangeInfo("XMIL", "Borsa Italiana"),         // 🇮🇹 Borsa Italiana
                new StockExchangeInfo("XBRU", "Euronext Brussels"),      // 🇧🇪 Euronext Brussels
            };

            return Task.FromResult(Result<CompanyError, IReadOnlyCollection<StockExchangeInfo>>.Ok(exchanges));
        }
    }
}
