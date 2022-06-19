using bransjekartlegging.Models;

namespace bransjekartlegging.Services.Interfaces
{
    public interface IRegnskapsregisterService
    {
        public Task<IDictionary<string, RegnskapsregisterSearchResult?>> Search(List<string> organizationNumbers);
    }
}
