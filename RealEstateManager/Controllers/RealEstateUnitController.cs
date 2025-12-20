using Microsoft.AspNetCore.Mvc;
using RealEstateManager.Services;

namespace RealEstateManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RealEstateUnitController : ControllerBase
{
    private readonly IRealEstateUnitService _realEstateUnitService;

    public RealEstateUnitController(IRealEstateUnitService realEstateUnitService)
    {
        _realEstateUnitService = realEstateUnitService;
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> GetRentalPropertiesCountPerAgency(string city)
    {
        try
        {
            var result = await _realEstateUnitService.GetRentalPropertiesCountPerAgencyAsync(city);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here for brevity)
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}