using System.Security.Claims;
using Deathwrap.GarageGenius.Helper;
using Deathwrap.GarageGenius.Service.RefreshTokens;
using Deathwrap.GarageGenius.Service.Token;
using Deathwrap.GarageGenius.Service.Workers;
using Deathwrap.GerageGenius.FrontAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Deathwrap.GerageGenius.FrontAPI.Controllers;

public class WorkersController: ControllerBase
{
    
    private readonly IWorkersService _workersService;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokensService _refreshTokensService;
    
    
    public WorkersController(IWorkersService workersService, 
        IConfiguration configuration,
        ITokenService tokenService,
        IRefreshTokensService refreshTokensService)
    {
        _workersService = workersService;
        _configuration = configuration;
        _tokenService = tokenService;
        _refreshTokensService = refreshTokensService;
    }
    
    [HttpPost("auth/admin/sign_in")]
    public async Task<IActionResult> AdminAuthenticate(WorkerAuthRequest request)
    {
        var worker = await _workersService.GetAndCheckWorker(request.Login, request.Password);
        
        if (worker == null)
        {
            return BadRequest("Bad credentials");
        }

        var position = await _workersService.GetPositionName(worker.PositionId);

        if (position != "admin")
        {
            return BadRequest("Bad credentials");
        }

        var sessionId = Guid.NewGuid();
        var accessToken = _tokenService.CreateWorkerToken(worker, sessionId, "admin");
        
        var refreshToken = _configuration.GenerateRefreshToken();

        await _refreshTokensService.AddRefreshToken(worker.Id, sessionId, refreshToken);

        return Ok(new AuthResponse
        {
            Name = worker.Name,
            AccessToken  = accessToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPost("auth/admin/refresh")]
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