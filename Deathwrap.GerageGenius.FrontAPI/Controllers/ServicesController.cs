using Deathwrap.GarageGenius.Service.Models;
using Deathwrap.GarageGenius.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deathwrap.GerageGenius.FrontAPI.Controllers;
[Route("api/services")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IServicesService _servicesService;
    public ServicesController(IServicesService serviceService)
    {
        _servicesService = serviceService;
    }
    [HttpGet("categories")]
    public async Task<IActionResult> GetServicesCategories()
    {
        return Ok( await _servicesService.GetServiceCategories());
    }

    [HttpGet("by-id")]
    public async Task<IActionResult> GetServicesByCategoryId(int categoryId)
    {
        return Ok(await _servicesService.GetServicesByCategoryId(categoryId));
    }

    [HttpPost("add-category")]
    //[Authorize]
    public async Task<IActionResult> AddServiceCategory(string name)
    {
        var categoryId = await _servicesService.AddCategory(name);
        return Ok(new {categoryId = categoryId});
    }

    [HttpPost("add-service")]
    public async Task<IActionResult> AddService(ServiceMinModel serviceMin)
    {
        var serviceId = await _servicesService.AddService(serviceMin);

        return Ok(new { serviceId = serviceId });
    }
    
    
    
}