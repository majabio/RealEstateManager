using System.Text.Json.Serialization;

namespace RealEstateManager.Models;

public class RealEstateUnitsResponse
{
    [JsonPropertyName("Objects")]
    public required RealEstateUnitResponse[] RealEstateUnits { get; set; }

    [JsonPropertyName("Paging")]
    public PaginationResponse? Pagination { get; set; }
}

public class RealEstateUnitResponse
{
    [JsonPropertyName("MakelaarId")]
    public required uint AgencyId { get; set; }
    [JsonPropertyName("MakelaarNaam")]
    public string AgencyName { get; set; } = string.Empty;
}

public class PaginationResponse
{
    [JsonPropertyName("AantalPaginas")]
    public uint NumberOfPages { get; set; }
}