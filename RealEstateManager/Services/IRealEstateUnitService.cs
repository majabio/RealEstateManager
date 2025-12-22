using RealEstateManager.Models;

namespace RealEstateManager.Services;

public interface IRealEstateUnitService
{
    public Task<IEnumerable<RealEstateUnit>> GetRentalPropertiesCountPerAgencyAsync(string city);

}