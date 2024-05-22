using System.Security.Claims;
using Deathwrap.GarageGenius.Data.Models;
using Deathwrap.GarageGenius.Service.Cars;
using Deathwrap.GarageGenius.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Deathwrap.GerageGenius.FrontAPI.Controllers;

[Route("api/cars")]
[ApiController]
public class CarsController : Controller
{
    private readonly ICarsService _carsService;

    public CarsController(ICarsService carsService)
    {
        _carsService = carsService;
    }
    
    [Authorize]
    [HttpGet("get-by-client")]
    public async Task<IActionResult> GetByClient()
    {
        var clientIdFromClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (clientIdFromClaim.IsNullOrEmpty())
        {
            return BadRequest("Bad credentials");
        }
        var clientId = Guid.Parse(clientIdFromClaim);

        var responce = await _carsService.GetCarsByClientId(clientId);
        return Ok(responce);
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddByClient(CarMinModel car)
    {
        var clientIdFromClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (clientIdFromClaim.IsNullOrEmpty())
        {
            return BadRequest("Bad credentials");
        }
        var clientId = Guid.Parse(clientIdFromClaim);

        var carId = await _carsService.Add(car, clientId);

        return Ok(new {carId = carId});
    }
}