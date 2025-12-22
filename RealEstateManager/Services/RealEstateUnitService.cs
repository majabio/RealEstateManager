using RealEstateManager.Infrastructure;

namespace RealEstateManager.Services;

public class RealEstateUnitService : IRealEstateUnitService
{
    private readonly IFundaApiClient _fundaApiClient;

    public RealEstateUnitService(IFundaApiClient fundaApiClient)
    {
        _fundaApiClient = fundaApiClient;
    }
    
    public async Task<IDictionary<uint, uint>> GetRentalPropertiesCountPerAgencyAsync(string city)
    { 
        try
        {
            return await _fundaApiClient.GetRentalPropertiesCountPerAgencyAsync(city);
        }
        catch(Exception ex)
        {
            throw new ApplicationException("Error retrieving rental properties count per agency", ex);
        }
    }
}