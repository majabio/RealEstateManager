namespace RealEstateManager.Services;

public interface IRealEstateUnitService
{
    public  Task<IDictionary<uint, uint>> GetRentalPropertiesCountPerAgencyAsync(string city);

}