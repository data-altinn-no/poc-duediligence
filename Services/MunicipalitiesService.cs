using bransjekartlegging.Models;
using bransjekartlegging.Services.Interfaces;

namespace bransjekartlegging.Services;

public class MunicipalitiesService : IMunicipalitiesService
{
    private const string ApiEndpoint = "https://ws.geonorge.no/kommuneinfo/v1/kommuner";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private List<Municipality> _municipalities = new();

    private DateTime _refreshAt = DateTime.MinValue;
    private readonly TimeSpan _cacheTtl = TimeSpan.FromHours(12);

    public MunicipalitiesService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Municipality>> GetMunicipalities()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_refreshAt < DateTime.Now)
            {
                await RefreshCache();
            }

            return _municipalities;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<Municipality>> SearchMunicipalities(string searchString)
    {
        return (await GetMunicipalities()).Where(x => x.NameNorwegian.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).ToList();
    }

    private async Task RefreshCache()
    {
        var httpClient = _httpClientFactory.CreateClient("geonorge");

        var municipalities = await httpClient.GetFromJsonAsync<List<Municipality>>(ApiEndpoint);

        _municipalities = municipalities ?? throw new ArgumentNullException();
        _refreshAt = DateTime.Now + _cacheTtl;
    }
}