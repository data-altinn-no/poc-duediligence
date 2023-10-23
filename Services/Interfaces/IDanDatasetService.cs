using bransjekartlegging.Models;
using eduediligence.Models;

namespace eduediligence.Services.Interfaces
{
    public interface IDanDatasetService
    {
        public List<DanDataset> GetDatasetDefinitions(bool usePublicMetadata, string serviceContext, string environment);
    }
}
