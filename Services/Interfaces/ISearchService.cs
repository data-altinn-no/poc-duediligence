using bransjekartlegging.Models;

namespace bransjekartlegging.Services.Interfaces 
{
    public interface ISearchService 
    {
        public Task<List<SearchResult>> Search(List<string> industryCodes, List<string> municipalities);
    }
}