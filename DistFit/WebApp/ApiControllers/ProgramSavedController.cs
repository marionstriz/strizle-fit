using System.Security.Claims;
using App.Contracts.BLL;
using App.Public.DTO.v1;
using App.Public.v1.Mappers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Errors = WebApp.Helpers.RestApiErrorHelpers;

namespace WebApp.ApiControllers;

/// <summary>
/// Saved program entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProgramSavedController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly ProgramSavedMapper _mapper;

    /// <summary>
    /// Saved program Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// /// <param name="mapper">Saved program mapper</param>
    public ProgramSavedController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new ProgramSavedMapper(mapper);
    }

    // GET: api/ProgramSaved
    /// <summary>
    /// Get all saved programs owned by user or group, requires authorisation
    /// </summary>
    /// <returns>Enumerable of measurements</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.ProgramSaved> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.ProgramSaved>>> GetSavedPrograms()
    {
        return Ok((await _bll.ProgramsSaved
            .GetAllAsync(User.GetUserId(), true))
            .Select(x => _mapper.Map(x)));
    }

    // GET: api/ProgramSaved/5
    /// <summary>
    /// Get saved program by id, requires authorisation and ownership by user or user group
    /// </summary>
    /// <param name="id">Saved program GUID</param>
    /// <returns>Saved program if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Measurement), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.ProgramSaved>> GetSavedProgram(Guid id)
    {
        var savedProgram = _mapper.Map(await _bll.ProgramsSaved.FirstOrDefaultAsync(id));

        if (savedProgram == null || !UserIsAuthorised(savedProgram, User)) return NotFound();

        return savedProgram;
    }

    // PUT: api/ProgramSaved/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit saved program, requires authorisation and ownership by user or user role
    /// </summary>
    /// <param name="id">Saved program id to edit, must match request saved program id</param>
    /// <param name="savedProgram">Edited saved program, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSavedProgram(Guid id, App.Public.DTO.v1.ProgramSaved savedProgram)
    {
        if (id != savedProgram.Id || !UserIsAuthorised(savedProgram, User))
        {
            return BadRequest();
        }
        
        if (ModelState.IsValid)
        {
            _bll.ProgramsSaved.Update(_mapper.Map(savedProgram)!);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/ProgramSaved
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add saved program, requires authorisation and ownership by user.
    /// Currently can only save program to user
    /// </summary>
    /// <param name="savedProgram">Saved program to add</param>
    /// <returns>Added saved program</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.ProgramSaved), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.ProgramSaved>> PostSavedProgram(App.Public.DTO.v1.ProgramSaved savedProgram)
    {
        savedProgram.Id = Guid.NewGuid();
        if (savedProgram.AppUserId == null 
            ||savedProgram.AppUserId != null && savedProgram.AppUserId != User.GetUserId())
        {
            return BadRequest();
        }
        
        if (ModelState.IsValid)
        {
            _bll.ProgramsSaved.Add(_mapper.Map(savedProgram)!);
            await _bll.SaveChangesAsync();
        }

        return CreatedAtAction("GetSavedProgram", new
        {
            id = savedProgram.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString()
        }, savedProgram);
    }

    // DELETE: api/ProgramSaved/5
    /// <summary>
    /// Delete saved program with given GUID, requires authorisation and ownership by user or user role
    /// </summary>
    /// <param name="id">Id of saved program to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSavedProgram(Guid id)
    {
        var savedProgram = await _bll.ProgramsSaved.FirstOrDefaultAsync(id);
        if (savedProgram == null || !UserIsAuthorised(_mapper.Map(savedProgram), User))
        {
            return NotFound();
        }

        _bll.ProgramsSaved.Remove(savedProgram);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool SavedProgramExists(Guid id)
    {
        return _bll.ProgramsSaved.FirstOrDefault(id) != null;
    }

    private bool UserIsAuthorised(ProgramSaved? savedProgram, ClaimsPrincipal user)
    {
        if (savedProgram == null) return false;
        
        if (savedProgram.AppUserId != null)
        {
            if (savedProgram.AppUserId != User.GetUserId()) return false;
        }

        return true;
    }
}