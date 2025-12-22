using RealEstateManager.Models;

namespace RealEstateManager.Services;

public interface IRealEstateUnitService
{
    public Task<IEnumerable<RealEstateUnitResponse>> GetRentalPropertiesCountPerAgencyAsync(string city);

}