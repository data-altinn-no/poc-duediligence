using bransjekartlegging.Models;
using bransjekartlegging.Services.Interfaces;

namespace bransjekartlegging.Services;

public class SearchService : ISearchService
{
    private readonly IEnhetsregisterService _enhetsregisterService;

    public SearchService(IEnhetsregisterService enhetsregisterService)
    {
        _enhetsregisterService = enhetsregisterService;
    }

    public async Task<List<SearchResult>> Search(List<string> industryCodes, List<string> municipalities)
    {
        var erUnits = await _enhetsregisterService.Search(industryCodes, municipalities);

        // TODO! Look up accounting information per result
        var searchResults = new List<SearchResult>();
        foreach (var erUnit in erUnits)
        {
            searchResults.Add(new SearchResult()
            {
                OrganizationNumber = erUnit.Organisasjonsnummer,
                Name = erUnit.Navn,
                IndustryCodes = GetIndustryCodes(erUnit),
                Municipality = erUnit.Forretningsadresse.Kommune,
                Type = erUnit.Organisasjonsform.Beskrivelse,
                Employees = erUnit.AntallAnsatte,
                AccountingPeriod = (DateTime.Today, DateTime.Today),
                Revenue = 1000_000,
                Expenses = 400_000,
                Profit = 600_000
            });
        }

        return searchResults;
    }

    private static List<IndustryCode> GetIndustryCodes(EnhetsregisterUnit erUnit)
    {
        var result = new List<IndustryCode>
        {
            MapNaeringsKodeToIndustryCode(erUnit.Naeringskode1)
        };

        if (erUnit.Naeringskode2 != null)
        {
            result.Add(MapNaeringsKodeToIndustryCode(erUnit.Naeringskode2));
        }

        if (erUnit.Naeringskode3 != null)
        {
            result.Add(MapNaeringsKodeToIndustryCode(erUnit.Naeringskode3));
        }

        return result;
    }

    private static IndustryCode MapNaeringsKodeToIndustryCode(Naeringskode naeringskode)
    {
        return new IndustryCode()
        {
            Code = naeringskode.Kode,
            Name = naeringskode.Beskrivelse
        };
    }
}