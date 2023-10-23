using Altinn.ApiClients.Dan.Models;

namespace bransjekartlegging.Models
{
    public record SearchResult
    {
        public List<DataSetV2> DataSets { get; set; } = new();
    }
}

public class DataSetV2
{
    public DataSet DataSet { get; set; } = new();
    public string Source { get; set; } = string.Empty;
}