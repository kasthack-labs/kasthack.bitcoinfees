using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace kasthack.bitcoinfees
{
    /// <summary>
    /// String converter
    /// </summary>
    internal static class BtcToString
    {
        const string Satoshi = "sat", MicroBTC = "μBTC", Bitcoin = "฿";
        /// <summary>
        /// Satoshis => currency string
        /// </summary>
        /// <param name="satoshis"># of satoshis</param>
        /// <returns></returns>
        public static string ToString(long satoshis)
        {
            var thr = 10L;
            var inc = 1000;
            if (satoshis < thr * inc)
            {
                return $"{satoshis} {Satoshi}";
            }
            if (satoshis < thr * inc * inc)
            {
                return $"{satoshis / inc} {MicroBTC}";
            }
            return $"{Bitcoin} {((decimal)satoshis) / 100000000}";
        }
    }
    /// <summary>
    /// API client
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Gets transaction fees summary
        /// </summary>
        /// <returns></returns>
        Task<FeeList> GetList();
        /// <summary>
        /// Gets recommended transaction fees
        /// </summary>
        /// <returns></returns>
        Task<RecommendedFees> GetRecommendedFees();
    }

    /// <summary>
    /// API client for bitcoinfees.earn.com
    /// </summary>
    public class Client : IDisposable, IClient
    {
        private readonly HttpClient _client;
        private static readonly string _apiRoot = "https://bitcoinfees.earn.com/api/v1/fees/";
        /// <summary>
        /// Default client instance
        /// </summary>
        public static IClient Default { get; } = new Client();
        /// <summary>
        /// Creates new client
        /// </summary>
        public Client() : this(null) { }
        /// <summary>
        /// Creates new client with custom httpClient
        /// </summary>
        /// <param name="client">Http client</param>
        public Client(HttpClient client) => _client = client ?? new HttpClient();
        /// <inheritdoc/>
        public async Task<RecommendedFees> GetRecommendedFees() => JsonConvert.DeserializeObject<RecommendedFees>(await _client.GetStringAsync($"{_apiRoot}recommended").ConfigureAwait(false));
        /// <inheritdoc/>
        public async Task<FeeList> GetList() => JsonConvert.DeserializeObject<FeeList>(await _client.GetStringAsync($"{_apiRoot}list").ConfigureAwait(false));
        /// <summary>
        /// IDisposable.Dispose
        /// </summary>
        public void Dispose() => _client.Dispose();
    }
    /// <summary>
    /// Recommended fees response
    /// </summary>
    public class RecommendedFees
    {
        /// <summary>
        /// The lowest fee (in satoshis per byte) that will currently result in the fastest transaction confirmations (usually 0 to 1 block delay).
        /// </summary>
        public long FastestFee { get; set; }
        /// <summary>
        /// The lowest fee (in satoshis per byte) that will confirm transactions within half an hour (with 90% probability).
        /// </summary>
        public long HalfHourFee { get; set; }
        /// <summary>
        /// The lowest fee (in satoshis per byte) that will confirm transactions within an hour (with 90% probability).
        /// </summary>
        public long HourFee { get; set; }
        /// <inheritdoc/>
        public override string ToString() => $"Recommended fees: fastest @ {BtcToString.ToString(FastestFee)} / byte; 30 min @ {BtcToString.ToString(HalfHourFee)} / byte; hour @ {BtcToString.ToString(HourFee)} / byte";
    }
    /// <summary>
    /// Fee list response
    /// </summary>
    public class FeeList
    {
        /// <summary>
        /// Fee list
        /// </summary>
        public Fee[] Fees { get; set; }
    }
    /// <summary>
    /// Predictions about fees in the given range from minFee to maxFee in satoshis/byte.
    /// </summary>
    public class Fee
    {
        /// <summary>
        /// Number of confirmed transactions with this fee in the last 24 hours.
        /// </summary>
        public long DayCount { get; set; }
        /// <summary>
        /// Estimated maximum delay (in blocks) until transaction is confirmed (90% confidence interval).
        /// </summary>
        public long MaxDelay { get; set; }
        /// <summary>
        /// Fee range max(satoshis/byte)
        /// </summary>
        public long MaxFee { get; set; }
        /// <summary>
        /// Fee range min(satoshis/byte)
        /// </summary>
        public long MinFee { get; set; }
        /// <summary>
        /// Estimated maximum time (in minutes) until transaction is confirmed (90% confidence interval).
        /// </summary>
        public long MaxMinutes { get; set; }
        /// <summary>
        /// Number of unconfirmed transactions with this fee.
        /// </summary>
        public long MemCount { get; set; }
        /// <summary>
        ///  Estimated minimum delay (in blocks) until transaction is confirmed (90% confidence interval).
        /// </summary>
        public long MinDelay { get; set; }
        /// <summary>
        /// Estimated minimum time (in minutes) until transaction is confirmed (90% confidence interval).
        /// </summary>
        public long MinMinutes { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"Data for {BtcToString.ToString(MinFee)}-{BtcToString.ToString(MaxFee)} / byte: delays are {MinDelay}-{MaxDelay} blocks({MinMinutes}-{MaxMinutes} min), processed {DayCount} txs during last 24 hours, {MemCount} txs in mempool";
    }
}
