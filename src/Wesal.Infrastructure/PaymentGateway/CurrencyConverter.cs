using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Wesal.Infrastructure.PaymentGateway;

internal sealed class CurrencyConverter(
    HttpClient httpClient,
    IMemoryCache cache)
{
    private const int CacheMinutes = 60;
    private const string BaseUrl = "https://open.exchangerate-api.com/v6/latest";

    public async Task<long> EgpToUsdAsync(long egpPiasters)
    {
        var rate = await GetExchangeRateAsync("EGP", "USD");
        return (long)Math.Round(egpPiasters * rate);
    }

    public async Task<long> UsdToEgpAsync(long usdCents)
    {
        var rate = await GetExchangeRateAsync("USD", "EGP");
        return (long)Math.Round(usdCents * rate);
    }

    private async Task<decimal> GetExchangeRateAsync(string from, string to)
    {
        var cacheKey = $"exchange_rate_{from}_{to}";

        if (cache.TryGetValue<decimal>(cacheKey, out var cachedRate))
            return cachedRate;

        var url = $"{BaseUrl}/{from}";
        var response = await httpClient.GetFromJsonAsync<ExchangeRateResponse>(url);

        if (response?.Rates == null || !response.Rates.TryGetValue(to, out var rate))
            throw new InvalidOperationException($"Exchange rate for {from} to {to} not available");

        cache.Set(cacheKey, rate, TimeSpan.FromMinutes(CacheMinutes));

        return rate;
    }

    private sealed class ExchangeRateResponse
    {
        public Dictionary<string, decimal>? Rates { get; set; }
    }
}