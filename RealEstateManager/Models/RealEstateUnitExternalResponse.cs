using System.Text.Json.Serialization;

namespace RealEstateManager.Models;

public class RealEstateUnitsExternalResponse
{
    [JsonPropertyName("Objects")]
    public RealEstateUnitExternalResponse[]? RealEstateUnits { get; set; }

    [JsonPropertyName("Paging")]
    public PaginationResponse? Pagination { get; set; }
}

public class RealEstateUnitExternalResponse
{
    [JsonPropertyName("MakelaarNaam")]
    public string? AgencyName { get; set; }
}

public class PaginationResponse
{
    [JsonPropertyName("AantalPaginas")]
    public uint? NumberOfPages { get; set; }
}