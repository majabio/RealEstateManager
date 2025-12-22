using System.Text.Json;
using System.Threading.RateLimiting;
using RealEstateManager.Models;

namespace RealEstateManager.Infrastructure;

public class FundaApiClient : IFundaApiClient
{
    private const string FundaClientName = nameof(FundaApiClient);
    private readonly HttpClient _httpClient;
    private readonly ApiRateLimiter _rateLimiter;

    public FundaApiClient(IHttpClientFactory httpClientFactory, ApiRateLimiter rateLimiter)
    {
        _rateLimiter = rateLimiter;
        _httpClient = httpClientFactory.CreateClient(FundaClientName);
    }
    
    public async Task<IDictionary<uint, uint>> GetRentalPropertiesCountPerAgencyAsync(string city)
    {
        var endpoint = $"/feeds/Aanbod.svc/json/76666a29898f491480386d966b75f949/?type=koop&zo=/{city}/";
        var result = new Dictionary<uint, uint>();
        var currentPage = 10;
        var hasMorePages = true;

        while (hasMorePages)
        {
            var response = await FetchPageAsync(endpoint, currentPage);
            
            hasMorePages = response.Pagination.NumberOfPages > currentPage;
            currentPage++;
        }

        return result;
    }
    
    private async Task<RealEstateUnitResponse> FetchPageAsync(string endpoint, int page)
    {
        await _rateLimiter.WaitAsync();
        
        try
        {
            var response = await _httpClient.GetAsync($"{endpoint}&page={page}/");
        
            var rentalPropertyResponse = await response.Content.ReadFromJsonAsync<RealEstateUnitResponse>();
            return rentalPropertyResponse ?? new RealEstateUnitResponse();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error fetching data from Funda API", ex);
        }   
    }
}