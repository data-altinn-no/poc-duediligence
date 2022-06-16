using bransjekartlegging.Models;

namespace bransjekartlegging.Services.Interfaces;

public interface IMunicipalitiesService
{
    public Task<List<Municipality>> GetMunicipalities();
    public Task<List<Municipality>> SearchMunicipalities(string searchString);
}