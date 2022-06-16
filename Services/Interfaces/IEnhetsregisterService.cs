using bransjekartlegging.Models;

namespace bransjekartlegging.Services.Interfaces 
{
    public interface IEnhetsregisterService 
    {
        public Task<List<EnhetsregisterUnit>> Search(List<string> industryCodes, List<string> municipalities);
    }
}