using Microsoft.AspNetCore.Mvc;
using RealEstateManager.Services;

namespace RealEstateManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalPropertyController : ControllerBase
{
    private readonly IRentalPropertyService _rentalPropertyService;

    public RentalPropertyController(IRentalPropertyService rentalPropertyService)
    {
        _rentalPropertyService = rentalPropertyService;
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> GetRentalPropertiesCountPerAgency(string city)
    {
        try
        {
            var result = await _rentalPropertyService.GetRentalPropertiesCountPerAgencyAsync(city);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here for brevity)
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}