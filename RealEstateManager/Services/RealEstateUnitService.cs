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
    
    public async Task<IEnumerable<RealEstateUnit>> GetRentalPropertiesCountPerAgencyAsync(string city)
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
            throw new ApplicationException("Error retrieving rental properties count per agency", ex);
        }
    }
}