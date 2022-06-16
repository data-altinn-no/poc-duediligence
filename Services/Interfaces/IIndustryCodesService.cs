using bransjekartlegging.Models;

namespace bransjekartlegging.Services.Interfaces
{
    public interface IIndustryCodesService
    {
        public Task<List<IndustryCode>> GetIndustryCodes();
        public Task<List<IndustryCode>> SearchIndustryCodes(string searchString);
    }
}
