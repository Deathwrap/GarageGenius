using Deathwrap.GarageGenius.Service.Clients;
using Deathwrap.GerageGenius.FrontAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Deathwrap.GerageGenius.FrontAPI.Controllers;

[Controller]
public class EmailConfirmationController : Controller
{
    private readonly IClientsService _clientsService;
    private readonly IConfiguration _configuration;

    public EmailConfirmationController(IClientsService clientsService, IConfiguration configuration)
    {
        _configuration = configuration;
        _clientsService = clientsService;
    }
    
    [HttpGet("confirm_email")]
    public async Task<IActionResult> EmailConfirm([FromQuery] string email, [FromQuery] string code)
    {
        await _clientsService.ValidateEmail(email, code);

        var baseUrl = _configuration.GetSection("Frontend:BaseUrl").Get<string>();

        return View("EmailConfirmation", new EmailConfirmationViewModel
        {
            Email = email,
            BaseUrl = baseUrl
        });
    }
}