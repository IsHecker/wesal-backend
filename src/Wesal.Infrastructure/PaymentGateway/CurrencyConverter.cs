using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway;

internal sealed class CurrencyConverter(
    HttpClient httpClient,
    IMemoryCache cache)
{
    private const int CacheMinutes = 60;
    private const string BaseUrl = "https://open.exchangerate-api.com/v6/latest";

    public async Task<Result<long>> EgpToUsdAsync(long egpPiasters)
    {
        var rateResult = await GetExchangeRateAsync("EGP", "USD");

        if (rateResult.IsFailure)
            return rateResult.Error;

        return (long)Math.Round(egpPiasters * rateResult.Value);
    }

    private async Task<Result<decimal>> GetExchangeRateAsync(string from, string to)
    {
        var cacheKey = $"exchange_rate_{from}_{to}";

        if (cache.TryGetValue<decimal>(cacheKey, out var cachedRate))
            return cachedRate;

        try
        {
            var url = $"{BaseUrl}/{from}";
            var response = await httpClient.GetFromJsonAsync<ExchangeRateResponse>(url);

            if (response?.Rates == null || !response.Rates.TryGetValue(to, out var rate))
                return Error.Failure("CurrencyConverter.RateNotAvailable", $"Exchange rate for {from} to {to} not available");

            cache.Set(cacheKey, rate, TimeSpan.FromMinutes(CacheMinutes));

            return rate;
        }
        catch (Exception)
        {
            return Error.Failure("CurrencyConverter.ApiError", "Failed to fetch exchange rates from external API");
        }
    }

    private sealed class ExchangeRateResponse
    {
        public Dictionary<string, decimal>? Rates { get; set; }
    }
}