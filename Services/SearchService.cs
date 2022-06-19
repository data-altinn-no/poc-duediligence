using bransjekartlegging.Models;
using bransjekartlegging.Services.Interfaces;

namespace bransjekartlegging.Services;

public class SearchService : ISearchService
{
    private readonly IEnhetsregisterService _enhetsregisterService;
    private readonly IRegnskapsregisterService _regnskapsregisterService;

    public SearchService(IEnhetsregisterService enhetsregisterService, IRegnskapsregisterService regnskapsregisterService)
    {
        _enhetsregisterService = enhetsregisterService;
        _regnskapsregisterService = regnskapsregisterService;
    }

    public async Task<List<SearchResult>> Search(List<string> industryCodes, List<string> municipalities, int offsetPage = 0)
    {
        var erUnits = await _enhetsregisterService.Search(industryCodes, municipalities, offsetPage);
        var organizationNumbers = erUnits.Select(x => x.Organisasjonsnummer).ToList();
        var rrEntries = await _regnskapsregisterService.Search(organizationNumbers);
        
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
                AccountingPeriod = GetAccountingPeriod(rrEntries[erUnit.Organisasjonsnummer]),
                Revenue = GetRevenue(rrEntries[erUnit.Organisasjonsnummer]),
                Expenses = GetExpenses(rrEntries[erUnit.Organisasjonsnummer]),
                Profit = GetProfit(rrEntries[erUnit.Organisasjonsnummer])
            });
        }

        return searchResults;
    }

    private long GetRevenue(RegnskapsregisterSearchResult? rrEntry)
    {
        if (rrEntry == null) return 0;
        return Convert.ToInt64(rrEntry.ResultatregnskapResultat.Driftsresultat.Driftsinntekter.SumDriftsinntekter
               + rrEntry.ResultatregnskapResultat.Finansresultat.Finansinntekt.SumFinansinntekter);
    }

    private long GetExpenses(RegnskapsregisterSearchResult? rrEntry)
    {
        if (rrEntry == null) return 0;
        return Convert.ToInt64(rrEntry.ResultatregnskapResultat.Driftsresultat.Driftskostnad.SumDriftskostnad
                               + rrEntry.ResultatregnskapResultat.Finansresultat.Finanskostnad.SumFinanskostnad);
    }

    private long GetProfit(RegnskapsregisterSearchResult? rrEntry)
    {
        if (rrEntry == null) return 0;
        return Convert.ToInt64(rrEntry.ResultatregnskapResultat.Driftsresultat.DriftsresultatDriftsresultat
                               + rrEntry.ResultatregnskapResultat.Finansresultat.NettoFinans);
    }

    private static (DateTime, DateTime)? GetAccountingPeriod(RegnskapsregisterSearchResult? rrEntry)
    {
        if (rrEntry == null) return null;

        return (rrEntry.Regnskapsperiode.FraDato,
            rrEntry.Regnskapsperiode.TilDato);
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