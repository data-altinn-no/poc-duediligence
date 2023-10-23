using eduediligence.Models;
using eduediligence.Services.Interfaces;

namespace eduediligence.Services
{
    public class DanDatasetService : IDanDatasetService
    {
        private HttpClient _client;
        public DanDatasetService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("httpClient");
        }

        //Env = test, dev, prod
        public List<DanDataset> GetDatasetDefinitions(bool usePublicMetadata, string serviceContext, string environment)
        {
            List<DanDataset> dsDefs = new List<DanDataset>();

            if (usePublicMetadata)
            {
                var url = $"https://{environment + "-"}api.data.altinn.no/v1/evidencecodes/{serviceContext}";
            }
            else
            {
                dsDefs.Add(new DanDataset("UnitBasicInformation", "Enhetsregisteret", string.Empty, new(), new()));
                dsDefs.Add(new DanDataset("CertificateOfRegistration", "Foretaksregisteret", string.Empty, new(), new()));
                dsDefs.Add(new DanDataset("RettsstiftelserVirksomhet", "Løsøreregisteret", "810304642", new(), new()));
                dsDefs.Add(new DanDataset("Kunngjoringer", "Kunngjøringer", string.Empty, new(), new()));

                var parametersRegnskap = new Dictionary<string, string>
                {
                    { "Aar", "2021" },
                    { "Type", "SELSKAP" },
                };

                dsDefs.Add(new DanDataset("Regnskapsregisteret", "Årsregnskap", string.Empty, parametersRegnskap, new()));

                var parametersAfr = new Dictionary<string, string>
                {
                    { "NumberOfYears", "3" }
                };

                dsDefs.Add(new DanDataset("AnnualFinancialReport", "Regnskapsregisteret", string.Empty, parametersAfr, new()));
            }





            return dsDefs;
            
        }
    }
}
