using Microsoft.AspNetCore.Mvc.RazorPages;
using bransjekartlegging.Models;
using bransjekartlegging.Services.Interfaces;

namespace bransjekartlegging.Pages;

public class SearchModel : PageModel
{
    private readonly ILogger<SearchModel> _logger;
    private readonly ISearchService _searchService;

    public List<SearchResult> SearchResults = new();
    
    public SearchModel(ILogger<SearchModel> logger, ISearchService searchService)
    {
        _logger = logger;
        _searchService = searchService;
    }

    public async Task OnPostAsync(SearchQuery searchQuery)
    {
       SearchResults = await _searchService.Search(searchQuery.IndustryCodes, searchQuery.Municipalities);
    }
}
