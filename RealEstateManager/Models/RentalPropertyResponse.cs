using System.Text.Json.Serialization;

namespace RealEstateManager.Models;

public class RentalPropertyResponse
{
    [JsonPropertyName("Objects")]
    public ObjectResponse[]? RentalProperties { get; set; } 
    [JsonPropertyName("Paging")]
    public PaginationResponse? Pagination { get; set; }
}

public class ObjectResponse
{
    [JsonPropertyName("MakelaarId")]
    public uint AgencyId { get; set; }
    [JsonPropertyName("MakelaarNaam")]
    public string AgencyName { get; set; } = string.Empty;
}

public class PaginationResponse
{
    [JsonPropertyName("AantalPaginas")]
    public uint NumberOfPages { get; set; }
}