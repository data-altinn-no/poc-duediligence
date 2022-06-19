using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text.Json;
using bransjekartlegging.Models;
using bransjekartlegging.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace bransjekartlegging.Services
{
    public class RegnskapsregisterService : IRegnskapsregisterService
    {
        private const string ApiEndpoint = "https://data.brreg.no/regnskapsregisteret/regnskap/";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _cacheTtl = TimeSpan.FromHours(12);

        public RegnskapsregisterService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public async Task<IDictionary<string, RegnskapsregisterSearchResult?>> Search(List<string> organizationNumbers)
        {

            var organizationNumbersToSearchFor = new List<string>();
            var results = new ConcurrentDictionary<string, RegnskapsregisterSearchResult?>();
            foreach (var organizationNumber in organizationNumbers)
            {
                if (!_memoryCache.TryGetValue(organizationNumber, out RegnskapsregisterSearchResult cachedEntry))
                {
                    organizationNumbersToSearchFor.Add(organizationNumber);
                }
                else
                {
                    results.TryAdd(organizationNumber, cachedEntry);
                }
            }

            await GetAndUpdateCacheEntries(organizationNumbersToSearchFor, results);

            return results;

        }

        private async Task GetAndUpdateCacheEntries(List<string> organizationNumbers, ConcurrentDictionary<string, RegnskapsregisterSearchResult?> results)
        {
            // Number of concurrent tasks. 
            var throttler = new SemaphoreSlim(5);

            var tasks = organizationNumbers.Select(async organizationNumber =>
            {
                await throttler.WaitAsync();
                try
                {
                    await UpdateCacheEntry(organizationNumber, results);
                }
                finally
                {
                    throttler.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        private async Task UpdateCacheEntry(string organizationNumber, ConcurrentDictionary<string, RegnskapsregisterSearchResult?> results)
        {
            var httpClient = _httpClientFactory.CreateClient("rr");

            var clientId = _configuration["UsernameRegnskapsregisteret"];
            var clientSecret = _configuration["PasswordRegnskapsregisteret"];

            var authenticationString = $"{clientId}:{clientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

            RegnskapsregisterSearchResult? result = null;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoint + organizationNumber);
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var resultList = await response.Content.ReadFromJsonAsync<List<RegnskapsregisterSearchResult>>();
                    if (resultList != null)
                    {
                        result = resultList.LastOrDefault();
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            if (result == null)
            {
                results.TryAdd(organizationNumber, null);
            }
            else
            {
                results.TryAdd(organizationNumber, result);
                _memoryCache.Set(organizationNumber, result,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(_cacheTtl));
            }
        }
    }
}
