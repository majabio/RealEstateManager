using System.Text.Json;
using System.Threading.RateLimiting;
using RealEstateManager.Models;

namespace RealEstateManager.Infrastructure;

public class FundaApiClient : IFundaApiClient
{
    private const string FundaClientName = nameof(FundaApiClient);
    private readonly HttpClient _httpClient;
    private readonly ApiRateLimiter _rateLimiter;
    private readonly SemaphoreSlim _concurrencyLimiter;

    public FundaApiClient(IHttpClientFactory httpClientFactory, ApiRateLimiter rateLimiter, IConfiguration configuration)
    {
        _rateLimiter = rateLimiter;
        _httpClient = httpClientFactory.CreateClient(FundaClientName);
        
        var maxConcurrent = configuration.GetValue("ExternalApi:MaxConcurrentRequests", 20);
        _concurrencyLimiter = new SemaphoreSlim(maxConcurrent, maxConcurrent);
    }
    
    public async Task<IEnumerable<RealEstateUnitExternalResponse>> GetRentalPropertiesCountPerAgencyAsync(string endpoint)
    {
        return await GetAllPagesAsync(endpoint);
    }
    
    private async Task<IEnumerable<RealEstateUnitExternalResponse>> GetAllPagesAsync(string endpoint)
    {
        var firstPageResponse = await GetPageAsync(endpoint, 1);
        
        if (firstPageResponse?.RealEstateUnits == null)
        {
            return [];
        }

        var objects = new List<RealEstateUnitExternalResponse>(firstPageResponse.RealEstateUnits);
        var numberOfPages = firstPageResponse.Pagination?.NumberOfPages ?? 1;

        if (numberOfPages == 1)
        {
            return objects;
        }

        var pageNumbers = Enumerable.Range(2, (int)numberOfPages-3).ToList();
        var concurrentItems = new System.Collections.Concurrent.ConcurrentBag<RealEstateUnitExternalResponse>();
        
        foreach (var realEstateUnit in firstPageResponse.RealEstateUnits)
        {
            concurrentItems.Add(realEstateUnit);
        }

        var getTasks = pageNumbers.Select(async pageNumber =>
        {
            await _concurrencyLimiter.WaitAsync();
            
            try
            {
                var response = await GetPageAsync(endpoint, pageNumber);

                if (response?.RealEstateUnits != null)
                {
                    foreach (var unit in response.RealEstateUnits)
                    {
                        concurrentItems.Add(unit);
                    }
                }
            }
            finally
            {
                _concurrencyLimiter.Release();
            }
        });

        await Task.WhenAll(getTasks);

        return concurrentItems.ToList();;
    }

    private async Task<RealEstateUnitsExternalResponse?> GetPageAsync(string endpoint, int page)
    {
        await _rateLimiter.WaitAsync();
        
        try
        {
            var response = await _httpClient.GetAsync($"{endpoint}&page={page}");
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
        
            var realEstateUnitsResponse = await response.Content.ReadFromJsonAsync<RealEstateUnitsExternalResponse>();
            return realEstateUnitsResponse;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error fetching data from Funda API", ex);
        }   
    }
}