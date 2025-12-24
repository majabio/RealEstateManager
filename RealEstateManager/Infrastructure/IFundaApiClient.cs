using RealEstateManager.Models;

namespace RealEstateManager.Infrastructure;

public interface IFundaApiClient
{
    Task<IEnumerable<RealEstateUnitExternalResponse>> GetRentalPropertiesCountPerAgencyAsync(string endpoint);
    
}