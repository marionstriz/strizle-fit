using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.DAL.EF;
using App.Domain.Identity;
using Base.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using App.Public.DTO.v1.Identity;
using AppUser = App.Domain.Identity.AppUser;
using Errors = WebApp.Helpers.RestApiErrorHelpers;

namespace WebApp.ApiControllers.Identity;

/// <summary>
/// Login and register to REST API, refresh tokens
/// </summary>
[ApiVersion( "1.0" )]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly Random _rnd = new();
    private readonly AppDbContext _dbContext;
    
    /// <summary>
    /// Controller for REST API account management
    /// </summary>
    /// <param name="signInManager">Identity sign-in manager</param>
    /// <param name="userManager">Identity user manager</param>
    /// <param name="logger">Logger</param>
    /// <param name="configuration">Application configuration</param>
    /// <param name="dbContext">Application database context</param>
    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AccountController> logger, IConfiguration configuration, AppDbContext dbContext)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _configuration = configuration;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Log in to the REST backend - generates JWT to be included in
    /// Authorize: Bearer xyz
    /// </summary>
    /// <param name="loginData">Email and password</param>
    /// <returns>JWT, refresh token and email</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( JwtResponse ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<JwtResponse>> Login([FromBody] Login loginData)
    {
        var appUser = await _userManager.FindByEmailAsync(loginData.Email);
        if (appUser == null) return await AccountErrorNotFound(
            $"email {loginData.Email} not found", 
            "User/password problem");

        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginData.Password, false);
        if (!result.Succeeded) return await AccountErrorBadRequest(
                $"password problem for user {loginData.Email}", 
                "User/password problem");

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null) return await ClaimsPrincipalError(appUser.Email);

        var validTokens = GetValidUserRefreshTokens(appUser);
        RefreshToken refreshToken;
        if (validTokens.Count != 0)
        {
            if (validTokens.Count != 1) return Problem("More than one valid refresh token found");
            refreshToken = validTokens.First();
            UpdateRefreshToken(refreshToken);
        }
        else
        {
            refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpirationDateTime = DateTime.UtcNow.AddDays(30)
            };
            if (appUser.RefreshTokens == null) appUser.RefreshTokens = new List<RefreshToken> {refreshToken};
            else appUser.RefreshTokens.Add(refreshToken);
        }

        await _dbContext.SaveChangesAsync();
        var res = GenerateJwtResponse(claimsPrincipal, refreshToken.Token, appUser.Email!);

        return Ok(res);
    }

    /// <summary>
    /// Register to the REST backend - generates JWT to be included in
    /// Authorize: Bearer xyz
    /// </summary>
    /// <param name="registrationData">Email and password</param>
    /// <returns>JWT, refresh token and email</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( JwtResponse ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JwtResponse>> Register([FromBody] Register registrationData)
    {
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (appUser != null) return await AccountErrorBadRequest(
                $"user with email {registrationData.Email} is already registered",
                "User with email is already registered");

        var refreshToken = new RefreshToken();
        appUser = new AppUser
        {
            Email = registrationData.Email,
            UserName = registrationData.Email,
            RefreshTokens = new List<RefreshToken> {refreshToken}
        };

        var result = await _userManager.CreateAsync(appUser, registrationData.Password);
        if (!result.Succeeded) return GetBadRequestActionResult("user", result.ToString());

        appUser = await _userManager.FindByEmailAsync(appUser.Email);
        if (appUser == null) return await AccountErrorNotFound(
                $"user with email {registrationData.Email} is not found after registration",
                "User with email not found after registration");

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null) return await ClaimsPrincipalError(appUser.Email);

        var res = GenerateJwtResponse(claimsPrincipal, refreshToken.Token, appUser.Email);

        return Ok(res);
    }
    
    /// <summary>
    /// Refresh the JWT token for account and receive new refresh token
    /// </summary>
    /// <param name="refreshTokenModel">JWT and refresh token</param>
    /// <returns>JWT, new refresh token and email</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( JwtResponse ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<JwtResponse>> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
    {
        JwtSecurityToken jwtToken;
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
            if (jwtToken == null)
            {
                return GetBadRequestActionResult("token", "No token");
            }
        }
        catch (Exception e)
        {
            return GetBadRequestActionResult("token", $"Can't parse token, {e.Message}");
        }

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return GetBadRequestActionResult("token", "No email in jwt");
        }

        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            return GetNotFoundActionResult("user", $"User with email {userEmail} not found");
        }

        var validTokens = GetValidUserRefreshTokens(appUser, refreshTokenModel);
        if (!validTokens.Any()) return Problem("RefreshTokens collection is empty, no valid refresh tokens found");
        if (validTokens.Count != 1) return Problem("More than one valid refresh token found");

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null) return await ClaimsPrincipalError(appUser.Email);

        var refreshToken = validTokens.First();
        if (refreshToken.Token == refreshTokenModel.RefreshToken)
        {
            UpdateRefreshToken(refreshToken);
            await _dbContext.SaveChangesAsync();
        }

        var res = GenerateJwtResponse(claimsPrincipal, refreshToken.Token, appUser.Email);

        return Ok(res);
    }

    private async Task<ActionResult> ClaimsPrincipalError(string email)
    {
        return await AccountErrorBadRequest(
            $"could not get ClaimsPrincipal for user {email}",
            "could not get ClaimsPrincipal for user");
    }

    private JwtResponse GenerateJwtResponse(ClaimsPrincipal claimsPrincipal, string refreshToken, string email)
    {
        var token = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"]!,
            _configuration["JWT:Issuer"]!,
            _configuration["JWT:Issuer"]!,
            DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireInDays"))
        );
        
        return new JwtResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Email = email
        };
    }

    private List<RefreshToken> GetValidUserRefreshTokens(
        AppUser appUser, RefreshTokenModel refreshTokenModel)
    {
        return _dbContext.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x => 
                x.Token == refreshTokenModel.RefreshToken && x.ExpirationDateTime > DateTime.UtcNow ||
                x.PreviousToken == refreshTokenModel.RefreshToken && x.PreviousTokenExpirationDateTime > DateTime.UtcNow)
            .ToList();
    }
    
    private List<RefreshToken> GetValidUserRefreshTokens(AppUser appUser)
    {
        return _dbContext.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x => 
                x.ExpirationDateTime > DateTime.UtcNow || 
                x.PreviousTokenExpirationDateTime > DateTime.UtcNow)
            .ToList();
    }

    private void UpdateRefreshToken(RefreshToken refreshToken)
    {
        refreshToken.PreviousToken = refreshToken.Token;
        refreshToken.PreviousTokenExpirationDateTime = DateTime.UtcNow.AddMinutes(1);

        refreshToken.Token = Guid.NewGuid().ToString();
        refreshToken.ExpirationDateTime = DateTime.UtcNow.AddDays(7);
    }
    
    private async Task<ActionResult> AccountErrorNotFound(string logMessage, string errorMessage)
    {
        await AccountErrorAsync(logMessage);
        return GetNotFoundActionResult("account", errorMessage);
    }
    
    private async Task<ActionResult> AccountErrorBadRequest(string logMessage, string errorMessage)
    {
        await AccountErrorAsync(logMessage);
        return GetBadRequestActionResult("account", errorMessage);
    }

    private async Task AccountErrorAsync(string errorMessage)
    {
        _logger.LogWarning("WebApi login/registration failed, {}", errorMessage);
        await Task.Delay(_rnd.Next(100, 1000));
    }

    private ActionResult GetBadRequestActionResult(string errorTitle, string errorMessage)
    {
        var error = Errors.GetBadRequestErrorResponse(HttpContext.TraceIdentifier);
        error.Errors[errorTitle] = new List<string>{errorMessage};
        return BadRequest(error);
    }
    
    private ActionResult GetNotFoundActionResult(string errorTitle, string errorMessage)
    {
        var error = Errors.GetNotFoundErrorResponse(HttpContext.TraceIdentifier);
        error.Errors[errorTitle] = new List<string>{errorMessage};
        return NotFound(error);
    }
}