using RealEstateManager.Models;

namespace RealEstateManager.Services;

public interface IRealEstateGardenUnitService
{
    public Task<IEnumerable<RealEstateUnit>> GetRentalPropertiesWithGardenCountPerAgencyAsync(string city);
}