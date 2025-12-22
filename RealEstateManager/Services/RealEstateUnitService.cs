using RealEstateManager.Infrastructure;
using RealEstateManager.Models;

namespace RealEstateManager.Services;

public class RealEstateUnitService : IRealEstateUnitService
{
    private readonly IFundaApiClient _fundaApiClient;

    public RealEstateUnitService(IFundaApiClient fundaApiClient)
    {
        _fundaApiClient = fundaApiClient;
    }
    
    public async Task<IEnumerable<RealEstateUnitResponse>> GetRentalPropertiesCountPerAgencyAsync(string city)
    { 
        try
        {
            var response = await _fundaApiClient.GetRentalPropertiesCountPerAgencyAsync(city);
            return response;
        }
        catch(Exception ex)
        {
            throw new ApplicationException("Error retrieving rental properties count per agency", ex);
        }
    }
}