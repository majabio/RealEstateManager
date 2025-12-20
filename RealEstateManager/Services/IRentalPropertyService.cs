namespace RealEstateManager.Services;

public interface IRentalPropertyService
{
    public  Task<IDictionary<uint, uint>> GetRentalPropertiesCountPerAgencyAsync(string city);

}