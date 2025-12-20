namespace RealEstateManager.Infrastructure;

public interface IFundaApiClient
{
    Task<IDictionary<uint, uint>> GetRentalPropertiesCountPerAgencyAsync(string city);
}