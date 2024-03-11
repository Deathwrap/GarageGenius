using System.Security.Claims;
using Deathwrap.GarageGenius.Helper;
using Deathwrap.GarageGenius.Service.Clients;
using Deathwrap.GarageGenius.Service.Email;
using Deathwrap.GarageGenius.Service.RefreshTokens;
using Deathwrap.GarageGenius.Service.Token;
using Deathwrap.GarageGenius.Service.Validation;
using Deathwrap.GerageGenius.FrontAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Deathwrap.GerageGenius.FrontAPI.Controllers;
[Route("api/")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IValidationService _validationService;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokensService _refreshTokensService;
    
    public ClientsController(IClientsService clientsService, 
        IConfiguration configuration,
        IValidationService validationService,
        IEmailService emailService,
        ITokenService tokenService,
        IRefreshTokensService refreshTokensService)
    {
        _clientsService = clientsService;
        _configuration = configuration;
        _validationService = validationService;
        _emailService = emailService;
        _tokenService = tokenService;
        _refreshTokensService = refreshTokensService;
    }
    
    [HttpPost("auth/sign_up")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {

        var code = await _validationService.GenerateCode();
        
        await _clientsService.AddClientForVerification(request.Name, request.Email, request.Password, code);

        var url = await _validationService.GenerateUrl(request.Email, code);

        await _emailService.Send(url, request.Email);

        return Ok();
    }
    
    [HttpPost("auth/sign_in")]
    public async Task<IActionResult> Authenticate(AuthRequest request)
    {
        var client = await _clientsService.GetAndCheckClient(request.Email, request.Password);

        if (client == null)
        {
            return BadRequest("Bad credentials");
        }

        var sessionId = Guid.NewGuid();
        var accessToken = _tokenService.CreateToken(client, sessionId);
        
        var refreshToken = _configuration.GenerateRefreshToken();

        await _refreshTokensService.AddRefreshToken(client.Id, sessionId, refreshToken);

        return Ok(new AuthResponse
        {
            Name = client.Name,
            AccessToken  = accessToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPost("auth/refresh")]
    public async Task<IActionResult> RefreshToken(TokenApi tokenApi)
    {
        if (tokenApi is null)
            return BadRequest("Invalid client request");
        
        string accessToken = tokenApi.AccessToken;
        string refreshToken = tokenApi.RefreshToken;
        
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

        var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));
        var sessionId = Guid.Parse(principal.FindFirstValue("session_id"));

        var oldRefreshToken = await _refreshTokensService.GetRefreshToken(userId, sessionId);

        if (oldRefreshToken.Token != refreshToken)
        {
            return BadRequest("Invalid client request");
        }
        if (DateTime.UtcNow >
            oldRefreshToken.CreationDate.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays")
                .Get<int>()))
        {
            return BadRequest("Refresh token has been expired");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _configuration.GenerateRefreshToken();

        await _refreshTokensService.UpdateRefreshToken(oldRefreshToken, newRefreshToken);
        return Ok(new AuthResponse()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
        });
    }


    [HttpPost("auth/logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var clientIdFromClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var sessionIdFromClaim = User.FindFirstValue("session_id");
        
        if (clientIdFromClaim.IsNullOrEmpty())
        {
            return BadRequest("Bad credentials");
        }

        if (sessionIdFromClaim.IsNullOrEmpty())
        {
            return BadRequest("Bad credentials");
        }
            
        var clientId = Guid.Parse(clientIdFromClaim);
        var sessionId = Guid.Parse(sessionIdFromClaim);

        await _refreshTokensService.DeleteRefreshToken(clientId, sessionId);

        return Ok("deleted");
    }
}