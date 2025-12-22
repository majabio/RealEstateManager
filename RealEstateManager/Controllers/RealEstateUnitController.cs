using Microsoft.AspNetCore.Mvc;
using RealEstateManager.Models;
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
            var realEstateUnits = await _realEstateUnitService.GetRentalPropertiesCountPerAgencyAsync(city);
            return Ok(realEstateUnits.Select(unit => new RealEstateUnitResponse() { AgencyName = unit.AgencyName, NumberOfUnits = unit.NumberOfUnits}));
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}