using RealEstateManager.Infrastructure;
using RealEstateManager.Models;

namespace RealEstateManager.Services;

public class RealEstateGardenUnitService : IRealEstateGardenUnitService
{
    private readonly IFundaApiClient _fundaApiClient;

    public RealEstateGardenUnitService(IFundaApiClient fundaApiClient)
    {
        _fundaApiClient = fundaApiClient;
    }
    
    public async Task<IEnumerable<RealEstateUnit>> GetRentalPropertiesWithGardenCountPerAgencyAsync(string city)
    { 
        try
        {
            var realEstateUnits = await _fundaApiClient.GetRentalPropertiesCountPerAgencyAsync(city);
            
            var sortedUnits = realEstateUnits
                .GroupBy(unit => unit.AgencyName)
                .Select(g => new RealEstateUnit(AgencyName: g.Key, NumberOfUnits: (uint)g.Count()))
                .OrderByDescending(o => o.NumberOfUnits)
                .ToList();
            
            return sortedUnits;
        }
        catch(Exception ex)
        {
            throw new ApplicationException("Error retrieving rental properties with garden count", ex);
        }
    }
}