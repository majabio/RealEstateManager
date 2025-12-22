using RealEstateManager.Models;

namespace RealEstateManager.Infrastructure;

public interface IFundaApiClient
{
    Task<IEnumerable<RealEstateUnitResponse>> GetRentalPropertiesCountPerAgencyAsync(string city);
}