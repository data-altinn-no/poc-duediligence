using bransjekartlegging.Models;

namespace bransjekartlegging.Services.Interfaces 
{
    public interface ISearchService 
    {
        public Task<SearchResult> Search(string organisationNumber);
    }
}