using bransjekartlegging.Models;
using bransjekartlegging.Services.Interfaces;

namespace bransjekartlegging.Services
{
    public class EnhetsregisterService : IEnhetsregisterService
    {

        private const string ApiEndpoint = "https://data.brreg.no/enhetsregisteret/api/enheter";
        private readonly IHttpClientFactory _httpClientFactory;

        public EnhetsregisterService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<EnhetsregisterUnit>> Search(List<string> industryCodes, List<string> municipalities, int offsetPage = 0)
        {
            var httpClient = _httpClientFactory.CreateClient("er");
            
            var queryString  = "?kommunenummer=" + string.Join(',', municipalities);
                queryString += "&naeringskode=" + string.Join(',', industryCodes);
                queryString += "&page=" + offsetPage;

            var result = await httpClient.GetFromJsonAsync<EnhetsregisterSearchResultHalWrapper>(ApiEndpoint + queryString);
            if (result == null) {
                throw new ArgumentNullException(nameof(result));
            }

            return result.Embedded.Enheter;
        }
    }
}