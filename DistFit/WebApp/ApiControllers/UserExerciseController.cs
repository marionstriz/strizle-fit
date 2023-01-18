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
public class UserExerciseController : ControllerBase
{
    private readonly IAppBll _bll;
    private readonly UserExerciseMapper _mapper;

    /// <summary>
    /// User exercise Controller - takes in BLL and automapper interfaces
    /// </summary>
    /// <param name="bll">Business logic layer implementation</param>
    /// /// <param name="mapper">User exercise mapper</param>
    public UserExerciseController(IAppBll bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = new UserExerciseMapper(mapper);
    }

    // GET: api/UserExercise
    /// <summary>
    /// Get all user exercises owned by user, requires authorisation
    /// </summary>
    /// <returns>Enumerable of user exercises</returns>
    [Produces( "application/json" )]
    [Consumes( "application/json" )]
    [ProducesResponseType( typeof( IEnumerable<App.Public.DTO.v1.UserExercise> ), StatusCodes.Status200OK )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.UserExercise>>> GetUserExercises()
    {
        return Ok((await _bll.UserExercises
            .GetAllAsync(User.GetUserId(), true))
            .Select(x => _mapper.Map(x)));
    }

    // GET: api/UserExercise/5
    /// <summary>
    /// Get user exercise by id, requires authorisation and ownership by user
    /// </summary>
    /// <param name="id">User exercise GUID</param>
    /// <returns>User exercise if found, null if not found</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.UserExercise), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.UserExercise>> GetUserExercise(Guid id)
    {
        var userExercise = await _bll.UserExercises.FirstOrDefaultAsync(id);

        if (userExercise == null || userExercise.AppUserId != User.GetUserId())
        {
            return NotFound();
        }

        return _mapper.Map(userExercise)!;
    }

    // PUT: api/UserExercise/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edit user exercise, requires authorisation and ownership by user
    /// </summary>
    /// <param name="id">User exercise id to edit, must match request country id</param>
    /// <param name="userExercise">Edited user exercise, id must match request id</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserExercise(Guid id, App.Public.DTO.v1.UserExercise userExercise)
    {
        if (id != userExercise.Id || userExercise.AppUserId != User.GetUserId())
        {
            return BadRequest();
        }
        
        if (ModelState.IsValid)
        {
            _bll.UserExercises.Update(_mapper.Map(userExercise)!);
            await _bll.SaveChangesAsync();
        }

        return NoContent();
    }

    // POST: api/UserExercise
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Add user exercise, requires authorisation and ownership by user
    /// </summary>
    /// <param name="userExercise">User exercise to add</param>
    /// <returns>Added user exercise</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.UserExercise), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.UserExercise>> PostUserExercise(App.Public.DTO.v1.UserExercise userExercise)
    {
        userExercise.AppUserId = User.GetUserId();
        
        if (ModelState.IsValid)
        {
            _bll.UserExercises.Add(_mapper.Map(userExercise)!);
            await _bll.SaveChangesAsync();
        }

        return CreatedAtAction("GetUserExercise", new
        {
            id = userExercise.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString()
        }, userExercise);
    }

    // DELETE: api/UserExercise/5
    /// <summary>
    /// Delete user exercise with given GUID, requires authorisation and ownership by user
    /// </summary>
    /// <param name="id">Id of user exercise to delete</param>
    /// <returns>No content if succeeded, otherwise valid error code</returns>
    /// [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserExercise(Guid id)
    {
        var userExercise = await _bll.UserExercises.FirstOrDefaultAsync(id);
        if (userExercise == null || userExercise.AppUserId != User.GetUserId())
        {
            return NotFound();
        }

        _bll.UserExercises.Remove(userExercise);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExerciseExists(Guid id)
    {
        return _bll.UserExercises.FirstOrDefault(id) != null;
    }
}