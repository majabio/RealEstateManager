using RealEstateManager.Models;

namespace RealEstateManager.Infrastructure;

public class FundaApiClient : IFundaApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string FundaClientName = nameof(FundaApiClient);
    
    public FundaApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IDictionary<uint, uint>> GetRentalPropertiesCountPerAgencyAsync(string city)
    {
        var endpoint = $"/feeds/Aanbod.svc/json/76666a29898f491480386d966b75f949/?type=koop&zo=/{city}/";
        var result = new Dictionary<uint, uint>();
        var currentPage = 1;
        var hasMorePages = true;

        while (hasMorePages)
        {
            var response = await FetchPageAsync(endpoint, currentPage);
            
            hasMorePages = response.Pagination.NumberOfPages > currentPage;
            currentPage++;
        }

        return result;
    }
    
    private async Task<RentalPropertyResponse> FetchPageAsync(string endpoint, int page)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient(FundaClientName);
            
            var response = await httpClient.GetAsync($"{endpoint}&page={page}/");
        
            var rentalPropertyResponse = await response.Content.ReadFromJsonAsync<RentalPropertyResponse>();
            return rentalPropertyResponse ?? new RentalPropertyResponse();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error fetching data from Funda API", ex);
        }   
    }
}