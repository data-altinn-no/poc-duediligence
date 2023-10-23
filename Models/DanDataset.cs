using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;


namespace eduediligence.Models
{
    public class DanDataset 
    {
        public string Name { get; set; }

        public string ReadableSource { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public string HardCodedOrgNo { get; set; }

        public ResponseGrouping? Grouping { get; set; }

        public DanDataset(string name, string readableSource, string hardcodedOrgNo, Dictionary<string, string> parameters, ResponseGrouping grouping)
        {
            Name = name;
            ReadableSource = readableSource; 
            HardCodedOrgNo = hardcodedOrgNo;
            Parameters = parameters;
            Grouping = grouping; 
        }
    }

    public class ResponseGrouping
    {
        public bool MultipleResponseSets { get; set; }

        public string GroupingIdentifier { get; set; } = string.Empty;
    }
}
