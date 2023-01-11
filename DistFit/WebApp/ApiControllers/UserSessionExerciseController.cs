using App.Contracts.BLL;
using App.Public.v1.Mappers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Errors = WebApp.Helpers.RestApiErrorHelpers;

namespace WebApp.ApiControllers;

/// <summary>
/// User exercise entity REST API operations
/// </summary>
[ApiVersion( "1.0" )]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserSessionExerciseController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly UserSessionExerciseMapper _mapper;

    /// <summary>
    /// User session exercise Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// /// <param name="mapper">User session exercise mapper</param>
    public UserSessionExerciseController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new UserSessionExerciseMapper(mapper);
    }

    // GET: api/UserExercise
    /// <summary>
    /// Get all user session exercises, requires authorisation
    /// </summary>
    /// <returns>Enumerable of user session exercises</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.UserSessionExercise> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.UserSessionExercise>>> GetUserSessionExercises()
    {
        return Ok((await _bll.UserSessionExercises
            .GetAllAsync())
            .Select(x => _mapper.Map(x)));
    }

    // GET: api/UserSessionExercise/5
    /// <summary>
    /// Get user session exercise by id
    /// </summary>
    /// <param name="id">User session exercise GUID</param>
    /// <returns>User session exercise if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.UserSessionExercise), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.UserSessionExercise>> GetUserSessionExercise(Guid id)
    {
        var userSessionExercise = await _bll.UserSessionExercises.FirstOrDefaultAsync(id);

        if (userSessionExercise == null)
        {
            return NotFound();
        }

        return _mapper.Map(userSessionExercise)!;
    }

    // PUT: api/UserSessionExercise/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit user session exercise, requires authorisation and ownership by user
    /// </summary>
    /// <param name="id">User session exercise id to edit, must match request country id</param>
    /// <param name="userSessionExercise">Edited user session exercise, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserSessionExercise(Guid id, App.Public.DTO.v1.UserSessionExercise userSessionExercise)
    {
        if (id != userSessionExercise.Id)
        {
            return BadRequest();
        }
        
        if (ModelState.IsValid)
        {
            _bll.UserSessionExercises.Update(_mapper.Map(userSessionExercise)!);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/UserSessionExercise
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add user session exercise, requires authorisation and ownership by user
    /// </summary>
    /// <param name="userSessionExercise">User session exercise to add</param>
    /// <returns>Added user session exercise</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.UserSessionExercise), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.UserSessionExercise>> PostUserExercise(App.Public.DTO.v1.UserSessionExercise userSessionExercise)
    {
        userSessionExercise.Id = Guid.NewGuid();
        
        if (ModelState.IsValid)
        {
            _bll.UserSessionExercises.Add(_mapper.Map(userSessionExercise)!);
            await _bll.SaveChangesAsync();
        }

        return CreatedAtAction("GetUserSessionExercise", new
        {
            id = userSessionExercise.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString()
        }, userSessionExercise);
    }

    // DELETE: api/UserSessionExercise/5
    /// <summary>
    /// Delete user session exercise with given GUID, requires authorisation
    /// </summary>
    /// <param name="id">Id of user session exercise to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    /// [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserSessionExercise(Guid id)
    {
        var userSessionExercise = await _bll.UserSessionExercises.FirstOrDefaultAsync(id);
        if (userSessionExercise == null)
        {
            return NotFound();
        }

        _bll.UserSessionExercises.Remove(userSessionExercise);
        await _bll.SaveChangesAsync();

        return NoContent();
    }
}