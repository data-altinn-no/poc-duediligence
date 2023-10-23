using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;
using bransjekartlegging.Models;
using bransjekartlegging.Services.Interfaces;
using eduediligence.Models;
using eduediligence.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace bransjekartlegging.Services;

public class SearchService : ISearchService
{
    private IDanClient _danClient;
    private ILogger _logger;
    private List<DanDataset> _danDatasets;

    public SearchService(IDanClient danclient, ILoggerFactory loggerFactory, IDanDatasetService danDatasetService)
    {
        _danClient = danclient;
        _logger = loggerFactory.CreateLogger<SearchService>();
        _danDatasets = danDatasetService.GetDatasetDefinitions(false, "eDueDiligence", "test");
    }

    public async Task<SearchResult> Search(string organisationNumber)
    {
        SearchResult result = new SearchResult()
        {
            DataSets = new List<DataSetV2>()
        };


        var list2 = new List<Task<DataSetV2>>();

        foreach (var dataset in _danDatasets)
        {
            list2.Add(GetData(dataset.Name, dataset.ReadableSource, !string.IsNullOrEmpty(dataset.HardCodedOrgNo) ? dataset.HardCodedOrgNo : organisationNumber, dataset.Parameters));
        }

        var list = new List<Task<DataSetV2>>();

        list.Add(GetData("UnitBasicInformation","Enhetsregisteret", organisationNumber, new()));
        list.Add(GetData("CertificateOfRegistration", "Foretaksregisteret", organisationNumber, new()));
        list.Add(GetData("RettsstiftelserVirksomhet", "Løsøreregisteret", "810304642", new()));
        list.Add(GetData("Kunngjoringer", "Kunngjøringer", organisationNumber, new()));


        var parametersRegnskap = new Dictionary<string, string> {
            { "Aar", "2021" },
            { "Type", "SELSKAP"},
        };

        list.Add(GetData("Regnskapsregisteret", "Årsregnskap", organisationNumber, parametersRegnskap));

        var parametersAnnual = new Dictionary<string, string> {
      
            { "NumberOfYears", "3"} 
        };

        list.Add(GetData("AnnualFinancialReport", "Regnskapsregisteret", organisationNumber, parametersAnnual));

        await Task.WhenAll(list);

        foreach (var task in list)
        {
            if (task.Result != null && task.Result.DataSet.Values != null)
            {
                if (task.Result.DataSet.Values.FirstOrDefault(x => x.Name == "default") != null)
                {
                    DataSetV2 ds = new()
                    {
                        Source = task.Result.Source,
                        DataSet = new DataSet()
                        {
                            Values = new List<DataSetValue>()
                        }
                    };

                    var flat = DeserializeAndFlatten(task.Result.DataSet.Values.First(x => x.Name == "default").Value.ToString());

                    foreach (var kvp in flat)
                    {
                        ds.DataSet.Values.Add(new DataSetValue()
                        {
                            Value = kvp.Value.ToString(),
                            Name = kvp.Key, 
                            Source = task.Result.Source,
                        });
                    }

                    result.DataSets.Add(ds);
                }
                else
                {
                    result.DataSets.Add(task.Result);
                }

            }
        }

        try
        {   


        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }

        return result;
    }

    private async Task<DataSetV2> GetData(string danDSName, string sourceName, string organisationNumber, Dictionary<string, string> parameters )
    {
        try
        {
            return new DataSetV2()
                {
                    Source = sourceName,
                    DataSet = await _danClient.GetDataSet(danDSName, organisationNumber, null, parameters)
                };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new DataSetV2()
            {
                Source = sourceName,
                DataSet = new DataSet()
            };
        }
    }

    public static Dictionary<string, object> DeserializeAndFlatten(string json)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        JToken token = JToken.Parse(json);
        FillDictionaryFromJToken(dict, token, "");
        return dict;
    }

    private static void FillDictionaryFromJToken(Dictionary<string, object> dict, JToken token, string prefix)
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                foreach (JProperty prop in token.Children<JProperty>())
                {
                    FillDictionaryFromJToken(dict, prop.Value, Join(prefix, prop.Name));
                }
                break;

            case JTokenType.Array:
                int index = 0;
                foreach (JToken value in token.Children())
                {
                    FillDictionaryFromJToken(dict, value, Join(prefix, index.ToString()));
                    index++;
                }
                break;

            default:
                dict.Add(prefix, ((JValue)token).Value);
                break;
        }
    }

    private static string Join(string prefix, string name)
    {
        return (string.IsNullOrEmpty(prefix) ? name : prefix + "." + name);
    }


}